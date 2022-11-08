using System.Collections.Generic;

namespace VEngine
{
    /// <summary>
    ///     清单资源
    /// </summary>
    public class ManifestFile : Loadable
    {
        /// <summary>
        ///     未使用的列表
        /// </summary>
        protected internal static readonly List<ManifestFile> Unused = new List<ManifestFile>();

        protected override void OnUnused()
        {
            Unused.Add(this);
        }

        protected string name;

        public static ManifestFile LoadAsync(string name, bool builtin = false)
        {
            var asset = Versions.CreateManifest(name, builtin);
            asset.Load();
            return asset;
        }

        internal static ManifestFile Create(string name, bool builtin)
        {
            if (builtin)
            {
                return new BuiltinManifestFile
                {
                    name = name
                };
            }
            return new DownloadManifestFile
            {
                name = name
            };
        }

        public static void UpdateFiles()
        {
            for (var index = 0; index < Unused.Count; index++)
            {
                var item = Unused[index];
                if (Updater.Instance.busy)
                {
                    break;
                }
                if (!item.isDone)
                {
                    continue;
                }
                Unused.RemoveAt(index);
                index--;
                item.Unload();
            }
        }
    }
}