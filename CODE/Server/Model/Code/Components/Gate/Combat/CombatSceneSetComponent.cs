using System.Collections.Generic;

namespace ET
{
    public sealed class CombatSceneSetComponent : Entity
    {
        public List<Scene> Scenes { get; set; }
        public Dictionary<long, Scene> PlayerScenes { get; set; }
    }
}