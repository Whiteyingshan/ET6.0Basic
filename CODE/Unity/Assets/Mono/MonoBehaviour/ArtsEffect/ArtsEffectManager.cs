using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace ET
{
	/// <summary>
	/// 动画名称
	/// </summary>
	public static class SpineAnimationName
	{
		/// <summary>
		/// Spine单位根对象名称
		/// </summary>
		public const string SpineUnitRoot = "Root";
		/// <summary>
		/// Spine单位模型对象名称
		/// </summary>
		public const string SpineUnitModel = "Model";
		/// <summary>
		/// Spine单位Spine特效单位名称
		/// </summary>
		public const string SpineUnitEffect = "Effect";
		/// <summary>
		/// Spine单位Unit特效单位名称
		/// </summary>
		public const string SpineUnitArtsEffect = "ArtsEffect";

		/// <summary>
		/// 先知
		/// </summary>
		public const string idle = "idle";
		/// <summary>
		/// 行走
		/// </summary>
		public const string move = "move";
		/// <summary>
		/// 奔跑
		/// </summary>
		public const string run = "run";
		/// <summary>
		/// 移动攻击
		/// </summary>
		public const string move_attack = "move_attack";
		/// <summary>
		/// 被击
		/// </summary>
		public const string attacked = "hit";
		/// <summary>
		/// 死亡
		/// </summary>
		public const string die = "die";
		/// <summary>
		/// 普通攻击
		/// </summary>
		public const string attack = "attack";
		/// <summary>
		/// 技能1
		/// </summary>
		public const string skill01 = "skill01";
		/// <summary>
		/// 技能2
		/// </summary>
		public const string skill02 = "skill02";
		/// <summary>
		/// 泡澡动画
		/// </summary>
		public const string vertigo = "vertigo";
	}

	public partial class ArtsEffectManager : MonoBehaviour
	{
		/// <summary>
		/// 关联的Spine
		/// </summary>
		public SkeletonAnimation SkeletonAnimation;
		/// <summary>
		/// 命中位置
		/// </summary>
		[Tooltip("命中位置")]
		public Transform HitPosition;
		/// <summary>
		/// 特效容器
		/// </summary>
		private readonly Dictionary<string, ArtsEffectDepend> artsEffectDict = new Dictionary<string, ArtsEffectDepend>();
		/// <summary>
		/// 当前播放的ArtsEffectDepend
		/// </summary>
		public ArtsEffectDepend CurArtsEffectDepend;

        public void ResetArts()
        {
            foreach (var effectDepend in artsEffectDict.Values)
            {
				effectDepend.ClearCallBack();
            }
        }

        public void OnInit(SkeletonAnimation skeletonAnimation)
		{
			this.SkeletonAnimation = skeletonAnimation;

			if (this.SkeletonAnimation == default)
			{
				Debug.LogError($"特效管理器脚本(ArtsEffectManager)没有指定Spine动画对象！请将对象拖拽到 ArtsEffectManager 脚本的 SkeletonAnimation字段上！");
				return;
			}

			this.transform.SetParent(skeletonAnimation.transform);
			//skeletonAnimation.transform.localPosition = this.transform.localPosition = Vector3.zero;
			//skeletonAnimation.transform.localScale = this.transform.localScale = Vector3.one;
			this.SkeletonAnimation.AnimationState.AddAnimation(0, SpineAnimationName.idle, true, 0);

			for (int i = 0; i < this.transform.childCount; i++)
			{
				var effectGO = transform.GetChild(i);
				var artsEffectView = effectGO.GetComponent<ArtsEffectDepend>();

				if (artsEffectView != default)
				{
					if (artsEffectView.PlayableDirector == default)
					{
						Debug.LogError($"特效对象:{effectGO.name}, 不存在特效播放控制器(PlayableDirector)！");
						continue;
					}

					if (this.artsEffectDict.ContainsKey(effectGO.name))
					{
						Debug.LogError($"重名的特效对象:{effectGO.name}！ 请按照标准流程制作！");
						continue;
					}
					else if (this.SkeletonAnimation.Skeleton.Data.Animations.Find(anim => anim.Name == effectGO.name) == default)
					{
						Debug.LogError($"特效对象:{effectGO.name}命名错误！请使用Spine对象的动画名称命名！ 请按照标准流程制作！");
						continue;
					}

					foreach (var bind in artsEffectView.PlayableDirector.playableAsset.outputs)
					{
						if (bind.streamName == "Spine Animation State Track")
						{
							artsEffectView.PlayableDirector.SetGenericBinding(bind.sourceObject, SkeletonAnimation);
							break;
						}
					}

					artsEffectView.PlayableDirector.stopped += playbleDirector =>
					{
						playbleDirector.time = 0;
						//this.SkeletonAnimation.AnimationState.SetAnimation(0, SpineAnimationName.idle, true);
					};

					this.artsEffectDict[effectGO.name] = artsEffectView;
				}
			}
		}

		/// <summary>
		/// 播放
		/// </summary>
		/// <param name="animationName"></param>
		public void Play(string animationName)
		{
			if (this.CurArtsEffectDepend != default && this.CurArtsEffectDepend.PlayableDirector.state == PlayState.Playing) return;

			ArtsEffectDepend artsEffectView;

			if (!this.artsEffectDict.TryGetValue(animationName, out artsEffectView))
			{
				Debug.LogError($"不存在的动画:{animationName}");
				return;
			}

			this.CurArtsEffectDepend?.PlayableDirector?.Stop();
			this.CurArtsEffectDepend = artsEffectView;
			this.CurArtsEffectDepend.transform.localPosition = Vector3.zero;
			this.CurArtsEffectDepend.gameObject.SetActive(true);
			this.CurArtsEffectDepend.PlayableDirector.Play();
		}

		/// <summary>
		/// 播放并修改位置
		/// </summary>
		public void PlayWithPosition(string animationName, Vector3 pos)
		{
			this.Play(animationName);

			if (this.CurArtsEffectDepend != default)
			{
				this.CurArtsEffectDepend.transform.position = pos;
			}
		}

		public void Action(string actionType)
		{
#if NET452
			Func<ETTask> func;
			if (this.artsFrameEventDict.TryGetValue(actionType, out func))
			{
				func?.Invoke().Coroutine();
			}
			this.artsFrameEventDict.Remove(actionType);
#endif
		}
	}

	}