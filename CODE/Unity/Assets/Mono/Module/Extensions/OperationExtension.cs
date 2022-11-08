using System.Threading.Tasks;
using VEngine;

namespace ET
{
    public static class OperationExtension
    {
        public static Task<Asset> ETAsync(this Asset self)
        {
            TaskCompletionSource<Asset> tcs = new TaskCompletionSource<Asset>();

            self.completed += (op) =>
            {
                tcs.SetResult(op);
            };

            return tcs.Task;
        }

        public static Task<Scene> ETAsync(this Scene self)
        {
            TaskCompletionSource<Scene> tcs = new TaskCompletionSource<Scene>();

            self.completed += (scene) =>
            {
                tcs.SetResult(scene);
            };

            return tcs.Task;
        }

        public static Task ETAsync(this Operation self)
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

            self.completed += (op) =>
            {
                tcs.SetResult(true);
            };

            return tcs.Task;
        }

        public static Task<T> ETAsync<T>(this Operation self) where T : Operation
        {
            TaskCompletionSource<T> tcs = new TaskCompletionSource<T>();

            self.completed += (op) =>
            {
                tcs.SetResult(op as T);
            };

            return tcs.Task;
        }
    }
}