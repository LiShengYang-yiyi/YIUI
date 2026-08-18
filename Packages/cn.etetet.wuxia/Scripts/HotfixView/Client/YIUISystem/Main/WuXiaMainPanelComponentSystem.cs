// 【武侠迁移 2026-08-18】项目自有主界面系统占位实现。
// 作用：为后续 HUD、摇杆和移动指令预留系统入口。
// 原因：进入地图后的操作界面需要由 WuXia 独立维护。
using System;
using UnityEngine;
using YIUIFramework;
using System.Collections.Generic;

namespace ET.Client
{
    [FriendOf(typeof(WuXiaMainPanelComponent))]
    public static partial class WuXiaMainPanelComponentSystem
    {
        [EntitySystem]
        private static void YIUIInitialize(this WuXiaMainPanelComponent self)
        {
        }

        [EntitySystem]
        private static void Destroy(this WuXiaMainPanelComponent self)
        {
        }

        [EntitySystem]
        private static async ETTask<bool> YIUIOpen(this WuXiaMainPanelComponent self)
        {
            await ETTask.CompletedTask;
            return true;
        }

        #region YIUIEvent开始
        #endregion YIUIEvent结束
    }
}
