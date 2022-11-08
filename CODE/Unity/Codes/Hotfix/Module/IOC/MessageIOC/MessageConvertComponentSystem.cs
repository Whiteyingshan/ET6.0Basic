using System;
using System.Collections.Generic;

namespace ET
{
    public static class MessageConvertComponentSystem
    {
        private sealed class MessageConvertComponentAwakeSystem : AwakeSystem<MessageConvertComponent>
        {
            public override void Awake(MessageConvertComponent self)
            {
                self.Load();
                MessageConvertComponent.Inst = self;
            }
        }

        private sealed class MessageConvertComponentLoadSystem : LoadSystem<MessageConvertComponent>
        {
            public override void Load(MessageConvertComponent self)
            {
                self.Load();
            }
        }

        private static void Load(this MessageConvertComponent self)
        {
            self.Converts.Clear();
            HashSet<Type> types = Game.EventSystem.GetTypes(typeof(MessageIOCAttribute));
            foreach (Type type in types)
            {
                MessageConvert convert = Activator.CreateInstance(type) as MessageConvert;

                self.Converts.Add((convert.ObjectType, convert.MessageType), convert);
            }
        }

        public static void Handle(this MessageConvertComponent self, object entity, IMessage message)
        {
            try
            {
                if (entity is null)
                {
                    Log.Error("Entity is null");
                }
                if (message is null)
                {
                    Log.Error("Message is null");
                }
                if (self.Converts.TryGetValue((entity.GetType(), message.GetType()), out MessageConvert convert))
                {
                    convert.Handle(entity, message);
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        public static void Handle(this MessageConvertComponent self, IMessage message, object entity)
        {
            try
            {
                if (self.Converts.TryGetValue((entity.GetType(), message.GetType()), out MessageConvert convert))
                {
                    convert.Handle(message, entity);
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
        public static TObject NewObject<TObject>(this MessageConvertComponent self, IMessage message) where TObject : new()
        {
            try
            {
                if (self.Converts.TryGetValue((typeof(TObject), message.GetType()), out MessageConvert convert))
                {
                    TObject entity = new TObject();
                    convert.Handle(entity, message);
                    return entity;
                }
                Log.Error($"没有{typeof(TObject)}和{message.GetType()}的IOC!");
                return default;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return default;
            }
        }

        public static TEntity NewEntity<TEntity>(this MessageConvertComponent self, Entity parent, IMessage message, bool fromPool = false) where TEntity : Entity
        {
            try
            {
                if (self.Converts.TryGetValue((typeof(TEntity), message.GetType()), out MessageConvert convert))
                {
                    TEntity entity = parent.AddChild<TEntity>(fromPool);
                    convert.Handle(entity, message);
                    return entity;
                }
                Log.Error($"没有{typeof(TEntity)}和{message.GetType()}的IOC!");
                return default;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return default;
            }
        }

        public static TMessage GetMessage<TMessage>(this MessageConvertComponent self, object entity) where TMessage : IMessage, new()
        {
            try
            {
                if (self.Converts.TryGetValue((entity.GetType(), typeof(TMessage)), out MessageConvert convert))
                {
                    TMessage message = new TMessage();
                    convert.Handle(message, entity);
                    return message;
                }
                Log.Error($"没有{entity.GetType()}和{typeof(TMessage)}的IOC!");
                return default;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return default;
            }
        }
    }
}