using System;
using YIUIFramework;
using System.Collections.Generic;

namespace ET.Client
{
    /// <summary>
    /// Author  Lsy
    /// Date    2023.9.19
    /// Desc
    /// </summary>
    [FriendOf(typeof(LoginPanelComponent))]
    public static partial class LoginPanelComponentSystem
    {
        [ObjectSystem]
        public class LoginPanelComponentInitializeSystem: YIUIInitializeSystem<LoginPanelComponent>
        {
            protected override void YIUIInitialize(LoginPanelComponent self)
            {
            }
        }
        
        [ObjectSystem]
        public class LoginPanelComponentDestroySystem: DestroySystem<LoginPanelComponent>
        {
            protected override void Destroy(LoginPanelComponent self)
            {
            }
        }
        
        [ObjectSystem]
        public class LoginPanelComponentOpenSystem: YIUIOpenSystem<LoginPanelComponent>
        {
            protected override async ETTask<bool> YIUIOpen(LoginPanelComponent self)
            {
                await ETTask.CompletedTask;
                return true;
            }
        }
        
        #region YIUIEvent开始
        
        private static void OnEventPasswordAction(this LoginPanelComponent self, string p1)
        {
            Log.Info($"当前密码: {p1}");
            self.Password = p1;
        }
        
        private static void OnEventAccountAction(this LoginPanelComponent self, string p1)
        {
            Log.Info($"当前账号: {p1}");
            self.Account = p1;
        }
        
        private static async void OnEventLoginAction(this LoginPanelComponent self)
        {
            Log.Info($"登录");
            var banId = YIUIMgrComponent.Inst.BanLayerOptionForever();
            await LoginHelper.Login(self.DomainScene(), self.Account, self.Password);
            YIUIMgrComponent.Inst.RecoverLayerOptionForever(banId);
        }
        #endregion YIUIEvent结束
    }
}