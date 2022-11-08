using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace VEngine.Editor
{
    [Serializable]
    public class GroupBuild
    {
        public string[] bundles = new string[0];
        public string name;
        public bool includeInBuild;
    }

    [Serializable]
    public class AssetBuild
    {
        public string path;
        public string bundle;
        public long time;
        public long metaTime;
        public string[] bundles = new string[0];
        public string metaPath;
        public int id { get; set; }
        public string[] dependencies = new string[0];

        public bool dirty => time != Settings.GetLastWriteTime(path) || metaTime != Settings.GetLastWriteTime(metaPath);

        public AssetInfo GetInfo(Dictionary<string, BundleBuild> buildBundles)
        {
            var mainId = -1;
            if (buildBundles.TryGetValue(bundle, out var main))
            {
                mainId = main.id;
            }
            else
            {
                Logger.E("Bundle not found {0} with {1}.", bundle, path);
            }
            var ids = new List<int>();
            foreach (var item in bundles)
            {
                if (!buildBundles.TryGetValue(item, out var dep))
                {
                    Logger.E("Bundle not found {0} with {1}.", item, path);
                    continue;
                }
                ids.Add(dep.id);
            }
            return new AssetInfo
            {
                id = id,
                bundle = mainId,
                bundles = ids.ToArray()
            };
        }

        public void AfterBuild()
        {
            time = Settings.GetLastWriteTime(path);
            metaTime = Settings.GetLastWriteTime(metaPath);
        }
    }

    [Serializable]
    public class BundleBuild
    {
        public string name;
        public List<AssetBuild> assets = new List<AssetBuild>();
        public long time;
        public ulong size;
        public uint crc;
        public string nameWithAppendHash;
        public int id;

        public BundleInfo GetInfo()
        {
            var ids = new int[assets.Count];
            for (var index = 0; index < assets.Count; index++)
            {
                var asset = assets[index];
                ids[index] = asset.id;
            }
            var info = new BundleInfo
            {
                id = id,
                assets = ids,
                name = nameWithAppendHash,
                crc = crc,
                size = size
            };
            return info;
        }

        public void AfterBuild()
        {
            var file = Settings.GetBuildPath(nameWithAppendHash);
            if (File.Exists(file))
            {
                time = Settings.GetLastWriteTime(file);
                using (var stream = File.OpenRead(file))
                {
                    size = (ulong)stream.Length;
                    crc = Utility.ComputeCRC32(stream);
                }
            }

            foreach (var asset in assets)
            {
                asset.AfterBuild();
            }
        }

        public AssetBundleBuild GetBuild()
        {
            var build = new AssetBundleBuild
            {
                assetBundleName = name,
                assetNames = Array.ConvertAll(assets.ToArray(), input => input.path)
            };
            return build;
        }

        public void GetDependencies(Dictionary<string, string> assetWithBundles)
        {
            // Generate bundles for each entry
            foreach (var asset in assets)
            {
                var bundles = new HashSet<string>();
                foreach (var dependency in asset.dependencies)
                {
                    string bundle;
                    if (assetWithBundles.TryGetValue(dependency, out bundle))
                    {
                        bundles.Add(bundle);
                    }
                }
                asset.bundles = bundles.ToArray();
            }
        }
    }

    /// <summary>
    ///     打包后的缓存数据
    /// </summary>
    public class Build : ScriptableObject
    {
        public List<BundleBuild> bundles = new List<BundleBuild>();
        public List<GroupBuild> groups = new List<GroupBuild>();
        public int version;

        public Dictionary<string, BundleBuild> GetBundles()
        {
            var dictionary = new Dictionary<string, BundleBuild>();
            foreach (var bundle in bundles)
            {
                dictionary[bundle.name] = bundle;
            }
            return dictionary;
        }

        public Dictionary<string, AssetBuild> GetAssets()
        {
            var dictionary = new Dictionary<string, AssetBuild>();
            foreach (var bundle in bundles)
            {
                foreach (var asset in bundle.assets)
                {
                    dictionary[asset.path] = asset;
                }
            }
            return dictionary;
        }

        public Dictionary<string, GroupBuild> GetGroups()
        {
            var dictionary = new Dictionary<string, GroupBuild>();
            foreach (var group in groups)
            {
                dictionary[group.name] = group;
            }
            return dictionary;
        }

        public void Clear()
        {
            groups.Clear();
            bundles.Clear();
            version = 0;
        }

        public void CreateManifest(bool includeInBuild)
        {
            var assetNames = new List<string>();
            var manifest = new VEngine.Manifest();
            var buildBundles = GetBundles();
            var filename = $"{name}".ToLower();
            var savePath = Settings.GetBuildPath(filename);
            var bundlesInBuild = new List<BundleBuild>();
            if (includeInBuild)
            {
                savePath = $"{EditorUtility.BuildPlayerDataPath}/{filename}";
                GetBundlesInBuild(buildBundles, bundlesInBuild);
            }
            else
            {
                foreach (var bundle in bundles)
                {
                    bundlesInBuild.Add(bundle);
                }
            }

            buildBundles = new Dictionary<string, BundleBuild>();
            var assetsInBuild = new List<AssetBuild>();
            for (var index = 0; index < bundlesInBuild.Count; index++)
            {
                var bundle = bundlesInBuild[index];
                bundle.id = index;
                buildBundles[bundle.name] = bundle;
                foreach (var asset in bundle.assets)
                {
                    asset.id = assetsInBuild.Count;
                    assetsInBuild.Add(asset);
                    assetNames.Add(asset.path);
                }
            }

            if (!includeInBuild)
            {
                manifest.groups = groups.ConvertAll(ConverterGroup(buildBundles));
            }

            manifest.assets = assetsInBuild.ConvertAll(input => input.GetInfo(buildBundles));
            manifest.bundles = bundlesInBuild.ConvertAll(input => input.GetInfo());
            manifest.SetAllAssetPaths(assetNames.ToArray());
            manifest.version = version;
            manifest.Save(savePath);
        }

        private static Converter<GroupBuild, GroupInfo> ConverterGroup(Dictionary<string, BundleBuild> buildBundles)
        {
            return input =>
            {
                var groupInfo = new GroupInfo
                {
                    name = input.name,
                    bundles = Array.ConvertAll(input.bundles, s =>
                    {
                        if (buildBundles.TryGetValue(s, out var value))
                        {
                            return value.id;
                        }
                        Logger.W("Bundle {0} not find", s);
                        return -1;
                    })
                };
                return groupInfo;
            };
        }

        private void GetBundlesInBuild(Dictionary<string, BundleBuild> buildBundles, List<BundleBuild> bundlesInBuild)
        {
            foreach (var groupBuild in groups)
            {
                if (groupBuild.includeInBuild)
                {
                    foreach (var item in groupBuild.bundles)
                    {
                        if (buildBundles.TryGetValue(item, out var bundle) && !bundlesInBuild.Contains(bundle))
                        {
                            var destFile = Path.Combine(EditorUtility.BuildPlayerDataPath, bundle.nameWithAppendHash);
                            var srcFile = Settings.GetBuildPath(bundle.nameWithAppendHash);
                            var dir = Path.GetDirectoryName(destFile);
                            if (!Directory.Exists(dir) && !string.IsNullOrEmpty(dir))
                            {
                                Directory.CreateDirectory(dir);
                            }
                            File.Copy(srcFile, destFile, true);
                            bundlesInBuild.Add(bundle);
                        }
                    }
                }
            }
        }
    }
}