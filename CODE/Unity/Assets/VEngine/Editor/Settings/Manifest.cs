using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace VEngine.Editor
{
    public class Manifest : ScriptableObject
    {
        /// <summary>
        ///     版本中的所有自定义分组
        /// </summary>
        [Tooltip("所有自定分组")] public List<Group> groups = new List<Group>();

        public ulong size;
        public Settings settings;
        private readonly List<Group> rawGroups = new List<Group>();

        public Dictionary<string, Asset> GetAssets()
        {
            var assets = new Dictionary<string, Asset>();
            foreach (var group in groups)
            {
                foreach (var asset in group.assets)
                {
                    if (!assets.TryGetValue(asset.path, out var value))
                    {
                        assets[asset.path] = asset;
                    }
                    else
                    {
                        Logger.W("can't add {0} with {1} already exist in {2}", asset.path, group.name,
                            value.parentGroup.name);
                    }
                }
            }
            return assets;
        }

        /// <summary>
        ///     移除分组
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public void RemoveGroup(Group group)
        {
            group.assets.Clear();
            groups.Remove(group);
            var path = AssetDatabase.GetAssetPath(group);
            AssetDatabase.DeleteAsset(path);
        }

        public Group GetOrCreateGroup(string groupName)
        {
            var assetGroup = groups.Find(group => group.name == groupName);
            if (assetGroup == null)
            {
                assetGroup = AddGroup(groupName);
            }
            return assetGroup;
        }

        /// <summary>
        ///     添加分组
        /// </summary>
        /// <param name="groupType"></param>
        /// <param name="raw"></param>
        /// <returns></returns>
        public Group AddGroup(string groupType, bool raw = false)
        {
            var dir = Settings.GetGroupsDataPath(name);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            var path = Settings.GetGroupsDataPath($"{name}/{groupType}.asset");
            if (File.Exists(path))
            {
                var findAssets = AssetDatabase.FindAssets($"t:Group {groupType}", new[]
                {
                    dir
                });
                path = Settings.GetGroupsDataPath($"{name}/{groupType}{findAssets.Length}.asset");
            }

            var assetGroup = EditorUtility.GetOrCreateAsset<Group>(path);
            assetGroup.manifest = this;
            assetGroup.bundleMode = raw ? BundleMode.PackByRaw : BundleMode.PackByEntry;
            groups.Add(assetGroup);
            return assetGroup;
        }

        /// <summary>
        ///     添加一个要打包的 entry 到设置中
        /// </summary>
        /// <param name="file"></param>
        /// <param name="group"></param>
        /// <param name="label"></param>
        /// ///
        /// <returns></returns>
        public Asset AddAsset(string file, Group group, string label = null)
        {
            var asset = Asset.Create(file, group, label);
            group.assets.Add(asset);
            return asset;
        }

        /// <summary>
        ///     从设置中移除
        /// </summary>
        /// <param name="asset"></param>
        public void RemoveAsset(Asset asset)
        {
            asset.parentGroup.assets.Remove(asset);
        }

        public Build GetBuild()
        {
            var build = CreateInstance<Build>();
            var path = $"{EditorUtility.PlatformBuildPath}/{name}.json";
            if (File.Exists(path))
            {
                var json = File.ReadAllText(path);
                EditorJsonUtility.FromJsonOverwrite(json, build);
            }
            return build;
        }

        public IEnumerable<BundleBuild> CreateBundles(List<Asset> assets, out List<BundleBuild> bundleBuilds)
        {
            var bundlesToBuild = new HashSet<BundleBuild>();
            bundleBuilds = new List<BundleBuild>();

            var nameWithBundles = new Dictionary<string, BundleBuild>();
            var assetWithBundles = new Dictionary<string, string>();
            foreach (var asset in assets)
            {
                if (asset.isFolder)
                {
                    continue;
                }
                asset.bundle = asset.PackWithBundleMode();
                if (!assetWithBundles.TryGetValue(asset.path, out var bundleName))
                {
                    assetWithBundles[asset.path] = asset.bundle;
                    if (!nameWithBundles.TryGetValue(asset.bundle, out var bundle))
                    {
                        bundle = new BundleBuild
                        {
                            name = asset.bundle
                        };
                        bundleBuilds.Add(bundle);
                        nameWithBundles.Add(bundle.name, bundle);
                    }
                    bundle.assets.Add(new AssetBuild
                    {
                        path = asset.path,
                        metaPath = asset.metaPath,
                        dependencies = asset.dependencies,
                        bundle = asset.bundle
                    });
                }
                else
                {
                    Logger.W("Asset {0} already exist with bundle {1} with newGroup {2}.", asset.path, bundleName,
                        asset.parentGroup.name);
                }
            }
            var build = GetBuild();
            var bundles = build.GetBundles();
            foreach (var bundle in bundleBuilds)
            {
                bundle.GetDependencies(assetWithBundles);
                //单纯删除 AB 中的文件，AB 也要 Rebuild，后面看情况可以不用 Rebuild。
                if (bundles.TryGetValue(bundle.name, out var value) && bundle.assets.Count != value.assets.Count)
                {
                    bundlesToBuild.Add(bundle);
                }
            }
            Logger.I("CreateBundles {0}", bundleBuilds.Count);
            var buildAssets = build.GetAssets();
            // 根据内容有变化的 生成 builds
            // TODO: 这里只把修改的内容传给 Unity 的构建管线，对性能的提升有待测试
            foreach (var asset in assets)
            {
                if (!buildAssets.TryGetValue(asset.path, out var buildAsset) ||
                    buildAsset.dirty ||
                    buildAsset.bundle != asset.bundle ||
                    Array.Exists(asset.dependencies, path =>
                    {
                        if (buildAssets.TryGetValue(path, out var dep))
                        {
                            return dep.dirty;
                        }
                        return true;
                    }))
                {
                    if (nameWithBundles.TryGetValue(asset.bundle, out var value))
                    {
                        bundlesToBuild.Add(value);
                    }
                    else
                    {
                        Logger.W("Bundle {0} not found for {1}.", asset.bundle, asset.path);
                    }

                    foreach (var bundle in asset.bundles)
                    {
                        if (nameWithBundles.TryGetValue(bundle, out var dep))
                        {
                            bundlesToBuild.Add(dep);
                        }
                        else
                        {
                            Logger.W("Bundle {0} not found for {1}.", bundle, asset.path);
                        }
                    }
                }
            }
            return bundlesToBuild;
        }

        public void AfterBuild(List<BundleBuild> bundles)
        {
            var build = GetBuild();
            build.version++;
            var buildBundles = build.GetBundles();

            size = 0UL;
            foreach (var bundle in bundles)
            {
                if (string.IsNullOrEmpty(bundle.nameWithAppendHash))
                {
                    if (buildBundles.TryGetValue(bundle.name, out var value))
                    {
                        bundle.size = value.size;
                        bundle.crc = value.crc;
                        bundle.nameWithAppendHash = value.nameWithAppendHash;
                    }
                }
                size += bundle.size;
            }
            var pathWithAssets = new Dictionary<string, AssetBuild>();
            foreach (var bundle in bundles)
            {
                bundle.AfterBuild();
                foreach (var asset in bundle.assets)
                {
                    pathWithAssets[asset.path] = asset;
                }
            }
            build.groups = groups.ConvertAll(ConverterGroup(pathWithAssets));
            build.bundles = bundles;
            EditorUtility.SaveAsset(build);
            var json = EditorJsonUtility.ToJson(build);
            File.WriteAllText($"{EditorUtility.PlatformBuildPath}/{name}.json", json);
            File.WriteAllText($"{EditorUtility.PlatformBuildPath}/{name}_v{build.version}.json", json);
        }

        private static Converter<Group, GroupBuild> ConverterGroup(Dictionary<string, AssetBuild> pathWithAssets)
        {
            return input =>
            {
                var group = new GroupBuild();
                group.name = input.name;
                group.includeInBuild = input.includeInBuild;
                var set = new HashSet<string>();
                foreach (var asset in input.assets)
                {
                    if (asset.isFolder)
                    {
                        foreach (var child in asset.GetChildren())
                        {
                            GetBundles(pathWithAssets, child, set);
                        }
                        continue;
                    }
                    GetBundles(pathWithAssets, asset.path, set);
                }
                group.bundles = set.ToArray();
                return group;
            };
        }

        private static void GetBundles(Dictionary<string, AssetBuild> pathWithAssets, string asset, HashSet<string> set)
        {
            if (pathWithAssets.TryGetValue(asset, out var value))
            {
                set.Add(value.bundle);
                foreach (var bundle in value.bundles)
                {
                    set.Add(bundle);
                }
            }
        }

        public void CreateBuild(bool includeInBuild = false)
        {
            var build = GetBuild();
            build.name = name;
            build.CreateManifest(includeInBuild);
        }

        public void Save(bool clear = false)
        {
            for (var index = 0; index < groups.Count; index++)
            {
                var group = groups[index];
                if (group == null)
                {
                    groups.RemoveAt(index);
                    index--;
                    continue;
                }
                UnityEditor.EditorUtility.SetDirty(group);
            }
            if (clear)
            {
                size = 0;
                var build = GetBuild();
                build.Clear();
            }

            EditorUtility.SaveAsset(this);
        }

        /// <summary>
        ///     处理依赖。
        /// </summary>
        public Dictionary<string, List<Asset>> AnalysisDependencies(List<Asset> assets,
            Dictionary<string, Asset> pathWithAssets)
        {
            var dependencyWithAssets = new Dictionary<string, List<Asset>>();
            foreach (var asset in assets)
            {
                var dependencies = asset.GetDependencies();
                for (int index = 0, max = dependencies.Length; index < dependencies.Length; index++)
                {
                    var dependency = dependencies[index];

                    if (!dependency.StartsWith("Assets"))
                    {
                        continue;
                    }
                    EditorUtility.DisplayProgressBar("Analysis dependencies...", asset.path, index, max);
                    // 子节点以外的没有主动打包的依赖
                    if (!pathWithAssets.ContainsKey(dependency))
                    {
                        List<Asset> value;
                        if (!dependencyWithAssets.TryGetValue(dependency, out value))
                        {
                            value = new List<Asset>();
                            dependencyWithAssets.Add(dependency, value);
                        }

                        value.Add(asset);
                    }
                }
            }

            // 清理进度条
            EditorUtility.ClearProgressBar();
            return dependencyWithAssets;
        }

        /// <summary>
        ///     对公共依赖进行自动分组
        /// </summary>
        public void AutoGrouping(Dictionary<string, Asset> pathWithAssets,
            Dictionary<string, List<Asset>> dependencyWithAssets, List<Asset> assets)
        {
            if (dependencyWithAssets.Count > 0)
            {
                var auto = GetOrCreateGroup("Auto");
                auto.bundleMode = BundleMode.PackByDirectory;
                auto.manifest = this;
                var dependencies = new List<string>(dependencyWithAssets.Keys);
                dependencies.Sort();
                var builder = new StringBuilder();
                for (int index = 0, max = dependencies.Count; index < max; index++)
                {
                    var path = dependencies[index];
                    EditorUtility.DisplayProgressBar("Auto grouping...", path, index, max);
                    if (dependencyWithAssets.TryGetValue(path, out var value))
                    {
                        // ---- 资源的交叉引用 ----
                        var set = new List<string>();
                        foreach (var item in value)
                        {
                            if (!set.Contains(item.bundle))
                            {
                                set.Add(item.bundle);
                            }
                        }
                        set.Sort();
                        builder.AppendLine(path);
                        foreach (var bundle in set)
                        {
                            builder.AppendLine($" - {bundle}");
                        }
                        // ---- 
                        if (!pathWithAssets.TryGetValue(path, out var asset))
                        {
                            asset = AddAsset(path, auto);
                            asset.bundle = asset.PackWithBundleMode();
                            pathWithAssets[path] = asset;
                            assets.Add(asset);
                        }
                    }
                    else
                    {
                        Logger.I("Dependency not found {0}", path);
                    }
                }

                var file = $"{EditorUtility.PlatformBuildPath}/auto_assets_dependencies_for_{name.ToLower()}.txt";
                File.WriteAllText(file, builder.ToString());
                EditorUtility.ClearProgressBar();
            }
        }

        /// <summary>
        ///     采集规则中的所有资源
        /// </summary>
        public List<Asset> CollectAssets()
        {
            rawGroups.Clear();
            // 临时数据
            var collectAssets = new List<Asset>();
            var pathWithAssets = GetAssets();
            // 采集分组中的资源 
            for (var groupIndex = 0; groupIndex < groups.Count; groupIndex++)
            {
                var group = groups[groupIndex];
                if (group == null)
                {
                    groups.RemoveAt(groupIndex);
                    groupIndex--;
                    continue;
                }

                if (group.bundleMode == BundleMode.PackByRaw)
                {
                    rawGroups.Add(group);
                    continue;
                }

                if (group.name.Contains("Auto"))
                {
                    group.assets.Clear();
                    continue;
                }
                group.manifest = this;

                EditorUtility.DisplayProgressBar("Collect assets...", group.name, groupIndex, groups.Count);
                for (var assetIndex = 0; assetIndex < group.assets.Count; assetIndex++)
                {
                    // 跳过自动分组
                    var asset = group.assets[assetIndex];
                    // 跳过不存在的资源
                    if (!File.Exists(asset.path) && !Directory.Exists(asset.path))
                    {
                        group.assets.RemoveAt(assetIndex);
                        assetIndex--;
                        continue;
                    }
                    // 文件夹不用打包，只采集子文件
                    CollectAsset(asset, collectAssets, group, pathWithAssets);
                }
            }

            // 清理进度条
            EditorUtility.ClearProgressBar();
            return collectAssets;
        }

        public AssetBundleBuild[] BuildGroups(out List<BundleBuild> bundleBuilds, out List<BundleBuild> rawBundleBuilds,
            out List<Asset> assets)
        {
            assets = CollectAssets();
            var pathWithAssets = new Dictionary<string, Asset>();
            foreach (var asset in assets)
            {
                if (!pathWithAssets.TryGetValue(asset.path, out var value))
                {
                    pathWithAssets.Add(asset.path, asset);
                }
                else
                {
                    Logger.W("asset {0} with {1} already exist in group {2}.", asset.path, asset.parentGroup,
                        value.parentGroup);
                }
            }
            var dependencyWithAssets = AnalysisDependencies(assets, pathWithAssets);
            AutoGrouping(pathWithAssets, dependencyWithAssets, assets);
            var bundlesToBuild = CreateBundles(assets, out bundleBuilds);
            var builds = new Dictionary<string, AssetBundleBuild>();
            foreach (var bundle in bundlesToBuild)
            {
                if (!builds.ContainsKey(bundle.name))
                {
                    builds.Add(bundle.name, bundle.GetBuild());
                }
                else
                {
                    Logger.W("Bundle {0} already exist.", bundle.name);
                }
            }
            rawBundleBuilds = BuildRaws(assets, bundleBuilds);
            return builds.Values.ToArray();
        }

        private void CollectAsset(Asset asset, ICollection<Asset> collectAssets, Group parentGroup,
            IDictionary<string, Asset> pathWithAssets)
        {
            if (asset.isFolder)
            {
                foreach (var child in asset.GetChildren())
                {
                    if (!pathWithAssets.TryGetValue(child, out var value))
                    {
                        value = Asset.Create(child, parentGroup, asset.label, asset.path);
                        pathWithAssets.Add(value.path, value);
                    }
                    value.readOnly = true;
                    value.rootPath = asset.path;
                    collectAssets.Add(value);
                }
            }
            else
            {
                if (!pathWithAssets.TryGetValue(asset.path, out var value))
                {
                    // 缓存中没有
                    value = Asset.Create(asset.path, parentGroup, asset.label);
                    pathWithAssets.Add(value.path, value);
                }
                collectAssets.Add(asset);
            }
        }


        /// <summary>
        ///     创建 bundle 名字到 hash 名字之间的映射
        /// </summary>
        /// <param name="assetBundleManifest"></param>
        /// <param name="bundleBuilds"></param>
        /// <param name="assets"></param>
        public void CreateVersions(AssetBundleManifest assetBundleManifest, List<BundleBuild> bundleBuilds,
            List<Asset> assets)
        {
            var nameWithBundles = new Dictionary<string, BundleBuild>();
            foreach (var bundleBuild in bundleBuilds)
            {
                nameWithBundles[bundleBuild.name] = bundleBuild;
            }
            if (assetBundleManifest != null)
            {
                var assetBundles = assetBundleManifest.GetAllAssetBundles();
                foreach (var assetBundle in assetBundles)
                {
                    var pos = assetBundle.LastIndexOf("_", StringComparison.Ordinal) + 1;
                    var hash = assetBundle.Substring(pos);
                    var originBundle = assetBundle.Replace("_" + hash, "");
                    if (nameWithBundles.TryGetValue(originBundle, out var bundle))
                    {
                        bundle.nameWithAppendHash = assetBundle;
                        bundle.AfterBuild();
                    }
                    else
                    {
                        Logger.E("Bundle not exist: {0}", originBundle);
                    }
                }
            }
            AfterBuild(bundleBuilds);
            CreateBuild();
            Save();
        }

        private void BuildRaw(List<Asset> assets, List<BundleBuild> bundles, string path, Asset asset,
            Dictionary<string, BundleBuild> nameWithBundles,
            Dictionary<string, Asset> pathWithAssets)
        {
            using (var stream = File.OpenRead(path))
            {
                var crc = Utility.ComputeCRC32(stream);
                if (!nameWithBundles.TryGetValue(path, out var bundle))
                {
                    bundle = new BundleBuild
                    {
                        name = path
                    };
                    bundles.Add(bundle);
                    nameWithBundles.Add(path, bundle);
                    pathWithAssets.Add(asset.path, asset);
                    assets.Add(asset);
                }

                bundle.size = (ulong)stream.Length;
                bundle.assets.Add(new AssetBuild
                {
                    path = asset.path,
                    metaPath = asset.metaPath,
                    bundle = asset.bundle
                });
                bundle.nameWithAppendHash = $"{name}_raw_{crc}".ToLower();
            }
        }


        public List<BundleBuild> BuildRaws(List<Asset> assets, List<BundleBuild> bundles)
        {
            var nameWithBundles = new Dictionary<string, BundleBuild>();
            foreach (var bundle in bundles)
            {
                nameWithBundles[bundle.name] = bundle;
            }
            var pathWithAssets = new Dictionary<string, Asset>();
            foreach (var asset in assets)
            {
                pathWithAssets[asset.path] = asset;
            }
            var count = bundles.Count;
            var buildRaws = new List<BundleBuild>();

            for (var index = 0; index < rawGroups.Count; index++)
            {
                var group = rawGroups[index];
                EditorUtility.DisplayProgressBar("Collect assets...", group.name, index, groups.Count);
                foreach (var asset in group.assets)
                {
                    if (!asset.isFolder)
                    {
                        if (File.Exists(asset.path))
                        {
                            var path = asset.path;
                            BuildRaw(assets, bundles, path, asset, nameWithBundles, pathWithAssets);
                        }
                    }
                    else
                    {
                        var children = asset.GetChildren();
                        foreach (var child in children)
                        {
                            if (File.Exists(child))
                            {
                                BuildRaw(assets, bundles, child, Asset.Create(child, group), nameWithBundles,
                                    pathWithAssets);
                            }
                        }
                    }
                }
            }

            for (var i = count; i < bundles.Count; i++)
            {
                var bundle = bundles[i];
                var path = Settings.GetBuildPath(bundle.nameWithAppendHash);
                if (File.Exists(path))
                {
                    continue;
                }
                File.Copy(bundle.name, path, true);
                bundle.AfterBuild();
                buildRaws.Add(bundle);
            }
            EditorUtility.ClearProgressBar();
            return buildRaws;
        }
    }
}