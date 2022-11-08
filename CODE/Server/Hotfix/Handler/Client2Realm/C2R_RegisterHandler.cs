using System;
using System.Collections.Generic;

namespace ET.Handler.C2R
{
    internal class C2R_RegisterHandler : AMRpcHandler<C2R_Register, R2C_Register>
    {
        protected override async ETTask Run(Session session, C2R_Register request, R2C_Register response, Action reply)
        {
            if (string.IsNullOrEmpty(request.Username))
            {
                response.Error = ErrorCode.ERR_UsernameIsNull;
                reply();
                return;
            }

            if (string.IsNullOrEmpty(request.Password))
            {
                response.Error = ErrorCode.ERR_PasswordIsNull;
                reply();
                return;
            }

            /*if (request.Username.Length < 6)
            {
                response.Error = ErrorCode.ERR_UsernameIsTooShort;
                reply();
                return;
            }

            if (request.Username.Length > 16)
            {
                response.Error = ErrorCode.ERR_UsernameIsTooLong;
                reply();
                return;
            }

            if (request.Password.Length < 10)
            {
                response.Error = ErrorCode.ERR_PasswordIsTooShort;
                reply();
                return;
            }

            if (request.Password.Length > 16)
            {
                response.Error = ErrorCode.ERR_PasswordIsTooLong;
                reply();
                return;
            }*/

            DBComponent db = session.Domain.GetComponent<DBComponent>();
            List<Account> accounts = await db.Query<Account>(account => account.Username == request.Username);
            if (accounts != null && accounts.Count > 0)
            {
                response.Error = ErrorCode.ERR_AccountAlreadyExistent;
                reply();
                return;
            }

            List<Account> RepeatAccount;
            using (Account account = session.AddChild<Account>())
            {
                account.Username = request.Username;
                account.Password = request.Password;
                account.Age = 0;
                /*account.UUID = Guid.NewGuid().ToString();*/
                do
                {
                    int hashCode = Guid.NewGuid().ToString().GetHashCode();
                    if (hashCode < 0)
                    {
                        hashCode = -hashCode;
                    }
                    // 0 代表前面补充0 
                    // 9 代表长度为9
                    // d 代表参数为正数型
                    // 1 代表区服（暂时只有一个）
                    account.UUID = string.Concat("1", string.Format("{0:D09}", hashCode).AsSpan(0, 9));
                    RepeatAccount = await db.Query<Account>(RepeatAccount => RepeatAccount.UUID == account.UUID);
                } while (RepeatAccount != null && RepeatAccount.Count > 0);
                await db.Save(account);
            }
            reply();
        }
    }
}