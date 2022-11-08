namespace ET
{
    public static class PlayerSystem
    {
#if !NOT_UNITY
        internal class PlayerAwakeSystem0 : AwakeSystem<Player>
        {
            public override void Awake(Player self)
            {
                Player.Inst = self;
            }

        }
#endif
    }
}