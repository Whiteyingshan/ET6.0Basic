using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FairyGUI;

namespace ET
{
    public static class TransitionExtension
    {
        public static ETTask PlayAsync(this Transition transition, ETCancellationToken token = null)
        {
            ETTask tcs = ETTask.Create();
            token?.Add(Cancel);
            transition.Play(() => tcs.SetResult());
            return tcs;

            void Cancel()
            {
                transition.Stop();
            }
        }
    }
}
