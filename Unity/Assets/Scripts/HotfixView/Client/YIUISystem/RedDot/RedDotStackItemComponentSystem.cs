using System;
using YIUIFramework;
using System.Collections.Generic;

namespace ET.Client
{
    [FriendOf(typeof(RedDotStackItemComponent))]
    public static partial class RedDotStackItemComponentSystem
    {
        [EntitySystem]
        public class RedDotStackItemComponentInitializeSystem: YIUIInitializeSystem<RedDotStackItemComponent>
        {
            protected override void YIUIInitialize(RedDotStackItemComponent self)
            {
            }
        }
        
        [EntitySystem]
        public class RedDotStackItemComponentDestroySystem: DestroySystem<RedDotStackItemComponent>
        {
            protected override void Destroy(RedDotStackItemComponent self)
            {
            }
        }
        
        #region YIUIEvent开始
        
        private static void OnEventShowStackAction(this RedDotStackItemComponent self)
        {
            self.u_ComStackText.text = self.RedDotStackData?.GetStackContent() ?? "";
        }
        #endregion YIUIEvent结束
    }
}