using System;
using UnityEngine;
using YIUIFramework;
using System.Collections.Generic;

namespace ET.Client
{

    [FriendOf(typeof(LoopScrollRectDemoPanelComponent))]
    public static partial class LoopScrollRectDemoPanelComponentSystem
    {
        [EntitySystem]
        private static void YIUIInitialize(this LoopScrollRectDemoPanelComponent self)
        {
        }

        [EntitySystem]
        private static void Destroy(this LoopScrollRectDemoPanelComponent self)
        {
        }

        [EntitySystem]
        private static async ETTask<bool> YIUIOpen(this LoopScrollRectDemoPanelComponent self)
        {
            await ETTask.CompletedTask;
            return true;
        }

        [EntitySystem]
        private static async ETTask<bool> YIUIOpen(this LoopScrollRectDemoPanelComponent self, ELoopScrollRectDemoPanelViewEnum view)
        {
            await self.UIPanel.OpenViewAsync(view.ToString());
            return true;
        }

        #region YIUIEvent开始

        [YIUIInvoke(LoopScrollRectDemoPanelComponent.OnEventTabInvoke)]
        private static async ETTask OnEventTabInvoke(this LoopScrollRectDemoPanelComponent self, int p1)
        {
            await self.UIPanel.OpenViewAsync(((ELoopScrollRectDemoPanelViewEnum)p1).ToString());
        }

        #endregion YIUIEvent结束
    }
}