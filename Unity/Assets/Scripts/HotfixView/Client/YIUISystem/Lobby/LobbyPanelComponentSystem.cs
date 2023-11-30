using System;
using YIUIFramework;
using System.Collections.Generic;

namespace ET.Client
{
    [FriendOf(typeof(LobbyPanelComponent))]
    public static partial class LobbyPanelComponentSystem
    {
        [EntitySystem]
        public class LobbyPanelComponentInitializeSystem: YIUIInitializeSystem<LobbyPanelComponent>
        {
            protected override void YIUIInitialize(LobbyPanelComponent self)
            {
            }
        }
        
        [EntitySystem]
        public class LobbyPanelComponentDestroySystem: DestroySystem<LobbyPanelComponent>
        {
            protected override void Destroy(LobbyPanelComponent self)
            {
            }
        }
        
        [EntitySystem]
        public class LobbyPanelComponentOpenSystem: YIUIOpenSystem<LobbyPanelComponent>
        {
            protected override async ETTask<bool> YIUIOpen(LobbyPanelComponent self)
            {
                await ETTask.CompletedTask;
                return true;
            }
        }
        
        #region YIUIEvent开始
        
        private static async ETTask OnEventEnterAction(this LobbyPanelComponent self)
        {
            var banId = YIUIMgrComponent.Inst.BanLayerOptionForever();
            await EnterMapHelper.EnterMapAsync(self.Root());
            YIUIMgrComponent.Inst.RecoverLayerOptionForever(banId);
            self.UIPanel.Close(false,true);
        }
        
        #endregion YIUIEvent结束
    }
}