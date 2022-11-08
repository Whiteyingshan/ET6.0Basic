using ET;
using ProtoBuf;
using System.Collections.Generic;
namespace ET
{
//========================================IMessage========================================
	[Message(OuterOpcode.Int_Int)]
	[ProtoContract]
	public partial class Int_Int: Object, IMessage
	{
		[ProtoMember(1)]
		public int Key { get; set; }

		[ProtoMember(2)]
		public int Value { get; set; }

	}

	[Message(OuterOpcode.Int_Long)]
	[ProtoContract]
	public partial class Int_Long: Object, IMessage
	{
		[ProtoMember(1)]
		public int Key { get; set; }

		[ProtoMember(2)]
		public long Value { get; set; }

	}

	[Message(OuterOpcode.Long_Long)]
	[ProtoContract]
	public partial class Long_Long: Object, IMessage
	{
		[ProtoMember(1)]
		public long Key { get; set; }

		[ProtoMember(2)]
		public long Value { get; set; }

	}

	[Message(OuterOpcode.Int_String)]
	[ProtoContract]
	public partial class Int_String: Object, IMessage
	{
		[ProtoMember(1)]
		public int Key { get; set; }

		[ProtoMember(2)]
		public string Value { get; set; }

	}

	[Message(OuterOpcode.String_LongList)]
	[ProtoContract]
	public partial class String_LongList: Object, IMessage
	{
		[ProtoMember(1)]
		public string Key { get; set; }

		[ProtoMember(2)]
		public List<long> Value = new List<long>();

	}

	[Message(OuterOpcode.String_SkillList)]
	[ProtoContract]
	public partial class String_SkillList: Object, IMessage
	{
		[ProtoMember(1)]
		public string Key { get; set; }

		[ProtoMember(2)]
		public List<SkillInfo> Value = new List<SkillInfo>();

	}

	[Message(OuterOpcode.Vector3Info)]
	[ProtoContract]
	public partial class Vector3Info: Object, IMessage
	{
		[ProtoMember(1)]
		public float X { get; set; }

		[ProtoMember(2)]
		public float Y { get; set; }

		[ProtoMember(3)]
		public float Z { get; set; }

	}

	[Message(OuterOpcode.NumericInfo)]
	[ProtoContract]
	public partial class NumericInfo: Object, IMessage
	{
		[ProtoMember(1)]
		public List<Long_Long> Numerics = new List<Long_Long>();

	}

	[Message(OuterOpcode.ServerInfo)]
	[ProtoContract]
	public partial class ServerInfo: Object, IMessage
	{
		[ProtoMember(1)]
		public string ServerName { get; set; }

		[ProtoMember(2)]
		public string ServerAddress { get; set; }

	}

	[Message(OuterOpcode.AccountZoneInfo)]
	[ProtoContract]
	public partial class AccountZoneInfo: Object, IMessage
	{
		[ProtoMember(1)]
		public long Id { get; set; }

		[ProtoMember(2)]
		public List<String_LongList> Players = new List<String_LongList>();

	}

//玩家数据
	[Message(OuterOpcode.PlayerInfo)]
	[ProtoContract]
	public partial class PlayerInfo: Object, IMessage
	{
		[ProtoMember(1)]
		public long PlayerId { get; set; }

		[ProtoMember(2)]
		public BasicDataInfo BasicDataInfo { get; set; }

		[ProtoMember(3)]
		public NumericInfo NumericInfo { get; set; }

		[ProtoMember(4)]
		public ItemManagerInfo ItemManagerInfo { get; set; }

		[ProtoMember(5)]
		public PlayerMailInfo MailComponentInfo { get; set; }

		[ProtoMember(6)]
		public SeasonComponentInfo SeasonComponentInfo { get; set; }

		[ProtoMember(7)]
		public BattleArrayComponentInfo BattleArrayComponentInfo { get; set; }

		[ProtoMember(8)]
		public AchievementComponentInfo AchievementComponentInfo { get; set; }

		[ProtoMember(9)]
		public SquadsControllerComponentInfo SquadsControllerComponentInfo { get; set; }

		[ProtoMember(10)]
		public GongFaComponentInfo GongFaComponentInfo { get; set; }

	}

	[Message(OuterOpcode.BasicDataInfo)]
	[ProtoContract]
	public partial class BasicDataInfo: Object, IMessage
	{
		[ProtoMember(1)]
		public string Account { get; set; }

		[ProtoMember(2)]
		public string Nickname { get; set; }

		[ProtoMember(3)]
		public long UnitId { get; set; }

	}

	[Message(OuterOpcode.ItemValueInfo)]
	[ProtoContract]
	public partial class ItemValueInfo: Object, IMessage
	{
		[ProtoMember(1)]
		public int JB { get; set; }

		[ProtoMember(2)]
		public int ZS { get; set; }

		[ProtoMember(3)]
		public int JY { get; set; }

	}

	[Message(OuterOpcode.ItemEntityInfo)]
	[ProtoContract]
	public partial class ItemEntityInfo: Object, IMessage
	{
		[ProtoMember(1)]
		public long Id { get; set; }

		[ProtoMember(2)]
		public long TableId { get; set; }

		[ProtoMember(3)]
		public int Count { get; set; }

		[ProtoMember(4)]
		public HeroInfo HeroInfo { get; set; }

		[ProtoMember(5)]
		public EquipInfo EquipInfo { get; set; }

		[ProtoMember(6)]
		public ConsumableInfo ConsumableInfo { get; set; }

	}

	[Message(OuterOpcode.HeroInfo)]
	[ProtoContract]
	public partial class HeroInfo: Object, IMessage
	{
		[ProtoMember(1)]
		public long Id { get; set; }

		[ProtoMember(2)]
		public long TableId { get; set; }

		[ProtoMember(3)]
		public long OwnerId { get; set; }

		[ProtoMember(4)]
		public int Count { get; set; }

		[ProtoMember(5)]
		public long HeroTableId { get; set; }

		[ProtoMember(6)]
		public int Grade { get; set; }

		[ProtoMember(7)]
		public int PhaseGrade { get; set; }

		[ProtoMember(8)]
		public int StarGrade { get; set; }

		[ProtoMember(9)]
		public int SumRankPoint { get; set; }

		[ProtoMember(10)]
		public NumericInfo NumericInfo { get; set; }

		[ProtoMember(11)]
		public SkillManagerInfo SkillManagerInfo { get; set; }

		[ProtoMember(12)]
		public EquipSlotsInfo EquipSlotsInfo { get; set; }

//英雄天赋信息
		[ProtoMember(13)]
		public HeroTalentInfo HeroTalentInfo { get; set; }

//英雄修炼信息
		[ProtoMember(14)]
		public HeroPracticeInfo HeroPracticeInfo { get; set; }

	}

	[Message(OuterOpcode.EquipInfo)]
	[ProtoContract]
	public partial class EquipInfo: Object, IMessage
	{
		[ProtoMember(1)]
		public long Id { get; set; }

		[ProtoMember(2)]
		public long TableId { get; set; }

		[ProtoMember(3)]
		public long Count { get; set; }

		[ProtoMember(4)]
		public long UserId { get; set; }

		[ProtoMember(5)]
		public int RankPoint { get; set; }

