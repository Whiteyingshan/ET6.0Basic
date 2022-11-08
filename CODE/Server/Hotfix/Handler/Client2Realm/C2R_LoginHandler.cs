using System;

namespace ET
{
    [MessageHandler]
    public class C2R_LoginHandler : AMRpcHandler<C2R_Login, R2C_Login>
    {
        protected override async ETTask Run(Session session, C2R_Login request, R2C_Login response, Action reply)
        {
            DBComponent db = session.Domain.GetComponent<DBComponent>();
            Account account = await db.QueryFirst<Account>(account => account.Username == request.Username);

            if (account == null)
            {
                response.Error = ErrorCode.ERR_AccountNonExistent;
                reply();
                return;
            }
            if (account.Password != request.Password)
            {
                response.Error = ErrorCode.ERR_PasswordError;
                reply();
                return;
            }

            string token = RandomHelper.RandInt64().ToString();
            session.Domain.GetComponent<GateSessionKeyComponent>().Add(token, account.UUID);
            response.Message = token;
            response.Age = account.Age;
            reply();
        }
    }
}