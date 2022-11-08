using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Handler.C2G.Setting
{
    internal class C2G_SettingHandler : AMRpcHandler<C2G_Setting, G2C_Setting>
    {
        protected override async ETTask Run(Session session, C2G_Setting request, G2C_Setting response, Action reply)
        {
            Player player = session.GetComponent<SessionPlayerComponent>().Player;
            player.BasicData.Nickname = request.NikiName;

            reply();
            await ETTask.CompletedTask;
        }
    }
}
