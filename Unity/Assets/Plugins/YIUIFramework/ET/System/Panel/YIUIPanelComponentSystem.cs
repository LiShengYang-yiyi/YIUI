//------------------------------------------------------------
// Author: 亦亦
// Mail: 379338943@qq.com
// Data: 2023年2月12日
//------------------------------------------------------------

namespace ET.Client
{
    /// <summary>
    /// UI面板组件
    /// </summary>
    [FriendOf(typeof (YIUIPanelComponent))]
    public static partial class YIUIPanelComponentSystem
    {
        [ObjectSystem]
        public class YIUIPanelComponentInitializeSystem: YIUIInitializeSystem<YIUIPanelComponent>
        {
            protected override void YIUIInitialize(YIUIPanelComponent self)
            {
                self.InitPanelViewData();
            }
        }
    }
}