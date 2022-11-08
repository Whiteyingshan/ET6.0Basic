using System;
using System.Collections.Generic;
using System.IO;

namespace VEngine
{
    /// <summary>
    ///     资源清单，记录了所有要加载的资源的寻址信息和依赖关系。
    /// </summary>
    public class Manifest
    {
        private const string key_version = "[Version]";
        private const string key_groups = "[Groups]";
        private const string key_paths = "[Paths]";
        private const string key_directories = "[Directories]";
        private const string key_bundles = "[Bundles]";
        private const string key_assets = "[Assets]";

        private static readonly HashSet<string> all_keys = new HashSet<string>
        {
            key_version,
            key_groups,
            key_paths,
            key_directories,
            key_bundles,
            key_assets
        };

        /// <summary>
        ///     所有资源路径
        /// </summary>
        internal readonly List<string> allAssetPaths = new List<string>();

        /// <summary>
        ///     所有资源的目录
        /// </summary>
        private readonly List<string> directories = new List<string>();

        /// <summary>
        ///     按 bundle 名字关联运行时信息
        /// </summary>
        private readonly Dictionary<string, BundleInfo> nameWithBundles = new Dictionary<string, BundleInfo>();

        /// <summary>
        ///     按 group 名字关联运行时信息
        /// </summary>
        private readonly Dictionary<string, GroupInfo> nameWithGroups = new Dictionary<string, GroupInfo>();

        /// <summary>
        ///     按 asset 名字关联运行时信息
        /// </summary>
        private readonly Dictionary<string, AssetInfo> pathWithAssets = new Dictionary<string, AssetInfo>();

        /// <summary>
        ///     所有 asset 的运行时信息
        /// </summary>
        public List<AssetInfo> assets = new List<AssetInfo>();

        /// <summary>
        ///     所有 bundle 的运行时信息
        /// </summary>
        public List<BundleInfo> bundles = new List<BundleInfo>();

        /// <summary>
        ///     所有 group 的运行时信息
        /// </summary>
        public List<GroupInfo> groups = new List<GroupInfo>();

        public bool includeInBuild;

        /// <summary>
        ///     版本号
        /// </summary>
        public int version = 0;

        public string name { get; set; }

        public int id { get; set; }

        /// <summary>
        ///     所有版本内的资源路径
        /// </summary>
        public string[] AllAssetPaths => allAssetPaths.ToArray();

        public void Load(string path)
        {
            pathWithAssets.Clear();
            nameWithBundles.Clear();
            nameWithGroups.Clear();
            allAssetPaths.Clear();
            assets.Clear();
            bundles.Clear();
            groups.Clear();
            directories.Clear();

            if (!File.Exists(path))
            {
                return;
            }

            using (var reader = new StreamReader(File.OpenRead(path)))
            {
                string line;
                var parseType = string.Empty;
                while ((line = reader.ReadLine()) != null)
                {
                    if (string.IsNullOrEmpty(line) || line.StartsWith("//") || line.StartsWith("#"))
                    {
                        continue;
                    }

                    if (all_keys.Contains(line))
                    {
                        parseType = line;
                        continue;
                    }

                    switch (parseType)
                    {
                        case key_directories:
                            ReadDirectory(line);
                            break;
                        case key_paths:
                            ReadPath(line);
                            break;
                        case key_assets:
                            ReadAsset(line);
                            break;
                        case key_bundles:
                            ReadBundle(line);
                            break;
                        case key_groups:
                            ReadGroups(line);
                            break;
                    }
                }
            }
        }

        private void ReadDirectory(string line)
        {
            directories.Add(line.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)[1]);
        }

        private void ReadPath(string line)
        {
            var fields = line.Split(',');
            var dir = fields[1].IntValue();
            var file = fields[2];
            if (dir >= 0 && dir < directories.Count)
            {
                allAssetPaths.Add($"{directories[dir]}/{file}");
            }
            else
            {
                allAssetPaths.Add(file);
            }
        }

        private void ReadAsset(string line)
        {
            var asset = new AssetInfo();
            asset.Deserialize(line);
            assets.Add(asset);
            var assetPath = allAssetPaths[asset.id];
            pathWithAssets[assetPath] = asset;
        }

        private void ReadBundle(string line)
        {
            var bundle = new BundleInfo();
            bundle.Deserialize(line);
            nameWithBundles[bundle.name] = bundle;
            bundles.Add(bundle);
        }

        private void ReadGroups(string line)
        {
            var group = new GroupInfo();
            group.Deserialize(line);
            groups.Add(group);
            nameWithGroups.Add(group.name, group);
        }

        public void Save(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            using (var writer = new StreamWriter(File.OpenWrite(path)))
            {
                WriteVersion(writer);
                WriteDirectories(writer);
                WriteAssetPaths(writer);
                WriteAssets(writer);
                WriteBundles(writer);
                WriteGroups(writer);
            }

            SaveVersion(path);
        }

