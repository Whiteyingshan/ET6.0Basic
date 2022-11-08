using System.IO;
using UnityEngine.Networking;

namespace VEngine
{
    /// <summary>
    ///     包体内的 manifest 文件，使用 UnityWebRequest copy 到包外
    /// </summary>
    public class BuiltinManifestFile : ManifestFile
    {
        private UnityWebRequest request;

        private void DownloadAsync(string url, string savePath)
        {
            if (File.Exists(savePath))
            {
                File.Delete(savePath);
            }
            Logger.I("Download {0} and save to {1}", url, savePath);
            request = UnityWebRequest.Get(url);
            request.downloadHandler = new DownloadHandlerFile(savePath);
            request.SendWebRequest();
        }

        protected override void OnLoad()
        {
            pathOrURL = Versions.GetPlayerDataURL(name);
            var versionFile = Manifest.GetVersionFile(name);
            var url = Versions.GetPlayerDataURL(versionFile);
            DownloadAsync(url, Versions.GetTemporaryPath(versionFile));
            status = LoadableStatus.CheckVersion;
        }

        protected override void OnUpdate()
        {
            switch (status)
            {
                case LoadableStatus.CheckVersion:
                    UpdateVersion();
                    break;

                case LoadableStatus.Downloading:
                    UpdateDownloading();
                    break;

                case LoadableStatus.Loading:
                    var manifest = new Manifest
                    {
                        includeInBuild = true
                    };
                    var path = Versions.GetTemporaryPath(name);
                    manifest.Load(path);
                    Versions.Load(path, manifest);
                    Finish();
                    break;
            }
        }

        private void UpdateDownloading()
        {
            if (request == null)
            {
                Finish("request == nul with " + status);
                return;
            }

            progress = 0.2f + request.downloadProgress;
            if (!request.isDone)
            {
                return;
            }
            if (!string.IsNullOrEmpty(request.error))
            {
                Finish(request.error);
                return;
            }

            request.Dispose();
            request = null;

            status = LoadableStatus.Loading;
        }

        private void UpdateVersion()
        {
            if (request == null)
            {
                Finish("request == null with " + status);
                return;
            }

            progress = 0.2f * request.downloadProgress;
            if (!request.isDone)
            {
                return;
            }
            if (!string.IsNullOrEmpty(request.error))
            {
                Finish(request.error);
                return;
            }
            var versionFile = Manifest.GetVersionFile(name);
            var savePath = Versions.GetTemporaryPath(versionFile);
            if (!File.Exists(savePath))
            {
                Finish("version not exist.");
                return;
            }

            var content = File.ReadAllText(savePath);
            var fields = content.Split(',');
            var version = fields[0].IntValue();
            var crc = fields[2].UIntValue();
            Logger.I("Read {0} with version {1} crc {2}", name, version, crc);
            request.Dispose();
            request = null;

            var path = Versions.GetTemporaryPath(name);
            if (File.Exists(path))
            {
                using (var stream = File.OpenRead(path))
                {
                    if (Utility.ComputeCRC32(stream) == crc)
                    {
                        Logger.I("Skip to download {0}, because nothing to update.", name);
                        status = LoadableStatus.Loading;
                        return;
                    }
                }
            }

            DownloadAsync(pathOrURL, path);
            status = LoadableStatus.Downloading;
        }
    }
}