		[ProtoMember(6)]
		public NumericInfo NumericInfo { get; set; }

		[ProtoMember(7)]
		public List<EquipAppendPropertyInfo> EquipAppendPropertyInfo = new List<EquipAppendPropertyInfo>();

	}

	[Message(OuterOpcode.ConsumableInfo)]
	[ProtoContract]
	public partial class ConsumableInfo: Object, IMessage
	{
		[ProtoMember(1)]
		public long Id { get; set; }

		[ProtoMember(2)]
		public long TableId { get; set; }

		[ProtoMember(3)]
		public long Count { get; set; }

	}

	[Message(OuterOpcode.EquipAppendPropertyInfo)]
	[ProtoContract]
	public partial class EquipAppendPropertyInfo: Object, IMessage
	{
		[ProtoMember(1)]
		public long Id { get; set; }

		[ProtoMember(2)]
		public bool IsEpic { get; set; }

		[ProtoMember(3)]
		public int ValueRankGrade { get; set; }

		[ProtoMember(4)]
		public int PropertyId { get; set; }

	}

	[Message(OuterOpcode.SkillManagerInfo)]
	[ProtoContract]
	public partial class SkillManagerInfo: Object, IMessage
	{
		[ProtoMember(1)]
		public long Id { get; set; }

		[ProtoMember(2)]
		public List<String_SkillList> SkillDict = new List<String_SkillList>();

	}

	[Message(OuterOpcode.SkillInfo)]
	[ProtoContract]
	public partial class SkillInfo: Object, IMessage
	{
		[ProtoMember(1)]
		public long Id { get; set; }

		[ProtoMember(2)]
		public int Grade { get; set; }

		[ProtoMember(3)]
		public long RealSkillId { get; set; }

	}

	[Message(OuterOpcode.ItemManagerInfo)]
	[ProtoContract]
	public partial class ItemManagerInfo: Object, IMessage
	{
		[ProtoMember(1)]
		public List<ItemEntityInfo> ItemEntityInfos = new List<ItemEntityInfo>();

	}

	[Message(OuterOpcode.EquipSlotsInfo)]
	[ProtoContract]
	public partial class EquipSlotsInfo: Object, IMessage
	{
		[ProtoMember(1)]
		public long Id { get; set; }

		[ProtoMember(2)]
		public List<Int_Long> EquipIdSlotDict = new List<Int_Long>();

		[ProtoMember(3)]
		public List<Int_Int> EquipSuitDict = new List<Int_Int>();

	}

	[Message(OuterOpcode.AwardInfo)]
	[ProtoContract]
	public partial class AwardInfo: Object, IMessage
	{
		[ProtoMember(1)]
		public ItemValueInfo itemValueInfo { get; set; }

		[ProtoMember(2)]
		public List<ItemEntityInfo> ItemEntityInfos = new List<ItemEntityInfo>();

	}

	[Message(OuterOpcode.PlayerRankInfo)]
	[ProtoContract]
	public partial class PlayerRankInfo: Object, IMessage
	{
		[ProtoMember(1)]
		public long PlayerId { get; set; }

		[ProtoMember(2)]
		public int SeasonId { get; set; }

		[ProtoMember(3)]
		public string PlayerName { get; set; }

		[ProtoMember(4)]
		public int WinCount { get; set; }

		[ProtoMember(5)]
		public int DeaftCount { get; set; }

		[ProtoMember(6)]
		public int RankScore { get; set; }

	}

	[Message(OuterOpcode.PlayerRankListInfo)]
	[ProtoContract]
	public partial class PlayerRankListInfo: Object, IMessage
	{
		[ProtoMember(1)]
		public List<PlayerRankInfo> PlayerRankInfo = new List<PlayerRankInfo>();

	}

	[Message(OuterOpcode.UnitInfo)]
	[ProtoContract]
	public partial class UnitInfo: Object, IMessage
	{
		[ProtoMember(1)]
		public long UnitId { get; set; }

		[ProtoMember(2)]
		public int ConfigId { get; set; }

		[ProtoMember(3)]
		public float X { get; set; }

		[ProtoMember(4)]
		public float Y { get; set; }

		[ProtoMember(5)]
		public float Z { get; set; }

		[ProtoMember(6)]
		public List<long> Ks = new List<long>();

		[ProtoMember(7)]
		public List<long> Vs = new List<long>();

	}

	[Message(OuterOpcode.ChapterAwardInfo)]
	[ProtoContract]
	public partial class ChapterAwardInfo: Object, IMessage
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public long CommonCurrency { get; set; }

		[ProtoMember(3)]
		public List<ItemEntityInfo> itemEntityInfos = new List<ItemEntityInfo>();

		[ProtoMember(4)]
		public List<HeroInfo> HeroInfos = new List<HeroInfo>();

	}

	[Message(OuterOpcode.AchievementComponentInfo)]
	[ProtoContract]
	public partial class AchievementComponentInfo: Object, IMessage
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public List<long> CompletedIds = new List<long>();

		[ProtoMember(3)]
		public List<Int_Long> DailyRecord = new List<Int_Long>();

		[ProtoMember(4)]
		public List<long> DailyTaskReceiveId = new List<long>();

	}

//邮件对象
	[Message(OuterOpcode.MailObjectInfo)]
	[ProtoContract]
	public partial class MailObjectInfo: Object, IMessage
	{
		[ProtoMember(1)]
		public long MailId { get; set; }

		[ProtoMember(2)]
		public long Sender { get; set; }

		[ProtoMember(3)]
		public long SendTime { get; set; }

		[ProtoMember(4)]
		public string Title { get; set; }

		[ProtoMember(5)]
		public string Content { get; set; }

		[ProtoMember(6)]
		public List<int> RewardsId = new List<int>();

		[ProtoMember(7)]
		public List<int> RewardsCount = new List<int>();

		[ProtoMember(8)]
		public bool Read { get; set; }

		[ProtoMember(9)]
		public bool Received { get; set; }

	}

	[Message(OuterOpcode.PlayerMailInfo)]
	[ProtoContract]
	public partial class PlayerMailInfo: Object, IMessage
	{
		[ProtoMember(1)]
		public List<MailObjectInfo> MailObjectInfos = new List<MailObjectInfo>();

	}

//赛事信息
	[Message(OuterOpcode.SeasonComponentInfo)]
	[ProtoContract]
	public partial class SeasonComponentInfo: Object, IMessage
	{
		[ProtoMember(1)]
		public bool IsOpen { get; set; }

		[ProtoMember(2)]
		public long SeasonId { get; set; }

		[ProtoMember(3)]
		public long Rank { get; set; }

		[ProtoMember(4)]
		public int StarCount { get; set; }

		[ProtoMember(5)]
		public int FightCount { get; set; }

		[ProtoMember(6)]
		public long RefreshTime { get; set; }

		[ProtoMember(7)]
		public int ContinueWinCount { get; set; }

		[ProtoMember(8)]
		public int TopRankId { get; set; }

		[ProtoMember(9)]
		public bool IsFight { get; set; }

	}

	[Message(OuterOpcode.SquadsControllerComponentInfo)]
	[ProtoContract]
	public partial class SquadsControllerComponentInfo: Object, IMessage
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public List<long> SquadsMemberIds = new List<long>();

		[ProtoMember(3)]
		public int CountMax { get; set; }

	}

	[Message(OuterOpcode.PlayerPositionInfo)]
	[ProtoContract]
	public partial class PlayerPositionInfo: Object, IMessage
	{
		[ProtoMember(1)]
		public int Number_ { get; set; }

		[ProtoMember(2)]
		public int HeroId { get; set; }

		[ProtoMember(3)]
		public int Grade_ { get; set; }

		[ProtoMember(4)]
		public Vector3Info Position { get; set; }

		[ProtoMember(5)]
		public Vector3Info LowerLeft_ { get; set; }

		[ProtoMember(6)]
		public Vector3Info UpperRight_ { get; set; }

	}

