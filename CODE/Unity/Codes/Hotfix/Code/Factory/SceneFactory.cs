namespace ET
{
#if !SERVER
    public static class SceneFactory
    {
        public static async ETTask<Scene> CreateZoneScene(int zone, string name, Entity parent)
        {
            Scene zoneScene = EntitySceneFactory.CreateScene(zone, SceneType.Zone, name, parent);
            zoneScene.AddComponent<ZoneSceneFlagComponent>();
            zoneScene.AddComponent<NetKcpComponent, int>(SessionStreamDispatcherType.SessionStreamDispatcherClientOuter);
            zoneScene.AddComponent<SceneTimeComponent>();
            await Game.EventSystem.Publish(new EventType.AfterCreateZoneScene() { ZoneScene = zoneScene });
            return zoneScene;
        }
    }
#endif
}