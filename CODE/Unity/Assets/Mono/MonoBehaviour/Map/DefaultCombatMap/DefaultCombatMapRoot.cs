using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ET
{
    public class DefaultCombatMapRoot : MonoBehaviour
    {
        /// <summary>
        /// 地图相机
        /// </summary>
        public Camera MapCamera;
        /// <summary>
        /// 地图精炼
        /// </summary>
        public SpriteRenderer MapSpriteRenderer;
        /// <summary>
        /// 玩家队伍点位
        /// </summary>
        public RotateMapTeamPoints PlayerPoints;
        /// <summary>
        /// 怪物点位
        /// </summary>
        public RotateMapTeamPoints EnemyPoints;

        /// <summary>
        /// 初始化地图
        /// </summary>
        public void InitMap(GameObject[] playerUnits, GameObject[] enemyUnits)
        {
            this.PlayerPoints.InitTeam(playerUnits);
            this.EnemyPoints.InitTeam(enemyUnits);
        }
        /// <summary>
        /// 清除玩家单位
        /// </summary>
        public void ClearPlayer()
        {
            this.PlayerPoints.Clear();
        }
        /// <summary>
        /// 清除敌人单位
        /// </summary>
        public void ClearEnemy()
        {
            this.EnemyPoints.Clear();
        }
        /// <summary>
        /// 清除所有单位
        /// </summary>
        public void ClearUnits()
        {

            this.ClearPlayer();
            this.ClearEnemy();
        }
    }
}
