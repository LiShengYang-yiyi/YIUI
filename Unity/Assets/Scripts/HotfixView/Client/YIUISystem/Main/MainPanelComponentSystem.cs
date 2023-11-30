using System;
using YIUIFramework;
using System.Collections.Generic;

namespace ET.Client
{
    [FriendOf(typeof(MainPanelComponent))]
    public static partial class MainPanelComponentSystem
    {
        [EntitySystem]
        public static void YIUIInitialize(this MainPanelComponent self)
        {
        }

        [EntitySystem]
        public static void Awake(this MainPanelComponent self)
        {
        }

        [EntitySystem]
        public static void Destroy(this MainPanelComponent self)
        {
        }

        [EntitySystem]
        public static async ETTask<bool> YIUIOpen(this MainPanelComponent self)
        {
            await ETTask.CompletedTask;
            return true;
        }

        #region YIUIEvent开始
        #endregion YIUIEvent结束
    }
}