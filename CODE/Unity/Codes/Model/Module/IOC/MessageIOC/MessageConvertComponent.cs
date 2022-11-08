using System;
using System.Collections.Generic;

namespace ET
{
    public sealed class MessageConvertComponent : Entity
    {
        public static MessageConvertComponent Inst { get; set; }
        /// <summary>
        /// key :Item1 EntityType Item2 MessageType
        /// </summary>
        public readonly Dictionary<(Type, Type), MessageConvert> Converts = new Dictionary<(Type, Type), MessageConvert>();
    }
}