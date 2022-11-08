namespace ET
{
    [MessageIOC]
    internal class PlayerIOC : MessageConvert<Player, PlayerInfo>
    {
        public override void ToObject(Player entity, PlayerInfo message)
        {
            entity.Id = message.PlayerId;
            ConvertHelper.Handle(entity.BasicData, message.BasicDataInfo);
            ConvertHelper.Handle(entity.NumericComponent, message.NumericInfo);
        }

        public override void ToMessage(PlayerInfo message, Player entity)
        {
            message.PlayerId = entity.Id;
            message.BasicDataInfo = ConvertHelper.NewMessage<BasicDataInfo>(entity.BasicData);
            message.NumericInfo = ConvertHelper.NewMessage<NumericInfo>(entity.NumericComponent);
        }
    }
}