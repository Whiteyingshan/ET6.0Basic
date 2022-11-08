using System.Collections.Generic;

namespace ET
{
    public class GateSessionKeyComponent : Entity
    {
        private readonly Dictionary<string, string> sessionKey = new Dictionary<string, string>();

        public void Add(string key, string account)
        {
            this.sessionKey.Add(key, account);
            this.TimeoutRemoveKey(key).Coroutine();
        }

        public string Get(string key)
        {
            return string.IsNullOrEmpty(key) ? null : sessionKey[key];
        }

        public void Remove(string key)
        {
            this.sessionKey.Remove(key);
        }

        private async ETTask TimeoutRemoveKey(string key)
        {
            await TimerComponent.Instance.WaitAsync(1000000);
            this.sessionKey.Remove(key);
        }
    }
}
