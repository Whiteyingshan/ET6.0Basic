using System;

namespace ET
{
    [MessageHandler]
    public class C2R_SetRealHandler : AMRpcHandler<C2R_SetReal, R2C_SetReal>
    {
        protected override async ETTask Run(Session session, C2R_SetReal request, R2C_SetReal response, Action reply)
        {
            DBComponent db = session.Domain.GetComponent<DBComponent>();
            Account account = await db.QueryFirst<Account>(account => account.Username == request.Username);

            if (account == null)
            {
                response.Error = ErrorCode.ERR_AccountNonExistent;
                reply();
                return;
            }

            account.Age = request.Age;
            await db.Save(account);
            reply();
        }
    }
}