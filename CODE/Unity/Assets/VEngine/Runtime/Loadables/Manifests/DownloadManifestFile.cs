using System.IO;

namespace VEngine
{
    public class DownloadManifestFile : ManifestFile
    {
        private Download download;

        public string versionName { get; set; }

        protected override void OnLoad()
        {
            pathOrURL = Versions.GetDownloadURL(name);
            versionName = Manifest.GetVersionFile(name);
            var path = Versions.GetDownloadDataPath(versionName);
            var url = Versions.GetDownloadURL(versionName);
            download = Download.DownloadAsync(url, path);
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
                        includeInBuild = false
                    };
                    var path = Versions.GetDownloadDataPath(name);
                    manifest.Load(path);
                    Versions.Load(path, manifest);
                    Finish();
                    break;
            }
        }

        private void UpdateDownloading()
        {
            if (download == null)
            {
                Finish("request == nul with " + status);
                return;
            }

            progress = 0.2f + download.progress;
            if (!download.isDone)
            {
                return;
            }
            if (!string.IsNullOrEmpty(download.error))
            {
                Finish(download.error);
                return;
            }
            download = null;
            status = LoadableStatus.Loading;
        }

        private void UpdateVersion()
        {
            if (download == null)
            {
                Finish("request == null with " + status);
                return;
            }

            progress = 0.2f * download.progress;
            if (!download.isDone)
            {
                return;
            }
            if (!string.IsNullOrEmpty(download.error))
            {
                Finish(download.error);
                return;
            }

            var savePath = Versions.GetDownloadDataPath(versionName);
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

            var path = Versions.GetDownloadDataPath(name);
            if (File.Exists(path))
            {
                using FileStream stream = File.OpenRead(path);
                if (Utility.ComputeCRC32(stream) == crc)
                {
                    Logger.I("Skip to download {0}, because nothing to update.", name);
                    status = LoadableStatus.Loading;
                    return;
                }
            }

            download = Download.DownloadAsync(pathOrURL, path);
            status = LoadableStatus.Downloading;
        }
    }
}