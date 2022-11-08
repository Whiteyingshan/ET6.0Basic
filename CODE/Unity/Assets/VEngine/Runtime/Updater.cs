using System;
using UnityEngine;

namespace VEngine
{
    /// <summary>
    ///     更新器，运行时所有需要分帧的 Update 操作通过此类集中处理，并尽可能将单帧的 Update 操作
    ///     控制在 maxUpdateTimeSlice 以内，从而让程序的流畅度得到控制。
    /// </summary>
    public sealed class Updater : MonoBehaviour
    {
        /// <summary>
        ///     单帧最大处理的时间片（毫秒）
        /// </summary>
        public float maxUpdateTimeSlice = 10;

        /// <summary>
        ///     当前帧的初始时间，单位毫秒。
        /// </summary>
        public double time { get; private set; }

        /// <summary>
        ///     判断当前更新器是否处于超时状态，如果超时表示当前帧已经满负荷了，余下的操作应该放到下一帧处理。
        /// </summary>
        public bool busy => DateTime.Now.TimeOfDay.TotalMilliseconds - time >= maxUpdateTimeSlice;

        public static Updater Instance { get; private set; }

        public Action onUpdate { get; set; }

        private void Update()
        {
            time = DateTime.Now.TimeOfDay.TotalMilliseconds;
            if (onUpdate != null)
            {
                onUpdate();
            }
        }

        public void AddUpdateCallback(Action callback)
        {
            onUpdate += callback;
        }

        public void RemoveUpdateCallback(Action callback)
        {
            onUpdate -= callback;
        }

        public static void Initialize(params Action[] actions)
        {
            if (Instance == null)
            {
                Instance = FindObjectOfType<Updater>();
            }

            if (Instance == null)
            {
                var go = new GameObject("Updater");
                DontDestroyOnLoad(go);
                Instance = go.AddComponent<Updater>();
                foreach (var action in actions)
                {
                    Instance.AddUpdateCallback(action);
                }
            }
        }
    }
}