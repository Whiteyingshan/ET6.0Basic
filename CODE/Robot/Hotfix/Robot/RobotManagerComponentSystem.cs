using System;
using System.Linq;

namespace ET
{
    public static class RobotManagerComponentSystem
    {
        public static async ETTask<Scene> NewRobot(this RobotManagerComponent self, int zone)
        {
            Scene zoneScene = null;
            try
            {
                zoneScene = await SceneFactory.CreateZoneScene(zone, "Robot", self);
                await LoginHelper.LoginRobot(zoneScene, ConstValue.LoginAddress, zone.ToString(), zone.ToString());
                Log.Debug($"create robot ok: {zone}");
                return zoneScene;
            }
            catch (Exception e)
            {
                zoneScene?.Dispose();
                throw new Exception($"RobotSceneManagerComponent create robot fail, zone: {zone}", e);
            }
        }
        
        public static void RemoveAll(this RobotManagerComponent self)
        {
            foreach (Entity robot in self.Children.Values.ToArray())        
            {
                robot.Dispose();
            }
        }
        
        public static void Remove(this RobotManagerComponent self, long id)
        {
            self.GetChild<Scene>(id)?.Dispose();
        }

        public static void Clear(this RobotManagerComponent self)
        {
            foreach (Entity entity in self.Children.Values.ToArray())
            {
                entity.Dispose();
            }
        }
    }
}