//球员阵容_Pl
	[Message(OuterOpcode.BattleArrayInfo)]
	[ProtoContract]
	public partial class BattleArrayInfo: Object, IMessage
	{
		[ProtoMember(1)]
		public int ArrayId { get; set; }

		[ProtoMember(2)]
		public int TacticId { get; set; }

		[ProtoMember(3)]
		public int DefTacticId { get; set; }

		[ProtoMember(4)]
		public List<long> BattleHeros = new List<long>();

		[ProtoMember(5)]
		public List<long> ReserveHeros = new List<long>();

		[ProtoMember(6)]
		public List<PlayerPositionInfo> PlayerPosition = new List<PlayerPositionInfo>();

		[ProtoMember(7)]
		public List<int> ArrayFetterAddition = new List<int>();

	}

	[Message(OuterOpcode.BattleArrayComponentInfo)]
	[ProtoContract]
	public partial class BattleArrayComponentInfo: Object, IMessage
	{
		[ProtoMember(1)]
		public int CurrentArray { get; set; }

		[ProtoMember(2)]
		public List<BattleArrayInfo> BattleArrayInfo = new List<BattleArrayInfo>();

	}

//功法信息
	[Message(OuterOpcode.GongFaInfo)]
	[ProtoContract]
	public partial class GongFaInfo: Object, IMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long Id { get; set; }

		[ProtoMember(2)]
		public long TableId { get; set; }

		[ProtoMember(3)]
		public int Grade { get; set; }

		[ProtoMember(4)]
		public long ParentId { get; set; }

	}

//功法组件信息
	[Message(OuterOpcode.GongFaComponentInfo)]
	[ProtoContract]
	public partial class GongFaComponentInfo: Object, IMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long Id { get; set; }

		[ProtoMember(2)]
		public List<GongFaInfo> infos = new List<GongFaInfo>();

	}

//单个节点英雄天赋信息
	[Message(OuterOpcode.HeroTalentSingleInfo)]
	[ProtoContract]
	public partial class HeroTalentSingleInfo: Object, IMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int TalentNum { get; set; }

		[ProtoMember(2)]
		public List<long> TalentValueArray = new List<long>();

	}

//单页英雄天赋信息
	[Message(OuterOpcode.HeroTalentSinglepageInfo)]
	[ProtoContract]
	public partial class HeroTalentSinglepageInfo: Object, IMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int PageNum { get; set; }

		[ProtoMember(2)]
		public List<HeroTalentSingleInfo> HeroTalentSingleInfos = new List<HeroTalentSingleInfo>();

	}

//英雄天赋信息
	[Message(OuterOpcode.HeroTalentInfo)]
	[ProtoContract]
	public partial class HeroTalentInfo: Object, IMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public List<HeroTalentSinglepageInfo> HeroTalentPageInfos = new List<HeroTalentSinglepageInfo>();

		[ProtoMember(2)]
		public NumericInfo NumericInfo { get; set; }

	}

//英雄修炼信息
	[Message(OuterOpcode.HeroPracticeInfo)]
	[ProtoContract]
	public partial class HeroPracticeInfo: Object, IMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int CurPliesNum { get; set; }

		[ProtoMember(2)]
		public int CurPracticeNum { get; set; }

		[ProtoMember(3)]
		public int CurPracticeProgressPct { get; set; }

		[ProtoMember(4)]
		public NumericInfo NumericInfo { get; set; }

	}

//========================================IRequest========================================
	[Message(OuterOpcode.Response)]
	[ProtoContract]
	public partial class Response: Object, IResponse
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int Error { get; set; }

		[ProtoMember(3)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(G2C_Ping))]
	[Message(OuterOpcode.C2G_Ping)]
	[ProtoContract]
	public partial class C2G_Ping: Object, IRequest
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

	}

	[Message(OuterOpcode.G2C_Ping)]
	[ProtoContract]
	public partial class G2C_Ping: Object, IResponse
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int Error { get; set; }

		[ProtoMember(3)]
		public string Message { get; set; }

		[ProtoMember(4)]
		public long Time { get; set; }

	}

	[ResponseType(nameof(R2C_Register))]
	[Message(OuterOpcode.C2R_Register)]
	[ProtoContract]
	public partial class C2R_Register: Object, IRequest
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public string Username { get; set; }

		[ProtoMember(3)]
		public string Password { get; set; }

	}

	[Message(OuterOpcode.R2C_Register)]
	[ProtoContract]
	public partial class R2C_Register: Object, IResponse
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int Error { get; set; }

		[ProtoMember(3)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(R2C_SetReal))]
	[Message(OuterOpcode.C2R_SetReal)]
	[ProtoContract]
	public partial class C2R_SetReal: Object, IRequest
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public string Username { get; set; }

		[ProtoMember(3)]
		public int Age { get; set; }

	}

	[Message(OuterOpcode.R2C_SetReal)]
	[ProtoContract]
	public partial class R2C_SetReal: Object, IResponse
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int Error { get; set; }

		[ProtoMember(3)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(R2C_Login))]
	[Message(OuterOpcode.C2R_Login)]
	[ProtoContract]
	public partial class C2R_Login: Object, IRequest
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public string Username { get; set; }

		[ProtoMember(3)]
		public string Password { get; set; }

	}

	[Message(OuterOpcode.R2C_Login)]
	[ProtoContract]
	public partial class R2C_Login: Object, IResponse
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int Error { get; set; }

		[ProtoMember(3)]
		public string Message { get; set; }

		[ProtoMember(4)]
		public int Age { get; set; }

	}

	[ResponseType(nameof(R2C_GetServers))]
	[Message(OuterOpcode.C2R_GetServers)]
	[ProtoContract]
	public partial class C2R_GetServers: Object, IRequest
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

	}

	[Message(OuterOpcode.R2C_GetServers)]
	[ProtoContract]
	public partial class R2C_GetServers: Object, IResponse
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int Error { get; set; }

		[ProtoMember(3)]
		public string Message { get; set; }

		[ProtoMember(4)]
		public string Address { get; set; }

		[ProtoMember(5)]
		public List<ServerInfo> Servers = new List<ServerInfo>();

	}

	[ResponseType(nameof(G2C_LoginGate))]
	[Message(OuterOpcode.C2G_LoginGate)]
	[ProtoContract]
	public partial class C2G_LoginGate: Object, IRequest
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public string Token { get; set; }

	}

	[Message(OuterOpcode.G2C_LoginGate)]
	[ProtoContract]
	public partial class G2C_LoginGate: Object, IResponse
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int Error { get; set; }

		[ProtoMember(3)]
		public string Message { get; set; }

		[ProtoMember(4)]
		public AccountZoneInfo AccountZoneInfo { get; set; }

	}

	[ResponseType(nameof(G2C_CreatePlayer))]
	[Message(OuterOpcode.C2G_CreatePlayer)]
	[ProtoContract]
	public partial class C2G_CreatePlayer: Object, IRequest
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public string ServerId { get; set; }

	}

	[Message(OuterOpcode.G2C_CreatePlayer)]
	[ProtoContract]
	public partial class G2C_CreatePlayer: Object, IResponse
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int Error { get; set; }

		[ProtoMember(3)]
		public string Message { get; set; }

		[ProtoMember(4)]
		public AccountZoneInfo AccountZoneInfo { get; set; }

	}

	[ResponseType(nameof(G2C_LoginPlayer))]
	[Message(OuterOpcode.C2G_LoginPlayer)]
	[ProtoContract]
	public partial class C2G_LoginPlayer: Object, IRequest
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public long PlayerId { get; set; }

	}

	[Message(OuterOpcode.G2C_LoginPlayer)]
	[ProtoContract]
	public partial class G2C_LoginPlayer: Object, IResponse
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int Error { get; set; }

		[ProtoMember(3)]
		public string Message { get; set; }

		[ProtoMember(4)]
		public PlayerInfo PlayerInfo { get; set; }

		[ProtoMember(5)]
		public string NikiName { get; set; }

	}

	[ResponseType(nameof(G2C_PassLevel))]
	[Message(OuterOpcode.C2G_PassLevel)]
	[ProtoContract]
	public partial class C2G_PassLevel: Object, IRequest
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int LevelId { get; set; }

	}

	[Message(OuterOpcode.G2C_PassLevel)]
	[ProtoContract]
	public partial class G2C_PassLevel: Object, IResponse
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int Error { get; set; }

		[ProtoMember(3)]
		public string Message { get; set; }

		[ProtoMember(4)]
		public int LevelId { get; set; }

		[ProtoMember(5)]
		public AwardInfo AwardInfo { get; set; }

	}

