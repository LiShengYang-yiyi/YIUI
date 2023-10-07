using System;
using YIUIFramework;
using System.Collections.Generic;

namespace ET.Client
{
    [FriendOf(typeof(LobbyPanelComponent))]
    public static partial class LobbyPanelComponentSystem
    {
        [ObjectSystem]
        public class LobbyPanelComponentInitializeSystem: YIUIInitializeSystem<LobbyPanelComponent>
        {
            protected override void YIUIInitialize(LobbyPanelComponent self)
            {
            }
        }
        
        [ObjectSystem]
        public class LobbyPanelComponentDestroySystem: DestroySystem<LobbyPanelComponent>
        {
            protected override void Destroy(LobbyPanelComponent self)
            {
            }
        }
        
        [ObjectSystem]
        public class LobbyPanelComponentOpenSystem: YIUIOpenSystem<LobbyPanelComponent>
        {
            protected override async ETTask<bool> YIUIOpen(LobbyPanelComponent self)
            {
                await ETTask.CompletedTask;
                return true;
            }
        }
        
        #region YIUIEvent开始
        
        private static async void OnEventEnterAction(this LobbyPanelComponent self)
        {
            var banId = YIUIMgrComponent.Inst.BanLayerOptionForever();
            await EnterMapHelper.EnterMapAsync(self.ClientScene());
            YIUIMgrComponent.Inst.RecoverLayerOptionForever(banId);
            self.UIPanel.Close(false,true);
        }
        #endregion YIUIEvent结束
    }
}