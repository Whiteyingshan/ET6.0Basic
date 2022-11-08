using ET;
using ProtoBuf;
using System.Collections.Generic;
namespace ET
{
//========================================IActorMessage========================================
	[Message(InnerOpcode.G2Mail_Logout)]
	[ProtoContract]
	public partial class G2Mail_Logout: Object, IActorMessage
	{
		[ProtoMember(1)]
		public long PlayerId { get; set; }

	}

	[Message(InnerOpcode.G2Mail_ReadMail)]
	[ProtoContract]
	public partial class G2Mail_ReadMail: Object, IActorMessage
	{
		[ProtoMember(1)]
		public long PlayerId { get; set; }

		[ProtoMember(2)]
		public long MailId { get; set; }

	}

	[Message(InnerOpcode.G2Mail_DeleteMail)]
	[ProtoContract]
	public partial class G2Mail_DeleteMail: Object, IActorMessage
	{
		[ProtoMember(1)]
		public long PlayerId { get; set; }

		[ProtoMember(2)]
		public long MailId { get; set; }

	}

	[Message(InnerOpcode.P2Mail_SendMailToPlayer)]
	[ProtoContract]
	public partial class P2Mail_SendMailToPlayer: Object, IActorMessage
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public long PlayerId { get; set; }

		[ProtoMember(3)]
		public MailObjectInfo MailObjectInfo { get; set; }

	}

	[Message(InnerOpcode.P2Mail_SendMailToOnline)]
	[ProtoContract]
	public partial class P2Mail_SendMailToOnline: Object, IActorMessage
	{
		[ProtoMember(1)]
		public MailObjectInfo MailObjectInfo { get; set; }

	}

	[Message(InnerOpcode.P2Mail_DeletePlayerMail)]
	[ProtoContract]
	public partial class P2Mail_DeletePlayerMail: Object, IActorMessage
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public long PlayerId { get; set; }

		[ProtoMember(3)]
		public int MailId { get; set; }

	}

