using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Handler.C2G.Setting
{
    internal class C2G_OnSetNameHandler : AMRpcHandler<C2G_OnSetName, G2C_OnSetName>
    {
        protected override async ETTask Run(Session session, C2G_OnSetName request, G2C_OnSetName response, Action reply)
        {
            Player player = session.GetComponent<SessionPlayerComponent>().Player;
            player.BasicData.Nickname = request.NikiName;

            reply();
            await ETTask.CompletedTask;
        }
    }
}
