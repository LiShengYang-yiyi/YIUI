using System.Collections.Generic;
using UnityEngine.UI;
using YIUIFramework;

namespace ET.Client
{
    /// <summary>
    /// 文档: https://lib9kmxvq7k.feishu.cn/wiki/XzyawmryHitNVNk9QVtcDAftn5O
    /// </summary>
    public partial class RedDotPanelComponent : Entity,
            IDynamicEvent<OnClickParentListEvent>,
            IDynamicEvent<OnClickChildListEvent>,
            IDynamicEvent<OnClickItemEvent>
    {
        public EntityRef<YIUILoopScrollChild> m_SearchScroll;
        public YIUILoopScrollChild            SearchScroll => m_SearchScroll;

        public List<RedDotData>              m_CurrentDataList      = new();
        public Dictionary<int, int>          m_AllDropdownSearchDic = new();
        public List<Dropdown.OptionData>     m_DropdownOptionData   = new();
        public RedDotData                    m_InfoData;

        public EntityRef<YIUILoopScrollChild> m_StackScroll;
        public YIUILoopScrollChild            StackScroll => m_StackScroll;
    }

    public struct OnClickParentListEvent
    {
        public RedDotData Data;
    }

    public struct OnClickChildListEvent
    {
        public RedDotData Data;
    }

    public struct OnClickItemEvent
    {
        public RedDotData Data;
    }
}