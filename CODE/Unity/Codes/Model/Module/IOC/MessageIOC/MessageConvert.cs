using System;

namespace ET
{
    public abstract class MessageConvert
    {
        public abstract Type ObjectType { get; }
        public abstract Type MessageType { get; }
        public abstract void Handle(object entity, IMessage message);
        public abstract void Handle(IMessage message, object entity);
    }

    [MessageIOC]
    public abstract class MessageConvert<TObject, TMessage> : MessageConvert where TMessage : IMessage
    {
        public override Type ObjectType => typeof(TObject);

        public override Type MessageType => typeof(TMessage);

        public abstract void ToObject(TObject entity, TMessage message);

        public abstract void ToMessage(TMessage message, TObject entity);

        public override void Handle(object entity, IMessage message) => ToObject((TObject)entity, (TMessage)message);

        public override void Handle(IMessage message, object entity) => ToMessage((TMessage)message, (TObject)entity);
    }
}