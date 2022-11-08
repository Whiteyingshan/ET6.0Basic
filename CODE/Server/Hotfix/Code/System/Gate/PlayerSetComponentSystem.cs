using System.Collections.Generic;

namespace ET
{
    public static class PlayerSetComponentSystem
    {
        public sealed class PlayerSetComponentAwake : AwakeSystem<PlayerSetComponent>
        {
            public override void Awake(PlayerSetComponent self)
            {
                self.CallNextDay().Coroutine();
            }
        }

        private static async ETTask CallNextDay(this PlayerSetComponent self)
        {
            long instId = self.InstanceId;
            List<long> ids = new List<long>();
            long nextDay = TimeHelper.NextDayTime();
            await TimerComponent.Instance.WaitAsync(nextDay);
            while (instId == self.InstanceId)
            {
                ids.Clear();
                ids.AddRange(self.Keys);
                for (int i = 0; i < ids.Count; i++)
                {
                    self.Get(ids[i])?.OnNextDay();
                    await TimerComponent.Instance.WaitFrameAsync();
                }
                nextDay = TimeHelper.NextDayTime();
                await TimerComponent.Instance.WaitAsync(nextDay);
            }
        }

        public static ETTask Save(this PlayerSetComponent self, long id)
        {
            Player player = self.Get(id);
            return self.DomainScene().GetComponent<DBComponent>().Save(player);
        }
    }
}