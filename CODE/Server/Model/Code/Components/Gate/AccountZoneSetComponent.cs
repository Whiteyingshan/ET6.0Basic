using System.Collections.Generic;

namespace ET
{
    public sealed class AccountZoneSetComponent : Entity
    {
        private readonly Dictionary<string, Session> Sessions = new Dictionary<string, Session>();
        private readonly Dictionary<string, AccountZone> Accounts = new Dictionary<string, AccountZone>();

        public void Add(AccountZone accountZone, Session session)
        {
            Sessions.Add(accountZone.UUID, session);
            Accounts.Add(accountZone.UUID, accountZone);
        }

        public bool Contains(string UUID)
        {
            return Accounts.ContainsKey(UUID);
        }

        public bool Remove(string UUID)
        {
            return Accounts.Remove(UUID) && Sessions.Remove(UUID);
        }
        public Session GetSession(string UUID)
        {
            return Sessions[UUID];
        }
    }
}