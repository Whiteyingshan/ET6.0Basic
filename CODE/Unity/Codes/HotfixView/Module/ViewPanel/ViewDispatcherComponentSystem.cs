using System;
using System.Collections.Generic;

namespace ET.Module.ViewPanel
{
    public static class ViewDispatcherComponentSystem
    {
        private sealed class ViewDispatcherComponentAwakeSystem : AwakeSystem<ViewDispatcherComponent>
        {
            public override void Awake(ViewDispatcherComponent self)
            {
                self.LoadType();
            }
        }

        private sealed class ViewDispatcherComponentLoadSystem : LoadSystem<ViewDispatcherComponent>
        {
            public override void Load(ViewDispatcherComponent self)
            {
                self.LoadType();
            }
        }

        private static void LoadType(this ViewDispatcherComponent self)
        {
            self.ViewTypes.Clear();
            HashSet<Type> types = Game.EventSystem.GetTypes(typeof(ViewMethodAttribute));
            foreach (Type type in types)
            {
                ViewMethodAttribute attribute = type.GetCustomAttribute<ViewMethodAttribute>();
                self.ViewTypes.Add(attribute.UIResName, type);
            }
        }
    }
}