using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace VEngine.Editor
{
    /// <summary>
    ///     参与打包的资源，可以是单个文件，或文件夹
    /// </summary>
    [Serializable]
    public class Asset : IEquatable<Asset>, IComparable<Asset>
    {
        [SerializeField] private Object _target;

        /// <summary>
        ///     资源的 label，用来生成 bundle 的名字
        /// </summary>
        public string label;

        /// <summary>
        ///     所有依赖的 bundle名字
        /// </summary>
        public List<string> bundles = new List<string>();

        /// <summary>
        ///     资源的所有依赖
        /// </summary>
        public string[] dependencies = new string[0];

        /// <summary>
        ///     是否是只读的内容，例如 children
        /// </summary>
        public bool readOnly;

        /// <summary>
        ///     资源的路径
        /// </summary>
        public string path = string.Empty;

        /// <summary>
        ///     跟节点的路径
        /// </summary>
        public string rootPath;

        /// <summary>
        ///     打包的分组
        /// </summary>
        public Group parentGroup;

        /// <summary>
        ///     获取 Bundle 的名字
        /// </summary>
        public string bundle;

        /// <summary>
        ///     自定义打包器，可以按自己的喜好为资源打包，相同名字的资源会打包到一起。
        /// </summary>
        public static Func<Asset, string> customPacker { get; set; }

        /// <summary>
        ///     包含依赖的大小
        /// </summary>
        public ulong size { get; set; }


        /// <summary>
        ///     在 Assets 下的目标对象
        /// </summary>
        public Object target
        {
            get
            {
                if (_target == null)
                {
                    _target = AssetDatabase.LoadAssetAtPath<Object>(path);
                }
                return _target;
            }
        }

        /// <summary>
        ///     是否是文件夹，文件夹需要采集子文件，但本身不参与打包。
        /// </summary>
        public bool isFolder => Directory.Exists(path);

        /// <summary>
        ///     资源是否已经修改
        /// </summary>
        public bool dirty { get; set; }

        public string metaPath => AssetDatabase.GetTextMetaFilePathFromAssetPath(path);

        public int CompareTo(Asset other)
        {
            return string.Compare(path, other.path, StringComparison.Ordinal);
        }

        public bool Equals(Asset other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return path == other.path;
        }

        public string PackWithBundleMode()
        {
            if (customPacker != null)
            {
                return customPacker(this);
            }

            if (parentGroup != null)
            {
                var groupName = $"{parentGroup.manifest.name}_{parentGroup.name}";
                var isScene = path.EndsWith(".unity") || path.EndsWith(".lighting");
                if (isScene)
                {
                    var assetName = Path.GetFileNameWithoutExtension(path);
                    return $"{groupName}_scenes_{assetName}".ToLower();
                }
                if (isFolder)
                {
                    return string.Empty;
                }
                switch (parentGroup.bundleMode)
                {
                    case BundleMode.PackTogether:
                        return PackTogether(groupName);
                    case BundleMode.PackByEntry:
                        return PackByEntry(groupName);
                    case BundleMode.PackByLabel:
                        return PackByLabel(groupName);
                    case BundleMode.PackByTopSubDirectoryOnly:
                        return PackByTopSubDirectoryOnly(groupName);
                    case BundleMode.PackByDirectory:
                        return PackByDirectory(groupName);
                    case BundleMode.PackByFile:
                        return PackByFile(groupName);
                    case BundleMode.PackByRaw:
                        return path;
                    default:
                        return $"{groupName}_default".ToLower();
                }
            }

            return "invalid entry! parentGroup == null";
        }

        private static string PackTogether(string groupName)
        {
            return $"{groupName}_all_assets".ToLower();
        }

        private string PackByEntry(string groupName)
        {
            var assetName = Path.GetFileNameWithoutExtension(readOnly ? rootPath : path);
            return $"{groupName}_{assetName}".ToLower();
        }

        private string PackByLabel(string groupName)
        {
            if (string.IsNullOrEmpty(label))
            {
                return $"{groupName}_default".ToLower();
            }
            return $"{groupName}_{label}".ToLower();
        }

        private string PackByDirectory(string groupName)
        {
            if (!string.IsNullOrEmpty(rootPath))
            {
                var assetName = Path.GetFileName(rootPath);
                var directoryName = Path.GetDirectoryName(path.Substring(rootPath.Length + 1));
                if (string.IsNullOrEmpty(directoryName))
                {
                    return $"{groupName}_{assetName}".ToLower();
                }
                directoryName = directoryName.Replace("\\", "/").Replace("/", "_");
                return $"{groupName}_{assetName}_{directoryName}".ToLower();
            }
            else
            {
                var assetName = Path.GetFileName(rootPath);
                var directoryName = Path.GetDirectoryName(path);
                if (string.IsNullOrEmpty(directoryName))
                {
                    return $"{groupName}_{assetName}".ToLower();
                }
                directoryName = directoryName.Replace("\\", "/").Replace("/", "_");
                return $"{groupName}{assetName}_{directoryName}".ToLower();
            }
        }

        private string PackByTopSubDirectoryOnly(string groupName)
        {
            if (string.IsNullOrEmpty(rootPath))
            {
                return $"{groupName}_all_assets".ToLower();
            }
            var assetName = Path.GetFileName(rootPath);
            var directoryName = Path.GetDirectoryName(path.Substring(rootPath.Length + 1));
            if (string.IsNullOrEmpty(directoryName))
            {
                return $"{groupName}_{assetName}".ToLower();
            }
            directoryName = directoryName.Replace("\\", "/");
            var pos = directoryName.IndexOf("/", StringComparison.Ordinal);
            if (pos != -1)
            {
                directoryName = directoryName.Substring(0, pos);
            }
            directoryName = directoryName.Replace("/", "_");
            return $"{groupName}_{assetName}_{directoryName}".ToLower();
        }

        private string PackByFile(string groupName)
        {
            if (string.IsNullOrEmpty(rootPath))
            {
                var directoryName = Path.GetDirectoryName(path);
                directoryName = directoryName?.Replace("\\", "/").Replace("/", "_");
                directoryName = $"{directoryName}_{Path.GetFileNameWithoutExtension(path)}";
                return $"{groupName}_{directoryName}".ToLower();
            }
            else
            {
                var assetName = Path.GetFileName(rootPath);
                var directoryName = Path.GetDirectoryName(path.Substring(rootPath.Length + 1));
                directoryName = directoryName?.Replace("\\", "/").Replace("/", "_");
                directoryName = $"{directoryName}_{Path.GetFileNameWithoutExtension(path)}";
                return $"{groupName}_{assetName}_{directoryName}".ToLower();
            }
        }

        public static Asset Create(string path, Group group, string label = null,
            string rootPath = null)
        {
            var asset = new Asset
            {
                label = label,
                path = path,
                parentGroup = group,
                rootPath = rootPath, 
            };
            asset.bundle = asset.PackWithBundleMode();
            return asset;
        }


        /// <summary>
        ///     获取资源的所有依赖，不包括自己
        /// </summary>
        /// <returns></returns>
        public string[] GetDependencies()
        {
            var items = new List<string>();
            items.AddRange(isFolder
                ? AssetDatabase.GetDependencies(GetChildren(), true)
                : AssetDatabase.GetDependencies(path, true));
            for (var index = 0; index < items.Count; index++)
            {
                var dependency = items[index];
                if (dependency == path
                    || Directory.Exists(dependency)
                    || EditorUtility.Exclude(dependency) ||
                    isFolder &&
                    isChild(dependency))
                {
                    items.RemoveAt(index);
                    index--;
                }
            }

            items.Sort();
            dependencies = items.ToArray();
            return dependencies;
        }

        public bool isChild(string file)
        {
            return file.Contains(path);
        }

        /// <summary>
        ///     获取子文件-递归。
        /// </summary>
        /// <returns></returns>
        public string[] GetChildren()
        {
            return EditorUtility.GetChildren(path);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj.GetType() == GetType() && Equals((Asset) obj);
        }

        public override int GetHashCode()
        {
            return path.GetHashCode();
        }
    }
}