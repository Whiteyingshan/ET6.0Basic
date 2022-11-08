using System;
using System.Collections.Concurrent;
using System.Threading;
using UnityEngine;
using ET;

namespace ET.ThreadController
{
	internal class ActionRunner
	{
		private readonly ManualResetEventSlim _manualResetEventSlim = new ManualResetEventSlim(false);
		private Action _act;

		public ActionRunner(Action act)
		{
			_act = act;
		}

		public void Run()
		{
			try
			{
                _act?.Invoke();
            }
			catch (Exception e)
			{
#if SERVER
                Log.Error(e);
#else
                Debug.LogError(e);
#endif
            }

			_manualResetEventSlim?.Set();
		}

		public void Wait()
		{
			_manualResetEventSlim?.Wait();
		}
	}
}
