//----------------------------
//作者:XXX
//修订日期:XXX
//联系方式:XXX
//----------------------------
//修改者:XXX
//修改日期:XXX
//联系方式:XXX
//修改内容:XXX
//----------------------------

using FairyGUI;
using UnityEngine;

namespace ET
{
    [ObjectSystem]
    public class TweenControllerComponentAwakeSystem : AwakeSystem<TweenControllerComponent>
    {
        public override void Awake(TweenControllerComponent self)
        {
            self.Awake();
        }
    }

    public class TweenControllerComponent : Entity
    {
        public static TweenControllerComponent Instance { get; private set; }

        public void Awake()
        {
            Instance = this;
        }

        /// <summary>
        /// 移动到某ui位置
        /// </summary>
        /// <param name="gObject"></param>
        /// <param name="targetGObject"></param>
        /// <param name="duration"></param>
        /// <param name="easeType"></param>
        /// <returns></returns>
        public GTweener FUIMoveTween(GObject gObject, GObject targetGObject, float duration, EaseType easeType = EaseType.QuadOut)
        {
            GTweener tweenr = GTween.To(gObject.xy, targetGObject.xy, duration).SetEase(easeType);
            tweenr.OnUpdate(() =>
            {
                gObject.SetXY(tweenr.value.vec2.x, tweenr.value.vec2.y);
            });

            return tweenr;
        }
    }
}