//临时视频播放次数信息
	[ResponseType(nameof(G2C_PlayLevelVideo))]
	[Message(OuterOpcode.C2G_PlayLevelVideo)]
	[ProtoContract]
	public partial class C2G_PlayLevelVideo: Object, IRequest
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public string VideoTimes { get; set; }

	}

	[Message(OuterOpcode.G2C_PlayLevelVideo)]
	[ProtoContract]
	public partial class G2C_PlayLevelVideo: Object, IResponse
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int Error { get; set; }

		[ProtoMember(3)]
		public string Message { get; set; }

		[ProtoMember(4)]
		public string VideoTimes { get; set; }

	}

//战斗结束发送信息
	[ResponseType(nameof(G2C_BattleEndInformation))]
	[Message(OuterOpcode.C2G_BattleEndInformation)]
	[ProtoContract]
	public partial class C2G_BattleEndInformation: Object, IRequest
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int PlayerGoal { get; set; }

		[ProtoMember(3)]
		public bool IsWin { get; set; }

	}

	[Message(OuterOpcode.G2C_BattleEndInformation)]
	[ProtoContract]
	public partial class G2C_BattleEndInformation: Object, IResponse
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int Error { get; set; }

		[ProtoMember(3)]
		public string Message { get; set; }

	}

//领取奖励
	[ResponseType(nameof(G2C_ReceiveRewards))]
	[Message(OuterOpcode.C2G_ReceiveRewards)]
	[ProtoContract]
	public partial class C2G_ReceiveRewards: Object, IRequest
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int ReceiveType { get; set; }

		[ProtoMember(3)]
		public int MailId { get; set; }

		[ProtoMember(4)]
		public long TaskId { get; set; }

		[ProtoMember(5)]
		public long AchieveId { get; set; }

	}

	[Message(OuterOpcode.G2C_ReceiveRewards)]
	[ProtoContract]
	public partial class G2C_ReceiveRewards: Object, IResponse
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int Error { get; set; }

		[ProtoMember(3)]
		public string Message { get; set; }

		[ProtoMember(4)]
		public long TaskId { get; set; }

		[ProtoMember(5)]
		public long AchieveId { get; set; }

	}

	[ResponseType(nameof(G2C_EnterMap))]
	[Message(OuterOpcode.C2G_EnterMap)]
	[ProtoContract]
	public partial class C2G_EnterMap: Object, IRequest
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

	}

	[Message(OuterOpcode.G2C_EnterMap)]
	[ProtoContract]
	public partial class G2C_EnterMap: Object, IResponse
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

	[ResponseType(nameof(M2C_Reload))]
	[Message(OuterOpcode.C2M_Reload)]
	[ProtoContract]
	public partial class C2M_Reload: Object, IRequest
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public string Account { get; set; }

		[ProtoMember(3)]
		public string Password { get; set; }

	}

	[Message(OuterOpcode.M2C_Reload)]
	[ProtoContract]
	public partial class M2C_Reload: Object, IResponse
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int Error { get; set; }

		[ProtoMember(3)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(C2G_GetChapterAward))]
	[Message(OuterOpcode.C2G_GetChapterAward)]
	[ProtoContract]
	public partial class C2G_GetChapterAward: Object, IRequest
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int AwardTableId { get; set; }

	}

	[Message(OuterOpcode.G2C_GetChapterAward)]
	[ProtoContract]
	public partial class G2C_GetChapterAward: Object, IResponse
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int Error { get; set; }

		[ProtoMember(3)]
		public string Message { get; set; }

		[ProtoMember(4)]
		public ChapterAwardInfo chapterAwardInfo { get; set; }

	}

	[ResponseType(nameof(C2G_GetTaskReward))]
	[Message(OuterOpcode.C2G_GetTaskReward)]
	[ProtoContract]
	public partial class C2G_GetTaskReward: Object, IRequest
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int TaskTableId { get; set; }

	}

	[Message(OuterOpcode.G2C_GetTaskReward)]
	[ProtoContract]
	public partial class G2C_GetTaskReward: Object, IResponse
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int Error { get; set; }

		[ProtoMember(3)]
		public string Message { get; set; }

		[ProtoMember(4)]
		public ChapterAwardInfo TaskRewardInfo { get; set; }

	}

	[ResponseType(nameof(C2G_HeroUpGrade))]
	[Message(OuterOpcode.C2G_HeroUpGrade)]
	[ProtoContract]
	public partial class C2G_HeroUpGrade: Object, IRequest
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public long ItemId { get; set; }

	}

	[Message(OuterOpcode.G2C_HeroUpGrade)]
	[ProtoContract]
	public partial class G2C_HeroUpGrade: Object, IResponse
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int Error { get; set; }

		[ProtoMember(3)]
		public string Message { get; set; }

		[ProtoMember(4)]
		public int Grade { get; set; }

		[ProtoMember(5)]
		public int JBCount { get; set; }

	}

	[ResponseType(nameof(C2G_HeroSell))]
	[Message(OuterOpcode.C2G_HeroSell)]
	[ProtoContract]
	public partial class C2G_HeroSell: Object, IRequest
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int Sell_JBSumCount { get; set; }

	}

	[Message(OuterOpcode.G2C_HeroSell)]
	[ProtoContract]
	public partial class G2C_HeroSell: Object, IResponse
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int Error { get; set; }

		[ProtoMember(3)]
		public string Message { get; set; }

		[ProtoMember(4)]
		public int JBCount { get; set; }

	}

	[ResponseType(nameof(C2G_LevelInformation))]
	[Message(OuterOpcode.C2G_LevelInformation)]
	[ProtoContract]
	public partial class C2G_LevelInformation: Object, IRequest
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int LevelId { get; set; }

	}

	[Message(OuterOpcode.G2C_LevelInformation)]
	[ProtoContract]
	public partial class G2C_LevelInformation: Object, IResponse
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int Error { get; set; }

		[ProtoMember(3)]
		public string Message { get; set; }

		[ProtoMember(4)]
		public int Count { get; set; }

	}

	[ResponseType(nameof(C2G_AwardInfo))]
	[Message(OuterOpcode.C2G_AwardInfo)]
	[ProtoContract]
	public partial class C2G_AwardInfo: Object, IRequest
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public long PlayerId { get; set; }

		[ProtoMember(3)]
		public int tableID { get; set; }

		[ProtoMember(4)]
		public int itemCount { get; set; }

	}

	[Message(OuterOpcode.G2C_AwardInfo)]
	[ProtoContract]
	public partial class G2C_AwardInfo: Object, IResponse
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int Error { get; set; }

		[ProtoMember(3)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(G2C_CompletedAchievement))]
	[Message(OuterOpcode.C2G_CompletedAchievement)]
	[ProtoContract]
	public partial class C2G_CompletedAchievement: Object, IRequest
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public long AchievementTid { get; set; }

	}

	[Message(OuterOpcode.G2C_CompletedAchievement)]
	[ProtoContract]
	public partial class G2C_CompletedAchievement: Object, IResponse
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int Error { get; set; }

		[ProtoMember(3)]
		public string Message { get; set; }

	}

