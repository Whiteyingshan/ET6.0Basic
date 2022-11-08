using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace VEngine
{
    /// <summary>
    /// 下载操作，可以通过 MaxDownloads 控制最大并发数量，以及 MaxBandwidth 控制最大带宽，目前底层采用的是 c# 的 HttpWebRequest 下载资源暂时没有测试
    /// </summary>
    public sealed class Download : CustomYieldInstruction
    {
        /// <summary>
        ///     最大并行下载的数量，建议不操过 10 个，多了可能会出现异常，而 Unity 官方博客有篇老文章之前说同时下载的 Bundle 数量需要控制在 3 - 5 个。
        /// </summary>
        public static uint MaxDownloads = 10;

        /// <summary>
        ///     最大带宽, 大于 0 时 表示开启限速，默认为 0 不限速。
        /// </summary>
        public static ulong MaxBandwidth = 0; // 1 MB

        /// <summary>
        ///     准备下载的队列，当 Progressing 中的数量小于 MaxDownloads 的时候会自动按照 FIFO 的策略，从这个队列启动新的下载。
        /// </summary>
        private static readonly List<Download> Prepared = new List<Download>();

        /// <summary>
        ///     下载中的队列，最大数量收到 MaxDownloads 控制。
        /// </summary>
        public static readonly List<Download> Progressing = new List<Download>();

        /// <summary>
        ///     缓冲区的内容，防止重复下载，当所有内容下载完成后，自动清理。
        /// </summary>
        private static readonly Dictionary<string, Download> Cache = new Dictionary<string, Download>();

        private static float lastSampleTime;

        private static ulong lastTotalDownloadedBytes;

        private Download()
        {
            status = DownloadStatus.Wait;
            downloadedBytes = 0;
        }

        public float progress => downloadedBytes * 1f / info.size;

        /// <summary>
        ///     下载 <see cref="DownloadInfo" />
        /// </summary>
        public DownloadInfo info { get; private set; }

        /// <summary>
        ///     下载的状态
        /// </summary>
        public DownloadStatus status { get; private set; }

        /// <summary>
        ///     错误内容
        /// </summary>
        public string error { get; private set; }

        /// <summary>
        ///     下载完成的回调
        /// </summary>
        public Action<Download> completed { get; set; }

        /// <summary>
        ///     是否下载完成
        /// </summary>
        public bool isDone => status == DownloadStatus.Failed || status == DownloadStatus.Success;

        /// <summary>
        ///     已经下载的字节
        /// </summary>
        public ulong downloadedBytes { get; private set; }

        private UnityWebRequest UnityWebRequest;

        /// <summary>
        ///     是否让协程继续等待
        /// </summary>
        public override bool keepWaiting => !isDone;

        /// <summary>
        ///     是否有内容在下载中
        /// </summary>
        public static bool Working => Progressing.Count > 0;

        /// <summary>
        ///     所有下载内容的已经下载的字节大小
        /// </summary>
        public static ulong TotalDownloadedBytes
        {
            get
            {
                var size = 0ul;
                foreach (var item in Cache)
                {
                    size += item.Value.downloadedBytes;
                }

                return size;
            }
        }

        /// <summary>
        ///     所有下载内容的总字节大小
        /// </summary>
        public static ulong TotalSize
        {
            get
            {
                var value = 0UL;
                foreach (var item in Cache)
                {
                    value += item.Value.info.size;
                }

                return value;
            }
        }

        /// <summary>
        ///     当前下载的总带宽
        /// </summary>
        public static ulong TotalBandwidth { get; private set; }

        /// <summary>
        ///     异步下载一个文件
        /// </summary>
        /// <param name="url"></param>
        /// <param name="savePath"></param>
        /// <param name="completed"></param>
        /// <param name="size"></param>
        /// <param name="crc"></param>
        /// <returns></returns>
        public static Download DownloadAsync(string url, string savePath, Action<Download> completed = null, ulong size = 0, uint crc = 0)
        {
            return DownloadAsync(new DownloadInfo
            {
                url = url,
                savePath = savePath,
                crc = crc,
                size = size
            }, completed);
        }

        /// <summary>
        ///     下载指定信息的内容
        /// </summary>
        /// <param name="info"></param>
        /// <param name="completed"></param>
        /// <returns></returns>
        public static Download DownloadAsync(DownloadInfo info, Action<Download> completed = null)
        {
            if (Cache.TryGetValue(info.url, out Download download))
            {
                Logger.W("Download url {0} already exist.", info.url);
            }
            else
            {
                download = new Download { info = info };
                Prepared.Add(download);
                Cache.Add(info.url, download);
            }

            if (completed != null)
            {
                download.completed += completed;
            }

            return download;
        }


        /// <summary>
        ///     更新所有下载操作
        /// </summary>
        public static void UpdateDownloads()
        {
            if (Prepared.Count > 0)
            {
                for (var index = 0; index < Mathf.Min(Prepared.Count, MaxDownloads - Progressing.Count); index++)
                {
                    Download download = Prepared[index];
                    Prepared.RemoveAt(index);
                    index--;
                    Progressing.Add(download);
                    download.Start();
                }
            }

            if (Progressing.Count > 0)
            {
                for (var index = 0; index < Progressing.Count; index++)
                {
                    var download = Progressing[index];
                    if (!download.isDone)
                    {
                        download.Update();
                    }
                    else
                    {
                        if (download.status == DownloadStatus.Failed)
                        {
                            Logger.E("Unable to download {0} with error {1}", download.info.url, download.error);
                        }
                        else
                        {
                            Logger.I("Success to download {0}", download.info.url);
                        }
                        download.Complete();
                        Progressing.RemoveAt(index);
                        index--;
                    }
                }

                if (Time.realtimeSinceStartup - lastSampleTime >= 1)
                {
                    TotalBandwidth = TotalDownloadedBytes - lastTotalDownloadedBytes;
                    lastTotalDownloadedBytes = TotalDownloadedBytes;
                    lastSampleTime = Time.realtimeSinceStartup;
                }
            }
            else
            {
                if (Cache.Count <= 0)
                {
                    return;
                }
                Cache.Clear();
                lastTotalDownloadedBytes = 0;
                lastSampleTime = Time.realtimeSinceStartup;
            }
        }

        /// <summary>
        ///     重试下载
        /// </summary>
        public void Retry()
        {
            status = DownloadStatus.Wait;
            Start();
        }

        /// <summary>
        ///     取消下载
        /// </summary>
        public void Cancel()
        {
            error = "User Cancel.";
            status = DownloadStatus.Failed;
            UnityWebRequest?.Abort();
        }

        private void Update()
        {
            if (!UnityWebRequest.isDone)
            {
                downloadedBytes = UnityWebRequest.downloadedBytes;
                if (info.size == 0 && UnityWebRequest.downloadProgress > 0)
                {
                    string length = UnityWebRequest.GetResponseHeader("Content-Length");
                    info.size = length is null ? 0 : ulong.Parse(length);
                }
            }
            else
            {
                error = UnityWebRequest.error;
                status = UnityWebRequest.result == UnityWebRequest.Result.ConnectionError ? DownloadStatus.Failed : DownloadStatus.Success;
                downloadedBytes = UnityWebRequest.downloadedBytes;
                UnityWebRequest.Dispose();
            }
        }

        private void Complete()
        {
            if (completed != null)
            {
                completed.Invoke(this);
                completed = null;
            }
        }

        private void Start()
        {
            try
            {
                if (status != DownloadStatus.Wait)
                {
                    return;
                }

                status = DownloadStatus.Progressing;
                UnityWebRequest = new UnityWebRequest(info.url);
                UnityWebRequest.method = UnityWebRequest.kHttpVerbGET;
                UnityWebRequest.downloadHandler = new DownloadHandlerFile(info.savePath);
                UnityWebRequest.certificateHandler = new AcceptAllCertificate();
                UnityWebRequest.SendWebRequest();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}