        private void WriteVersion(StreamWriter writer)
        {
            writer.WriteLine(key_version);
            writer.WriteLine(version);
            writer.WriteLine();
        }

        private void WriteDirectories(StreamWriter writer)
        {
            writer.WriteLine(key_directories);
            directories.Clear();
            var directoryWithIDs = new Dictionary<string, int>();
            for (var index = 0; index < allAssetPaths.Count; index++)
            {
                var assetPath = allAssetPaths[index];
                var directoryName = Path.GetDirectoryName(assetPath);
                var file = Path.GetFileName(assetPath);
                var dir = -1;
                if (string.IsNullOrEmpty(directoryName))
                {
                    allAssetPaths[index] = $"{index},{dir},{file}";
                    continue;
                }

                directoryName = directoryName.Replace('\\', '/');
                if (!directoryWithIDs.TryGetValue(directoryName, out dir))
                {
                    dir = directories.Count;
                    directoryWithIDs.Add(directoryName, dir);
                    directories.Add(directoryName);
                    writer.WriteLine($"{dir},{directoryName}");
                }

                allAssetPaths[index] = $"{index},{dir},{file}";
            }

            writer.WriteLine();
        }

        private void WriteAssetPaths(TextWriter writer)
        {
            directories.Clear();
            writer.WriteLine(key_paths);
            foreach (var assetPath in allAssetPaths)
            {
                writer.WriteLine(assetPath);
            }

            writer.WriteLine();
        }

        private void WriteAssets(StreamWriter writer)
        {
            writer.WriteLine(key_assets);
            foreach (var asset in assets)
            {
                writer.WriteLine(asset.Serialize());
            }

            writer.WriteLine();
        }

        private void WriteBundles(StreamWriter writer)
        {
            writer.WriteLine(key_bundles);
            foreach (var bundle in bundles)
            {
                writer.WriteLine(bundle.Serialize());
            }

            writer.WriteLine();
        }

        private void WriteGroups(StreamWriter writer)
        {
            writer.WriteLine(key_groups);
            foreach (var info in groups)
            {
                writer.WriteLine(info.Serialize());
            }
        }

        public static string GetVersionFile(string name)
        {
            return $"{name}.version";
        }

        private void SaveVersion(string path)
        {
            var file = Path.GetFileName(path);
            var outputFolder = Path.GetDirectoryName(path);
            using (var stream = File.OpenRead(path))
            {
                var crc = Utility.ComputeCRC32(stream);
                var versionFile = $"{outputFolder}/{GetVersionFile(file)}";
                if (File.Exists(versionFile))
                {
                    var text = File.ReadAllText(versionFile);
                    var fields = text.Split(',');
                    var lastCRC = fields[2].UIntValue();
                    if (lastCRC.Equals(crc))
                    {
                        Logger.I("Version not changed.");
                    }

                    File.Delete(versionFile);
                }

                var content = $"{version},{stream.Length},{crc}";
                File.WriteAllText(versionFile, content);
            }
        }

        public BundleInfo GetBundle(string assetBundleName)
        {
            nameWithBundles.TryGetValue(assetBundleName, out var bundle);
            return bundle;
        }

        public IEnumerable<BundleInfo> GetBundlesWithGroups(params string[] groupNames)
        {
            if (groupNames == null || groupNames.Length == 0)
            {
                return bundles.ToArray();
            }

            var set = new HashSet<BundleInfo>();
            foreach (var groupName in groupNames)
            {
                if (nameWithGroups.TryGetValue(groupName, out var groupInfo))
                {
                    foreach (var item in groupInfo.bundles)
                    {
                        var bundle = GetBundle(item);
                        if (bundle != null)
                        {
                            set.Add(bundle);
                        }
                    }
                }
                else
                {
                    Logger.W("Unable to get bundles from group {0}", groupName);
                }
            }

            return set;
        }

        public BundleInfo GetBundle(int bundleId)
        {
            if (bundleId >= 0 && bundleId < bundles.Count)
            {
                return bundles[bundleId];
            }
            return null;
        }

        public AssetInfo GetAsset(string path)
        {
            pathWithAssets.TryGetValue(path, out var asset);
            return asset;
        }

        public void SetAllAssetPaths(IEnumerable<string> assetPaths)
        {
            allAssetPaths.Clear();
            allAssetPaths.AddRange(assetPaths);
        }

        public void Remapping()
        {
            for (var index = 0; index < allAssetPaths.Count; index++)
            {
                var assetPath = allAssetPaths[index];
                pathWithAssets[assetPath] = assets[index];
            }
        }

        public BundleInfo[] GetBundles(AssetInfo info)
        {
            return Array.ConvertAll(info.bundles, GetBundle);
        }
    }
}