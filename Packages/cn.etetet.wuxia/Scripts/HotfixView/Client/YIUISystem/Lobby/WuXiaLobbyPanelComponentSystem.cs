// 【武侠迁移 2026-08-18】项目自有大厅面板系统占位实现。
// 作用：为后续角色选择、服务器选择和进入地图按钮预留系统入口。
// 原因：大厅交互必须和 Demo 的大厅系统隔离，保证项目可独立替换。
using System;
using UnityEngine;
using YIUIFramework;
using System.Collections.Generic;

namespace ET.Client
{
    [FriendOf(typeof(WuXiaLobbyPanelComponent))]
    public static partial class WuXiaLobbyPanelComponentSystem
    {
        [EntitySystem]
        private static void YIUIInitialize(this WuXiaLobbyPanelComponent self)
        {
        }

        [EntitySystem]
        private static void Destroy(this WuXiaLobbyPanelComponent self)
        {
        }

        [EntitySystem]
        private static async ETTask<bool> YIUIOpen(this WuXiaLobbyPanelComponent self)
        {
            await ETTask.CompletedTask;
            return true;
        }

        #region YIUIEvent开始

        [YIUIInvoke(WuXiaLobbyPanelComponent.OnEventEnterMapInvoke)]
        private static async ETTask OnEventEnterMapInvoke(this WuXiaLobbyPanelComponent self)
        {
            self.UIBase.SetActive(false);
            await EnterMapHelper.EnterMapAsync(self.Root());
            await self.UIPanel.CloseAsync();
        }

        #endregion YIUIEvent结束
    }
}
