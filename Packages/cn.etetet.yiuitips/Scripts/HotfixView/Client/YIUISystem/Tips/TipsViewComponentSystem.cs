using System;
using System.Collections.Generic;
using YIUIFramework;
using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(TipsViewComponent))]
    [FriendOf(typeof(TipsViewComponent))]
    public static partial class TipsViewComponentSystem
    {
        [EntitySystem]
        private static void Awake(this TipsViewComponent self)
        {
        }

        [EntitySystem]
        private static void Destroy(this TipsViewComponent self)
        {
            if (!self.m_IsFromTips) return;
            self.m_IsFromTips = false;
            self.Fiber()?.EntitySystem?.DynamicEvent(new EventPutTipsView() { View = self.GetParent<YIUIWindowComponent>()?.OwnerUIEntity, Destroy = true });
        }

        [EntitySystem]
        private static async ETTask YIUIWindowClose(this TipsViewComponent self, bool viewCloseResult)
        {
            if (!self.m_IsFromTips) return;
            self.m_IsFromTips = false;
            if (viewCloseResult)
            {
                WaitFrameDynamicEvent(self.Fiber(), new EventPutTipsView() { View = self?.GetParent<YIUIWindowComponent>()?.OwnerUIEntity }).NoContext();
            }
            else
            {
                Log.Info($"View {self.GetParent<YIUIWindowComponent>()?.UIBase?.OwnerGameObject.name} 被关闭，但其父级未关闭 所以不触发回收Tips 请注意");
            }

            await ETTask.CompletedTask;
        }

        private static async ETTask WaitFrameDynamicEvent(Fiber fiber, EventPutTipsView putTipsEvent)
        {
            await fiber.Root?.GetComponent<TimerComponent>()?.WaitFrameAsync();
            await fiber.EntitySystem?.DynamicEvent(putTipsEvent);
        }

        public static void Reset(this TipsViewComponent self)
        {
            self.m_IsFromTips = true;
        }
    }
}