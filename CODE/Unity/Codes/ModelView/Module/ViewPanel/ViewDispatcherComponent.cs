using System;

namespace ET.Module.ViewPanel
{
    public sealed class ViewDispatcherComponent : Entity
    {
        public readonly DoubleMap<string, Type> ViewTypes = new DoubleMap<string, Type>();
    }
}