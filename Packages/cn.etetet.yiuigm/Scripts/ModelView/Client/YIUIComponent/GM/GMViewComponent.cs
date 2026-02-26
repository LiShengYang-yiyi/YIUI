using System.Collections.Generic;
using YIUIFramework;

namespace ET.Client
{
    public partial class GMViewComponent : Entity, IDynamicEvent<OnGMEventClose>
    {
        public bool                           Opened;
        public List<int>                      GMTypeData;
        public EntityRef<YIUILoopScrollChild> m_GMTypeLoop;
        public YIUILoopScrollChild            GMTypeLoop => m_GMTypeLoop;

        public EntityRef<YIUILoopScrollChild> m_GMCommandLoop;
        public YIUILoopScrollChild            GMCommandLoop => m_GMCommandLoop;

        public EntityRef<GMCommandComponent> m_CommandComponent;
        public GMCommandComponent            CommandComponent => m_CommandComponent;

        public IntPrefs m_GMTypeIndex = new("GMTypeIndex");
    }
}