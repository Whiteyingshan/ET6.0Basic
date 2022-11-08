using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public class ViewControllerComponent : Entity
    {
        public static ViewControllerComponent Instance { get; set; }
        public Scene ZoneScene => this.DomainScene();
        public readonly Dictionary<string, ViewPanel> ViewPanels = new Dictionary<string, ViewPanel>();
    }
}