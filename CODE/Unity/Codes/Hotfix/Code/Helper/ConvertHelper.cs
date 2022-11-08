namespace ET
{
    public static class ConvertHelper
    {
        public static T Handle<T>(T obj, IMessage message)
        {
            Game.Scene.GetComponent<MessageConvertComponent>().Handle(obj, message);
            return obj;
        }

        public static T Handle<T>(T message, object obj) where T : IMessage
        {
            Game.Scene.GetComponent<MessageConvertComponent>().Handle(message, obj);
            return message;
        }

        public static TObject NewObject<TObject>(IMessage message) where TObject : new()
        {
            return Game.Scene.GetComponent<MessageConvertComponent>().NewObject<TObject>(message);
        }

        public static TEntity NewEntity<TEntity>(Entity parent, IMessage message, bool fromPool = false) where TEntity : Entity
        {
            return Game.Scene.GetComponent<MessageConvertComponent>().NewEntity<TEntity>(parent, message, fromPool);
        }

        public static TMessage NewMessage<TMessage>(object obj) where TMessage : IMessage, new()
        {
            return Game.Scene.GetComponent<MessageConvertComponent>().GetMessage<TMessage>(obj);
        }
    }
}