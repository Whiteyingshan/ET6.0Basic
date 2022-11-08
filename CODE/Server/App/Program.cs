using System;
using System.Threading;
using CommandLine;
using NLog;

namespace ET
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            ETTask.ExceptionHandler += Log.Error;
            AppDomain.CurrentDomain.UnhandledException += (sender, e) => Log.Error(e.ExceptionObject.ToString());

            // 异步方法全部会回掉到主线程
            SynchronizationContext.SetSynchronizationContext(ThreadSynchronizationContext.Instance);

            try
            {
                Game.EventSystem.Add(typeof(Game).Assembly);
                Game.EventSystem.Add(DllHelper.GetHotfixAssembly());

                MongoRegister.Init();
                ProtobufHelper.Init();

                // 命令行参数
                Parser.Default
                    .ParseArguments<Options>(args)
                    .WithParsed(o => { Options.Instance = o; })
                    .WithNotParsed(error => throw new Exception($"命令行格式错误!"));

                Log.ILog = new NLogger(Game.Options.AppType.ToString());
                LogManager.Configuration.Variables["appIdFormat"] = $"{Game.Options.Process:000000}";

                Log.Info($"server start........................ {Game.Scene.Id}");

                Game.EventSystem.Publish(new EventType.AppStart()).Coroutine();

                while (true)
                {
                    try
                    {
                        Thread.Sleep(1);
                        Game.Update();
                        Game.LateUpdate();
                        Game.FrameFinish();
                    }
                    catch (Exception e)
                    {
                        Log.Error(e);
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}