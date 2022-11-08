using System.Collections.Generic;

namespace ET
{
    public sealed class AccountZone : Entity
    {
#if !SERVER
        public static AccountZone Inst { get; set; }
#endif
        public string UUID { get; set; }
        public Dictionary<string, List<long>> Players { get; set; }
        public int Age { get; set; }
    }
}