namespace ET
{
    public static class MailBoxComponentSystem
    {
        public sealed class MailBoxComponentAwakeSystem : AwakeSystem<MailBoxComponent>
        {
            public override void Awake(MailBoxComponent self)
            {
                self.MailboxType = MailboxType.MessageDispatcher;
            }
        }

        public sealed class MailBoxComponentAwake1System : AwakeSystem<MailBoxComponent, MailboxType>
        {
            public override void Awake(MailBoxComponent self, MailboxType mailboxType)
            {
                self.MailboxType = mailboxType;
            }
        }
    }
}