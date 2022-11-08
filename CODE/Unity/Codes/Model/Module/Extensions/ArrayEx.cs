using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace ET
{
    public static class ArrayEx
    {
        /// <summary>
        /// 二维数组螺旋迭代方向
        /// </summary>
        private enum DoubleArrayDiffusionMoveNextDirection
        {
            Rigth,
            Down,
            Left,
            Up
        }
        /// <summary>
        /// 二维数组螺旋遍历迭代器        
        /// </summary>
        private static readonly Func<Vector2Int, Vector2Int>[] DoubleArrayDiffusionMoveNextMethods = new Func<Vector2Int, Vector2Int>[4];

        static ArrayEx()
        {
            DoubleArrayDiffusionMoveNextMethods[0] = v2Int => new Vector2Int(v2Int.x + 1, v2Int.y);
            DoubleArrayDiffusionMoveNextMethods[1] = v2Int => new Vector2Int(v2Int.x, v2Int.y + 1);
            DoubleArrayDiffusionMoveNextMethods[2] = v2Int => new Vector2Int(v2Int.x - 1, v2Int.y);
            DoubleArrayDiffusionMoveNextMethods[3] = v2Int => new Vector2Int(v2Int.x, v2Int.y - 1);
        }

        /// <summary>
        /// 获取某二维数组对应二维索引的一维数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static T[] GetArrayFormIndex<T>(this T[,] self, int index)
        {
            int length = self.GetLength(1);
            T[] array = new T[length];

            for (int i = 0; i < length; i++)
            {
                array[i] = self[index, i];
            }

            return array;
        }


        /// <summary>
        /// 索引数据是否有效
        /// </summary>
        /// <param name="self"></param>
        /// <param name="point">*** x = row ; y = col ***</param>
        /// <typeparam name="T"></typeparam>
        /// <returns>true = 合法false反之</returns>
        public static bool IsIndexValid<T>(this T[][] self, Vector2Int point)
        {
            return !(point.y < 0 || point.y >= self.Length || point.x < 0 || point.x >= self[point.y].Length);
        }

        /// <summary>
        /// 二维数组方向迭代器
        /// </summary>
        /// <param name="self"></param>
        /// <param name="originPoint">起点</param>
        /// <param name="direction">方向 参考枚举类型：DoubleArrayDiffusionMoveNextDirection</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public static T DirectionMoveNext<T>(this T[][] self, Vector2Int originPoint, int direction)
        {
            Vector2Int targetPoint = DoubleArrayDiffusionMoveNextMethods[direction].Invoke(originPoint);
            if (self.IsIndexValid(targetPoint))
                throw new IndexOutOfRangeException();

            return self[targetPoint.y][targetPoint.x];
        }

        /// <summary>
        /// 螺旋扩散遍历查找所需的数据
        /// </summary>
        /// <param name="self"></param>
        /// <param name="centerPoint">扩散中心点</param>
        /// <param name="predicate">成立条件</param>
        /// <param name="originSearchRange">起始搜寻范围</param>
        /// <param name="maxSearchRange">最大搜寻范围（圈数）</param>
        /// <typeparam name="T">数组的类型</typeparam>
        /// <returns>返回目标距离数据索引坐标最近的索引坐标</returns>
        public static async ETTask<Vector2Int> DiffusionSearch<T>(this T[][] self, Vector2Int centerPoint, Predicate<T> predicate, ETCancellationToken cancellationToken = default, int originSearchRange = 1, int maxSearchRange = 100)
        {
            //循环起点索引
            Vector2Int loopOriginPoint = centerPoint;

            //无论在任意方向上遍历出可用数据都将进入该容器
            Dictionary<float, Vector2Int> distancePointPair = new Dictionary<float, Vector2Int>();
            //搜寻计数
            int searchCount = originSearchRange;

            //最大遍历范围
            for (int i = 0; i < maxSearchRange - originSearchRange; i++)
            {
                //重置循环起始点
                loopOriginPoint.x = loopOriginPoint.x - 1;
                loopOriginPoint.y = loopOriginPoint.y - 1;

                //转向起始点
                Vector2Int curPoint = loopOriginPoint;
                //转向计数（二维数组只需要4个方向）
                for (int j = 0; j < 4; j++)
                {
                    //搜寻圈内转向所需次数
                    int switchCount = searchCount * 2;

                    //转向前该方向上需要的遍历循环
                    for (int k = 0; k < switchCount; k++)
                    {
                        curPoint = DoubleArrayDiffusionMoveNextMethods[j].Invoke(curPoint);

                        if (!self.IsIndexValid(curPoint)) continue;

                        // Vector3 v = Game.Scene.GetComponent<MapComponent>().GetLocationByMapDataIndex(curPoint);
                        // MapComponent.MapInfoGizmosTestMono.PathNodeVector3s.Add(v);
                        // TimerComponent.Instance.WaitAsync(5000).GetAwaiter().OnCompleted(() => { MapComponent.MapInfoGizmosTestMono.PathNodeVector3s.Remove(v); });

                        if (predicate.Invoke(self[curPoint.y][curPoint.x]))
                        {
                            // Vector3 v2 = Game.Scene.GetComponent<MapComponent>().GetLocationByMapDataIndex(curPoint);
                            // MapComponent.MapInfoGizmosTestMono.PathNodeVector3s2.Add(v2);
                            // TimerComponent.Instance.WaitAsync(5000).GetAwaiter().OnCompleted(() => { MapComponent.MapInfoGizmosTestMono.PathNodeVector3s2.Remove(v2); });

                            float distance = Vector2Int.Distance(centerPoint, curPoint);
                            if (!distancePointPair.ContainsKey(distance))
                            {
                                distancePointPair.Add(distance, curPoint);
                            }
                        }

                        if (cancellationToken != default && cancellationToken.IsCancel())
                            return Vector2Int.zero;
                    }
                }

                if (distancePointPair.Count > 0)
                {
                    return distancePointPair.First().Value;
                }

                searchCount++;

                if (i > 100)
                {
                    await TimerComponent.Instance.WaitFrameAsync(cancellationToken);

                    if (cancellationToken != default && cancellationToken.IsCancel())
                        return Vector2Int.zero;
                }
            }

            throw new Exception($"需要优化的数据源，定义的数据源在您定义的范围:{maxSearchRange}内没有有效数据！");
        }
    }
}