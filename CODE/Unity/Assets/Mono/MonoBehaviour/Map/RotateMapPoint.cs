using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ET
{
    /// <summary>
    /// 这个类用来定位战斗位置
    /// 同一个点位只能有一个战斗单位
    /// </summary>
    public class RotateMapPoint : MonoBehaviour
    {
        public GameObject GOC;
        public Transform[] combatTransform;
        public SpriteRenderer RendererSample;
        public Collider2D AfkMeleeAttackEventTrigger;
        public Collider2D AfkRemoteAttackEventTrigger;

        /// <summary>
        /// 点位注入游戏对象
        /// </summary>
        public void InjectGOC(GameObject goc)
        {
            this.GOC = goc;
            goc.transform.SetParent(this.transform);
        }

        /// <summary>
        /// 激活触发器
        /// </summary>
        public void ActivateTrigger()
        {
            this.AfkMeleeAttackEventTrigger.gameObject.SetActive(true);
            this.AfkRemoteAttackEventTrigger.gameObject.SetActive(true);
        }

        /// <summary>
        /// 禁用触发器
        /// </summary>
        public void DisabledTrigger()
        {
            this.AfkMeleeAttackEventTrigger.gameObject.SetActive(false);
            this.AfkRemoteAttackEventTrigger.gameObject.SetActive(false);
        }
    }
}
