using System.Collections.Generic;
using System.Net;

namespace ET
{
    public class AppStart_Init : AEvent<EventType.AppStart>
    {
        protected override async ETTask Run(EventType.AppStart args)
        {
            Game.Scene.AddComponent<ConfigComponent>().Load();
            Game.Scene.AddComponent<TimerComponent>();
            Game.Scene.AddComponent<NetThreadComponent>();
            Game.Scene.AddComponent<OpcodeTypeComponent>();
            Game.Scene.AddComponent<CoroutineLockComponent>();
            Game.Scene.AddComponent<SessionStreamDispatcher>();
            Game.Scene.AddComponent<MessageDispatcherComponent>();
            Game.Scene.AddComponent<ActorMessageDispatcherComponent>();
            // 发送普通actor消息
            Game.Scene.AddComponent<ActorMessageSenderComponent>();
            // 发送location actor消息
            Game.Scene.AddComponent<ActorLocationSenderComponent>();
            // 访问location server的组件
            Game.Scene.AddComponent<LocationProxyComponent>();
            // 数值订阅组件
            Game.Scene.AddComponent<NumericWatcherComponent>();
            Game.Scene.AddComponent<MessageConvertComponent>();

            switch (Game.Options.AppType)
            {
                case AppType.Server:
                    {
                        StartProcessConfig processConfig = StartProcessConfigCategory.Instance.Get(Game.Options.Process);
                        Game.Scene.AddComponent<NetInnerComponent, IPEndPoint, int>(processConfig.InnerIPPort, SessionStreamDispatcherType.SessionStreamDispatcherServerInner);

                        List<StartSceneConfig> processScenes = StartSceneConfigCategory.Instance.GetByProcess(Game.Options.Process);
                        foreach (StartSceneConfig startConfig in processScenes)
                        {
                            await SceneFactory.Create(Game.Scene, startConfig.Id, startConfig.InstanceId, startConfig.Zone, startConfig.Name, startConfig.Type, startConfig);
                        }
                        break;
                    }
                case AppType.Watcher:
                    {
                        StartMachineConfig startMachineConfig = WatcherHelper.GetThisMachineConfig();
                        Game.Scene.AddComponent<NetInnerComponent, IPEndPoint, int>(NetworkHelper.ToIPEndPoint($"{startMachineConfig.InnerIP}:{startMachineConfig.WatcherPort}"), SessionStreamDispatcherType.SessionStreamDispatcherServerInner);

                        Game.Scene.AddComponent<WatcherComponent>().Start(Game.Options.CreateScenes);
                        break;
                    }
                case AppType.GameTool:
                    break;
            }
            if (Game.Options.Console == 1)
            {
                Game.Scene.AddComponent<ConsoleComponent>();
            }
        }
    }
}