//========================================IActorRequest========================================
	[ResponseType(nameof(ObjectAddResponse))]
	[Message(InnerOpcode.ObjectAddRequest)]
	[ProtoContract]
	public partial class ObjectAddRequest: Object, IActorRequest
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public long ActorId { get; set; }

		[ProtoMember(3)]
		public long Key { get; set; }

		[ProtoMember(4)]
		public long InstanceId { get; set; }

	}

	[Message(InnerOpcode.ObjectAddResponse)]
	[ProtoContract]
	public partial class ObjectAddResponse: Object, IActorResponse
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int Error { get; set; }

		[ProtoMember(3)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(ObjectRemoveResponse))]
	[Message(InnerOpcode.ObjectRemoveRequest)]
	[ProtoContract]
	public partial class ObjectRemoveRequest: Object, IActorRequest
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public long ActorId { get; set; }

		[ProtoMember(3)]
		public long Key { get; set; }

	}

	[Message(InnerOpcode.ObjectRemoveResponse)]
	[ProtoContract]
	public partial class ObjectRemoveResponse: Object, IActorResponse
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int Error { get; set; }

		[ProtoMember(3)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(ObjectLockResponse))]
	[Message(InnerOpcode.ObjectLockRequest)]
	[ProtoContract]
	public partial class ObjectLockRequest: Object, IActorRequest
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public long ActorId { get; set; }

		[ProtoMember(3)]
		public long Key { get; set; }

		[ProtoMember(4)]
		public long InstanceId { get; set; }

		[ProtoMember(5)]
		public int Time { get; set; }

	}

	[Message(InnerOpcode.ObjectLockResponse)]
	[ProtoContract]
	public partial class ObjectLockResponse: Object, IActorResponse
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int Error { get; set; }

		[ProtoMember(3)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(ObjectUnLockResponse))]
	[Message(InnerOpcode.ObjectUnLockRequest)]
	[ProtoContract]
	public partial class ObjectUnLockRequest: Object, IActorRequest
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public long ActorId { get; set; }

		[ProtoMember(3)]
		public long Key { get; set; }

		[ProtoMember(4)]
		public long OldInstanceId { get; set; }

		[ProtoMember(5)]
		public long InstanceId { get; set; }

	}

	[Message(InnerOpcode.ObjectUnLockResponse)]
	[ProtoContract]
	public partial class ObjectUnLockResponse: Object, IActorResponse
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int Error { get; set; }

		[ProtoMember(3)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(ObjectGetResponse))]
	[Message(InnerOpcode.ObjectGetRequest)]
	[ProtoContract]
	public partial class ObjectGetRequest: Object, IActorRequest
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public long ActorId { get; set; }

		[ProtoMember(3)]
		public long Key { get; set; }

	}

	[Message(InnerOpcode.ObjectGetResponse)]
	[ProtoContract]
	public partial class ObjectGetResponse: Object, IActorResponse
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int Error { get; set; }

		[ProtoMember(3)]
		public string Message { get; set; }

		[ProtoMember(4)]
		public long InstanceId { get; set; }

	}

	[ResponseType(nameof(ObjectQueryResponse))]
	[Message(InnerOpcode.ObjectQueryRequest)]
	[ProtoContract]
	public partial class ObjectQueryRequest: Object, IActorRequest
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public long ActorId { get; set; }

		[ProtoMember(3)]
		public long Key { get; set; }

		[ProtoMember(4)]
		public long InstanceId { get; set; }

	}

	[ResponseType(nameof(G2G_LockResponse))]
	[Message(InnerOpcode.G2G_LockRequest)]
	[ProtoContract]
	public partial class G2G_LockRequest: Object, IActorRequest
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public long ActorId { get; set; }

		[ProtoMember(3)]
		public long Id { get; set; }

		[ProtoMember(4)]
		public string Address { get; set; }

	}

	[Message(InnerOpcode.G2G_LockResponse)]
	[ProtoContract]
	public partial class G2G_LockResponse: Object, IActorResponse
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int Error { get; set; }

		[ProtoMember(3)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(G2G_LockReleaseResponse))]
	[Message(InnerOpcode.G2G_LockReleaseRequest)]
	[ProtoContract]
	public partial class G2G_LockReleaseRequest: Object, IActorRequest
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public long ActorId { get; set; }

		[ProtoMember(3)]
		public long Id { get; set; }

		[ProtoMember(4)]
		public string Address { get; set; }

	}

	[Message(InnerOpcode.G2G_LockReleaseResponse)]
	[ProtoContract]
	public partial class G2G_LockReleaseResponse: Object, IActorResponse
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int Error { get; set; }

		[ProtoMember(3)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(A2M_Reload))]
	[Message(InnerOpcode.M2A_Reload)]
	[ProtoContract]
	public partial class M2A_Reload: Object, IActorRequest
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public long ActorId { get; set; }

	}

	[Message(InnerOpcode.A2M_Reload)]
	[ProtoContract]
	public partial class A2M_Reload: Object, IActorResponse
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int Error { get; set; }

		[ProtoMember(3)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(R2G_CheckLoginKey))]
	[Message(InnerOpcode.G2R_CheckLoginKey)]
	[ProtoContract]
	public partial class G2R_CheckLoginKey: Object, IActorRequest
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public string Token { get; set; }

	}

	[Message(InnerOpcode.R2G_CheckLoginKey)]
	[ProtoContract]
	public partial class R2G_CheckLoginKey: Object, IActorResponse
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int Error { get; set; }

		[ProtoMember(3)]
		public string Message { get; set; }

		[ProtoMember(4)]
		public string UUID { get; set; }

	}

	[ResponseType(nameof(G2R_GetLoginKey))]
	[Message(InnerOpcode.R2G_GetLoginKey)]
	[ProtoContract]
	public partial class R2G_GetLoginKey: Object, IActorRequest
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public long ActorId { get; set; }

		[ProtoMember(3)]
		public string Account { get; set; }

	}

	[Message(InnerOpcode.G2R_GetLoginKey)]
	[ProtoContract]
	public partial class G2R_GetLoginKey: Object, IActorResponse
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int Error { get; set; }

		[ProtoMember(3)]
		public string Message { get; set; }

		[ProtoMember(4)]
		public long Key { get; set; }

		[ProtoMember(5)]
		public long GateId { get; set; }

	}

	[ResponseType(nameof(M2G_CreateUnit))]
	[Message(InnerOpcode.G2M_CreateUnit)]
	[ProtoContract]
	public partial class G2M_CreateUnit: Object, IActorRequest
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public long ActorId { get; set; }

		[ProtoMember(3)]
		public long PlayerId { get; set; }

		[ProtoMember(4)]
		public long GateSessionId { get; set; }

	}

	[Message(InnerOpcode.M2G_CreateUnit)]
	[ProtoContract]
	public partial class M2G_CreateUnit: Object, IActorResponse
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int Error { get; set; }

		[ProtoMember(3)]
		public string Message { get; set; }

