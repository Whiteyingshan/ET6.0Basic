using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace ET
{
	/// <summary>
	/// ��������
	/// </summary>
	public static class SpineAnimationName
	{
		/// <summary>
		/// Spine��λ����������
		/// </summary>
		public const string SpineUnitRoot = "Root";
		/// <summary>
		/// Spine��λģ�Ͷ�������
		/// </summary>
		public const string SpineUnitModel = "Model";
		/// <summary>
		/// Spine��λSpine��Ч��λ����
		/// </summary>
		public const string SpineUnitEffect = "Effect";
		/// <summary>
		/// Spine��λUnit��Ч��λ����
		/// </summary>
		public const string SpineUnitArtsEffect = "ArtsEffect";

		/// <summary>
		/// ��֪
		/// </summary>
		public const string idle = "idle";
		/// <summary>
		/// ����
		/// </summary>
		public const string move = "move";
		/// <summary>
		/// ����
		/// </summary>
		public const string run = "run";
		/// <summary>
		/// �ƶ�����
		/// </summary>
		public const string move_attack = "move_attack";
		/// <summary>
		/// ����
		/// </summary>
		public const string attacked = "hit";
		/// <summary>
		/// ����
		/// </summary>
		public const string die = "die";
		/// <summary>
		/// ��ͨ����
		/// </summary>
		public const string attack = "attack";
		/// <summary>
		/// ����1
		/// </summary>
		public const string skill01 = "skill01";
		/// <summary>
		/// ����2
		/// </summary>
		public const string skill02 = "skill02";
		/// <summary>
		/// ���趯��
		/// </summary>
		public const string vertigo = "vertigo";
	}

	public partial class ArtsEffectManager : MonoBehaviour
	{
		/// <summary>
		/// ������Spine
		/// </summary>
		public SkeletonAnimation SkeletonAnimation;
		/// <summary>
		/// ����λ��
		/// </summary>
		[Tooltip("����λ��")]
		public Transform HitPosition;
		/// <summary>
		/// ��Ч����
		/// </summary>
		private readonly Dictionary<string, ArtsEffectDepend> artsEffectDict = new Dictionary<string, ArtsEffectDepend>();
		/// <summary>
		/// ��ǰ���ŵ�ArtsEffectDepend
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
				Debug.LogError($"��Ч�������ű�(ArtsEffectManager)û��ָ��Spine���������뽫������ק�� ArtsEffectManager �ű��� SkeletonAnimation�ֶ��ϣ�");
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
						Debug.LogError($"��Ч����:{effectGO.name}, ��������Ч���ſ�����(PlayableDirector)��");
						continue;
					}

					if (this.artsEffectDict.ContainsKey(effectGO.name))
					{
						Debug.LogError($"��������Ч����:{effectGO.name}�� �밴�ձ�׼����������");
						continue;
					}
					else if (this.SkeletonAnimation.Skeleton.Data.Animations.Find(anim => anim.Name == effectGO.name) == default)
					{
						Debug.LogError($"��Ч����:{effectGO.name}����������ʹ��Spine����Ķ������������� �밴�ձ�׼����������");
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
		/// ����
		/// </summary>
		/// <param name="animationName"></param>
		public void Play(string animationName)
		{
			if (this.CurArtsEffectDepend != default && this.CurArtsEffectDepend.PlayableDirector.state == PlayState.Playing) return;

			ArtsEffectDepend artsEffectView;

			if (!this.artsEffectDict.TryGetValue(animationName, out artsEffectView))
			{
				Debug.LogError($"�����ڵĶ���:{animationName}");
				return;
			}

			this.CurArtsEffectDepend?.PlayableDirector?.Stop();
			this.CurArtsEffectDepend = artsEffectView;
			this.CurArtsEffectDepend.transform.localPosition = Vector3.zero;
			this.CurArtsEffectDepend.gameObject.SetActive(true);
			this.CurArtsEffectDepend.PlayableDirector.Play();
		}

		/// <summary>
		/// ���Ų��޸�λ��
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