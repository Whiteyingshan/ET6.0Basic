using ET.Module.Numeric;
using ET.Module.ViewPanel;
using FairyGUI;
using Spine.Unity;

namespace ET
{
    public sealed class AppStart : AEvent<EventType.AppStart>
    {
        protected override async ETTask Run(EventType.AppStart args)
        {
            Game.Scene.AddComponent<TimerComponent>();
            Game.Scene.AddComponent<CoroutineLockComponent>();

            // 加载配置
            Game.Scene.AddComponent<ConfigComponent>().Load();

            Game.Scene.AddComponent<OpcodeTypeComponent>();
            Game.Scene.AddComponent<MessageDispatcherComponent>();
            Game.Scene.AddComponent<SessionStreamDispatcher>();

            Game.Scene.AddComponent<NetThreadComponent>();
            Game.Scene.AddComponent<ZoneSceneManagerComponent>();

            Game.Scene.AddComponent<LoginServerComponent>();
            Game.Scene.AddComponent<MessageConvertComponent>();
            Game.Scene.AddComponent<NumericWatcherComponent>();
            Game.Scene.AddComponent<NumericExpressionComponent>();

            Game.Scene.AddComponent<ViewDispatcherComponent>();
            Game.Scene.AddComponent<TweenControllerComponent>();
            Game.Scene.AddComponent<TimeControlComponent>();
            AssetHelper.Init();

            GRoot.inst.SetContentScaleFactor(1280, 720);
            GRoot.inst.MakeFullScreen();
            Scene zoneScene = await SceneFactory.CreateZoneScene(1, "Game", Game.Scene);
            await Game.EventSystem.Publish(new EventType.CreatePublicPanel() { ZoneScene = zoneScene });
            await Game.EventSystem.Publish(new EventType.CreateLoginPanel() { ZoneScene = zoneScene });
        }
    }
}