using System;
using UnityEngine;
using YIUIFramework;
using System.Collections.Generic;

namespace ET.Client
{
    [FriendOf(typeof(LoginPanelComponent))]
    public static partial class LoginPanelComponentSystem
    {
        [EntitySystem]
        private static void YIUIInitialize(this LoginPanelComponent self)
        {
        }

        [EntitySystem]
        private static void Destroy(this LoginPanelComponent self)
        {
        }

        [EntitySystem]
        private static async ETTask<bool> YIUIOpen(this LoginPanelComponent self)
        {
            await ETTask.CompletedTask;
            return true;
        }

        #region YIUIEvent开始

        [YIUIInvoke(LoginPanelComponent.OnEventLoginInvoke)]
        private static async ETTask OnEventLoginInvoke(this LoginPanelComponent self)
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