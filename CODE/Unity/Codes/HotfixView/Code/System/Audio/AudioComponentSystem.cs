using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VEngine;

namespace ET
{
    internal static class AudioComponentSystem
    {
        private sealed class AudiComponentAwakeSystem : AwakeSystem<AudioComponent>
        {
            public override void Awake(AudioComponent self)
            {
                AudioComponent.Instance = self;
                GameObject parent = new GameObject("Audios");
                UnityEngine.Object.DontDestroyOnLoad(parent);
                parent.AddComponent<AudioListener>();
                self.BGMSource = parent.AddComponent<AudioSource>();
                self.EventSource = parent.AddComponent<AudioSource>();
                self.GamingSystemSource = parent.AddComponent<AudioSource>();

            }
        }


        public static async ETTask LoadAudio(this AudioComponent self, int sceneNum)
        {
            foreach (var item in AudioConfigCategory.Instance.GetAll())
            {
                if (item.Value.Type == sceneNum)
                {
                    string path = item.Value.Path;
                    Asset asset = await Asset.LoadAsync(path, typeof(AudioClip)).ETAsync();
                    asset = Asset.LoadAsync(path, typeof(AudioClip));
                    self.Clips.Add(item.Value.Name, asset);
                }

            }
        }
        private static AudioSource SelectSource(this AudioComponent self, AudioType audioType)
        {

            switch (audioType)
            {
                case AudioType.BGM:
                    return self.BGMSource;
                case AudioType.GamingSystem:
                    return self.GamingSystemSource;
                case AudioType.Event:
                    return self.EventSource;
                default:
                    return null;
            }
        }
        public static void Play(this AudioComponent self, string name, AudioType audioType, bool isLoop = false, float volume = 100)
        {

            AudioSource audioSource;
            if (!self.Clips.TryGetValue(name, out Asset asset))
            {
                throw new Exception($"找不到该音频: {name}");
            }
            audioSource = self.SelectSource(audioType);
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
            audioSource.clip = asset.asset as AudioClip;
            audioSource.loop = isLoop;
            audioSource.volume = volume;
            audioSource.Play();
        }

        public static void Play(this AudioComponent self, AudioConfig config)
        {

        }

        public static async void PlayByTime(this AudioComponent self, string name, AudioType audioType, float Time, bool isLoop = false, float volume = 100)
        {
            AudioSource audioSource;
            Asset asset;
            audioSource = self.SelectSource(audioType);
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
            if (!self.Clips.TryGetValue(name, out asset))
            {
                throw new Exception($"找不到该音频: {name}");
            }

            audioSource.clip = asset.asset as AudioClip;
            audioSource.loop = isLoop;
            audioSource.volume = volume;
            audioSource.Play();
            float time = TimeHelper.ClientNowSeconds();
            while (TimeHelper.ClientNowSeconds() - time < Time)
            {
                audioSource.Stop();
                TimerComponent.Instance.WaitFrameAsync().Coroutine();
            }
            await ETTask.CompletedTask;
        }

        public static void StopAll(this AudioComponent self)
        {
            self.BGMSource.Stop();
            self.EventSource.Stop();
            self.GamingSystemSource.Stop();
        }

        public static void StopOne(this AudioComponent self, AudioType audioType)
        {
            self.SelectSource(audioType).Stop();
        }
    }
}