// 自己的unit id
		[ProtoMember(4)]
		public long UnitId { get; set; }

// 所有的unit
		[ProtoMember(5)]
		public List<UnitInfo> Units = new List<UnitInfo>();

	}

	[ResponseType(nameof(Mail2G_Login))]
	[Message(InnerOpcode.G2Mail_Login)]
	[ProtoContract]
	public partial class G2Mail_Login: Object, IActorRequest
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public long PlayerId { get; set; }

		[ProtoMember(3)]
		public long InstanceId { get; set; }

	}

	[Message(InnerOpcode.Mail2G_Login)]
	[ProtoContract]
	public partial class Mail2G_Login: Object, IActorResponse
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int Error { get; set; }

		[ProtoMember(3)]
		public string Message { get; set; }

		[ProtoMember(4)]
		public PlayerMailInfo PlayerMailInfo { get; set; }

	}

	[ResponseType(nameof(G2Mail_SendMailToPlayer))]
	[Message(InnerOpcode.Mail2G_SendMailToPlayer)]
	[ProtoContract]
	public partial class Mail2G_SendMailToPlayer: Object, IActorRequest
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public MailObjectInfo MailObjectInfo { get; set; }

	}

	[Message(InnerOpcode.G2Mail_SendMailToPlayer)]
	[ProtoContract]
	public partial class G2Mail_SendMailToPlayer: Object, IActorResponse
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int Error { get; set; }

		[ProtoMember(3)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(Mail2G_AutoDeleteMail))]
	[Message(InnerOpcode.G2Mail_AutoDeleteMail)]
	[ProtoContract]
	public partial class G2Mail_AutoDeleteMail: Object, IActorRequest
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public long PlayerId { get; set; }

	}

	[Message(InnerOpcode.Mail2G_AutoDeleteMail)]
	[ProtoContract]
	public partial class Mail2G_AutoDeleteMail: Object, IActorResponse
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int Error { get; set; }

		[ProtoMember(3)]
		public string Message { get; set; }

		[ProtoMember(4)]
		public PlayerMailInfo PlayerMailInfo { get; set; }

	}

	[ResponseType(nameof(Mail2G_GetMailEnclosure))]
	[Message(InnerOpcode.G2Mail_GetMailEnclosure)]
	[ProtoContract]
	public partial class G2Mail_GetMailEnclosure: Object, IActorRequest
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public long PlayerId { get; set; }

		[ProtoMember(3)]
		public long MailId { get; set; }

	}

	[Message(InnerOpcode.Mail2G_GetMailEnclosure)]
	[ProtoContract]
	public partial class Mail2G_GetMailEnclosure: Object, IActorResponse
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int Error { get; set; }

		[ProtoMember(3)]
		public string Message { get; set; }

		[ProtoMember(4)]
		public List<int> ItemId = new List<int>();

		[ProtoMember(5)]
		public List<int> ItemCount = new List<int>();

	}

	[ResponseType(nameof(Mail2G_AutoGetMailEnclosure))]
	[Message(InnerOpcode.G2Mail_AutoGetMailEnclosure)]
	[ProtoContract]
	public partial class G2Mail_AutoGetMailEnclosure: Object, IActorRequest
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public long PlayerId { get; set; }

	}

	[Message(InnerOpcode.Mail2G_AutoGetMailEnclosure)]
	[ProtoContract]
	public partial class Mail2G_AutoGetMailEnclosure: Object, IActorResponse
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int Error { get; set; }

		[ProtoMember(3)]
		public string Message { get; set; }

		[ProtoMember(4)]
		public List<int> ItemId = new List<int>();

		[ProtoMember(5)]
		public List<int> ItemCount = new List<int>();

		[ProtoMember(6)]
		public PlayerMailInfo PlayerMailInfo { get; set; }

	}

	[Message(InnerOpcode.G2Rank_SetPlayerName)]
	[ProtoContract]
	public partial class G2Rank_SetPlayerName: Object, IActorMessage
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public long PlayerId { get; set; }

		[ProtoMember(3)]
		public string PlayerName { get; set; }

	}

	[ResponseType(nameof(Rank2G_SetPlayerRankScore))]
	[Message(InnerOpcode.G2Rank_SetPlayerRankScore)]
	[ProtoContract]
	public partial class G2Rank_SetPlayerRankScore: Object, IActorRequest
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public long PlayerId { get; set; }

		[ProtoMember(3)]
		public int PlayerScore { get; set; }

	}

	[Message(InnerOpcode.Rank2G_SetPlayerRankScore)]
	[ProtoContract]
	public partial class Rank2G_SetPlayerRankScore: Object, IActorResponse
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int Error { get; set; }

		[ProtoMember(3)]
		public string Message { get; set; }

		[ProtoMember(4)]
		public int PlayerScore { get; set; }

	}

	[ResponseType(nameof(Rank2G_GetPlayerRank))]
	[Message(InnerOpcode.G2Rank_GetPlayerRank)]
	[ProtoContract]
	public partial class G2Rank_GetPlayerRank: Object, IActorRequest
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public long PlayerId { get; set; }

	}

	[Message(InnerOpcode.Rank2G_GetPlayerRank)]
	[ProtoContract]
	public partial class Rank2G_GetPlayerRank: Object, IActorResponse
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int Error { get; set; }

		[ProtoMember(3)]
		public string Message { get; set; }

		[ProtoMember(4)]
		public int Rank { get; set; }

		[ProtoMember(5)]
		public PlayerRankInfo playerRankInfo { get; set; }

	}

	[ResponseType(nameof(Rank2G_GetPlayerRankList))]
	[Message(InnerOpcode.G2Rank_GetPlayerRankList)]
	[ProtoContract]
	public partial class G2Rank_GetPlayerRankList: Object, IActorRequest
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public long PlayerId { get; set; }

	}

	[Message(InnerOpcode.Rank2G_GetPlayerRankList)]
	[ProtoContract]
	public partial class Rank2G_GetPlayerRankList: Object, IActorResponse
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int Error { get; set; }

		[ProtoMember(3)]
		public string Message { get; set; }

		[ProtoMember(4)]
		public int Rank { get; set; }

		[ProtoMember(5)]
		public PlayerRankInfo playerRankInfo { get; set; }

		[ProtoMember(6)]
		public PlayerRankListInfo playerRankListInfo { get; set; }

	}

	[ResponseType(nameof(Rank2G_GetPlayerSeasonRankList))]
	[Message(InnerOpcode.G2Rank_GetPlayerSeasonRankList)]
	[ProtoContract]
	public partial class G2Rank_GetPlayerSeasonRankList: Object, IActorRequest
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public long PlayerId { get; set; }

	}

	[Message(InnerOpcode.Rank2G_GetPlayerSeasonRankList)]
	[ProtoContract]
	public partial class Rank2G_GetPlayerSeasonRankList: Object, IActorResponse
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int Error { get; set; }

		[ProtoMember(3)]
		public string Message { get; set; }

		[ProtoMember(4)]
		public PlayerRankListInfo Seasons { get; set; }

	}

	[ResponseType(nameof(Rank2G_MatchRankPlayer))]
	[Message(InnerOpcode.G2Rank_MatchRankPlayer)]
	[ProtoContract]
	public partial class G2Rank_MatchRankPlayer: Object, IActorRequest
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public long PlayerId { get; set; }

	}

	[Message(InnerOpcode.Rank2G_MatchRankPlayer)]
	[ProtoContract]
	public partial class Rank2G_MatchRankPlayer: Object, IActorResponse
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int Error { get; set; }

		[ProtoMember(3)]
		public string Message { get; set; }

		[ProtoMember(4)]
		public List<int> SelfScore = new List<int>();

		[ProtoMember(5)]
		public List<long> PlayerId = new List<long>();

		[ProtoMember(6)]
		public List<int> EnemyScore = new List<int>();

	}

//========================================IActorLocationMessage========================================
	[Message(InnerOpcode.G2M_SessionDisconnect)]
	[ProtoContract]
	public partial class G2M_SessionDisconnect: Object, IActorLocationMessage
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public long ActorId { get; set; }

	}

}
