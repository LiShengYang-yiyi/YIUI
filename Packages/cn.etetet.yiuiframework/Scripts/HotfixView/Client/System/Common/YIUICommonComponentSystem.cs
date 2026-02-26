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
    [FriendOf(typeof(YIUICommonComponent))]
    [EntitySystemOf(typeof(YIUICommonComponent))]
    public static partial class YIUICommonComponentSystem
    {
        [EntitySystem]
        private static void Awake(this YIUICommonComponent self)
        {
        }

        [EntitySystem]
        private static void YIUIInitialize(this YIUICommonComponent self)
        {
        }

        [EntitySystem]
        private static void Destroy(this YIUICommonComponent self)
        {
        }
    }
}
