using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Object = UnityEngine.Object;

namespace VEngine
{
    /// <summary>
    ///     资源类，游戏中场景以外的资源可以通过此类来加载，不论什么资源都是通过对大家都非常简明直观的相对路径进行加载，例如:
    ///     <code>
    ///         // 加载一个 prefab 可以这样写:
    ///         var asset = Asset.Load("Assets/Prefabs/UIRoot.prefab", typeof(GameObject));
    ///         var prefab = asset.asset;
    ///         var go = GameObject.Instantiate(prefab);
    ///         // 如果不想释放这个资源可以这样：
    ///         Asset.KeepAliveOnLoad(asset);
    ///         // 在 go 被 Destroy 后， 当资源不再使用的时候可以这样释放：
    ///         asset.Release(); 
    ///      </code>
    /// </summary>
    public class Asset : Loadable, IEnumerator
    {
        /// <summary>
        ///     按 路径 缓存的所有加载的 Asset 对象
        /// </summary>
        protected internal static readonly Dictionary<string, Asset> Cache = new Dictionary<string, Asset>();

        /// <summary>
        ///     未使用的列表
        /// </summary>
        protected internal static readonly List<Asset> Unused = new List<Asset>();


        /// <summary>
        ///     加载完成回调，使用时请通过 += 赋值
        /// </summary>
        public Action<Asset> completed;

        /// <summary>
        ///     在 Unity 中使用的对象，加载完成后，可以将该对象强转为具体的资源对象。
        /// </summary>
        public Object asset { get; protected set; }

        /// <summary>
        ///     在 Unity 中使用的目录类型
        /// </summary>
        protected Type type { get; set; }

        public bool MoveNext()
        {
            return !isDone;
        }

        public void Reset()
        {
        }

        public object Current { get; } = null;

        /// <summary>
        ///     获取目标对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Get<T>() where T : Object
        {
            return asset as T;
        }

        protected override void OnComplete()
        {
            if (completed == null)
            {
                return;
            }

            var saved = completed;
            if (completed != null)
            {
                completed(this);
            }

            completed -= saved;
        }

        protected override void OnUnused()
        {
            // 不需要使用的时候，加载完成不回调。
            completed = null;
            Unused.Add(this);
        }

        /// <summary>
        ///     在加载后不释放资源
        /// </summary>
        /// <param name="asset"></param>
        public static void KeepAliveOnLoad(Asset asset)
        {
            asset.keepAliveOnLoad = true;
        }

        /// <summary>
        ///     异步加载资源，底层支持当服务器资源比本地新的时候直接从服务器下载最新的资源。
        /// </summary>
        /// <param name="path">资源路径，以 “Assets” 开头</param>
        /// <param name="type">资源类型</param>
        /// <param name="completed">加载完成的回调</param>
        /// <returns></returns>
        public static Asset LoadAsync(string path, Type type, Action<Asset> completed = null)
        {
            return LoadInternal(path, type, false, completed);
        }

        /// <summary>
        ///     同步加载资源，此接口不支持直接从服务器下载资源。
        /// </summary>
        /// <param name="path">资源路径，以 “Assets” 开头</param>
        /// <param name="type">资源类型</param>
        /// <returns></returns>
        public static Asset Load(string path, Type type)
        {
            return LoadInternal(path, type, true);
        }

        internal static Asset LoadInternal(string path, Type type, bool mustCompleteOnNextFrame, Action<Asset> completed = null)
        {
            var info = Versions.GetAsset(path);
            if (info == null)
            {
                throw new FileNotFoundException(path);
            }

            if (!Cache.TryGetValue(path, out var item))
            {
                item = Versions.CreateAsset(path, type);
                Cache.Add(path, item);
            }

            if (completed != null)
            {
                item.completed += completed;
            }

            item.mustCompleteOnNextFrame = mustCompleteOnNextFrame;
            item.Load();
            if (mustCompleteOnNextFrame)
            {
                item.LoadImmediate();
            }

            return item;
        }

        public static void UpdateAssets()
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
                if (!item.reference.unused)
                {
                    continue;
                }

                item.Unload();
                Cache.Remove(item.pathOrURL);
            }
        }
    }
}