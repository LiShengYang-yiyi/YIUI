//------------------------------------------------------------
// Author: 亦亦
// Mail: 379338943@qq.com
// Data: 2023年2月12日
//------------------------------------------------------------

using YIUIFramework;

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
                YIUIMgrComponent.m_InstRef = self;
                self.InitAllBind();
                self.AddComponent<YIUIEventComponent>();
                self.AddComponent<YIUILoadComponent>();
                self.m_RootRef = self.DomainScene().AddComponent<YIUIRootComponent>();
            }
        }

        [ObjectSystem]
        public class YIUIMgrComponentDestroySystem: DestroySystem<YIUIMgrComponent>
        {
            protected override void Destroy(YIUIMgrComponent self)
            {
                self.OnBlockDispose();
                self.DomainScene().RemoveComponent<YIUIRootComponent>();
                self.Destroy();
            }
        }
    }
}