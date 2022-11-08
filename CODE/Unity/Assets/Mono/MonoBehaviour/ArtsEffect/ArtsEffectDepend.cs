using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;

namespace ET
{
    public enum SkillSpriteBehaviourType
    {
        None = 0,
        /// <summary>
        /// 飞行
        /// </summary>
        fly,
        /// <summary>
        /// 命中
        /// </summary>
        hit,
    }

    [Serializable]
    public class FlyBombInfo
    {
        private readonly Queue<GameObject> pool = new Queue<GameObject>();

        /// <summary>
        /// 取出飞弹
        /// </summary>
        public GameObject Fetch()
        {
            GameObject go;
            if (pool.Count == 0)
                go = GameObject.Instantiate(this.FlyBombPrefab);
            else
                go = pool.Dequeue();

            go.transform.SetParent(this.FlyBombGeneratePoint);
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.Euler(Vector3.zero);
            go.transform.localScale = this.FlyBombScale;

            return go;
        }

        /// <summary>
        /// 回收飞弹
        /// </summary>
        public void Recycle(GameObject go)
        {
            go.SetActive(false);
            this.pool.Enqueue(go);
/*#if NET452
            go.transform.SetParent(Game.GOPool);
#endif*/
        }

        /// <summary>
        /// 飞弹预制体资源路径
        /// </summary>
        [Tooltip("飞弹预制体")]
        public GameObject FlyBombPrefab;
        /// <summary>
        /// 飞弹缩放
        /// </summary>
        [Tooltip("飞弹缩放")]
        public Vector2 FlyBombScale;
        /// <summary>
        /// 飞弹移动速度
        /// </summary>
        [Tooltip("飞弹移动速度")]
        public float FlyBombMoveSpeed;
        /// <summary>
        /// 飞弹生成点
        /// </summary>
        [Tooltip("飞弹生成位置")]
        public Transform FlyBombGeneratePoint;
        /// <summary>
        /// 是否应用命中偏移
        /// </summary>
        [Tooltip("是否应用命中偏移")]
        public bool IsApplyHitOffset = true;
        /// <summary>
        /// 是否应用命中偏移
        /// </summary>
        [Tooltip("是否朝向目标")]
        public bool IsLookAt = true;
    }

    public class ArtsEffectDepend : MonoBehaviour
    {
        /// <summary>
        /// 飞弹信息
        /// </summary>
        [Tooltip("飞弹预制体配置")]
        public FlyBombInfo[] FlyBombConfigs;

        /// <summary>
        /// 命中特效
        /// </summary>
        [Tooltip("命中敌人时会在敌人身上生成该特效")]
        public GameObject[] HitEffectPrefabs;

        /// <summary>
        /// 当前特效的播放控制器
        /// </summary>
        public PlayableDirector PlayableDirector => this.GetComponent<PlayableDirector>();

        public void ClearCallBack()
        {
            //this.PlayableDirector.stopped -= 
            ////this.PlayableDirector.stopped = default;

        }

        ///// <summary>
        ///// 
        ///// </summary>
        //public GameObject FetchBomb(int index = -1)
        //{
        //    if (this.FlyBombPrefabs.Length <= 0)
        //    {
        //        throw new Exception($"不存在绑定飞弹，无法发射飞弹！");
        //    }

        //    GameObject flyBombPrefab;

        //    if (index == -1)
        //    {
        //        index = UnityEngine.Random.Range(0, this.FlyBombPrefabs.Length);
        //        flyBombPrefab = this.FlyBombPrefabs[index];
        //    }
        //    else
        //    {
        //        flyBombPrefab = this.FlyBombPrefabs[index];
        //    }

        //    if (flyBombPrefab == default)
        //    {
        //        throw new Exception($"发射器{this.name}没有指定飞弹预制体！");
        //    }

        //    return Fetch(flyBombPrefab, this.FlyBombGeneratePoint);
        //}

        ///// <summary>
        ///// -1 表示从所有改特效绑定飞弹中随即一个
        ///// </summary>
        ///// <param name="index"></param>
        ///// <param name="hitAction"></param>
        ///// <returns></returns>
        //public PlayableDirector Shot(int index = -1)
        //{
        //    if (this.FlyBombPrefabs.Length <= 0)
        //    {
        //        throw new Exception($"不存在绑定飞弹，无法发射飞弹！");
        //    }

        //    GameObject flyBombPrefab;

        //    if (index == -1)
        //    {
        //        index = UnityEngine.Random.Range(0, this.FlyBombPrefabs.Length);
        //        flyBombPrefab = this.FlyBombPrefabs[index];
        //    }
        //    else
        //    {
        //        flyBombPrefab = this.FlyBombPrefabs[index];
        //    }

        //    if (flyBombPrefab == default)
        //    {
        //        throw new Exception($"发射器{this.name}没有指定飞弹预制体！");
        //    }

        //    GameObject flyBomb = Fetch(flyBombPrefab, this.FlyBombGeneratePoint);
        //    PlayableDirector flyPD = flyBomb.transform.Find(SkillSpriteBehaviourType.fly.ToString()).GetComponent<PlayableDirector>();
        //    if (flyPD == default)
        //    {
        //        Debug.LogError($"飞弹{flyBombPrefab.name}不存在飞行动画控制器");
        //        return default;
        //    }

        //    PlayableDirector hitPD = flyBomb.transform.Find(SkillSpriteBehaviourType.hit.ToString()).GetComponent<PlayableDirector>();
        //    if (hitPD == default)
        //    {
        //        Debug.LogError($"飞弹{flyBombPrefab.name}不存在命中动画控制器");
        //        return default;
        //    }

        //    flyPD.Play();

        //    return hitPD;
        //}
    }
}