//保存阵容
	[ResponseType(nameof(G2C_SaveBattleArray))]
	[Message(OuterOpcode.C2G_SaveBattleArray)]
	[ProtoContract]
	public partial class C2G_SaveBattleArray: Object, IRequest
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public BattleArrayComponentInfo BattleArrayComponentInfo { get; set; }

	}

	[Message(OuterOpcode.G2C_SaveBattleArray)]
	[ProtoContract]
	public partial class G2C_SaveBattleArray: Object, IResponse
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int Error { get; set; }

		[ProtoMember(3)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(G2C_SaveBattleArrayCurrentIndex))]
	[Message(OuterOpcode.C2G_SaveBattleArrayCurrentIndex)]
	[ProtoContract]
	public partial class C2G_SaveBattleArrayCurrentIndex: Object, IRequest
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int SaveBattleArrayCurrentIndex { get; set; }

	}

	[Message(OuterOpcode.G2C_SaveBattleArrayCurrentIndex)]
	[ProtoContract]
	public partial class G2C_SaveBattleArrayCurrentIndex: Object, IResponse
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int Error { get; set; }

		[ProtoMember(3)]
		public string Message { get; set; }

	}

//读邮件
	[Message(OuterOpcode.C2G_ReadMail)]
	[ProtoContract]
	public partial class C2G_ReadMail: Object, IMessage
	{
		[ProtoMember(1)]
		public long MailId { get; set; }

	}

//删除邮件
	[Message(OuterOpcode.C2G_DeleteMail)]
	[ProtoContract]
	public partial class C2G_DeleteMail: Object, IMessage
	{
		[ProtoMember(1)]
		public long MailId { get; set; }

	}

//一键删除邮件
	[ResponseType(nameof(G2C_AutoDeleteMail))]
	[Message(OuterOpcode.C2G_AutoDeleteMail)]
	[ProtoContract]
	public partial class C2G_AutoDeleteMail: Object, IRequest
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

	}

	[Message(OuterOpcode.G2C_AutoDeleteMail)]
	[ProtoContract]
	public partial class G2C_AutoDeleteMail: Object, IResponse
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int Error { get; set; }

		[ProtoMember(3)]
		public string Message { get; set; }

		[ProtoMember(4)]
		public PlayerMailInfo MailInfos { get; set; }

	}

//获取邮件附件
	[ResponseType(nameof(G2C_GetMailEnclosure))]
	[Message(OuterOpcode.C2G_GetMailEnclosure)]
	[ProtoContract]
	public partial class C2G_GetMailEnclosure: Object, IRequest
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public long MailId { get; set; }

	}

	[Message(OuterOpcode.G2C_GetMailEnclosure)]
	[ProtoContract]
	public partial class G2C_GetMailEnclosure: Object, IResponse
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int Error { get; set; }

		[ProtoMember(3)]
		public string Message { get; set; }

		[ProtoMember(4)]
		public AwardInfo awardInfo { get; set; }

	}

//一键领取
	[ResponseType(nameof(G2C_AutoGetMailEnclosure))]
	[Message(OuterOpcode.C2G_AutoGetMailEnclosure)]
	[ProtoContract]
	public partial class C2G_AutoGetMailEnclosure: Object, IRequest
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

	}

	[Message(OuterOpcode.G2C_AutoGetMailEnclosure)]
	[ProtoContract]
	public partial class G2C_AutoGetMailEnclosure: Object, IResponse
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int Error { get; set; }

		[ProtoMember(3)]
		public string Message { get; set; }

		[ProtoMember(4)]
		public AwardInfo awardInfo { get; set; }

		[ProtoMember(5)]
		public PlayerMailInfo PlayerMailInfo { get; set; }

	}

//开始排位
	[ResponseType(nameof(G2C_StartSeasonRank))]
	[Message(OuterOpcode.C2G_StartSeasonRank)]
	[ProtoContract]
	public partial class C2G_StartSeasonRank: Object, IRequest
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public long PlayerId { get; set; }

	}

	[Message(OuterOpcode.G2C_StartSeasonRank)]
	[ProtoContract]
	public partial class G2C_StartSeasonRank: Object, IResponse
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int Error { get; set; }

		[ProtoMember(3)]
		public string Message { get; set; }

	}

