using UnityEngine;

namespace ET.Code.Events
{
    public class AfterCreateZoneScene_AddComponent : AEvent<EventType.AfterCreateZoneScene>
    {
        protected override async ETTask Run(EventType.AfterCreateZoneScene args)
        {
            args.ZoneScene.AddComponent<ViewControllerComponent>();
            AudioComponent audioComponent = args.ZoneScene.AddComponent<AudioComponent>();

            await audioComponent.LoadAudio(1);

            PlayerPrefs.GetFloat("GameSoundeffect", 50);
            audioComponent.Play("Login_Start", AudioType.BGM, true, PlayerPrefs.GetFloat("MusicVolume", 50) / 100);
            await ETTask.CompletedTask;
        }
    }
}