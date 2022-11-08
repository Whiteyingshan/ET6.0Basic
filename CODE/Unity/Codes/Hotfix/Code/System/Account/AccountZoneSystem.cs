using System.Collections.Generic;

namespace ET.Code.System
{
    internal class AccountZoneSystem
    {
        public sealed class AccountZoneAwakeSystem : AwakeSystem<AccountZone>
        {
            public override void Awake(AccountZone self)
            {
#if !NOT_UNITY
                AccountZone.Inst = self;
#endif
                self.Players = new Dictionary<string, List<long>>();
            }
        }

        public sealed class AccountZoneDestorySystem : DestroySystem<AccountZone>
        {
            public override void Destroy(AccountZone self)
            {
#if !NOT_UNITY
                AccountZone.Inst = null;
#endif
                self.Players = null;
            }
        }
    }
}