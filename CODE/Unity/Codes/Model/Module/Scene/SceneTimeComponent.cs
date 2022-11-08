using System;

namespace ET
{
    public sealed class SceneTimeComponent : Entity
    {
        private sealed class SceneTimeComponentAwake : AwakeSystem<SceneTimeComponent>
        {
            public override void Awake(SceneTimeComponent self)
            {
                self.pauseTime = 0;
                self.startDateTime = DateTime.Now;
                self.pauseDateTime = DateTime.Now;
                self.Pause = false;
                self.Time = 0;
                self.deltaTime = 0;
                self.NowTime = DateTime.Now;
                self.frameTime = 0;
            }
        }

        private sealed class SceneTimeComponentUpdate : UpdateSystem<SceneTimeComponent>
        {
            DateTime now;
            DateTime lastDateTime;
            float lastFrameTime;
            public override void Update(SceneTimeComponent self)
            {
                if (self.Pause)
                {
                    self.deltaTime = 0;
                    self.pauseTime += (float)(DateTime.Now - self.pauseDateTime).TotalSeconds;
                    self.pauseDateTime = DateTime.Now;
                }
                else
                {
                    lastFrameTime = self.Time;
                    now = DateTime.Now;
                    self.Time = (float)((now - self.startDateTime).TotalSeconds) - self.pauseTime;
                    self.deltaTime = self.Time - lastFrameTime;
                }
                lastDateTime = self.NowTime;
                self.NowTime = DateTime.Now;
                self.frameTime = (float)(self.NowTime - lastDateTime).TotalSeconds;
            }
        }

        private float pauseTime = 0;
        private DateTime startDateTime = DateTime.MinValue;
        private DateTime pauseDateTime = DateTime.MinValue;
        private DateTime StartAddTime = DateTime.MinValue;
        public bool Pause { get; private set; }
        public float Time { get; private set; }
        public float deltaTime { get; private set; }

        public DateTime NowTime { get; private set; }
        public float frameTime { get; private set; }
        public void SetPause(bool value)
        {
            Pause = value;
            pauseDateTime = DateTime.Now;
        }
    }
}