using System;

namespace ET.Handler.G2R
{
    internal class G2R_CheckLoginKeyHandler : AMActorRpcHandler<Scene, G2R_CheckLoginKey, R2G_CheckLoginKey>
    {
        protected override async ETTask Run(Scene scene, G2R_CheckLoginKey request, R2G_CheckLoginKey response, Action reply)
        {
            string UUID = scene.GetComponent<GateSessionKeyComponent>().Get(request.Token ?? string.Empty);
            if (string.IsNullOrEmpty(UUID))
            {
                response.Error = ErrorCore.ERR_ConnectGateKeyError;
                response.Message = "Gate key验证失败!";
                reply();
                return;
            }

            response.UUID = UUID;
            reply();
            await ETTask.CompletedTask;
        }
    }
}