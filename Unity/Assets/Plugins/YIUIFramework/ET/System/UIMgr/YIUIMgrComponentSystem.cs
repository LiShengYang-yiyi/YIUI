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
                self.AddComponent<YIUIEventComponent>();
                self.AddComponent<YIUILoadComponent>();
                self.BindInit  = YIUIBindHelper.InitAllBind();
                self.m_RootRef = self.DomainScene().AddComponent<YIUIRootComponent>();
            }
        }

        [ObjectSystem]
        public class YIUIMgrComponentDestroySystem: DestroySystem<YIUIMgrComponent>
        {
            protected override void Destroy(YIUIMgrComponent self)
            {
                self.DomainScene().RemoveComponent<YIUIRootComponent>();
                YIUIBindHelper.Reset();
                SingletonMgr.Dispose();
                self.ResetRoot();
                YIUIMgrComponent.m_InstRef = null;
            }
        }
    }
}