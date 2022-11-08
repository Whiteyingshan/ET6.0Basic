namespace ET
{
    public sealed class AutoRestoreComponent : Entity
    {
        public Player Player { get; set; }
        public ETCancellationToken CancellationToken { get; set; }
    }
}