using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace ET
{
    // 1 mono模式 2 ILRuntime模式 3 mono热重载模式
    public enum CodeMode
    {
        Mono = 1,
        ILRuntime = 2,
        Reload = 3,
    }

    public class Init : MonoBehaviour
    {
        public CodeMode CodeMode = CodeMode.Mono;

        private void Awake()
        {
#if ENABLE_IL2CPP
            this.CodeMode = CodeMode.ILRuntime;
#endif
            DontDestroyOnLoad(gameObject);

            Log.ILog = new UnityLogger();

            Options.Instance = new Options();

            ETTask.ExceptionHandler += Log.Error;

            AppDomain.CurrentDomain.UnhandledException += LogError;

            SynchronizationContext.SetSynchronizationContext(ThreadSynchronizationContext.Instance);

            CodeLoader.Instance.CodeMode = this.CodeMode;
        }

        private void Start()
        {
            StartAsync().Coroutine();
        }

        private async ETTask StartAsync()
        {
            await AssetsLoader.Instance.Update();
            await CodeLoader.Instance.Start();
        }

        private void Update()
        {
            CodeLoader.Instance.Update?.Invoke();
        }

        private void LateUpdate()
        {
            CodeLoader.Instance.LateUpdate?.Invoke();
        }

        private void OnApplicationQuit()
        {
            CodeLoader.Instance.OnApplicationQuit?.Invoke();
            CodeLoader.Instance.Dispose();
        }

        private void LogError(object sender, UnhandledExceptionEventArgs e)
        {
            Log.Error(e.ExceptionObject.ToString());
        }
    }
}