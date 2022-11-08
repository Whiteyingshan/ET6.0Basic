using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;

namespace VEngine.Editor
{
    /// <summary>
    ///     BuildScript 类，实现了具体的打包逻辑
    /// </summary>
    public static class BuildScript
    {
        /// <summary>
        ///     构建资源
        /// </summary>
        public static void BuildBundles()
        {
            var settings = Settings.GetDefaultSettings();
            var manifests = new List<string>();
            foreach (var manifest in settings.manifests)
            {
                manifests.Add(AssetDatabase.GetAssetPath(manifest));
            }
            foreach (var manifest in manifests)
            {
                var asset = EditorUtility.GetOrCreateAsset<Manifest>(manifest);
                Logger.T(() => BuildBundles(asset), $"Build Bundles for {asset.name}");
            }
        }

        public static void BuildBundles(Manifest manifest)
        {
            var builds = manifest.BuildGroups(out var bundleBuilds, out var rawBundleBuilds, out var assets);
            UnityEditor.EditorUtility.SetDirty(manifest);
            if (builds.Length > 0)
            {
                var assetPath = AssetDatabase.GetAssetPath(manifest);
                var platform = EditorUserBuildSettings.activeBuildTarget;
                var outputFolder = EditorUtility.PlatformBuildPath;
                var bundleOptions = manifest.settings.buildAssetBundleOptions |
                                    BuildAssetBundleOptions.AppendHashToAssetBundleName;
                var assetBundleManifest =
                    BuildPipeline.BuildAssetBundles(outputFolder, builds, bundleOptions, platform);
                // 重新获取之前的版本文件，因为打包后，之前的内存数据会被 Unity 清空
                manifest = EditorUtility.GetOrCreateAsset<Manifest>(assetPath);
                if (assetBundleManifest == null)
                {
                    Logger.E("Failed to build {0} with bundles, because assetBundleManifest == null.", manifest.name);
                    return;
                }
                manifest.CreateVersions(assetBundleManifest, bundleBuilds, assets);
            }
            else
            {
                if (rawBundleBuilds.Count > 0 || manifest.GetBuild().GetBundles().Count != bundleBuilds.Count)
                {
                    manifest.CreateVersions(null, bundleBuilds, assets);
                }
                else
                {
                    Logger.I("Nothing to build for {0}.", manifest.name);
                }
            }
        }

        /// <summary>
        ///     构建自动分组
        /// </summary>
        public static void BuildGroups()
        {
            var settings = Settings.GetDefaultSettings();
            foreach (var manifest in settings.manifests)
            {
                Logger.T(() => manifest.BuildGroups(out _, out _, out _), $"Build Groups for {manifest.name}");
            }
        }

        public static string GetTimeForNow()
        {
            return DateTime.Now.ToString("yyyyMMdd-HHmmss");
        }

        /// <summary>
        ///     获取打包播放器的目标名字
        /// </summary>
        /// <param name="target"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        private static string GetBuildTargetName(BuildTarget target, int version)
        {
            var productName = PlayerSettings.productName + "-v" + PlayerSettings.bundleVersion + ".";
            var targetName = string.Format("/{0}{1}-{2}", productName, version, GetTimeForNow());
            switch (target)
            {
                case BuildTarget.Android:
                    return targetName + ".apk";
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                    return targetName + ".exe";
                case BuildTarget.StandaloneOSX:
                    return targetName + ".app";
                default:
                    return targetName;
            }
        }

        /// <summary>
        ///     打包播放器
        /// </summary>
        public static void BuildPlayer()
        {
            var settings = Settings.GetDefaultSettings();
            if (settings.startupScene == null)
            {
                UnityEditor.EditorUtility.DisplayDialog(Constants.TIPS_TITLE_BUILD_PLAYER, Constants.TIPS_CONTENT_BUILD_PLAYER, Constants.TIPS_OK);
                return;
            }

            var path = Path.Combine(Environment.CurrentDirectory, "Build");
            if (path.Length == 0)
            {
                return;
            }

            var levels = settings.scenesInBuild;
            if (levels.Length == 0)
            {
                Logger.I("Nothing to build.");
                return;
            }

            var buildTarget = EditorUserBuildSettings.activeBuildTarget;
            var buildTargetName = GetBuildTargetName(buildTarget, settings.version);
            if (buildTargetName == null)
            {
                return;
            }

            var buildPlayerOptions = new BuildPlayerOptions
            {
                scenes = levels,
                locationPathName = path + buildTargetName,
                target = buildTarget,
                options = EditorUserBuildSettings.development ? BuildOptions.Development : BuildOptions.None
            };
            BuildPipeline.BuildPlayer(buildPlayerOptions);
        }

        public static void Clear()
        {
            Settings.GetDefaultSettings().Clear();
        }

        public static void CopyToStreamingAssets()
        {
            Settings.GetDefaultSettings().CopyToStreamingAssets();
        }

        public static void BuildManifests()
        {
            var settings = Settings.GetDefaultSettings();
            foreach (var manifest in settings.manifests)
            {
                manifest.CreateBuild();
            }
        }

        public static void ClearHistory()
        {
            var settings = Settings.GetDefaultSettings();
            var usedFiles = new List<string>();
            usedFiles.Add(EditorUtility.GetPlatformName());
            usedFiles.Add(EditorUtility.GetPlatformName() + ".manifest");
            foreach (var manifest in settings.manifests)
            {
                var build = manifest.GetBuild();
                usedFiles.Add(manifest.name + ".json");
                usedFiles.Add(manifest.name.ToLower());
                usedFiles.Add(VEngine.Manifest.GetVersionFile(manifest.name.ToLower()));
                foreach (var bundle in build.bundles)
                {
                    usedFiles.Add(bundle.nameWithAppendHash);
                    usedFiles.Add(bundle.name + ".manifest");
                }
            }

            var files = Directory.GetFiles(EditorUtility.PlatformBuildPath);
            foreach (var file in files)
            {
                var name = Path.GetFileName(file);
                if (usedFiles.Contains(name))
                {
                    continue;
                }
                File.Delete(file);
                Logger.I("Delete {0}", file);
            }
        }
    }
}