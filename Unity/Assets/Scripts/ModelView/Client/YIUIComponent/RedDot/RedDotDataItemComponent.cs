using YIUIFramework;

namespace ET.Client
{
    public partial class RedDotDataItemComponent: Entity
    {
        public EntityRef<RedDotPanelComponent> m_RedDotPanel;
        public RedDotPanelComponent            RedDotPanel => m_RedDotPanel;
        public RedDotData                      m_Data;
    }
}