//------------------------------------------------------------
// Author: 亦亦
// Mail: 379338943@qq.com
// Data: 2023年2月12日
//------------------------------------------------------------

namespace ET.Client
{
    [FriendOf(typeof (YIUIMgrComponent))]
    public static partial class YIUIMgrComponentSystem
    {
        [ObjectSystem]
        public class YIUIMgrComponentAwakeSystem: AwakeSystem<YIUIMgrComponent>
        {
            protected override void Awake(YIUIMgrComponent self)
            {
                YIUIMgrComponent.Inst = self;
                self.InitAllBind();
                self.AddComponent<YIUIEventComponent>();
                self.AddComponent<YIUILoadComponent>();
            }
        }

        [ObjectSystem]
        public class YIUIMgrComponentDestroySystem: DestroySystem<YIUIMgrComponent>
        {
            protected override void Destroy(YIUIMgrComponent self)
            {
                self.OnBlockDispose();
            }
        }
    }
}