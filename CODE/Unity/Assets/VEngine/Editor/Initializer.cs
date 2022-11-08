using System;
using System.IO;
using UnityEngine;
using Object = UnityEngine.Object;

namespace VEngine.Editor
{
    /// <summary>
    ///     初始化类，提供了编辑器的初始化操作
    /// </summary>
    public static class Initializer
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            var settings = Settings.GetDefaultSettings();
            Versions.DownloadDataPath = Path.Combine(Application.persistentDataPath, Utility.buildPath);
            Versions.PlatformName = EditorUtility.GetPlatformName();
            switch (settings.scriptPlayMode)
            {
                case ScriptPlayMode.Simulation:
                    Versions.FuncCreateAsset = EditorAsset.Create;
                    Versions.FuncCreateScene = EditorScene.Create;
                    Versions.FuncCreateManifest = EditorManifestFile.Create;
                    DisableAutoUpdate();
                    break;
                case ScriptPlayMode.Preload:
                    Versions.PlayerDataPath = Path.Combine(Environment.CurrentDirectory, EditorUtility.PlatformBuildPath);
                    DisableAutoUpdate();
                    break;
                case ScriptPlayMode.Incremental:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static void DisableAutoUpdate()
        {
            var startup = Object.FindObjectOfType<Startup>();
            if (startup != null)
            {
                foreach (var manifest in startup.manifests)
                {
                    manifest.autoUpdate = false;
                }
            }
        }
    }
}