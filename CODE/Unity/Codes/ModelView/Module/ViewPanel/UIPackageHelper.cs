using System;
using System.Collections.Generic;
using FairyGUI;
using UnityEngine;
using VEngine;

namespace ET
{
    public static class UIPackageHelper
    {
        /// <summary>
        /// 文件路径
        /// </summary>
        private const string PATH = "Assets/Bundles/FUI";
        /// <summary>
        /// PackItem名-Asset键值对
        /// </summary>
        private static readonly Dictionary<string, Asset> PackageItemDict = new Dictionary<string, Asset>();
        /// <summary>
        /// 包名-UIPackage键值对
        /// </summary>
        private static readonly Dictionary<string, UIPackage> UIPackageDict = new Dictionary<string, UIPackage>();

        private static byte[] LoadDesc(string packageName)
        {
            Asset asset = Asset.Load($"{PATH}/{packageName}/{packageName}_fui.bytes", typeof(TextAsset));
            byte[] bytes = asset.Get<TextAsset>().bytes;
            asset.Release();
            return bytes;
        }

        private static async ETTask<byte[]> LoadDescAsync(string packageName)
        {
            Asset asset = await Asset.LoadAsync($"{PATH}/{packageName}/{packageName}_fui.bytes", typeof(TextAsset)).ETAsync();
            byte[] bytes = asset.Get<TextAsset>().bytes;
            asset.Release();
            return bytes;
        }

        private static void LoadResource(string name, string extension, Type type, PackageItem item)
        {
            if (!UIPackageDict.ContainsKey(item.owner.name))
            {
                Log.Error($"package:{item.owner.name}未被规范加载，packageItem却在被加载！");
                return;
            }

            if (PackageItemDict.ContainsKey(item.file))
            {
                return;
            }

            DateTime startTime = DateTime.Now;
            Asset asset = Asset.Load($"{PATH}/{item.owner.name}/{item.file}", type);
            PackageItemDict[item.file] = asset;
            item.owner.SetItemAsset(item, asset.asset, DestroyMethod.None);
            Log.Info($"{item.file}资源加载Time:{DateTime.Now - startTime}");
        }

        private static void LoadResourceAsync(string name, string extension, Type type, PackageItem item)
        {
            LoadResourceTask().Coroutine();

            async ETTask LoadResourceTask()
            {
                if (!UIPackageDict.ContainsKey(item.owner.name))
                {
                    Log.Error($"package:{item.owner.name}未被规范加载，packageItem却在被加载！");
                    return;
                }

                if (PackageItemDict.ContainsKey(item.file))
                {
                    return;
                }

                DateTime startTime = DateTime.Now;
                Asset asset = Asset.LoadAsync($"{PATH}/{item.owner.name}/{item.file}", type);
                PackageItemDict[item.file] = asset;
                await asset.ETAsync();
                item.owner.SetItemAsset(item, asset.asset, DestroyMethod.None);
                Log.Info($"{item.file}资源加载Time:{DateTime.Now - startTime}");
            }
        }

        /// <summary>
        /// 加载UI资源包
        /// </summary>
        /// <param name="packageName"></param>
        /// <returns></returns>
        public static void AddPackage(string packageName)
        {
            if (UIPackageDict.ContainsKey(packageName))
            {
                return;
            }

            DateTime startTime = DateTime.Now;
            byte[] descData = LoadDesc(packageName);
            UIPackage uiPackage = UIPackage.AddPackage(descData, packageName, LoadResource);
            UIPackageDict[packageName] = uiPackage;
            Log.Info($"资源包{packageName}加载耗时:{DateTime.Now - startTime}");
        }

        /// <summary>
        /// 异步加载UI资源包
        /// </summary>
        /// <param name="packageName"></param>
        /// <returns></returns>
        public static async ETTask AddPackageAsync(string packageName)
        {
            if (UIPackageDict.ContainsKey(packageName))
            {
                return;
            }

            DateTime startTime = DateTime.Now;
            byte[] descData = await LoadDescAsync(packageName);
            UIPackage uiPackage = UIPackage.AddPackage(descData, packageName, LoadResourceAsync);
            UIPackageDict[packageName] = uiPackage;
            Log.Info($"资源包{packageName}加载耗时:{DateTime.Now - startTime}");
        }

        /// <summary>
        /// 卸载包
        /// </summary>
        /// <param name="packageName"></param>
        public static void RemovePackage(string packageName)
        {
            if (UIPackageDict.TryGetValue(packageName, out UIPackage package))
            {
                foreach (PackageItem item in package.GetItems())
                {
                    if (PackageItemDict.TryGetValue(item.file, out Asset asset))
                    {
                        asset.Release();
                        PackageItemDict.Remove(item.file);
                    }
                }

                UIPackage.RemovePackage(packageName);
                UIPackageDict.Remove(package.name);
            }
        }

        public static bool Contains(string packageName)
        {
            return UIPackageDict.ContainsKey(packageName);
        }
    }
}