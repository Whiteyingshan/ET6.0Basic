using System;
using System.Linq;

namespace ET
{
    public static class LoginHelper
    {
        public static string GateAddress;

#if NOT_UNITY
        public static async ETTask LoginRobot(Scene scene, string address, string username, string password)
        {
            R2C_Login r2c = await Login(scene, address, username, password) as R2C_Login;
            await GetServers(scene, address);
            G2C_LoginGate response = await LoginGate(scene, 0, r2c.Message) as G2C_LoginGate;
            AccountZone accountZone = scene.AddChild<AccountZone>();
            Game.Scene.GetComponent<MessageConvertComponent>().Handle(accountZone, response.AccountZoneInfo);
            await CreatePlayer(scene);
            await LoginPlayer(scene, accountZone.Players.First().Value.First());
        }
#endif

        public static async ETTask<IResponse> RealName(Scene zoneScene, string address, string username,int age)
        {
            Session session = default;

            try
            {
                // 创建一个ETModel层的Session
                session = zoneScene.GetComponent<NetKcpComponent>().Create(NetworkHelper.ToIPEndPoint(address));
                R2C_SetReal r2c_response = await session.Call(new C2R_SetReal() { Username = username, Age=age}) as R2C_SetReal;
                return r2c_response;
            }
            catch (Exception e)
            {
                return new Response() { Error = ErrorCode.ERR_UnkownError, Message = e.Message };
            }
            finally
            {
                session?.Dispose();
            }
        }

        public static async ETTask<IResponse> Login(Scene zoneScene, string address, string username, string password)
        {
            Session session = default;

            try
            {
                // 创建一个ETModel层的Session
                session = zoneScene.GetComponent<NetKcpComponent>().Create(NetworkHelper.ToIPEndPoint(address));
                R2C_Login r2c_response = await session.Call(new C2R_Login() { Username = username, Password = password }) as R2C_Login;
                return r2c_response;
            }
            catch (Exception e)
            {
                return new Response() { Error = ErrorCode.ERR_UnkownError, Message = e.Message };
            }
            finally
            {
                session?.Dispose();
            }
        }

        public static async ETTask<IResponse> Register(Scene scene, string address, string username, string password)
        {
            Session session = null;

            try
            {
                // 创建一个ETModel层的Session
                session = scene.GetComponent<NetKcpComponent>().Create(NetworkHelper.ToIPEndPoint(address));
                R2C_Register r2c_response = await session.Call(new C2R_Register() { Username = username, Password = password }) as R2C_Register;
                return r2c_response;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return new Response() { Error = ErrorCode.ERR_UnkownError };
            }
            finally
            {
                session?.Dispose();
            }
        }

        public static async ETTask<IResponse> GetServers(Scene zoneScene, string address)
        {
            Session session = default;

            try
            {
                // 创建一个ETModel层的Session
                session = zoneScene.GetComponent<NetKcpComponent>().Create(NetworkHelper.ToIPEndPoint(address));
                R2C_GetServers r2c = await session.Call(new C2R_GetServers()) as R2C_GetServers;
                Game.Scene.GetComponent<LoginServerComponent>().GateAddress = r2c.Address;
                return r2c;
            }
            catch (Exception e)
            {
                return new Response() { Error = ErrorCode.ERR_UnkownError, Message = e.Message };
            }
            finally
            {
                session?.Dispose();
            }
        }

        public static async ETTask<IResponse> LoginGate(Scene zoneScene, int serverId, string token)
        {
            Session session = default;

            try
            {
                // 创建一个gate Session,并且保存到SessionComponent中
                string address = Game.Scene.GetComponent<LoginServerComponent>().GateAddress;
                session = zoneScene.GetComponent<NetKcpComponent>().Create(NetworkHelper.ToIPEndPoint(address));
                session.AddComponent<PingComponent>();
                zoneScene.AddComponent<SessionComponent>().Session = session;

                G2C_LoginGate g2c_response = await session.Call(new C2G_LoginGate() { Token = token }) as G2C_LoginGate;
                if (g2c_response.Error != ErrorCode.ERR_Success)
                {
                    return g2c_response;
                }
                zoneScene.AddChild<AccountZone>();
                ConvertHelper.Handle(AccountZone.Inst, g2c_response.AccountZoneInfo);
                return g2c_response;
            }
            catch (Exception e)
            {
                Log.Error(e);
                session?.Dispose();
                AccountZone.Inst?.Dispose();
                zoneScene.RemoveComponent<SessionComponent>();
                return new Response() { Error = ErrorCode.ERR_UnkownError };
            }
        }

        public static async ETTask<IResponse> CreatePlayer(Scene zoneScene, string ServerId = null)
        {
            try
            {
                Session session = zoneScene.GetComponent<SessionComponent>().Session;
                G2C_CreatePlayer response = await session.Call(new C2G_CreatePlayer() { ServerId = ServerId }) as G2C_CreatePlayer;
                ConvertHelper.Handle(AccountZone.Inst, response.AccountZoneInfo);
                return response;
            }
            catch (Exception e)
            {
                Log.Error(e);
                zoneScene.RemoveComponent<SessionComponent>();
                return new Response() { Error = ErrorCode.ERR_UnkownError };
            }
        }

        public static async ETTask<IResponse> LoginPlayer(Scene zoneScene, long playerId)
        {
            try
            {
                Session session = zoneScene.GetComponent<SessionComponent>().Session;
                G2C_LoginPlayer response = await session.Call(new C2G_LoginPlayer() { PlayerId = playerId }) as G2C_LoginPlayer;
                if (response.Error != ErrorCode.ERR_Success)
                {
                    return response;
                }

                zoneScene.AddChild<Player>();
                ConvertHelper.Handle(Player.Inst, response.PlayerInfo);
                return response;
            }
            catch (Exception e)
            {
                Log.Error(e);
                Player.Inst?.Dispose();
                zoneScene.RemoveComponent<SessionComponent>();
                return new Response() { Error = ErrorCode.ERR_UnkownError };
            }
        }
    }
}