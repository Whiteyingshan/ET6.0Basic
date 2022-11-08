using System;
using System.Collections.Concurrent;
using System.Threading;
using UnityEngine;

namespace ET.ThreadController
{
	public class ThreadSynchronizationContext : SynchronizationContext
	{
		public static ThreadSynchronizationContext Instance { get; } = new ThreadSynchronizationContext(Thread.CurrentThread.ManagedThreadId);

		private readonly int threadId;

		private readonly int mainThreadId = Thread.CurrentThread.ManagedThreadId;

		// 线程同步队列,发送接收socket回调都放到该队列,由poll线程统一执行
		private readonly ConcurrentQueue<Action> queue = new ConcurrentQueue<Action>();
		// 其他线程抛向主线程的事件队列 统一由 ThreadPostToMain 回调到主线程
		private readonly ConcurrentQueue<ActionRunner> manualResetEventQueue = new ConcurrentQueue<ActionRunner>();

		private Action a;
		private ActionRunner aR;

		public ThreadSynchronizationContext(int threadId)
		{
			this.threadId = threadId;
		}

		public void Update()
		{
			while (true)
			{
				if (!this.queue.TryDequeue(out a))
				{
					break;
				}

				try
				{
					a();
				}
				catch (Exception e)
				{
#if SERVER
					Log.Error(e);
#else
					Debug.LogError(e);
#endif
				}
			}

			lock (this.manualResetEventQueue)
			{
				while (this.manualResetEventQueue.Count > 0)
				{
					if (!this.manualResetEventQueue.TryDequeue(out this.aR))
					{
						break;
					}

					aR.Run();
				}
			}
		}

		public override void Post(SendOrPostCallback callback, object state)
		{
			this.Post(() => callback(state));
		}

		public void Post(Action action)
		{
			if (Thread.CurrentThread.ManagedThreadId == this.threadId)
			{
				try
				{
					action();
				}
				catch (Exception ex)
				{
#if SERVER
					Log.Error(ex);
#else
					Debug.LogError(ex);
#endif
				}

				return;
			}

			this.queue.Enqueue(action);
		}

		public void PostNext(Action action)
		{
			this.queue.Enqueue(action);
		}


		private bool IsMainThread()
		{
			return Thread.CurrentThread.ManagedThreadId == this.mainThreadId;
		}
	}
}
