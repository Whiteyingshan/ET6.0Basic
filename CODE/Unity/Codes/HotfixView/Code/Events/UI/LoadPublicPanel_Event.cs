using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ET._Component_Public;
using ET.EventType;

namespace ET.Code.Events.UI
{
    internal class LoadPublicPanel_Event : AEvent<CreatePublicPanel>
    {
        protected override async ETTask Run(CreatePublicPanel args)
        {
            await UIPackageHelper.AddPackageAsync(FUIPackage._Component_Public);

            NoticeWindow noticeWindow = await NoticeWindow.CreateInstanceAsync();
            await ViewControllerComponent.Instance.InitPanel(noticeWindow);
        }
    }
}