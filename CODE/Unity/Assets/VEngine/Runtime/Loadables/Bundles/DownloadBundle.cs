using System;
using UnityEngine;

namespace VEngine
{
    /// <summary>
    ///     从服务器下载的 Bundle，下载完成后会自动加载，并更新下载地址到 bundleToURLs 的缓存。
    /// </summary>
    internal class DownloadBundle : Bundle
    {
        private Download download;
        private AssetBundleCreateRequest request;

        protected override void OnLoad()
        {
            download = Download.DownloadAsync(pathOrURL, Versions.GetDownloadDataPath(info.name), null, info.size,
                info.crc);
            if (mustCompleteOnNextFrame)
            {
                throw new InvalidOperationException();
            }

            download.completed += OnDownloaded;
        }

        private void OnDownloaded(Download obj)
        {
            request = AssetBundle.LoadFromFileAsync(obj.info.savePath);
            Versions.SetBundlePathOrURl(info.name, obj.info.savePath);
            download = null;
        }

        protected override void OnUpdate()
        {
            if (status != LoadableStatus.Loading)
            {
                return;
            }

            if (download != null && !download.isDone)
            {
                progress = download.downloadedBytes * 1f / download.info.size * 0.5f;
                if (!string.IsNullOrEmpty(download.error))
                {
                    Finish(download.error);
                    return;
                }
            }

            if (request == null)
            {
                return;
            }

            progress = 0.5f + request.progress;
            if (!request.isDone)
            {
                return;
            }

            assetBundle = request.assetBundle;
            Finish(assetBundle == null ? "assetBundle == null" : null);
            request = null;
        }
    }
}