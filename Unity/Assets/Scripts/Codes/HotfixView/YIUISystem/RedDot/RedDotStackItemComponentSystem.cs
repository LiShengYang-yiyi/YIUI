using System;
using YIUIFramework;
using System.Collections.Generic;

namespace ET.Client
{
    [FriendOf(typeof(RedDotStackItemComponent))]
    public static partial class RedDotStackItemComponentSystem
    {
        [ObjectSystem]
        public class RedDotStackItemComponentInitializeSystem: YIUIInitializeSystem<RedDotStackItemComponent>
        {
            protected override void YIUIInitialize(RedDotStackItemComponent self)
            {
            }
        }
        
        [ObjectSystem]
        public class RedDotStackItemComponentDestroySystem: DestroySystem<RedDotStackItemComponent>
        {
            protected override void Destroy(RedDotStackItemComponent self)
            {
            }
        }
        
        #region YIUIEvent开始
        
        private static void OnEventShowStackAction(this RedDotStackItemComponent self)
        {
            self.ShowStackAction?.Invoke();
        }
        #endregion YIUIEvent结束
    }
}