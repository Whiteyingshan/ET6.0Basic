using System;
using VEngine;

namespace ET.Code.System
{
    internal static class SceneAssetComponentSystem
    {
        private sealed class SceneAssetComponentDestory : DestroySystem<SceneAssetComponent>
        {
            public override void Destroy(SceneAssetComponent self)
            {
                foreach (var item in self.AssetCache)
                {
                    item.Value.Release();
                }
                self.AssetCache.Clear();
            }
        }

        public static T Load<T>(this SceneAssetComponent self, string path) where T : UnityEngine.Object
        {
            if (self.AssetCache.ContainsKey(path))
            {
                return self.AssetCache[path].asset as T;
            }
            else
            {
                Asset asset = Asset.Load(path, typeof(T));
                self.AssetCache[path] = asset;
                return asset.asset as T;
            }
        }

        public static Asset LoadAsync(this SceneAssetComponent self, string path, Type type)
        {
            if (self.AssetCache.ContainsKey(path))
            {
                return self.AssetCache[path];
            }
            else
            {
                Asset asset = Asset.LoadAsync(path, type);
                self.AssetCache[path] = asset;
                return asset;
            }
        }

        public static async ETTask<T> LoadAsync<T>(this SceneAssetComponent self, string path) where T : UnityEngine.Object
        {
            if (self.AssetCache.ContainsKey(path))
            {
                return self.AssetCache[path].asset as T;
            }
            else
            {
                Asset asset = await Asset.LoadAsync(path, typeof(T)).ETAsync();
                self.AssetCache[path] = asset;
                return asset.asset as T;
            }
        }
    }
}