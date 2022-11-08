using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace VEngine
{
    /// <summary>
    ///     Versions 类，持有包内和包外的版本信息，并提供版本内容的 初始化，更新，检查，下载等接口。
    /// </summary>
    public static class Versions
    {
        /// <summary>
        ///     运行时 API 的版本
        /// </summary>
        public const string APIVersion = "6.1.1";

        /// <summary>
        ///     运行时的清单文件，服务器的
        /// </summary>
        public static readonly List<Manifest> Manifests = new List<Manifest>();

        /// <summary>
        ///     按路径缓存的清单记录
        /// </summary>
        private static readonly Dictionary<string, Manifest> PathWithManifests = new Dictionary<string, Manifest>();

        /// <summary>
        ///     bundle 的加载地址缓存，可以优化 gc
        /// </summary>
        internal static readonly Dictionary<string, string> BundleWithPathOrUrLs = new Dictionary<string, string>();

        /// <summary>
        ///     短连接缓存
        /// </summary>
        private static readonly Dictionary<string, string> nameWithPaths = new Dictionary<string, string>();

        public static Func<string, Type, Asset> FuncCreateAsset { get; set; }
        public static Func<string, bool, Scene> FuncCreateScene { get; set; }
        public static Func<string, bool, ManifestFile> FuncCreateManifest { get; set; }

        public static Asset CreateAsset(string path, Type type)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException(nameof(path));
            }
            GetActualPath(ref path);
            return FuncCreateAsset(path, type);
        }
        
        public static Scene CreateScene(string path, bool additive)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException(nameof(path));
            }
            GetActualPath(ref path);
            return FuncCreateScene(path, additive);
        }
        
        public static ManifestFile CreateManifest(string name, bool builtin)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException(nameof(name));
            }
            return FuncCreateManifest(name.ToLower(), builtin);
        } 
        
        /// <summary>
        ///     清单版本
        /// </summary>
        public static string ManifestsVersion
        {
            get
            {
                var sb = new StringBuilder();
                for (var index = 0; index < Manifests.Count; index++)
                {
                    var manifest = Manifests[index];
                    sb.AppendFormat("{0}:{1}", manifest.name, manifest.version);
                    if (index < Manifests.Count - 1) sb.Append(", ");
                }

                return sb.ToString();
            }
        }


        /// <summary>
        ///     所有场景的名字
        /// </summary>
        public static string[] scenes
        {
            get
            {
                var set = new HashSet<string>();
                foreach (var item in PathWithManifests)
                foreach (var scene in item.Value.AllAssetPaths)
                    if (scene.EndsWith(".unity"))
                        set.Add(scene);
                return set.ToArray();
            }
        }

        /// <summary>
        ///     包体内的资源目录
        /// </summary>
        public static string PlayerDataPath { get; set; }

        /// <summary>
        ///     资源下载路径
        /// </summary>
        public static string DownloadURL { get; set; }

        /// <summary>
        ///     资源下载后保存的数据目录
        /// </summary>
        public static string DownloadDataPath { get; set; }

        /// <summary>
        ///     使用 UnityWebRequest 加载本地文件的时候的协议，在不同平台有不同的定义
        /// </summary>
        internal static string LocalProtocol { get; set; }

        public static string PlatformName { get; set; }

        public static Func<string, bool> addressableByName { get; set; }


        public static void Load(string path, Manifest manifest)
        {
            if (PathWithManifests.TryGetValue(path, out var value))
            {
                PathWithManifests[path] = manifest;
                Manifests[value.id] = manifest;
                return;
            }

            manifest.name = Path.GetFileName(path);
            manifest.id = Manifests.Count;
            Manifests.Add(manifest);
            PathWithManifests.Add(path, manifest); 
            
            // 实现 addressableByName 为资源自动生成短连接映射
            if (addressableByName != null)
                foreach (var assetPath in manifest.allAssetPaths)
                    if (AddressableByName(assetPath))
                    {
                        var assetName = Path.GetFileNameWithoutExtension(assetPath);
                        if (!nameWithPaths.TryGetValue(assetName, out var address))
                        {
                            nameWithPaths[assetName] = assetPath;
                            Logger.I("Addressable Asset {0} by {1}",assetPath, assetName);
                        }
                        else
                            // 有名字冲突
                            Logger.W($"{assetName} already exist {address}");
                    }
        }

        public static void GetActualPath(ref string path)
        {
            if (nameWithPaths.TryGetValue(path, out var value))
            {
                path = value;
            }
        }

        private static bool AddressableByName(string assetPath)
        {
            return addressableByName != null && addressableByName(assetPath);
        }

        /// <summary>
        ///     获取指定的 path 相对 <see cref="DownloadDataPath" /> 的完整路径
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string GetDownloadDataPath(string file)
        {
            return $"{DownloadDataPath}/{file}";
        }

        /// <summary>
        ///     获取指定的 path 相对 <see cref="DownloadDataPath" /> 的完整路径 由系统路径拼接
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string GetDownloadDataSystemPath(string file)
        {
            return Path.Combine(DownloadDataPath, file);
        }

        /// <summary>
        ///     获取 path 相对包体目录的用来给 UnityWebRequest 使用的 url
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string GetPlayerDataURL(string file)
        {
            return $"{LocalProtocol}{PlayerDataPath}/{file}";
        }

        /// <summary>
        ///     获取文件相对包体目录的路径
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string GetPlayerDataPath(string file)
        {
            return $"{PlayerDataPath}/{file}";
        }

        /// <summary>
        ///     获取 path 对应的 的下载路径
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string GetDownloadURL(string file)
        {
            return $"{DownloadURL}{PlatformName}/{file}";
        }

        /// <summary>
        ///     获取临时路径
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string GetTemporaryPath(string file)
        {
            var ret = $"{Application.temporaryCachePath}/{file}";
            var dir = Path.GetDirectoryName(ret);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir)) Directory.CreateDirectory(dir);

            return ret;
        }

        /// <summary>
        ///     清理版本内容，目前只有测试的时候用到
        /// </summary>
        public static void ClearDownloadData()
        {
            if (Directory.Exists(DownloadDataPath))
            {
                Directory.Delete(DownloadDataPath, true);
                Directory.CreateDirectory(DownloadDataPath);
            }

            BundleWithPathOrUrLs.Clear();
        }

        /// <summary>
        ///     清理旧的版本内容，下载目录不在版本中的文件将被删除。
        /// </summary>
        /// <returns></returns>
        public static ClearVersions ClearAsync()
        {
            var clearAsync = new ClearVersions();
            clearAsync.Start();
            return clearAsync;
        }

        /// <summary>
        ///     运行时自动初始化，无需主动调用
        /// </summary>
        public static void InitializeOnLoad()
        {
            if (FuncCreateAsset == null) FuncCreateAsset = BundledAsset.Create;
            if (FuncCreateScene == null) FuncCreateScene = BundledScene.Create;
            if (FuncCreateManifest == null) FuncCreateManifest = ManifestFile.Create;

            if (Application.platform != RuntimePlatform.OSXEditor &&
                Application.platform != RuntimePlatform.OSXPlayer &&
                Application.platform != RuntimePlatform.IPhonePlayer)
            {
                if (Application.platform == RuntimePlatform.WindowsEditor ||
                    Application.platform == RuntimePlatform.WindowsPlayer)
                    LocalProtocol = "file:///";
                else
                    LocalProtocol = string.Empty;
            }
            else
            {
                LocalProtocol = "file://";
            }

            if (string.IsNullOrEmpty(PlatformName)) PlatformName = Utility.GetPlatformName();

            // 包体内的资源路径
            if (string.IsNullOrEmpty(PlayerDataPath))
                PlayerDataPath = $"{Application.streamingAssetsPath}/{Utility.buildPath}";

            // 更新下载路径
            if (string.IsNullOrEmpty(DownloadDataPath))
                DownloadDataPath = $"{Application.persistentDataPath}/{Utility.buildPath}";

            if (!Directory.Exists(DownloadDataPath)) Directory.CreateDirectory(DownloadDataPath);

            // 更新器初始化
            Updater.Initialize(Download.UpdateDownloads, Loadable.UpdateLoadables, Operation.UpdateOperations);
        }

        /// <summary>
        ///     加载包体的清单文件
        /// </summary>
        /// <returns></returns>
        public static InitializeVersions InitializeAsync(params ManifestInfo[] manifests)
        {
            InitializeOnLoad();
            var operation = new InitializeVersions
            {
                manifests = manifests
            };
            operation.Start();
            return operation;
        }


        /// <summary>
        ///     更新版本的异步操作，可以通过协程或者 completed 事件等待更新的结果返回。
        /// </summary>
        /// <param name="manifests"></param>
        /// <returns></returns>
        public static UpdateVersions UpdateAsync(params string[] manifests)
        {
            var operation = new UpdateVersions
            {
                manifests = manifests
            };
            operation.Start();
            return operation;
        }

        /// <summary>
        ///     根据自定义分组检查更新，支持多个分组，不传默认检查所有文件的更新状态。此方法可以通过协程返回结果。
        /// </summary>
        /// <param name="groupNames">需要检查的分组名字</param>
        /// <returns>检查版本的对象，可以获取检查的进度和完成状态以及结果</returns>
        public static GetDownloadSize GetDownloadSizeAsync(params string[] groupNames)
        {
            var check = new GetDownloadSize
            {
                groupNames = groupNames
            };
            check.Start();
            return check;
        }

        /// <summary>
        ///     批量下载一组资源
        /// </summary>
        /// <param name="groups"></param>
        /// <returns></returns>
        public static DownloadVersions DownloadAsync(DownloadInfo[] groups)
        {
            var download = new DownloadVersions
            {
                groups = groups
            };
            download.Start();
            return download;
        }

        internal static void SetBundlePathOrURl(string assetBundleName, string url)
        {
            BundleWithPathOrUrLs[assetBundleName] = url;
        }

        internal static string GetBundlePathOrURL(string assetBundleName)
        {
            // 看缓存中是否有
            if (BundleWithPathOrUrLs.TryGetValue(assetBundleName, out var path)) return path;

            foreach (var manifest in Manifests)
                if (manifest.GetBundle(assetBundleName) != null)
                {
                    if (manifest.includeInBuild)
                    {
                        path = GetPlayerDataPath(assetBundleName);
                        BundleWithPathOrUrLs[assetBundleName] = path;
                        return path;
                    }

                    // TODO:更安全的做法是这里搞一次校验
                    // 先判断下载目录是否存在
                    path = GetDownloadDataPath(assetBundleName);
                    if (File.Exists(path))
                    {
                        BundleWithPathOrUrLs[assetBundleName] = path;
                        return path;
                    }
                }

            // 本地没有 去服务器找
            path = GetDownloadURL(assetBundleName);
            BundleWithPathOrUrLs[assetBundleName] = path;
            return path;
        }

        public static AssetInfo GetAsset(string path)
        {  
            foreach (var item in PathWithManifests)
            {
                var asset = item.Value.GetAsset(path);
                if (asset != null) return asset;
            }

            return null;
        }

        public static bool GetDependencies(string assetPath, out BundleInfo bundle, out BundleInfo[] bundles)
        {
            // 逆向寻址，先去包外找
            for (var index = Manifests.Count - 1; index >= 0; index--)
            {
                var manifest = Manifests[index];
                var asset = manifest.GetAsset(assetPath);
                if (asset != null)
                {
                    bundle = manifest.GetBundle(asset.bundle);
                    bundles = manifest.GetBundles(asset);
                    return true;
                }
            }

            bundle = null;
            bundles = null;
            return false;
        }

        public static List<BundleInfo> GetBundlesWithGroups(string[] groupNames)
        {
            var bundles = new List<BundleInfo>();
            var bundlesInBuild = new List<string>();
            foreach (var manifest in Manifests)
            {
                if (manifest.includeInBuild)
                {
                    bundlesInBuild.AddRange(manifest.bundles.ConvertAll(b => b.name));
                    continue;
                }

                var updateBundles = manifest.GetBundlesWithGroups(groupNames);
                foreach (var bundle in updateBundles)
                {
                    if (bundlesInBuild.Contains(bundle.name)) continue;
                    bundles.Add(bundle);
                }
            }

            return bundles;
        }

        /// <summary>
        ///     获取所有资源路径
        /// </summary>
        /// <returns></returns>
        public static string[] GetAllAssetPaths()
        {
            var set = new HashSet<string>();
            foreach (var manifest in Manifests) set.UnionWith(manifest.AllAssetPaths);
            return set.ToArray();
        }
    }
}