//取消排位
	[ResponseType(nameof(G2C_CancelSeasonRank))]
	[Message(OuterOpcode.C2G_CancelSeasonRank)]
	[ProtoContract]
	public partial class C2G_CancelSeasonRank: Object, IRequest
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public long PlayerId { get; set; }

	}

	[Message(OuterOpcode.G2C_CancelSeasonRank)]
	[ProtoContract]
	public partial class G2C_CancelSeasonRank: Object, IResponse
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int Error { get; set; }

		[ProtoMember(3)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(G2C_MatchRankPlayer))]
	[Message(OuterOpcode.C2G_MatchRankPlayer)]
	[ProtoContract]
	public partial class C2G_MatchRankPlayer: Object, IRequest
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public long PlayerId { get; set; }

	}

	[Message(OuterOpcode.G2C_MatchRankPlayer)]
	[ProtoContract]
	public partial class G2C_MatchRankPlayer: Object, IResponse
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int Error { get; set; }

		[ProtoMember(3)]
		public string Message { get; set; }

		[ProtoMember(4)]
		public List<long> PlayerId = new List<long>();

		[ProtoMember(6)]
		public List<string> PlayerName = new List<string>();

		[ProtoMember(7)]
		public List<long> PlayerRankPoint = new List<long>();

	}

	[ResponseType(nameof(G2C_SendRankResult))]
	[Message(OuterOpcode.C2G_SendRankResult)]
	[ProtoContract]
	public partial class C2G_SendRankResult: Object, IRequest
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public bool Win { get; set; }

	}

	[Message(OuterOpcode.G2C_SendRankResult)]
	[ProtoContract]
	public partial class G2C_SendRankResult: Object, IResponse
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int Error { get; set; }

		[ProtoMember(3)]
		public string Message { get; set; }

		[ProtoMember(4)]
		public int PlayerSocre { get; set; }

	}

	[ResponseType(nameof(G2C_GetPlayerRank))]
	[Message(OuterOpcode.C2G_GetPlayerRank)]
	[ProtoContract]
	public partial class C2G_GetPlayerRank: Object, IRequest
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

	}

	[Message(OuterOpcode.G2C_GetPlayerRank)]
	[ProtoContract]
	public partial class G2C_GetPlayerRank: Object, IResponse
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

	[ResponseType(nameof(G2C_GetPlayerRankList))]
	[Message(OuterOpcode.C2G_GetPlayerRankList)]
	[ProtoContract]
	public partial class C2G_GetPlayerRankList: Object, IRequest
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

	}

	[Message(OuterOpcode.G2C_GetPlayerRankList)]
	[ProtoContract]
	public partial class G2C_GetPlayerRankList: Object, IResponse
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
		public PlayerRankListInfo playerRankListInfo { get; set; }

	}

	[ResponseType(nameof(G2C_GetPlayerSeasonRankList))]
	[Message(OuterOpcode.C2G_GetPlayerSeasonRankList)]
	[ProtoContract]
	public partial class C2G_GetPlayerSeasonRankList: Object, IRequest
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int PlayerId { get; set; }

	}

	[Message(OuterOpcode.G2C_GetPlayerSeasonRankList)]
	[ProtoContract]
	public partial class G2C_GetPlayerSeasonRankList: Object, IResponse
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

	[ResponseType(nameof(G2C_Setting))]
	[Message(OuterOpcode.C2G_Setting)]
	[ProtoContract]
	public partial class C2G_Setting: Object, IRequest
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public string NikiName { get; set; }

		[ProtoMember(3)]
		public int EmblemID { get; set; }

	}

	[Message(OuterOpcode.G2C_Setting)]
	[ProtoContract]
	public partial class G2C_Setting: Object, IResponse
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int Error { get; set; }

		[ProtoMember(3)]
		public string Message { get; set; }

	}

//更改玩家姓名
	[ResponseType(nameof(G2C_OnSetName))]
	[Message(OuterOpcode.C2G_OnSetName)]
	[ProtoContract]
	public partial class C2G_OnSetName: Object, IRequest
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public string NikiName { get; set; }

	}

	[Message(OuterOpcode.G2C_OnSetName)]
	[ProtoContract]
	public partial class G2C_OnSetName: Object, IResponse
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int Error { get; set; }

		[ProtoMember(3)]
		public string Message { get; set; }

	}

//阵容切换
	[ResponseType(nameof(G2C_FormationInfoChange))]
	[Message(OuterOpcode.C2G_FormationInfoChange)]
	[ProtoContract]
	public partial class C2G_FormationInfoChange: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long RoleId { get; set; }

		[ProtoMember(2)]
		public int Index { get; set; }

		[ProtoMember(3)]
		public long HeroId { get; set; }

	}

	[Message(OuterOpcode.G2C_FormationInfoChange)]
	[ProtoContract]
	public partial class G2C_FormationInfoChange: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public SquadsControllerComponentInfo squadsInfo { get; set; }

	}

//自动布阵
	[ResponseType(nameof(G2C_AutoEmbattle))]
	[Message(OuterOpcode.C2G_AutoEmbattle)]
	[ProtoContract]
	public partial class C2G_AutoEmbattle: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long RoleId { get; set; }

	}

	[Message(OuterOpcode.G2C_AutoEmbattle)]
	[ProtoContract]
	public partial class G2C_AutoEmbattle: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public long RoleId { get; set; }

		[ProtoMember(2)]
		public SquadsControllerComponentInfo squadsInfo { get; set; }

		[ProtoMember(3)]
		public List<int> indexs = new List<int>();

	}

//穿戴装备
	[ResponseType(nameof(G2C_PutonEquip))]
	[Message(OuterOpcode.C2G_PutonEquip)]
	[ProtoContract]
	public partial class C2G_PutonEquip: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long RoleId { get; set; }

		[ProtoMember(2)]
		public long EquipId { get; set; }

		[ProtoMember(3)]
		public long HeroId { get; set; }

	}

	[Message(OuterOpcode.G2C_PutonEquip)]
	[ProtoContract]
	public partial class G2C_PutonEquip: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//卸载装备
	[ResponseType(nameof(G2C_UnfixEquip))]
	[Message(OuterOpcode.C2G_UnfixEquip)]
	[ProtoContract]
	public partial class C2G_UnfixEquip: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long RoleId { get; set; }

		[ProtoMember(2)]
		public long EquipId { get; set; }

		[ProtoMember(3)]
		public long HeroId { get; set; }

	}

	[Message(OuterOpcode.G2C_UnfixEquip)]
	[ProtoContract]
	public partial class G2C_UnfixEquip: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//充值请求
	[ResponseType(nameof(G2C_Purchase))]
	[Message(OuterOpcode.C2G_Purchase)]
	[ProtoContract]
	public partial class C2G_Purchase: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long PurchaseID { get; set; }

		[ProtoMember(2)]
		public long RoleId { get; set; }

	}

	[Message(OuterOpcode.G2C_Purchase)]
	[ProtoContract]
	public partial class G2C_Purchase: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(93)]
		public long PayCurrencyNum { get; set; }

		[ProtoMember(94)]
		public long BuyPrice { get; set; }

	}

//充值奖励获取
	[ResponseType(nameof(G2C_GetPurchaseAward))]
	[Message(OuterOpcode.C2G_GetPurchaseAward)]
	[ProtoContract]
	public partial class C2G_GetPurchaseAward: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long RoleId { get; set; }

		[ProtoMember(2)]
		public long AwardId { get; set; }

	}

	[Message(OuterOpcode.G2C_GetPurchaseAward)]
	[ProtoContract]
	public partial class G2C_GetPurchaseAward: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public AwardInfo awardInfo { get; set; }

	}

//冶炼请求
	[ResponseType(nameof(G2C_Recoin))]
	[Message(OuterOpcode.C2G_Recoin)]
	[ProtoContract]
	public partial class C2G_Recoin: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long RoleId { get; set; }

		[ProtoMember(2)]
		public long EquipAId { get; set; }

		[ProtoMember(3)]
		public long EquipBId { get; set; }

	}

	[Message(OuterOpcode.G2C_Recoin)]
	[ProtoContract]
	public partial class G2C_Recoin: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(10001)]
		public EquipAppendPropertyInfo EquipAppendProperties { get; set; }

	}

