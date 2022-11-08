using System;
using System.Collections.Generic;

namespace ET.Handler.C2G.Login
{
    internal class C2G_CreatePlayerHandler : AMRpcHandler<C2G_CreatePlayer, G2C_CreatePlayer>
    {
        protected override async ETTask Run(Session session, C2G_CreatePlayer request, G2C_CreatePlayer response, Action reply)
        {
            bool isTest = string.IsNullOrEmpty(request.ServerId);
            DBComponent dB = session.Domain.GetComponent<DBComponent>();
            AccountZone accountZone = session.GetComponent<SessionPlayerComponent>().AccountZone;

            if (isTest)
            {
                request.ServerId = session.DomainZone().ToString();
            }
            if (!accountZone.Players.ContainsKey(request.ServerId))
            {
                accountZone.Players[request.ServerId] = new List<long>();
            }
            if (isTest && accountZone.Players[request.ServerId].Count > 0)
            {
                reply();
                return;
            }

            using (Player player = session.DomainScene().GetComponent<PlayerSetComponent>().AddChild<Player>())
            {
                accountZone.Players[request.ServerId].Add(player.Id);
                player.BasicData.Account = accountZone.UUID;
                await player.OnCreate();
                await dB.Save(player);
            }

            await dB.Save(accountZone);
            response.AccountZoneInfo = ConvertHelper.NewMessage<AccountZoneInfo>(accountZone);
            reply();
        }
    }
}