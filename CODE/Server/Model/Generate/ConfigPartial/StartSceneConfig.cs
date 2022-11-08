using System.Collections.Generic;
using System.ComponentModel;
using System.Net;

namespace ET
{
    public partial class StartSceneConfigCategory
    {
        public StartSceneConfig RealmConfig;

        public StartSceneConfig LocationConfig;

        public List<StartSceneConfig> Robots = new List<StartSceneConfig>();

        public MultiMap<int, StartSceneConfig> Gates = new MultiMap<int, StartSceneConfig>();

        public MultiMap<int, StartSceneConfig> Battles = new MultiMap<int, StartSceneConfig>();

        public MultiMap<int, StartSceneConfig> ProcessScenes = new MultiMap<int, StartSceneConfig>();

        public Dictionary<long, Dictionary<string, StartSceneConfig>> ZoneScenesByName = new Dictionary<long, Dictionary<string, StartSceneConfig>>();

        public List<StartSceneConfig> GetByProcess(int process)
        {
            return ProcessScenes[process];
        }

        public StartSceneConfig GetBySceneName(int zone, string name)
        {
            return ZoneScenesByName[zone][name];
        }

        public override void AfterEndInit()
        {
            foreach (StartSceneConfig startSceneConfig in GetAll().Values)
            {
                ProcessScenes.Add(startSceneConfig.Process, startSceneConfig);

                if (!ZoneScenesByName.ContainsKey(startSceneConfig.Zone))
                {
                    ZoneScenesByName.Add(startSceneConfig.Zone, new Dictionary<string, StartSceneConfig>());
                }
                ZoneScenesByName[startSceneConfig.Zone].Add(startSceneConfig.Name, startSceneConfig);

                switch (startSceneConfig.Type)
                {
                    case SceneType.Location:
                        LocationConfig = startSceneConfig;
                        break;
                    case SceneType.Realm:
                        RealmConfig = startSceneConfig;
                        break;
                    case SceneType.Robot:
                        Robots.Add(startSceneConfig);
                        break;
                    case SceneType.Gate:
                        Gates.Add(startSceneConfig.Zone, startSceneConfig);
                        break;
                }
            }
        }
    }

    public partial class StartSceneConfig : ISupportInitialize
    {
        public long InstanceId;
        public SceneType Type;
        public StartProcessConfig StartProcessConfig => StartProcessConfigCategory.Instance.Get(Process);
        public StartZoneConfig StartZoneConfig => StartZoneConfigCategory.Instance.Get(Zone);

        // 内网地址外网端口，通过防火墙映射端口过来
        private IPEndPoint innerIPOutPort;
        public IPEndPoint InnerIPOutPort
        {
            get
            {
                if (innerIPOutPort == null)
                {
                    innerIPOutPort = NetworkHelper.ToIPEndPoint($"{StartProcessConfig.InnerIP}:{OuterPort}");
                }

                return innerIPOutPort;
            }
        }

        // 外网地址外网端口
        private IPEndPoint outerIPPort;
        public IPEndPoint OuterIPPort
        {
            get
            {
                if (outerIPPort == null)
                {
                    outerIPPort = NetworkHelper.ToIPEndPoint($"{StartProcessConfig.OuterIP}:{OuterPort}");
                }

                return outerIPPort;
            }
        }

        public IPEndPoint BindIP => new IPEndPoint(IPAddress.Any, OuterPort);

        public override void BeginInit()
        {
        }

        public override void EndInit()
        {
            Type = EnumHelper.FromString<SceneType>(SceneType);
            InstanceId = new InstanceIdStruct(Process, (uint)Id).ToLong();
        }
    }
}