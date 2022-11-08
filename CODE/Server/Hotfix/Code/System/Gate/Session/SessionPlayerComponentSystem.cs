using System;

namespace ET
{
    internal static class SessionPlayerComponentSystem
    {
        public class SessionPlayerComponentDestroySystem : DestroySystem<SessionPlayerComponent>
        {
            public override void Destroy(SessionPlayerComponent self)
            {
                try
                {
                    if (self.AccountZone != null)
                    {
                        self.Domain.GetComponent<AccountZoneSetComponent>().Remove(self.AccountZone.UUID);
                        self.AccountZone?.Dispose();
                        self.AccountZone = null;
                    }
                    if (self.Player != null)
                    {
                        self.Player.OnLogout();
                        PlayerSetComponent setComponent = self.Domain.GetComponent<PlayerSetComponent>();
                        setComponent.Save(self.Player.Id);
                        setComponent.Remove(self.Player.Id);
                        self.Player?.Dispose();
                        self.Player = null;
                    }
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }
    }
}