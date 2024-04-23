//------------------------------------------------------------
// Author: 亦亦
// Mail: 379338943@qq.com
// Data: 2023年2月12日
//------------------------------------------------------------

using YIUIFramework;

namespace ET.Client
{
    [FriendOf(typeof(YIUIMgrComponent))]
    [EntitySystemOf(typeof(YIUIMgrComponent))]
    public static partial class YIUIMgrComponentSystem
    {
        [EntitySystem]
        private static void Awake(this YIUIMgrComponent self)
        {
            YIUIMgrComponent.m_InstRef = self;
            self.InitAllBind();
            self.AddComponent<YIUIEventComponent>();
            self.AddComponent<YIUILoadComponent>();
            self.m_RootRef = self.Root().AddComponent<YIUIRootComponent>();
        }

        [EntitySystem]
        private static void Destroy(this YIUIMgrComponent self)
        {
            self.OnBlockDispose();
            self.Root().RemoveComponent<YIUIRootComponent>();
            YIUIBindHelper.Reset();
            MgrCenter.Inst.Dispose();
        }
    }
}