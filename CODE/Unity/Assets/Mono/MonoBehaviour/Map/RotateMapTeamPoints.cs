using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    public class RotateMapTeamPoints : MonoBehaviour
    {
        /// <summary>
        /// 阵型效果点位
        /// </summary>
        public Transform FormationArtsEffectPoint;
        /// <summary>
        /// 阵地前方
        /// </summary>
        public Transform FormationFrontArtsEffectPoint;
        /// <summary>
        /// 空中点位
        /// </summary>
        public RotateMapPoint AirPoint;
        /// <summary>
        /// 队伍点位
        /// </summary>
        public RotateMapPoint[] TeamPoints;
        /// <summary>
        /// 默认的队伍点位位置
        /// </summary>
        private Vector3[] DefaultTeamPointPosition;
        /// <summary>
        /// 点位对应的Unit单位
        /// </summary>
        private GameObject[] gocArray;

        private void Awake()
        {
            this.DefaultTeamPointPosition = new Vector3[this.TeamPoints.Length];

            for (int i = 0; i < this.TeamPoints.Length; i++)
            {
                this.DefaultTeamPointPosition[i] = this.TeamPoints[i].transform.position;
            }
        }

        /// <summary>
        /// 初始化队伍
        /// 会先调用一次Clear 无需手动调用
        /// </summary>
        public void InitTeam(GameObject[] _gocArray)
        {
            if (this.TeamPoints.Length < _gocArray.Length)
                throw new Exception($"队伍超出了最大单位数量，目前最大数量为：{this.TeamPoints.Length}");
            this.gocArray = _gocArray;

            for (int i = 0; i < this.gocArray.Length; i++)
            {
                if (this.gocArray[i] == default) continue;
                /*var goc = this.gocArray[i].GetComponent<CombatGameObjectComponent>();*/
                var go = gocArray[i];
                var point = this.TeamPoints[i];

                /*goc.Point = point;*/
                go.transform.SetParent(point.transform);
                go.transform.localPosition = Vector3.zero;
                go.transform.localEulerAngles = Vector3.zero;
                /*go.transform.localScale = new Vector3(NumericType.DefaultCombatUnitScale, NumericType.DefaultCombatUnitScale, 1);*/
                go.transform.localScale = new Vector3(1.5f, 1.5f, 1);

                /*goc.SetSortingOrder(point.RendererSample.sortingLayerID, point.RendererSample.sortingOrder);*/
                go.transform.SetChildrenLayers(this.gameObject.layer);
            }

            this.gameObject.SetActive(true);
        }

        /// <summary>
        /// 重置点位
        /// </summary>
        public void ResetTeamPointPosition()
        {
            for (int i = 0; i < this.DefaultTeamPointPosition.Length; i++)
            {
                this.TeamPoints[i].transform.position = this.DefaultTeamPointPosition[i];
            }
        }

        public void Clear()
        {
            this.gameObject.SetActive(false);

            for (int i = 0; i < this.TeamPoints.Length; i++)
            {
                var teamPoint = this.TeamPoints[i];
                if (teamPoint == default) continue;
                teamPoint.GOC = default;
            }

            if (gocArray == default) return;

            for (int i = 0; i < this.gocArray.Length; i++)
            {
                Destroy(gocArray[i]);
            }

            this.gocArray = default;
        }
    }
}
