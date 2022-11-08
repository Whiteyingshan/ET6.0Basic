using System.Net;

namespace ET
{
    public static class SceneFactory
    {
        public static async ETTask<Scene> Create(Entity parent, string name, SceneType sceneType)
        {
            long instanceId = IdGenerater.Instance.GenerateInstanceId();
            return await Create(parent, instanceId, instanceId, parent.DomainZone(), name, sceneType);
        }

        public static async ETTask<Scene> Create(Entity parent, long id, long instanceId, int zone, string name, SceneType sceneType, StartSceneConfig startSceneConfig = null)
        {
            Scene scene = EntitySceneFactory.CreateScene(id, instanceId, zone, sceneType, name, parent);
            StartZoneConfig startZoneConfig = StartZoneConfigCategory.Instance.Get(zone);
            scene.AddComponent<MailBoxComponent, MailboxType>(MailboxType.UnOrderMessageDispatcher);

            switch (scene.SceneType)
            {
                case SceneType.Realm:
                    scene.AddComponent<NetKcpComponent, IPEndPoint, int>(startSceneConfig.BindIP, SessionStreamDispatcherType.SessionStreamDispatcherServerOuter);
                    scene.AddComponent<DBComponent, string, string>(startZoneConfig.DBConnection, startZoneConfig.DBName);
                    scene.AddComponent<GateSessionKeyComponent>();
                    scene.AddComponent<AccountZoneSetComponent>();
                    break;
                case SceneType.Gate:
                    scene.AddComponent<NetKcpComponent, IPEndPoint, int>(startSceneConfig.BindIP, SessionStreamDispatcherType.SessionStreamDispatcherServerOuter);
                    scene.AddComponent<DBComponent, string, string>(startZoneConfig.DBConnection, startZoneConfig.DBName);
                    scene.AddComponent<AccountZoneSetComponent>();
                    scene.AddComponent<PlayerSetComponent>();
                    break;
                case SceneType.Location:
                    scene.AddComponent<LocationComponent>();
                    break;
                default:
                    await ETTask.CompletedTask;
                    break;
            }
            return scene;
        }
    }
}