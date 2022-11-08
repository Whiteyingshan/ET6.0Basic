using System;
using UnityEngine;

namespace ET
{
    [ObjectSystem]
    public class PingComponentAwakeSystem : AwakeSystem<PingComponent>
    {
        public override void Awake(PingComponent self)
        {
            PingAsync(self).Coroutine();
        }

        private static async ETTask PingAsync(PingComponent self)
        {
            long instanceId = self.InstanceId;
            Session session = self.GetParent<Session>();

            while (true)
            {
                try
                {
                    long time1 = TimeHelper.ClientNow();
                    G2C_Ping response = await session.Call(self.C2G_Ping) as G2C_Ping;
                    if (self.InstanceId != instanceId)
                    {
                        return;
                    }

                    long time2 = TimeHelper.ClientNow();
                    self.Ping = time2 - time1;
                    Game.TimeInfo.ServerMinusClientTime = response.Time + (time2 - time1) / 2 - time2;

                    await TimerComponent.Instance.WaitAsync(2000);
                    if (self.InstanceId != instanceId)
                    {
                        SessionComponent.Inst.Dispose();
                        NoticeWindowStyle noticeWindowStyle = new NoticeWindowStyle();
                        noticeWindowStyle.AddOkCallBack(async () =>
                        {
                            Application.Quit();
                            await ETTask.CompletedTask;
                        });
                        NoticeWindowPanel.Instance.ShowNoticeWindow(21000023, noticeWindowStyle).Coroutine();
                        return;
                    }
                }
                catch (RpcException e)
                {
                    // session断开导致ping rpc报错，记录一下即可，不需要打成error
                    Log.Info($"ping error: {self.Id} {e.Error}");
                    return;
                }
                catch (Exception e)
                {
                    Log.Error($"ping error: \n{e}");
                }
            }
        }
    }

    [ObjectSystem]
    public class PingComponentDestroySystem : DestroySystem<PingComponent>
    {
        public override void Destroy(PingComponent self)
        {
            self.Ping = default;
        }
    }
}