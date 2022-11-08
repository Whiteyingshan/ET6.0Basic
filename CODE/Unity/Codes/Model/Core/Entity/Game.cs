using System;
using System.Collections.Generic;
#if !NOT_UNITY
using ET.ThreadController;
using UnityEngine;
#endif
namespace ET
{
    public static class Game
    {
        private static Scene scene;
        public static Scene Scene
        {
            get
            {
                if (scene != null)
                {
                    return scene;
                }
                return scene = EntitySceneFactory.CreateScene(0, SceneType.Process, "Process");
            }
        }
#if !NOT_UNITY
        public static Transform GOPool;
#endif
        public static EventSystem EventSystem => EventSystem.Instance;
        public static IdGenerater IdGenerater => IdGenerater.Instance;
        public static ObjectPool ObjectPool => ObjectPool.Instance;
        public static Options Options => Options.Instance;
        public static ThreadSynchronizationContext ThreadSynchronizationContext => ThreadSynchronizationContext.Instance;
        public static TimeInfo TimeInfo => TimeInfo.Instance;

        public static readonly List<Action> FrameFinishCallback = new List<Action>();

        public static void Update()
        {
            ThreadSynchronizationContext.Update();
            TimeInfo.Update();
            EventSystem.Update();
        }

        public static void LateUpdate()
        {
            EventSystem.LateUpdate();
        }

        public static void FrameFinish()
        {
            foreach (Action action in FrameFinishCallback)
            {
                action.Invoke();
            }
            FrameFinishCallback.Clear();
        }

        public static void Close()
        {
            scene?.Dispose();
            scene = null;
            MonoPool.Instance.Dispose();
            EventSystem.Instance.Dispose();
            IdGenerater.Instance.Dispose();
        }
    }
}