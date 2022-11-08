using System.Collections;
using UnityEngine;

namespace VEngine
{
    /// <summary>
    ///     Startup 类，主要用来对 Runtime 进行初始化。
    /// </summary>
    public class Startup : MonoBehaviour
    {
        /// <summary>
        ///     下一个场景
        /// </summary>
        [Tooltip("下一个场景")] public string nextScene = "Assets/Arts/Scenes/Menu.unity";

        /// <summary>
        ///     资源下载地址，指向平台目录的父目录
        /// </summary>
        [Tooltip("资源下载地址，指向平台目录的父目录")] public string downloadURL = "http://127.0.0.1/Bundles/";

        /// <summary>
        ///     初始化的清单配置，可以配置包内和包外的清单，底层会自动按需更新下载
        /// </summary>
        [Tooltip("初始化的清单配置，可以配置包内和包外的清单，底层会自动按需更新下载")]
        public ManifestInfo[] manifests =
        {
            new ManifestInfo
            {
                autoUpdate = true,
                name = "arts"
            }
        };

        // Start is called before the first frame update
        private IEnumerator Start()
        {
            DontDestroyOnLoad(gameObject);
            Versions.DownloadURL = downloadURL;
            var operation = Versions.InitializeAsync(manifests);
            yield return operation;
            if (operation.status == OperationStatus.Failed)
            {
                Logger.E("Failed to initialize Runtime with error: {0}", operation.error);
                yield break;
            }

            Logger.I("Success to initialize Runtime with:");
            Logger.I("API Version: {0}", Versions.APIVersion);
            Logger.I("Manifests Version: {0}", Versions.ManifestsVersion);
            Logger.I("PlayerDataPath: {0}", Versions.PlayerDataPath);
            Logger.I("DownloadDataPath: {0}", Versions.DownloadDataPath);
            Logger.I("DownloadURL: {0}", Versions.DownloadURL);
            Scene.LoadAsync(nextScene, scene => Logger.I("{0}:{1}", scene.status, scene.pathOrURL));
        }
    }
}