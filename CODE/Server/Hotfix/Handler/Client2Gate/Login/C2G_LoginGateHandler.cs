using System;
using System.Collections.Generic;

namespace ET
{
    [MessageHandler]
    public class C2G_LoginGateHandler : AMRpcHandler<C2G_LoginGate, G2C_LoginGate>
    {
        protected override async ETTask Run(Session session, C2G_LoginGate request, G2C_LoginGate response, Action reply)
        {
            StartSceneConfig realm = StartSceneConfigCategory.Instance.RealmConfig;
            G2R_CheckLoginKey g2r = new G2R_CheckLoginKey() { Token = request.Token };
            R2G_CheckLoginKey r2g = await MessageHelper.CallActor(realm.InstanceId, g2r) as R2G_CheckLoginKey;
            if (r2g.Error != ErrorCode.ERR_Success)
            {
                response.Error = ErrorCore.ERR_ConnectGateKeyError;
                response.Message = "Gate key验证失败!";
                reply();
                return;
            }

            Scene scene = session.DomainScene();
            DBComponent db = scene.GetComponent<DBComponent>();
            AccountZoneSetComponent accountSet = scene.GetComponent<AccountZoneSetComponent>();
            session.RemoveComponent<SessionAcceptTimeoutComponent>();

            AccountZone accountZone = await db.QueryFirst<AccountZone>(accountZone => accountZone.UUID == r2g.UUID);
            if (accountZone == null)
            {
                accountZone = accountSet.AddChild<AccountZone>();
                accountZone.UUID = r2g.UUID;
                accountZone.Players = new Dictionary<string, List<long>>();
                await db.Save(accountZone);
            }

            if (accountSet.Contains(accountZone.UUID))
            {
                Session session1 = accountSet.GetSession(accountZone.UUID);
                session1?.Dispose();
                await ETTask.CompletedTask;
            }

            accountSet.Add(accountZone, session);
            session.AddComponent<SessionPlayerComponent>().AccountZone = accountZone;
            session.AddComponent<MailBoxComponent, MailboxType>(MailboxType.GateSession);
            response.AccountZoneInfo = ConvertHelper.NewMessage<AccountZoneInfo>(accountZone);
            reply();
        }
    }
}