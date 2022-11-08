using ET.DengLuZhuCe;
using ET.EventType;

namespace ET.Code.Events.UI
{
    internal class RemoveLoginPanel_Event : AEvent<RemoveLoginPanel>
    {
        protected override async ETTask Run(RemoveLoginPanel args)
        {
            ViewControllerComponent.Instance.Get(LoginForm.UIResName).Dispose();
            UIPackageHelper.RemovePackage(LoginForm.UIResName);
            await ETTask.CompletedTask;
        }
    }
}