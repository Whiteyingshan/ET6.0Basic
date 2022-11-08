using System.Collections.Generic;
using System.IO;

namespace VEngine
{
    /// <summary>
    ///     检查版本更新的操作，这是一个异步的操作，检查完成后会通过 result 返回需要下载更新的文件信息。
    /// </summary>
    public sealed class GetDownloadSize : Operation
    {
        /// <summary>
        ///     通过分组获取的所有 Bundle 回针对这些内容展开检查
        /// </summary>
        private readonly List<BundleInfo> bundles = new List<BundleInfo>();

        /// <summary>
        ///     检查的结果，可以提供给 <seealso cref="DownloadVersions" />> 下载用。
        /// </summary>
        public readonly List<DownloadInfo> result = new List<DownloadInfo>();

        /// <summary>
        ///     检查对象的数量
        /// </summary>
        public int count => bundles.Count;

        /// <summary>
        ///     当前检查的索引，会在update中更新，索引等于 bundles 的长度时表示检查已经完成。
        /// </summary>
        public int index { get; private set; }

        /// <summary>
        ///     需要更新的内容的大小
        /// </summary>
        public ulong totalSize { get; private set; }

        /// <summary>
        ///     需要检查的分组，默认不传则检查所有内容，反正则检查指定分组内容的更新
        /// </summary>
        public string[] groupNames { get; set; }

        /// <summary>
        ///     当前检查的文件名字
        /// </summary>
        public string current
        {
            get
            {
                if (index < bundles.Count)
                {
                    return bundles[index].name;
                }

                return string.Empty;
            }
        }

        /// <summary>
        ///     启动检查
        /// </summary>
        public override void Start()
        {
            base.Start();
            index = 0;
            totalSize = 0;
            if (bundles.Count > 0)
            {
                bundles.Clear();
            }

            bundles.AddRange(Versions.GetBundlesWithGroups(groupNames));
            if (bundles.Count == 0)
            {
                Finish();
            }
        }

        /// <summary>
        ///     更新检查进度，更新逻辑会收到 Updater 的 maxUpdateTimeSlice 控制，可以避免卡顿。
        /// </summary>
        protected override void Update()
        {
            switch (status)
            {
                case OperationStatus.Processing:
                    while (index < bundles.Count)
                    {
                        var bundle = bundles[index];
                        var savePath = Versions.GetDownloadDataPath(bundle.name);
                        var info = new FileInfo(savePath);
                        // 每个文件都是唯一的简单比较版本
                        if (!info.Exists || info.Length != (long) bundle.size)
                            // 去掉重复内容
                        {
                            if (!result.Exists(downloadInfo => downloadInfo.savePath == savePath))
                            {
                                totalSize += bundle.size;
                                result.Add(new DownloadInfo
                                {
                                    crc = bundle.crc,
                                    url = Versions.GetDownloadURL(bundle.name),
                                    size = bundle.size,
                                    savePath = savePath
                                });
                            }
                        }

                        index++;
                        if (index == bundles.Count)
                        {
                            Finish();
                        }
                        if (Updater.Instance.busy)
                        {
                            return;
                        }
                    }

                    break;
            }
        }
    }
}