namespace ET
{
    public static class EntitySceneFactory
    {
        public static Scene CreateScene(long id, long instanceId, int zone, SceneType sceneType, string name, Entity parent = null)
        {
            return new Scene(id, instanceId, zone, sceneType, name, parent);
        }

        public static Scene CreateScene(long instanceId, int zone, SceneType sceneType, string name, Entity parent = null)
        {
            return new Scene(instanceId, zone, sceneType, name, parent);
        }

        public static Scene CreateScene(int zone, SceneType sceneType, string name, Entity parent = null)
        {
            return new Scene(IdGenerater.Instance.GenerateInstanceId(), zone, sceneType, name, parent);
        }
    }
}