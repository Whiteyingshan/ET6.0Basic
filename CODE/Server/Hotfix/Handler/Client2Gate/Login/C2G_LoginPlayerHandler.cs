using System;

namespace ET.Handler.C2G.Login
{
    [MessageHandler]
    internal class C2G_LoginPlayerHandler : AMRpcHandler<C2G_LoginPlayer, G2C_LoginPlayer>
    {
        protected override async ETTask Run(Session session, C2G_LoginPlayer request, G2C_LoginPlayer response, Action reply)
        {
            Scene scene = session.DomainScene();
            DBComponent db = scene.GetComponent<DBComponent>();
            PlayerSetComponent playerComponent = scene.GetComponent<PlayerSetComponent>();
            Player player = await db.Query<Player>(request.PlayerId);

            if (playerComponent.Children.ContainsKey(player.Id))
            {
                playerComponent.Remove(player.Id);
                playerComponent.Children.Remove(player.Id);
            }

            playerComponent.AddChild(player);
            playerComponent.Add(player);

            player.AddComponent<MailBoxComponent>();
            player.AddComponent<PlayerSessionComponent>().Session = session;
            session.GetComponent<SessionPlayerComponent>().Player = player;

            player.OnLogin();
            response.PlayerInfo = ConvertHelper.NewMessage<PlayerInfo>(player);
            reply();
        }
    }
}