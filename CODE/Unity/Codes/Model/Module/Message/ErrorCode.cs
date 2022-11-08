namespace ET
{
    public static partial class ErrorCode
    {
        public const int ERR_Success = 0;

        // 1-11004 是SocketError请看SocketError定义
        //-----------------------------------
        // 100000-109999是Core层的错误

        // 110000以下的错误请看ErrorCore.cs

        // 这里配置逻辑层的错误码
        // 110000 - 200000是抛异常的错误
        // 200001以上不抛异常
        /// <summary>
		/// 服务器需要重新处理登录
		/// </summary>
        public const int ERR_LinkException_NotRelinkYetCall = 210001;

        //未知错误
        public const int ERR_UnkownError = 200002;

        //200003-200100 登录错误
        //账号为空
        public const int ERR_UsernameIsNull = 200003;
        //密码为空
        public const int ERR_PasswordIsNull = 200004;
        //账号太短
        public const int ERR_UsernameIsTooShort = 200005;
        //账号太长
        public const int ERR_UsernameIsTooLong = 200006;
        //密码太短
        public const int ERR_PasswordIsTooShort = 200007;
        //密码太长
        public const int ERR_PasswordIsTooLong = 200008;
        //账号重复
        public const int ERR_AccountAlreadyExistent = 200009;
        //账号不存在
        public const int ERR_AccountNonExistent = 200010;
        //密码错误
        public const int ERR_PasswordError = 200011;
        //玩家不同步
        public const int ERR_DataOutOfSync = 200012;

        //200101-200200 英雄错误
        //英雄等级上限
        public const int ERR_HeroGradeLimit = 200101;
        //升级材料不足
        public const int ERR_UpgradeAssetsNotEnough = 200102;


        //200201-200300 体力错误
        //疲劳不足
        public const int ERR_FatigueLack = 200201;
        //200301-200400
        //未匹配到玩家
        public const int ERR_NoMatchPlayer = 200301;
        //不在赛季开启时间
        public const int ERR_NotOpenMatch = 200302;
        //200401-200500
        //---------------------------GateError---------------------------------
        /// <summary>
		/// 游戏数据不同步
		/// </summary>
        public const int ERR_GameDataNotSync = 230001;
    }
}