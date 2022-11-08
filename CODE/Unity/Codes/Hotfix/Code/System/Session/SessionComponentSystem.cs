using System.IO;

namespace ET
{
    internal static class SessionComponentSystem
    {
        private sealed class SessionComponentAwakeSystem : AwakeSystem<SessionComponent>
        {
            public override void Awake(SessionComponent self)
            {
                SessionComponent.Inst = self;
            }
        }

        private sealed class SessionComponentDestroySystem : DestroySystem<SessionComponent>
        {
            public override void Destroy(SessionComponent self)
            {
                SessionComponent.Inst = null;
                self.Session?.Dispose();
            }
        }

        public static void Send(this SessionComponent self, IMessage message)
        {
            self.Session.Send(message);
        }

        public static void Send(this SessionComponent self, long actorId, IMessage message)
        {
            self.Session.Send(actorId, message);
        }

        public static void Send(this SessionComponent self, long actorId, MemoryStream memoryStream)
        {
            self.Session.Send(actorId, memoryStream);
        }

        public static ETTask<IResponse> Call(this SessionComponent self, IRequest request)
        {
            return self.Session.Call(request);
        }

        public static ETTask<IResponse> Call(this SessionComponent self, IRequest request, ETCancellationToken cancellationToken)
        {
            return self.Session.Call(request, cancellationToken);
        }

        public static async ETTask<TResponse> Call<TResponse>(this SessionComponent self, IRequest request) where TResponse : class, IResponse
        {
            return await self.Session.Call(request) as TResponse;
        }

        public static async ETTask<TResponse> Call<TResponse>(this SessionComponent self, IRequest request, ETCancellationToken cancellationToken) where TResponse : class, IResponse
        {
            return await self.Session.Call(request, cancellationToken) as TResponse;
        }
    }
}