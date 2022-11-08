using System.Collections.Generic;

namespace ET
{
    namespace EventType
    {
        public struct AppStart
        {
        }

        public struct AfterCreateZoneScene
        {
            public Scene ZoneScene;
        }

        public struct LoadLobbyScene
        {
            public Scene ZoneScene;
        }

        public struct CreatePublicPanel
        {
            public Scene ZoneScene;
        }

        public struct CreateLoginPanel
        {
            public Scene ZoneScene;
        }

        public struct CreateLobbyPanel
        {
            public Scene ZoneScene;
        }

        public struct RemoveLoginPanel
        {

        }

        public struct MailChange
        {
            public AwardInfo awardInfo;
        }

        

        public struct EnterMapFinish
        {
            public Scene ZoneScene;
        }
        public struct Setting
        {
            public string NikiName;
            public int TeamEmblem;
        }
        public struct FormationInfoChange
        {
            public long RoleId;
            public int Index;
            public long HeroId;
        }
        public struct NumericChangeEvent
        {
            public Entity changeObj;
            public NumericType numericType;
            public long finalId;
            public long[] result;
        }
        public struct ReceiveRewards
        {
            public int ReceiveType;
            public int MailId;
            public long TaskId;
            public long AchieveId;
            public string RewardID;
            public string RewardNumber;
        }
    }
}