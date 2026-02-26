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
    [FriendOf(typeof(YIUIPanelComponent))]
    [EntitySystemOf(typeof(YIUIPanelComponent))]
    public static partial class YIUIPanelComponentSystem
    {
        [EntitySystem]
        private static void Awake(this YIUIPanelComponent self)
        {
            self.UIBindVo = self.UIBase.UIBindVo;
        }

        [EntitySystem]
        private static void YIUIInitialize(this YIUIPanelComponent self)
        {
            self.InitPanelViewData();
        }

        [EntitySystem]
        private static void Destroy(this YIUIPanelComponent self)
        {
            self.StopCountDownDestroyPanel();

            EventSystem.Instance?.YIUIInvokeEntitySyncSafety(self, new YIUIInvokeEntity_DestroyPanel
            {
                PanelName = self.UIBindVo.ComponentType.Name
            });
        }
    }
}