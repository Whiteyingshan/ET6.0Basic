using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Code.IOC.MessageIOC
{
    internal class BasicDataIOC : MessageConvert<BasicData, BasicDataInfo>
    {
        public override void ToMessage(BasicDataInfo message, BasicData entity)
        {
            message.Account = entity.Account;
            message.Nickname = entity.Nickname;
            message.UnitId = entity.UnitId;
        }

        public override void ToObject(BasicData entity, BasicDataInfo message)
        {
            entity.Account = message.Account;
            entity.Nickname = message.Nickname;
            entity.UnitId = message.UnitId;
        }
    }
}