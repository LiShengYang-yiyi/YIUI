using System;
using YIUIFramework;
using System.Collections.Generic;

namespace ET.Client
{
    [FriendOf(typeof(MainPanelComponent))]
    public static partial class MainPanelComponentSystem
    {
        [EntitySystem]
        public class MainPanelComponentInitializeSystem: YIUIInitializeSystem<MainPanelComponent>
        {
            protected override void YIUIInitialize(MainPanelComponent self)
            {
            }
        }
        
        [EntitySystem]
        public class MainPanelComponentDestroySystem: DestroySystem<MainPanelComponent>
        {
            protected override void Destroy(MainPanelComponent self)
            {
            }
        }
        
        [EntitySystem]
        public class MainPanelComponentOpenSystem: YIUIOpenSystem<MainPanelComponent>
        {
            protected override async ETTask<bool> YIUIOpen(MainPanelComponent self)
            {
                await ETTask.CompletedTask;
                return true;
            }
        }
        
        #region YIUIEvent开始
        #endregion YIUIEvent结束
    }
}