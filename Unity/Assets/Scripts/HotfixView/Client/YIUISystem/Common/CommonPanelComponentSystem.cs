using System;
using YIUIFramework;
using System.Collections.Generic;

namespace ET.Client
{
    /// <summary>
    /// Author  Lsy
    /// Date    2023.11.10
    /// Desc
    /// </summary>
    [FriendOf(typeof(CommonPanelComponent))]
    public static partial class CommonPanelComponentSystem
    {
        [EntitySystem]
        public class CommonPanelComponentInitializeSystem: YIUIInitializeSystem<CommonPanelComponent>
        {
            protected override void YIUIInitialize(CommonPanelComponent self)
            {
            }
        }
        
        [EntitySystem]
        public class CommonPanelComponentDestroySystem: DestroySystem<CommonPanelComponent>
        {
            protected override void Destroy(CommonPanelComponent self)
            {
            }
        }
        
        [EntitySystem]
        public class CommonPanelComponentOpenSystem: YIUIOpenSystem<CommonPanelComponent>
        {
            protected override async ETTask<bool> YIUIOpen(CommonPanelComponent self)
            {
                await ETTask.CompletedTask;
                return true;
            }
        }
        
        #region YIUIEvent开始
        #endregion YIUIEvent结束
    }
}