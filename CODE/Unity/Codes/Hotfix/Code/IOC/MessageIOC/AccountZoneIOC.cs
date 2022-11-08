using System.Collections.Generic;

namespace ET.Code.IOC.MessageIOC
{
    internal class AccountZoneIOC : MessageConvert<AccountZone, AccountZoneInfo>
    {
        public override void ToObject(AccountZone entity, AccountZoneInfo message)
        {
            entity.Players.Clear();
            foreach (var item in message.Players)
            {
                entity.Players.Add(item.Key, new List<long>(item.Value));
            }
        }

        public override void ToMessage(AccountZoneInfo message, AccountZone entity)
        {
            message.Players = new List<String_LongList>();
            foreach (var item in entity.Players)
            {
                message.Players.Add(new String_LongList() { Key = item.Key, Value = new List<long>(item.Value) });
            }
        }
    }
}