//英雄升级强化
	[ResponseType(nameof(G2C_HeroGradeIntensify))]
	[Message(OuterOpcode.C2G_HeroGradeIntensify)]
	[ProtoContract]
	public partial class C2G_HeroGradeIntensify: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long RoleId { get; set; }

		[ProtoMember(2)]
		public long HeroId { get; set; }

		[ProtoMember(3)]
		public int HeroGrade { get; set; }

		[ProtoMember(4)]
		public int ReduceType { get; set; }

	}

	[Message(OuterOpcode.G2C_HeroGradeIntensify)]
	[ProtoContract]
	public partial class G2C_HeroGradeIntensify: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//英雄升阶
	[ResponseType(nameof(G2C_HeroPhaseGradeIntensify))]
	[Message(OuterOpcode.C2G_HeroPhaseGradeIntensify)]
	[ProtoContract]
	public partial class C2G_HeroPhaseGradeIntensify: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long RoleId { get; set; }

		[ProtoMember(2)]
		public long HeroId { get; set; }

		[ProtoMember(3)]
		public int HeroPhaseGrade { get; set; }

	}

	[Message(OuterOpcode.G2C_HeroPhaseGradeIntensify)]
	[ProtoContract]
	public partial class G2C_HeroPhaseGradeIntensify: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//选择功法请求
	[ResponseType(nameof(G2C_AffirmGongFa))]
	[Message(OuterOpcode.C2G_AffirmGongFa)]
	[ProtoContract]
	public partial class C2G_AffirmGongFa: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long RoleId { get; set; }

		[ProtoMember(2)]
		public long TableId { get; set; }

		[ProtoMember(3)]
		public long ParentId { get; set; }

	}

	[Message(OuterOpcode.G2C_AffirmGongFa)]
	[ProtoContract]
	public partial class G2C_AffirmGongFa: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public GongFaInfo info { get; set; }

	}

//升级功法请求
	[ResponseType(nameof(G2C_GongFaUp))]
	[Message(OuterOpcode.C2G_GongFaUp)]
	[ProtoContract]
	public partial class C2G_GongFaUp: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long RoleId { get; set; }

		[ProtoMember(2)]
		public long id { get; set; }

	}

	[Message(OuterOpcode.G2C_GongFaUp)]
	[ProtoContract]
	public partial class G2C_GongFaUp: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public GongFaInfo info { get; set; }

	}

//英雄天赋强化
	[ResponseType(nameof(G2C_HeroTalentIntensify))]
	[Message(OuterOpcode.C2G_HeroTalentIntensify)]
	[ProtoContract]
	public partial class C2G_HeroTalentIntensify: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long RoleId { get; set; }

		[ProtoMember(2)]
		public long HeroId { get; set; }

		[ProtoMember(3)]
		public int PliesNum { get; set; }

		[ProtoMember(4)]
		public int TalentNum { get; set; }

	}

	[Message(OuterOpcode.G2C_HeroTalentIntensify)]
	[ProtoContract]
	public partial class G2C_HeroTalentIntensify: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<long> TalentValueArray = new List<long>();

	}

//英雄修炼强化
	[ResponseType(nameof(G2C_HeroPracticeIntensify))]
	[Message(OuterOpcode.C2G_HeroPracticeIntensify)]
	[ProtoContract]
	public partial class C2G_HeroPracticeIntensify: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long RoleId { get; set; }

		[ProtoMember(2)]
		public long HeroId { get; set; }

	}

	[Message(OuterOpcode.G2C_HeroPracticeIntensify)]
	[ProtoContract]
	public partial class G2C_HeroPracticeIntensify: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public int PracticeProgressPct { get; set; }

	}

//关卡挑战胜利
	[ResponseType(nameof(G2C_GameLevelCombatVictory))]
	[Message(OuterOpcode.C2G_GameLevelCombatVictory)]
	[ProtoContract]
	public partial class C2G_GameLevelCombatVictory: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long RoleId { get; set; }

		[ProtoMember(2)]
		public long CurGameLevel { get; set; }

		[ProtoMember(3)]
		public long Time { get; set; }

		[ProtoMember(4)]
		public bool isNextTask { get; set; }

	}

	[Message(OuterOpcode.G2C_GameLevelCombatVictory)]
	[ProtoContract]
	public partial class G2C_GameLevelCombatVictory: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public long CurGameLevel { get; set; }

		[ProtoMember(2)]
		public AwardInfo awardInfo { get; set; }

	}

//购买商品
	[ResponseType(nameof(G2C_BuyShop))]
	[Message(OuterOpcode.C2G_BuyShop)]
	[ProtoContract]
	public partial class C2G_BuyShop: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long RoleId { get; set; }

		[ProtoMember(2)]
		public long GoodId { get; set; }

		[ProtoMember(3)]
		public int GoodCount { get; set; }

	}

	[Message(OuterOpcode.G2C_BuyShop)]
	[ProtoContract]
	public partial class G2C_BuyShop: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(2)]
		public AwardInfo awardInfo { get; set; }

	}

//刷新竞技场列表
	[ResponseType(nameof(G2C_RefreshRankList))]
	[Message(OuterOpcode.C2G_RefreshRankList)]
	[ProtoContract]
	public partial class C2G_RefreshRankList: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long RoleId { get; set; }

	}

	[Message(OuterOpcode.G2C_RefreshRankList)]
	[ProtoContract]
	public partial class G2C_RefreshRankList: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//竞技场挑战
	[ResponseType(nameof(G2C_RankListChallenge))]
	[Message(OuterOpcode.C2G_RankListChallenge)]
	[ProtoContract]
	public partial class C2G_RankListChallenge: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long RoleId { get; set; }

		[ProtoMember(2)]
		public long ChallengeId { get; set; }

	}

	[Message(OuterOpcode.G2C_RankListChallenge)]
	[ProtoContract]
	public partial class G2C_RankListChallenge: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<long> Heros = new List<long>();

	}

//副本挑战胜利
	[ResponseType(nameof(G2C_ChallengeVictory))]
	[Message(OuterOpcode.C2G_ChallengeVictory)]
	[ProtoContract]
	public partial class C2G_ChallengeVictory: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long RoleId { get; set; }

		[ProtoMember(2)]
		public long CurChallengeLevel { get; set; }

	}

	[Message(OuterOpcode.G2C_ChallengeVictory)]
	[ProtoContract]
	public partial class G2C_ChallengeVictory: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public long CurChallengeLevel { get; set; }

		[ProtoMember(2)]
		public AwardInfo awardInfo { get; set; }

	}

