using ET.Code.Extensions;
using System.Linq;

namespace ET
{
    public static class PlayerExtension
    {
        public static async ETTask OnCreate(this Player player)
        {
            await Register_Role(player);
        }
        public static async ETTask Register_Role(this Player player)
        {
            
            player.BasicData.OnCreate();

            
            await ETTask.CompletedTask;
        }
        public static void OnLogin(this Player player)
        {

        }

        public static void OnRelink(this Player player)
        {
        }

        public static void OnLogout(this Player player)
        {

        }

        public static void OnNextDay(this Player player)
        {

        }
    }
}