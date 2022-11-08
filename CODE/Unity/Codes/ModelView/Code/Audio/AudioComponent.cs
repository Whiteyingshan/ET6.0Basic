using System.Collections.Generic;
using UnityEngine;
using VEngine;

namespace ET
{
    public enum AudioType
    {
        BGM = 0,
        GamingSystem = 1,
        Event = 2
    }

    public class AudioComponent : Entity
    {
        public AudioSource BGMSource;
        public AudioSource EventSource;
        public AudioSource GamingSystemSource;
        public Dictionary<string, Asset> Clips = new Dictionary<string, Asset>();

        public static AudioComponent Instance { get; set; }
    }
}