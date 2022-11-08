using System;
using UnityEngine;
using VEngine;
using ET.ThreadController;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace ET
{
    /// <summary>
    /// 该组件挂载后任何情况都不要移除！
    /// </summary>
    public partial class ResourcesComponent : Entity
    {
        class ResourcesComponentAwakeSystem : AwakeSystem<ResourcesComponent>
        {
            public override void Awake(ResourcesComponent self)
            {
                Instance = self;
                AwakeLoad().Coroutine();
            }
            private async ETTask AwakeLoad()
            {
                Dictionary<long, cSpineAssetConfig> Config = cSpineAssetConfigCategory.Instance.GetAll();
                foreach (var item in Config)
                {
                    if(item.Key % 10000000000 != 0 && item.Key / 10000000000 < 10)
                        await Instance.LoadSkeletonDataAssetAsync(item.Key);
                }
            }
        }

        

        public static ResourcesComponent Instance { get; set; }


        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public T LoadAsset<T>(string path) where T : UnityEngine.Object
        {
            Asset asset = Asset.Load(path, typeof(T));

            var obj = asset.Get<T>();
            Renderer renderer;

            if (obj is GameObject go && (renderer = go.GetComponent<Renderer>()) != default)
            {
                var mats = renderer.materials;
                for (int i = 0; i < mats.Length; i++)
                {
                    mats[i].shader = Shader.Find($"{mats[i].shader.name}");
                }
            }

            return obj;
        }

        /// <summary>
        /// 异步加载资源
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public async ETTask<T> LoadAssetAsync<T>(string path) where T : UnityEngine.Object
        {
            Asset asset = await Asset.LoadAsync(path, typeof(T)).ETAsync();

            var obj = asset.Get<T>();
            Renderer renderer;

            if (obj is GameObject go && (renderer = go.GetComponent<Renderer>()) != default)
            {
                var mats = renderer.materials;
                for (int i = 0; i < mats.Length; i++)
                {
                    mats[i].shader = Shader.Find($"{mats[i].shader.name}");
                }
            }

            return obj;
        }
    }
}