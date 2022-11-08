using System;
using System.Collections.Generic;

namespace ET.Handler.C2R
{
    internal class C2R_GetServersHandler : AMRpcHandler<C2R_GetServers, R2C_GetServers>
    {
        protected override async ETTask Run(Session session, C2R_GetServers request, R2C_GetServers response, Action reply)
        {
            response.Address = RealmGateAddressHelper.GetGate(session.DomainZone()).OuterIPPort.ToString();
            reply();
            await ETTask.CompletedTask;
        }
    }
}