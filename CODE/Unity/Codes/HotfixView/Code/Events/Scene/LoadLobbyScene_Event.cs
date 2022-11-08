using ET.EventType;

namespace ET.Code.Events
{
    internal class LoadLobbyScene_Event : AEvent<LoadLobbyScene>
    {
        protected override async ETTask Run(LoadLobbyScene args)
        {
            await VEngine.Scene.LoadAsync("Assets/Bundles/Scenes/Lobby.unity", null, true).ETAsync();
        }
    }
}