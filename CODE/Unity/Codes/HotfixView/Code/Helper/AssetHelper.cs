using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEngine;
using Spine.Unity;
using UnityEngine;
using Sirenix.Utilities;

namespace ET
{
    public static class AssetHelper
    {
        private static Asset heroCombatPrefab;
        private static Asset heroPicPrefab;
        private static Asset topFUIPrefab;
        private static Queue<GameObject> TopFUIGameObjectPool = new Queue<GameObject>();
        public static async void Init()
        {
            await ETTask.CompletedTask;
        }
        public static async ETTask<GameObject> GetSpineAssetsAsync(long id,int Scale = 0)
        {
            Asset asset = heroCombatPrefab;
            GameObject obj = UnityEngine.Object.Instantiate(asset.asset) as GameObject;
            SkeletonDataAsset[] assets = ResourcesComponent.Instance.TryFetchSkeletonDataAssets(id);

            if (assets == default)
            {
                assets = ResourcesComponent.Instance.LoadSkeletonDataAsset(id);
            }
            obj.name = SpineAnimationName.SpineUnitRoot;
            /*GameObject mondel = obj.transform.Find(SpineAnimationName.SpineUnitModel).gameObject;
            GameObject effect = mondel.transform.Find(SpineAnimationName.SpineUnitEffect).gameObject;*/

            ETSkeletonAnimation animation = obj.GetComponent<ETSkeletonAnimation>();
            if (Scale != 0)
            {
                await animation.InitializeAndStartAnimationAsync(assets[0], "idle", true);
                animation.skeleton.ScaleX = Scale;
            }
            else
                await animation.InitializeAndInitAnimationAsync(assets[0]);
            return obj;

            /*try
            {
                
            }
            catch (Exception)
            {
                Log.Error($"资源出现异常!Id:{id}");

                return obj;
            }*/
        }

        public static async ETTask<GameObject> CreateNullHeroCombatGameObject()
        {
            Asset asset = heroPicPrefab;
            await ETTask.CompletedTask;
            return UnityEngine.Object.Instantiate(asset.asset) as GameObject;
        }

        /// <summary>
        /// 获取英雄战斗立绘
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async ETTask<GameObject> CreateHeroCombatGameObject(long id)
        {
            SkeletonDataAsset[] assets = ResourcesComponent.Instance.TryFetchSkeletonDataAssets(id);
            if (assets == default) return null;
            Asset asset = heroPicPrefab;
            GameObject go = UnityEngine.Object.Instantiate(asset.asset) as GameObject;
            go.name = "Root";
            GameObject mondel = go.transform.Find("Model").gameObject;
            GameObject effect = mondel.transform.Find("Effect").gameObject;

            var animation = mondel.GetComponent<ETSkeletonAnimation>();

            //await animation.InitializeAndInitAnimationAsync(assets[0]);
            await animation.InitializeAndStartAnimationAsync(assets[0], "idle", true);

            //var effectAnimation = effect.GetComponent<ETSkeletonAnimation>();
            //if (assets.Length > 1)
            //{
            //    await effectAnimation.InitializeAndInitAnimationAsync(assets[1]);
            //}

            UpdateSpineMaterial(go);

            return go;
        }

        private static GameObject CreateDefaultTopFUIGameObject()
        {
            return UnityEngine.Object.Instantiate(topFUIPrefab.asset) as GameObject;
        }

        /// <summary>
        /// 取出顶部UI
        /// </summary>
        public static GameObject FetchTopFUIGO()
        {
            GameObject gameObject;
            if (TopFUIGameObjectPool.Count > 0)
            {
                //Log.Error($"FetchSpineGO ID:{id}");
                gameObject = TopFUIGameObjectPool.Dequeue();
            }
            else
            {
                gameObject = CreateDefaultTopFUIGameObject();
            }

            return gameObject;
        }

        /// <summary>
        /// 回收顶部UI
        /// </summary>
        public static void RecycleTopFUIGO(GameObject go)
        {
            go.SetActive(false);
            go.transform.SetParent(Game.GOPool);

            TopFUIGameObjectPool.Enqueue(go);

            //Log.Error($"RecycleSpineGO ID:{id}");
        }

        public static void UpdateSpineMaterial(GameObject _gameObject)
        {
            var renderer = _gameObject.GetComponentsInChildren<Renderer>();

            var shader = Shader.Find("Spine/Skeleton");

            if (shader == default) return;

            renderer.ForEach(_r =>
            {
                _r.sharedMaterials.ForEach(_m =>
                {
                    if (_m == default) return;
                    _m.shader = shader;

                });
            });
        }

        public static async ETTask<Asset[]> GetAssetsAsync(string dir, Type type)
        {
            List<Asset> assets = new List<Asset>();
            List<Manifest> manifests = Versions.Manifests;
            foreach (Manifest manifest in manifests)
            {
                var paths = manifest.AllAssetPaths;
                foreach (string path in paths)
                {
                    if (path.StartsWith(dir))
                    {
                        Asset asset = await Asset.LoadAsync(path, type).ETAsync();
                        assets.Add(asset);
                    }
                }
            }
            return assets.ToArray();
        }

        public static Asset[] GetAssets(string dir, Type type)
        {
            List<Asset> assets = new List<Asset>();
            List<Manifest> manifests = Versions.Manifests;
            foreach (Manifest manifest in manifests)
            {
                var paths = manifest.AllAssetPaths;
                foreach (string path in paths)
                {
                    if (path.StartsWith(dir))
                    {
                        assets.Add(Asset.Load(path, type));
                    }
                }
            }
            return assets.ToArray();
        }
    }
}