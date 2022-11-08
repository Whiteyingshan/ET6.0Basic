using System;
using System.Collections.Generic;
using ET.Module.ViewPanel;
using FairyGUI;

namespace ET
{
    public static class ViewControllerComponentSystem
    {
        private sealed class ViewControllerComponentAwakeSystem : AwakeSystem<ViewControllerComponent>
        {
            public override void Awake(ViewControllerComponent self)
            {
                ViewControllerComponent.Instance = self;
            }
        }

        private sealed class ViewControllerComponentDestroySystem : DestroySystem<ViewControllerComponent>
        {
            public override void Destroy(ViewControllerComponent self)
            {
                self.Clear();
                ViewControllerComponent.Instance = null;
            }
        }

        public static ETTask InitPanel(this ViewControllerComponent self, GObject gObject)
        {
            return self.InitPanel(gObject.GetFObject());
        }

        public static async ETTask InitPanel(this ViewControllerComponent self, FObject fui)
        {
            if (fui is null)
            {
                Log.Error("初始化的FObject为null");
                return;
            }

            DoubleMap<string, Type> viewTypes = Game.Scene.GetComponent<ViewDispatcherComponent>().ViewTypes;
            if (viewTypes.TryGetValueByKey(fui.ResName, out var type))
            {
                try
                {
                    ViewPanel panel = Activator.CreateInstance(type) as ViewPanel;
                    self.ViewPanels.Add(fui.ResName, panel);
                    panel.FObject = fui;
                    panel.ZoneScene = self.ZoneScene;
                    panel.Init();
                    await panel.InitAsync();
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }

        public static async ETTask InitChildren(this ViewControllerComponent self, FObject fui)
        {
            GObject[] gObjects = fui.GObject.asCom.GetChildren();

            foreach (GObject gObject in gObjects)
            {
                if (gObject is GComponent)
                {
                    await self.InitPanel(gObject);
                }
            }
        }

        public static async ETTask ReinitAll(this ViewControllerComponent self)
        {
            List<GObject> gObjects = new List<GObject>();
            foreach (ViewPanel viewPanel in self.ViewPanels.Values)
            {
                gObjects.Add(viewPanel.FObject.GObject);
            }
            self.ViewPanels.Clear();
            foreach (GObject gObject in gObjects)
            {
                await self.InitPanel(gObject);
            }
        }

        public static ViewPanel Get(this ViewControllerComponent self, string name)
        {
            self.ViewPanels.TryGetValue(name, out ViewPanel viewPanel);
            return viewPanel;
        }

        public static T Get<T>(this ViewControllerComponent self, string name) where T : ViewPanel
        {
            return self.Get(name) as T;
        }

        public static T Get<T>(this ViewControllerComponent self) where T : ViewPanel
        {
            DoubleMap<string, Type> viewTypes = Game.Scene.GetComponent<ViewDispatcherComponent>().ViewTypes;
            return self.Get(viewTypes.GetKeyByValue(typeof(T))) as T;
        }

        public static void Show(this ViewControllerComponent self, string name)
        {
            self.Get(name)?.Show();
        }

        public static void Show(this ViewControllerComponent self, string name, string args)
        {
            self.Get(name)?.Show(args);
        }

        public static void Hide(this ViewControllerComponent self, string name)
        {
            self.Get(name)?.Hide();
        }

        public static void HideAll(this ViewControllerComponent self)
        {
            foreach (var item in self.ViewPanels)
            {
                if (!item.Value.HideAll)
                {
                    continue;
                }

                item.Value.Hide();
            }
        }

        public static void CloseAll(this ViewControllerComponent self)
        {
            foreach (var item in self.ViewPanels)
            {
                if (!item.Value.HideAll)
                {
                    continue;
                }
                if (item.Value.FObject == null)
                {
                    continue;
                }
                item.Value.Close();
            }
        }

        public static void Remove(this ViewControllerComponent self, string name)
        {
            ViewPanel viewPanel = self.Get(name);
            if (viewPanel != null)
            {
                viewPanel.Dispose();
            }
        }

        public static void Clear(this ViewControllerComponent sellf)
        {
            foreach (var item in sellf.ViewPanels)
            {
                item.Value.Dispose();
            }
            sellf.ViewPanels.Clear();
        }
    }
}