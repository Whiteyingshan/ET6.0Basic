using System;
using System.IO;
using UnityEditor;

namespace VEngine.Editor
{
    [InitializeOnLoad]
    public static class CustomAssetPacker
    {
        static CustomAssetPacker()
        {
            Asset.customPacker += CustomPacker;
        }

        private static string CustomPacker(Asset asset)
        {
            var parentGroup = asset.parentGroup;
            var path = asset.path;
            var isFolder = asset.isFolder;
            var readOnly = asset.readOnly;
            var rootPath = asset.rootPath;
            var label = asset.label;

            if (parentGroup != null)
            {
                var groupName = $"{parentGroup.manifest.name}_{parentGroup.name}";
                var isScene = path.EndsWith(".unity") || path.EndsWith(".lighting");
                if (isScene)
                {
                    var directoryName = Path.GetDirectoryName(path)?.Replace("\\", "/").Replace("/", "_");
                    var assetName = Path.GetFileNameWithoutExtension(path);
                    return $"{directoryName}_{assetName}".ToLower();
                }
                if (isFolder)
                {
                    return string.Empty;
                }
                switch (parentGroup.bundleMode)
                {
                    case BundleMode.PackTogether:
                        return $"{groupName}_all_assets".ToLower();
                    case BundleMode.PackByEntry:
                        return PackByEntry(readOnly, rootPath, path, groupName);
                    case BundleMode.PackByLabel:
                        return PackByLabel(label, groupName);
                    case BundleMode.PackByTopSubDirectoryOnly:
                        return PackByTopSubDirectoryOnly(rootPath, path);
                    case BundleMode.PackByDirectory:
                        return PackByDirectory(path);
                    case BundleMode.PackByFile:
                        return PackByFile(path);
                    case BundleMode.PackByRaw:
                        return path;
                    default:
                        return $"{groupName}_default".ToLower();
                }
            }
            return "invalid entry! parentGroup == null";
        }

        private static string PackByFile(string path)
        {
            var directoryName = Path.GetDirectoryName(path);
            directoryName = directoryName?.Replace("\\", "/").Replace("/", "_");
            directoryName = $"{directoryName}_{Path.GetFileNameWithoutExtension(path)}";
            return $"{directoryName}".ToLower();
        }

        private static string PackByEntry(bool readOnly, string rootPath, string path, string groupName)
        {
            var assetName = Path.GetFileNameWithoutExtension(readOnly ? rootPath : path);
            return $"{groupName}_{assetName}".ToLower();
        }

        private static string PackByLabel(string label, string groupName)
        {
            if (string.IsNullOrEmpty(label))
            {
                return $"{groupName}_default".ToLower();
            }
            return $"{groupName}_{label}".ToLower();
        }

        private static string PackByTopSubDirectoryOnly(string rootPath, string path)
        {
            if (!string.IsNullOrEmpty(rootPath))
            {
                var directoryName = Path.GetDirectoryName(path);
                if (string.IsNullOrEmpty(directoryName))
                {
                    return PackByDirectory(path);
                }
                directoryName = directoryName.Replace("\\", "/");
                if (directoryName != rootPath)
                {
                    var pos = directoryName.IndexOf("/", rootPath.Length + 1, StringComparison.Ordinal);
                    if (pos != -1)
                    {
                        directoryName = directoryName.Substring(0, pos);
                    }
                }
                directoryName = directoryName.Replace("/", "_");
                return $"{directoryName}".ToLower();
            }
            return PackByDirectory(path);
        }

        private static string PackByDirectory(string path)
        {
            var dir = Path.GetDirectoryName(path);
            if (string.IsNullOrEmpty(dir))
            {
                return path.Replace("\\", "/").Replace("/", "_");
            }
            var directoryName = dir.Replace("\\", "/").Replace("/", "_");
            return $"{directoryName}".ToLower();
        }
    }
}