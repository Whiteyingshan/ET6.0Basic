using ET;
using ProtoBuf;
using System.Collections.Generic;
namespace ET
{
//========================================IActorRequest========================================
	[Message(MongoOpcode.ObjectQueryResponse)]
	[ProtoContract]
	public partial class ObjectQueryResponse: Object, IActorResponse
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int Error { get; set; }

		[ProtoMember(3)]
		public string Message { get; set; }

		[ProtoMember(4)]
		public Entity entity { get; set; }

	}

}
