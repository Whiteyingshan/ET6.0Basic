using System.Collections.Generic;
using VEngine.Editor;

namespace VEngine
{
    public class EditorManifestFile : ManifestFile
    {
        private Editor.Manifest manifest;

        protected override void OnLoad()
        {
            pathOrURL = name;
            var settings = Settings.GetDefaultSettings();
            foreach (var item in settings.manifests)
            {
                var manifestName = $"{item.name}".ToLower();
                if (name != manifestName)
                {
                    continue;
                }
                manifest = item;
                return;
            }

            Finish("File not found.");
        }

        protected override void OnUpdate()
        {
            switch (status)
            {
                case LoadableStatus.Loading:
                    var rm = new Manifest();
                    var assetPaths = new List<string>();
                    foreach (var group in manifest.groups)
                    {
                        foreach (var asset in group.assets)
                        {
                            if (!asset.isFolder)
                            {
                                if (!assetPaths.Contains(asset.path))
                                {
                                    assetPaths.Add(asset.path);
                                }
                                else
                                {
                                    Logger.W("{0} already exist.", asset.path);
                                }
                            }
                            else
                            {
                                foreach (var child in asset.GetChildren())
                                {
                                    if (!assetPaths.Contains(child))
                                    {
                                        assetPaths.Add(child);
                                    }
                                    else
                                    {
                                        Logger.W("{0} already exist.", child);
                                    }
                                }
                            }
                        }
                    }
                    var infos = new List<AssetInfo>();
                    for (var index = 0; index < assetPaths.Count; index++)
                    {
                        infos.Add(new AssetInfo
                        {
                            id = index
                        });
                    }
                    rm.assets = infos;
                    rm.SetAllAssetPaths(assetPaths.ToArray());
                    rm.Remapping();
                    Versions.Load(pathOrURL, rm);
                    Finish();
                    break;
            }
        }

        public static EditorManifestFile Create(string name, bool builtin)
        {
            var asset = new EditorManifestFile
            {
                name = name
            };
            return asset;
        }
    }
}