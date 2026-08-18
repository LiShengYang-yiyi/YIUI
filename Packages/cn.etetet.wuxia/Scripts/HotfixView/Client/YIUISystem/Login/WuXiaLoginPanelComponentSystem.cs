// 【武侠迁移 2026-08-18】项目自有登录面板系统占位实现。
// 作用：为后续登录按钮、账号输入和状态提示预留系统入口。
// 原因：登录逻辑必须由 WuXia 包管理，不能与 Demo 系统类同名冲突。
using System;
using UnityEngine;
using YIUIFramework;
using System.Collections.Generic;

namespace ET.Client
{
    [FriendOf(typeof(WuXiaLoginPanelComponent))]
    public static partial class WuXiaLoginPanelComponentSystem
    {
        [EntitySystem]
        private static void YIUIInitialize(this WuXiaLoginPanelComponent self)
        {
        }

        [EntitySystem]
        private static void Destroy(this WuXiaLoginPanelComponent self)
        {
        }

        [EntitySystem]
        private static async ETTask<bool> YIUIOpen(this WuXiaLoginPanelComponent self)
        {
            await ETTask.CompletedTask;
            return true;
        }

        #region YIUIEvent开始

        [YIUIInvoke(WuXiaLoginPanelComponent.OnEventLoginInvoke)]
        private static async ETTask OnEventLoginInvoke(this WuXiaLoginPanelComponent self)
        {
            Log.Info($"登录");
            GlobalComponent globalComponent = self.Root().GetComponent<GlobalComponent>();
            await LoginHelper.Login(self.Root(),
                globalComponent.GlobalConfig.Address,
                self.u_ComAccount.text,
                self.u_ComPassword.text);
        }

        #endregion YIUIEvent结束
    }
}
