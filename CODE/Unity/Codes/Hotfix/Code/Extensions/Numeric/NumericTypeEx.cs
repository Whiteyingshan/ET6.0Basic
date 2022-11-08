using System.Collections.Generic;

namespace ET
{
	public static partial class NumericTypeEx
	{
		public const int CommonCurrency = 1;

		public const int PayCurrency = 2;

		public const int RoleExp = 3;

		public const int Skill_Point = 4;

		public const int CurGameLevel = 101;

		public const int BusinessProgress = 102;

		public const int CurBusinessId = 103;

		public const int BusinessAwardProgress = 104;

		public const int ManorGrade = 105;

		public const int LastOpenManorBoxTime = 106;

		public const int LuckCoalesceCount = 107;

		public const int FactionGuildId = 108;

		public const int FactionGuildEverydayCheckInCount = 109;

		public const int FactionGuildEverydayThreatCount = 110;

		public const int FactionGuildEverydayThreatBossCount = 111;

		public const int FactionGuildThreatCount = 112;

		public const int TodayFactionGuildEverydayThreatCount = 113;

		public const int NewPlayerTaskId = 114;

		public const int FirstPurchaseCount = 115;

		public const int MonthPaymentCount = 116;

		public const int EverydayTenfoldCount = 117;

		public const int RoleGrade = 118;

		public const int StartAfkTime = 119;

		public const int ArenaPointNum = 120;

		public const int ArenaChallengeCount = 121;

		public const int RoleHeadIconId = 122;

		public const int RoleMapImageId = 123;

		public const int InitialCheckInCount = 124;

		public const int LastInitialCheckInTime = 125;

		public const int RegisterTime = 126;

		public const int CurTask = 127;

		public const int WorldGrade = 140;

		public const int CurWorldEx = 141;

		public const int AmassPay = 142;

		public const int PackgeGridCount = 143;

		public const int DayOnlineTime = 144;

		public const int EquipIntensifyGrade = 1001;

		public const int EquipAppendGrade = 1002;

		public const int EquipAppendPerfectRotio = 1003;

		public const int EquipIntensifyPerfectRotio = 1004;

		public const int HeroPhaseExp_Cur = 1005;

		public const int EquipCurRankExp = 1006;

		public const int EquipCurRankGrade = 1007;

		public const int GameLevelAwardCount = 1008;

		public const int SummonCount = 1009;

		public const int CuiltivationReapCount = 1010;

