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
    [FriendOf(typeof (YIUILoadComponent))]
    public static partial class YIUILoadComponentSystem
    {
        [ObjectSystem]
        public class YIUILoadComponentAwakeSystem: AwakeSystem<YIUILoadComponent>
        {
            protected override void Awake(YIUILoadComponent self)
            {
                self.Awake();
            }
        }

        public class YIUILoadComponentAwake2System: AwakeSystem<YIUILoadComponent, string>
        {
            protected override void Awake(YIUILoadComponent self, string packageName)
            {
                self.Awake(packageName);
            }
        }

        [ObjectSystem]
        public class YIUILoadComponentDestroySystem: DestroySystem<YIUILoadComponent>
        {
            protected override void Destroy(YIUILoadComponent self)
            {
                self.Destroy();
            }
        }
    }
}