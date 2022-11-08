using System;

namespace ET
{
    public static class Entry
    {
        public static void Start()
        {
            try
            {
                CodeLoader.Instance.Reload = Reload;
                CodeLoader.Instance.Update += Game.Update;
                CodeLoader.Instance.LateUpdate += Game.LateUpdate;
                CodeLoader.Instance.OnApplicationQuit += Game.Close;

                Game.EventSystem.Add(CodeLoader.Instance.GetTypes());

                Game.EventSystem.Publish(new EventType.AppStart()).Coroutine();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        public static void Reload()
        {
            CodeLoader.Instance.LoadLogic();
            Game.EventSystem.Add(CodeLoader.Instance.GetTypes());
            Game.EventSystem.Load();
            Log.Info("代码重载!");
        }
    }
}