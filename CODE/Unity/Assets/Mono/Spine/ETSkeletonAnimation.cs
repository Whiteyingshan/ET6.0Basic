#if UNITY_2018_3 || UNITY_2019 || UNITY_2018_3_OR_NEWER
#define NEW_PREFAB_SYSTEM
#endif

using System;
using System.Threading.Tasks;
using ET.ThreadController;
using Sirenix.Utilities;
using Spine.Unity;
using UnityEngine;

namespace ET
{
#if NEW_PREFAB_SYSTEM
	[ExecuteAlways]
#else
    [ExecuteInEditMode]
#endif
    [AddComponentMenu("Spine/ETSkeletonAnimation")]
    public class ETSkeletonAnimation : Spine.Unity.SkeletonAnimation
    {
        /// <summary>
        /// 是否准备好了
        /// </summary>
        public bool IsReady { get; private set; }
        
        /// <summary>
        /// 初始化完成事件
        /// </summary>
        private Action InitCompleteEvent;
        /// <summary>
        /// 初始动画名称
        /// 使用特定方法初始化时自动播放的动画名称
        /// </summary>
        [SerializeField]
        [Tooltip("初始动画名称")]
        private string InitAniamtionName;
        
        public static async ETTask<ETSkeletonAnimation> NewETSkeletonAnimationAsync(SkeletonDataAsset skeletonDataAsset)
        {
            return await AddSpineComponentAsync(new GameObject("SpineGameObject") , skeletonDataAsset);
        }

        public static async ETTask<ETSkeletonAnimation> AddSpineComponentAsync(GameObject gameObject, SkeletonDataAsset skeletonDataAsset)
        {
            var c = gameObject.AddComponent<ETSkeletonAnimation>();
            if (skeletonDataAsset != null) 
            {
                await c.InitializeAsync(skeletonDataAsset);
            }
            return c;
        }

        /// <summary>
        /// 添加初始化事件
        /// </summary>
        /// <param name="action"></param>
        public void AddInitCompleteEvent(Action action)
        {
            if (this.InitCompleteEvent == default)
            {
                this.InitCompleteEvent = action;
            }
            else
            {
                this.InitCompleteEvent += action;
            }
            if(this.IsReady)
                action.Invoke();
        }

        /// <summary>
        /// 移除初始化事件
        /// </summary>
        /// <param name="action"></param>
        public void RemoveInitCompleteEvent(Action action)
        {
            if(this.InitCompleteEvent != default)
                this.InitCompleteEvent -= action;
        }

        /// <summary>
        /// 清空初始化事件
        /// </summary>
        public void ClearInitCompleteEvent()
        {
            this.InitCompleteEvent = default;
        }
        
        /// <summary>
        /// 获取轨道上的当前动画
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Spine.Animation GetCurAnimation(int index)
        {
            return this.state.GetCurrent(index)?.Animation;
        }

        public void Initialize(bool overwrite)
        {
            this.gameObject.SetActive(false);
            this.IsReady = false;
            
            base.Initialize(overwrite);

            this.IsReady = true;
            this.InitCompleteEvent?.Invoke();
            this.gameObject.SetActive(true);
        }

        /// <summary>
        /// 异步初始化
        /// </summary>
        public async ETTask InitializeAsync(SkeletonDataAsset _skeletonDataAsset)
        {
            this.gameObject.SetActive(false);
            this.IsReady = false;
            this.skeletonDataAsset = _skeletonDataAsset;
            var shader = Shader.Find("Spine/Skeleton");
            
            _skeletonDataAsset.atlasAssets.ForEach(_aa =>
            {
                _aa.Materials.ForEach(_m =>
                {
                    ThreadSynchronizationContext.Instance.Post(() =>
                    {
                        _m.shader = shader;
                    });
                });
            });
            
            base.Initialize(true);

            this.IsReady = true;
            this.InitCompleteEvent?.Invoke();
            this.gameObject.SetActive(true);
        }

        /// <summary>
        /// 异步初始化
        /// </summary>
        /// <returns></returns>
        public async ETTask InitializeAndStartAnimationAsync(SkeletonDataAsset _skeletonDataAsset, string initAnimation, bool _loop)
        {
            this.InitAniamtionName = initAnimation;
            this.loop = _loop;
            
            Spine.Animation _animation = default;
            
            
            this.AnimationState?.SetEmptyAnimations(0);
            await this.InitializeAsync(_skeletonDataAsset);
            if (!string.IsNullOrEmpty(this.InitAniamtionName))
            {
                _animation = this.AnimationState.Data.SkeletonData.FindAnimation(this.InitAniamtionName);
                if (_animation != default)
                {
                    this.AnimationState.SetAnimation(0, _animation, this.loop);
                }
            }
            else
            {
                Log.Error($"不存在的动画:{initAnimation}");
            }
        }

        /// <summary>
        /// 初始化spine与动画
        /// </summary>
        /// <param name="_skeletonDataAsset"></param>
        /// <returns></returns>
        public async ETTask InitializeAndInitAnimationAsync(SkeletonDataAsset _skeletonDataAsset)
        {
            this.AnimationState?.SetEmptyAnimations(0);
            await this.InitializeAsync(_skeletonDataAsset);
            
            if (!string.IsNullOrEmpty(this.InitAniamtionName))
            {
                Spine.Animation _animation = this.AnimationState.Data.SkeletonData.FindAnimation(this.InitAniamtionName);
                if (_animation != default)
                {
                    this.AnimationState.SetAnimation(0, _animation, this.loop);
                }
            }
        }
        
        /// <summary>
        /// 修改spine动画
        /// </summary>
        /// <param name="_skeletonDataAsset"></param>
        /// <returns></returns>
        public async ETTask SetSkeletonDataAssetAsync(SkeletonDataAsset _skeletonDataAsset)
        {
            if(!_skeletonDataAsset) return;
            
            this.StopAllCoroutines();
            this.ClearState();
            
            await this.InitializeAsync(_skeletonDataAsset);
            
            this.AnimationState?.SetEmptyAnimations(0);
        }

        public void PlayDefaultAnimation()
        {
            if (this.skeletonDataAsset == default) return;

            if (!string.IsNullOrEmpty(this.InitAniamtionName))
            {
                Spine.Animation _animation = this.Skeleton.Data.Animations.Find(animation => animation.Name == this.InitAniamtionName);
                if (_animation != default)
                {
                    this.AnimationState.SetAnimation(0, _animation, this.loop);
                }
            }
            else
            {
                Log.Error($"不存在的动画:{this.InitAniamtionName}");
            }
        }
        /// <summary>
		/// 重置动画控制器
		/// </summary>
		public void ResetAnimator()
        {
            if (this.SkeletonDataAsset == default) return;

            this.StopAllCoroutines();

            if (this.state != default)
            {
                foreach (var track in this.state.Tracks)
                {
                    track.Reset();
                }

                this.state.ClearTracks();
            }
        }
    }
}