		public static Dictionary<long, KeyValuePair<long,string>> numericNameDict = new Dictionary<long, KeyValuePair<long, string>>()
		{
				{1,new KeyValuePair<long, string>(2,"金币")},
				{2,new KeyValuePair<long, string>(2,"钻石")},
				{3,new KeyValuePair<long, string>(2,"主角经验")},
				{4,new KeyValuePair<long, string>(2,"技能点")},
				{101,new KeyValuePair<long, string>(2,"关卡进度")},
				{102,new KeyValuePair<long, string>(2,"开店进度")},
				{103,new KeyValuePair<long, string>(2,"当前开店奇遇Id")},
				{104,new KeyValuePair<long, string>(2,"开店奖励进度")},
				{105,new KeyValuePair<long, string>(2,"庄园等级")},
				{106,new KeyValuePair<long, string>(2,"上次打开庄园箱子的时间")},
				{107,new KeyValuePair<long, string>(2,"装备合成保底")},
				{108,new KeyValuePair<long, string>(2,"阵营公会编号")},
				{109,new KeyValuePair<long, string>(2,"阵营公会可用签到次数")},
				{110,new KeyValuePair<long, string>(2,"阵营公会可用每日讨伐次数")},
				{111,new KeyValuePair<long, string>(2,"阵营公会可用每日首领次数")},
				{112,new KeyValuePair<long, string>(2,"阵营公会可用阵营讨伐次数")},
				{113,new KeyValuePair<long, string>(2,"今日阵营公会每日讨伐次数")},
				{114,new KeyValuePair<long, string>(2,"新手任务ID")},
				{115,new KeyValuePair<long, string>(2,"首冲领取次数")},
				{116,new KeyValuePair<long, string>(2,"月卡领取次数")},
				{117,new KeyValuePair<long, string>(2,"高级月卡领取次数")},
				{118,new KeyValuePair<long, string>(2,"主角等级")},
				{119,new KeyValuePair<long, string>(2,"开始挂机的时间")},
				{120,new KeyValuePair<long, string>(2,"竞技场点数")},
				{121,new KeyValuePair<long, string>(2,"竞技场挑战次数")},
				{122,new KeyValuePair<long, string>(2,"角色头像Id")},
				{123,new KeyValuePair<long, string>(2,"地图形象Id")},
				{124,new KeyValuePair<long, string>(2,"开服签到次数")},
				{125,new KeyValuePair<long, string>(2,"上一次签到的时间")},
				{126,new KeyValuePair<long, string>(2,"注册时间")},
				{127,new KeyValuePair<long, string>(2,"当前显示任务")},
				{140,new KeyValuePair<long, string>(2,"世界等级")},
				{141,new KeyValuePair<long, string>(2,"当前世界经验")},
				{142,new KeyValuePair<long, string>(2,"累积充值")},
				{143,new KeyValuePair<long, string>(2,"背包格子数")},
				{144,new KeyValuePair<long, string>(2,"每日在线时长")},
				{1001,new KeyValuePair<long, string>(2,"装备强化等级")},
				{1002,new KeyValuePair<long, string>(2,"装备追加等级")},
				{1003,new KeyValuePair<long, string>(2,"追加完美度")},
				{1004,new KeyValuePair<long, string>(2,"强化完美度")},
				{1005,new KeyValuePair<long, string>(2,"当前英雄羁绊值")},
				{1006,new KeyValuePair<long, string>(2,"装备当前Rank经验值")},
				{1007,new KeyValuePair<long, string>(2,"当前装备等级")},
				{1008,new KeyValuePair<long, string>(2,"获得主线关卡奖励次数")},
				{1009,new KeyValuePair<long, string>(2,"招聘次数")},
				{1010,new KeyValuePair<long, string>(2,"挂机钓鱼收获次数")},
				{9001,new KeyValuePair<long, string>(2,"当前耐久")},
				{9002,new KeyValuePair<long, string>(2,"当前生命值")},
				{9003,new KeyValuePair<long, string>(2,"当前内力")},
				{100001,new KeyValuePair<long, string>(1,"耐久")},
				{100002,new KeyValuePair<long, string>(1,"耐力")},
				{100003,new KeyValuePair<long, string>(1,"幸运")},
				{100004,new KeyValuePair<long, string>(1,"急速")},
				{100005,new KeyValuePair<long, string>(1,"敏捷")},
				{100006,new KeyValuePair<long, string>(1,"灵巧")},
				{100007,new KeyValuePair<long, string>(1,"会心")},
				{100008,new KeyValuePair<long, string>(1,"抗会心")},
				{100009,new KeyValuePair<long, string>(1,"力量")},
				{100010,new KeyValuePair<long, string>(1,"精神")},
				{100011,new KeyValuePair<long, string>(1,"体质")},
				{100013,new KeyValuePair<long, string>(1,"全抗性")},
				{100014,new KeyValuePair<long, string>(1,"生命回复")},
				{100015,new KeyValuePair<long, string>(1,"抗物理会心")},
				{100016,new KeyValuePair<long, string>(1,"抗魔法会心")},
				{100017,new KeyValuePair<long, string>(1,"最终伤害百分比")},
				{100018,new KeyValuePair<long, string>(1,"技能伤害")},
				{100019,new KeyValuePair<long, string>(1,"最终伤害")},
				{100020,new KeyValuePair<long, string>(1,"附加阴伤害")},
				{100021,new KeyValuePair<long, string>(1,"附加阳伤害")},
				{100027,new KeyValuePair<long, string>(1,"附加全属性伤害")},
				{100028,new KeyValuePair<long, string>(1,"阴抗性减伤")},
				{100029,new KeyValuePair<long, string>(1,"阳抗性减伤")},
				{100030,new KeyValuePair<long, string>(1,"金抗性减伤")},
				{100031,new KeyValuePair<long, string>(1,"木抗性减伤")},
				{100032,new KeyValuePair<long, string>(1,"水抗性减伤")},
				{100033,new KeyValuePair<long, string>(1,"火抗性减伤")},
				{100034,new KeyValuePair<long, string>(1,"土抗性减伤")},
				{100035,new KeyValuePair<long, string>(1,"全抗性减伤")},
				{100037,new KeyValuePair<long, string>(1,"魔法攻击")},
				{100038,new KeyValuePair<long, string>(1,"物理防御")},
				{100039,new KeyValuePair<long, string>(1,"魔法防御")},
				{100040,new KeyValuePair<long, string>(1,"物理增伤值")},
				{100041,new KeyValuePair<long, string>(1,"魔法增伤值")},
				{100042,new KeyValuePair<long, string>(1,"物理减伤值")},
				{100043,new KeyValuePair<long, string>(1,"魔法减伤值")},
				{100044,new KeyValuePair<long, string>(1,"全减伤值")},
				{100045,new KeyValuePair<long, string>(1,"闪避")},
				{100046,new KeyValuePair<long, string>(1,"绝对闪避")},
				{100047,new KeyValuePair<long, string>(1,"命中")},
				{100048,new KeyValuePair<long, string>(1,"技能冷却")},
				{100049,new KeyValuePair<long, string>(1,"物理会心伤害提升百分比")},
				{100050,new KeyValuePair<long, string>(1,"魔法会心伤害提升百分比")},
				{100051,new KeyValuePair<long, string>(1,"物理攻击百分比")},
				{100052,new KeyValuePair<long, string>(1,"魔法攻击百分比")},
				{100053,new KeyValuePair<long, string>(1,"耐久提升百分比")},
				{100054,new KeyValuePair<long, string>(1,"耐力提升百分比")},
				{100055,new KeyValuePair<long, string>(1,"幸运提升百分比")},
				{100056,new KeyValuePair<long, string>(1,"急速提升百分比")},
				{100057,new KeyValuePair<long, string>(1,"敏捷提升百分比")},
				{100058,new KeyValuePair<long, string>(1,"灵巧提升百分比")},
				{100059,new KeyValuePair<long, string>(1,"会心伤害百分比")},
				{100060,new KeyValuePair<long, string>(1,"抗会心伤害百分比")},
				{100061,new KeyValuePair<long, string>(1,"力量提升百分比")},
				{100062,new KeyValuePair<long, string>(1,"精神提升百分比")},
				{100063,new KeyValuePair<long, string>(1,"体质提升百分比")},
				{100064,new KeyValuePair<long, string>(1,"生命值提升百分比")},
				{100065,new KeyValuePair<long, string>(1,"装备全属性百分比")},
				{100066,new KeyValuePair<long, string>(1,"对人族类防御百分比")},
				{100067,new KeyValuePair<long, string>(1,"对人族类攻击百分比")},
				{100068,new KeyValuePair<long, string>(1,"对魔族类攻击百分比")},
				{100069,new KeyValuePair<long, string>(1,"对魔族类防御百分比")},
				{100070,new KeyValuePair<long, string>(1,"对妖族类攻击百分比")},
				{100071,new KeyValuePair<long, string>(1,"对妖族类防御百分比")},
				{100072,new KeyValuePair<long, string>(1,"对精怪类攻击百分比")},
				{100073,new KeyValuePair<long, string>(1,"对精怪类防御百分比")},
				{100074,new KeyValuePair<long, string>(1,"对异种类攻击百分比")},
				{100075,new KeyValuePair<long, string>(1,"对异种类防御百分比")},
				{100076,new KeyValuePair<long, string>(1,"装备使用等级降低")},
				{100077,new KeyValuePair<long, string>(1,"装备等级")},
				{100078,new KeyValuePair<long, string>(1,"移动速度")},
				{100079,new KeyValuePair<long, string>(1,"视野范围")},
				{100080,new KeyValuePair<long, string>(1,"攻击范围")},
				{100081,new KeyValuePair<long, string>(1,"攻击速度")},
				{100082,new KeyValuePair<long, string>(1,"技能冷却缩减")},
				{100012,new KeyValuePair<long, string>(1,"生命值")},
				{100022,new KeyValuePair<long, string>(1,"附加金伤害")},
				{100023,new KeyValuePair<long, string>(1,"附加木伤害")},
				{100024,new KeyValuePair<long, string>(1,"附加水伤害")},
				{100025,new KeyValuePair<long, string>(1,"附加火伤害")},
				{100026,new KeyValuePair<long, string>(1,"附加土伤害")},
				{100121,new KeyValuePair<long, string>(1,"金属性抗性")},
				{100122,new KeyValuePair<long, string>(1,"木属性抗性")},
				{100123,new KeyValuePair<long, string>(1,"水属性抗性")},
				{100124,new KeyValuePair<long, string>(1,"火属性抗性")},
				{100125,new KeyValuePair<long, string>(1,"土属性抗性")},
				{100036,new KeyValuePair<long, string>(1,"物理攻击")},
				{100083,new KeyValuePair<long, string>(1,"暴击率")},
				{100084,new KeyValuePair<long, string>(1,"抗暴击率")},
				{100085,new KeyValuePair<long, string>(1,"内力")},
				{100086,new KeyValuePair<long, string>(1,"回复内力效率")},
				{100087,new KeyValuePair<long, string>(1,"触发效果机率额外效率_Buff")},
				{100088,new KeyValuePair<long, string>(1,"伤害抗性_Buff")},
				{100089,new KeyValuePair<long, string>(1,"增伤率")},
				{100090,new KeyValuePair<long, string>(1,"减增伤率")},
				{100091,new KeyValuePair<long, string>(1,"暴击伤害")},
				{100092,new KeyValuePair<long, string>(1,"减爆伤")},
				{100093,new KeyValuePair<long, string>(1,"物理抗性")},
		};

	}
}
