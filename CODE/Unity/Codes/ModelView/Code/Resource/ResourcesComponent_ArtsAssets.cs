using Spine;
using Spine.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VEngine;

namespace ET
{

	public partial class ResourcesComponent
	{
		/// <summary>
		/// key:资源类型: value:(key:资源Id value:池队列)
		/// </summary>
		private readonly Dictionary<int, Dictionary<long, Queue<GameObject>>> artsAssetsGOPool = new Dictionary<int, Dictionary<long, Queue<GameObject>>>();
		/// <summary>
		/// SkeletonDataAsset集合
		/// </summary>
		private readonly Dictionary<long, SkeletonDataAsset[]> skeletonDataAssets = new Dictionary<long, SkeletonDataAsset[]>();
		/// <summary>
		/// 异步加载资源
		/// </summary>
        public static Func<long, Transform, ETTask<GameObject>> LoadArtsAsyncFunc;
		/// <summary>
		/// 加载资源
		/// </summary>
		public static Func<long, Transform, GameObject> LoadArtsFunc;

        /// <summary>
        /// 尝试取出资源对象
        /// </summary>
        public GameObject TryFetchArtsGameObject(int assetType, long artsAssteId, Transform parent = null)
		{
			Dictionary<long, Queue<GameObject>> poolDict;
			if (!this.artsAssetsGOPool.TryGetValue(assetType, out poolDict))
			{
				poolDict = artsAssetsGOPool[assetType] = new Dictionary<long, Queue<GameObject>>();
			}

			Queue<GameObject> pool;
			if (!poolDict.TryGetValue(artsAssteId, out pool))
			{
				pool = poolDict[artsAssteId] = new Queue<GameObject>();
			}

			if (pool.Count > 0)
			{
				var go = pool.Dequeue();

				go.transform.SetParent(parent);
				go.transform.localPosition = Vector3.zero;
				go.transform.localRotation = Quaternion.Euler(Vector3.zero);

				return go;
			}

			return default;
		}

		/// <summary>
		/// 回收资源对象
		/// </summary>
		public void RecycleArtsGameObject(long artsAssteId, params GameObject[] gos)
		{
			int assetType = (int)(artsAssteId / /*(int)ArtsAssetType.TypeDivisor*/1000000);
			for (int i = 0; i < gos.Length; i++)
            {
				var go = gos[i];
				if (go == default) continue;
				if (artsAssteId == default)
					UnityEngine.Object.Destroy(go);
				else
                {
					//go.GetComponent<BoxCollider2D>().enabled = false;
					go.SetActive(false);
					go.transform.SetParent(Game.GOPool);
					go.transform.localScale = Vector3.one;
					/*this.artsAssetsGOPool[assetType][artsAssteId].Enqueue(go);*/
				}
            }
		}

		/// <summary>
		/// 尝试获取SkeletonDataAssets
		/// </summary>
		public SkeletonDataAsset[] TryFetchSkeletonDataAssets(long id)
		{
			SkeletonDataAsset[] assets;

			if (this.skeletonDataAssets.TryGetValue(id, out assets))
			{
				return assets;
			}

			return default;
		}

		/// <summary>
		/// 异步加载Spine资源
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public async ETTask<SkeletonDataAsset[]> LoadSkeletonDataAssetAsync(long id, params string[] paths)
		{
			cSpineAssetConfig spineAssetConfig = cSpineAssetConfigCategory.Instance.Get(id);
			cSpineAssetConfig TypeConfigs = cSpineAssetConfigCategory.Instance.Get(id / 10000000000 * 10000000000);
			paths = spineAssetConfig.AssetPath;
			SkeletonDataAsset[] assets = new SkeletonDataAsset[paths.Length];

			using(ListComponent<ETTask> tasks = ListComponent<ETTask>.Create())
			{
				for (int i = 0; i < paths.Length; i++)
				{
					if (string.IsNullOrEmpty(paths[i])) continue;
					int i2 = i;
					tasks.Add(OneLoadAsync(i2));
					//assets[i] = await this.LoadAssetAsync<SkeletonDataAsset>(paths[i]);
				}
				await ETTaskHelper.WaitAll(tasks);
			}

			async ETTask OneLoadAsync(int index)
			{
				string path = string.Format(TypeConfigs.AssetPath[0], spineAssetConfig.AssetPath[0]) + spineAssetConfig.AssetPath[0] + "_SkeletonData.asset";
				assets[index] = await this.LoadAssetAsync<SkeletonDataAsset>(path);
			}

			return this.skeletonDataAssets[id] = assets;
		}

		/// <summary>
		/// 直接加载该Spine资源
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public SkeletonDataAsset[] LoadSkeletonDataAsset(long id, params string[] paths)
		{
			cSpineAssetConfig spineAssetConfig = cSpineAssetConfigCategory.Instance.Get(id);
			paths = spineAssetConfig.AssetPath;
			SkeletonDataAsset[] assets = new SkeletonDataAsset[paths.Length];
			for (int i = 0; i < paths.Length; i++)
			{
				if (string.IsNullOrEmpty(paths[i])) continue;
				string path = string.Format("Assets/Bundles/Spine/Hero/HeroCombat/{0}/", paths[0]) + paths[0] + "_SkeletonData.asset";
				assets[i] = this.LoadAsset<SkeletonDataAsset>(path);
			}

			return this.skeletonDataAssets[id] = assets;
		}
	}
}