//新手任务完成
	[ResponseType(nameof(G2C_NewPlayerTaskFinish))]
	[Message(OuterOpcode.C2G_NewPlayerTaskFinish)]
	[ProtoContract]
	public partial class C2G_NewPlayerTaskFinish: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long RoleId { get; set; }

		[ProtoMember(2)]
		public long NewPlayerTaskId { get; set; }

		[ProtoMember(3)]
		public bool isNextTask { get; set; }

	}

	[Message(OuterOpcode.G2C_NewPlayerTaskFinish)]
	[ProtoContract]
	public partial class G2C_NewPlayerTaskFinish: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//通过ID查询玩家，返回玩家姓名与战力
	[ResponseType(nameof(G2C_QueryIdPlayer))]
	[Message(OuterOpcode.C2G_QueryIdPlayer)]
	[ProtoContract]
	public partial class C2G_QueryIdPlayer: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long RoleId { get; set; }

	}

	[Message(OuterOpcode.G2C_QueryIdPlayer)]
	[ProtoContract]
	public partial class G2C_QueryIdPlayer: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public string PlayerName { get; set; }

		[ProtoMember(2)]
		public long PlayerRankPoint { get; set; }

	}

//========================================IActorMessage========================================
	[Message(OuterOpcode.M2C_CreateUnits)]
	[ProtoContract]
	public partial class M2C_CreateUnits: Object, IActorMessage
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public long ActorId { get; set; }

		[ProtoMember(3)]
		public List<UnitInfo> Units = new List<UnitInfo>();

	}

	[Message(OuterOpcode.M2C_PathfindingResult)]
	[ProtoContract]
	public partial class M2C_PathfindingResult: Object, IActorMessage
	{
		[ProtoMember(1)]
		public long ActorId { get; set; }

		[ProtoMember(2)]
		public long Id { get; set; }

		[ProtoMember(3)]
		public float X { get; set; }

		[ProtoMember(4)]
		public float Y { get; set; }

		[ProtoMember(5)]
		public float Z { get; set; }

		[ProtoMember(6)]
		public List<float> Xs = new List<float>();

		[ProtoMember(7)]
		public List<float> Ys = new List<float>();

		[ProtoMember(8)]
		public List<float> Zs = new List<float>();

	}

	[Message(OuterOpcode.M2C_Stop)]
	[ProtoContract]
	public partial class M2C_Stop: Object, IActorMessage
	{
		[ProtoMember(1)]
		public int Error { get; set; }

		[ProtoMember(2)]
		public long Id { get; set; }

		[ProtoMember(3)]
		public float X { get; set; }

		[ProtoMember(4)]
		public float Y { get; set; }

		[ProtoMember(5)]
		public float Z { get; set; }

		[ProtoMember(6)]
		public float A { get; set; }

		[ProtoMember(7)]
		public float B { get; set; }

		[ProtoMember(8)]
		public float C { get; set; }

		[ProtoMember(9)]
		public float W { get; set; }

	}

	[Message(OuterOpcode.G2C_SendMail)]
	[ProtoContract]
	public partial class G2C_SendMail: Object, IActorMessage
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public long ActorId { get; set; }

		[ProtoMember(3)]
		public MailObjectInfo MailInfo { get; set; }

	}

//体力回复
	[Message(OuterOpcode.G2C_FatigueReply)]
	[ProtoContract]
	public partial class G2C_FatigueReply: Object, IActorMessage
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public long ActorId { get; set; }

		[ProtoMember(3)]
		public int FatigueValue { get; set; }

	}

	[Message(OuterOpcode.G2C_DeleteMail)]
	[ProtoContract]
	public partial class G2C_DeleteMail: Object, IActorMessage
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public long ActorId { get; set; }

		[ProtoMember(3)]
		public int Index { get; set; }

	}

//通知赛程的开启与关闭
	[Message(OuterOpcode.G2C_SeasonPass)]
	[ProtoContract]
	public partial class G2C_SeasonPass: Object, IActorMessage
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public bool IsActive { get; set; }

		[ProtoMember(3)]
		public long SeasonId { get; set; }

		[ProtoMember(4)]
		public SeasonComponentInfo SeasonComponentInfo { get; set; }

	}

//重置排位次数
	[Message(OuterOpcode.G2C_ResetRankCount)]
	[ProtoContract]
	public partial class G2C_ResetRankCount: Object, IActorMessage
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public long Time { get; set; }

		[ProtoMember(3)]
		public int Count { get; set; }

	}

//进入排位战斗
	[Message(OuterOpcode.G2C_RankCombatStart)]
	[ProtoContract]
	public partial class G2C_RankCombatStart: Object, IActorMessage
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

	}

//排位结果
	[Message(OuterOpcode.G2C_SeasonRankResult)]
	[ProtoContract]
	public partial class G2C_SeasonRankResult: Object, IActorMessage
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public bool Result { get; set; }

		[ProtoMember(3)]
		public AwardInfo AwardInfo { get; set; }

		[ProtoMember(4)]
		public SeasonComponentInfo Info { get; set; }

	}

//出战小队数据
	[Message(OuterOpcode.SquadsInfo)]
	[ProtoContract]
	public partial class SquadsInfo: Object, IMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1001)]
		public int CountMax { get; set; }

		[ProtoMember(10001)]
		public List<long> memberIds = new List<long>();

	}

//========================================IActorLocationMessage========================================
	[Message(OuterOpcode.C2M_PathfindingResult)]
	[ProtoContract]
	public partial class C2M_PathfindingResult: Object, IActorLocationMessage
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public long ActorId { get; set; }

		[ProtoMember(3)]
		public float X { get; set; }

		[ProtoMember(4)]
		public float Y { get; set; }

		[ProtoMember(5)]
		public float Z { get; set; }

	}

	[Message(OuterOpcode.C2M_Stop)]
	[ProtoContract]
	public partial class C2M_Stop: Object, IActorLocationMessage
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public long ActorId { get; set; }

	}

//========================================IActorLocationRequest========================================
	[ResponseType(nameof(M2C_TestResponse))]
	[Message(OuterOpcode.C2M_TestRequest)]
	[ProtoContract]
	public partial class C2M_TestRequest: Object, IActorLocationRequest
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public long ActorId { get; set; }

		[ProtoMember(3)]
		public string request { get; set; }

	}

	[Message(OuterOpcode.M2C_TestResponse)]
	[ProtoContract]
	public partial class M2C_TestResponse: Object, IActorLocationResponse
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int Error { get; set; }

		[ProtoMember(3)]
		public string Message { get; set; }

		[ProtoMember(4)]
		public string response { get; set; }

	}

	[ResponseType(nameof(Actor_TransferResponse))]
	[Message(OuterOpcode.Actor_TransferRequest)]
	[ProtoContract]
	public partial class Actor_TransferRequest: Object, IActorLocationRequest
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public long ActorId { get; set; }

		[ProtoMember(3)]
		public int MapIndex { get; set; }

	}

	[Message(OuterOpcode.Actor_TransferResponse)]
	[ProtoContract]
	public partial class Actor_TransferResponse: Object, IActorLocationResponse
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int Error { get; set; }

		[ProtoMember(3)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(M2C_TestRobotCase))]
	[Message(OuterOpcode.C2M_TestRobotCase)]
	[ProtoContract]
	public partial class C2M_TestRobotCase: Object, IActorLocationRequest
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public long ActorId { get; set; }

		[ProtoMember(3)]
		public int N { get; set; }

	}

	[Message(OuterOpcode.M2C_TestRobotCase)]
	[ProtoContract]
	public partial class M2C_TestRobotCase: Object, IActorLocationResponse
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int Error { get; set; }

		[ProtoMember(3)]
		public string Message { get; set; }

		[ProtoMember(4)]
		public int N { get; set; }

	}

}
