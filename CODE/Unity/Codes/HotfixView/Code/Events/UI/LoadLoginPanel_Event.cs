using ET.DengLuZhuCe;
using ET.EventType;

namespace ET.Code.Events.Init
{
    public sealed class LoadLoginPanel_Event : AEvent<CreateLoginPanel>
    {
        protected override async ETTask Run(CreateLoginPanel args)
        {
            await UIPackageHelper.AddPackageAsync(FUIPackage.DengLuZhuCe);

            LoginForm panel = LoginForm.CreateInstance();
            FRoot.inst.Add(panel);
            panel.MakeFullScreen();
            panel.self.AddRelation(FRoot.inst.GRoot, FairyGUI.RelationType.Size);
            await ViewControllerComponent.Instance.InitPanel(panel);
        }
    }
}