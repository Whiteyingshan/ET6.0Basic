using Spine.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Playables;

namespace ET
{
    public partial class ArtsEffectManager
    {
        /// <summary>
        /// 事件容器
        /// </summary>
        private readonly Dictionary<string, Func<ETTask>> artsFrameEventDict = new Dictionary<string, Func<ETTask>>();

        /// <summary>
        /// 添加帧事件
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="func"></param>
        public void AddFrameEventAction(string eventName, Func<ETTask> func)
        {
            if(this.artsFrameEventDict.ContainsKey(eventName))
            {
                this.artsFrameEventDict[eventName] += func;
            }
            else
            {
                this.artsFrameEventDict[eventName] = func;
            }
        }

        /// <summary>
        /// 移除帧事件
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="func"></param>
        public void RemoveFreamEventAction(string eventName, Func<ETTask> func)
        {
            if (this.artsFrameEventDict.ContainsKey(eventName))
            {
                this.artsFrameEventDict[eventName] -= func;
            }
        }

        /// <summary>
        /// 当前播放器的状态
        /// </summary>
        [Obsolete]
        public PlayState State
        {
            get
            {
                if(this.CurArtsEffectDepend?.PlayableDirector == default)
                {
                    return PlayState.Delayed;
                }

                return this.CurArtsEffectDepend.PlayableDirector.state;
            }
        }

        /// <summary>
        /// 停止当前动画
        /// </summary>
        public void StopCurrent()
        {
            if (this.CurArtsEffectDepend?.PlayableDirector == default || this.CurArtsEffectDepend.PlayableDirector.state != PlayState.Playing) return;

            this.CurArtsEffectDepend.PlayableDirector.Stop();

            this.CurArtsEffectDepend.PlayableDirector.gameObject.SetActive(false);
        }

        /// <summary>
        /// 暂停当前动画
        /// </summary>
        public void PauseCurrent()
        {
            if (this.CurArtsEffectDepend?.PlayableDirector == default || this.CurArtsEffectDepend.PlayableDirector.state == PlayState.Paused) return;

            this.SkeletonAnimation.timeScale = 0;
            this.CurArtsEffectDepend.PlayableDirector.Pause();
        }

        /// <summary>
        /// 继续当前动画
        /// </summary>
        public void ContinueCurrent()
        {
            if (this.CurArtsEffectDepend?.PlayableDirector == default || this.CurArtsEffectDepend.PlayableDirector.state != PlayState.Paused) return;

            this.SkeletonAnimation.timeScale = 1;
            this.CurArtsEffectDepend.PlayableDirector.Play();
        }

        /// <summary>
        /// 动画是否存由Model是否存在动画觉得
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool HasParameter(string parameter)
        {
            return this.artsEffectDict.ContainsKey(parameter);
        }
    }
}
