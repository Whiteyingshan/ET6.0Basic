namespace ET
{
    public sealed class Account : Entity
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Slot { get; set; }
        public string Hash { get; set; }
        public string UUID { get; set; }
        public int Age { get; set; }
    }
}