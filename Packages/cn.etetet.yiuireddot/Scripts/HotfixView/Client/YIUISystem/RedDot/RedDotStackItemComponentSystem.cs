using System;
using YIUIFramework;
using System.Collections.Generic;

namespace ET.Client
{
    [FriendOf(typeof(RedDotStackItemComponent))]
    public static partial class RedDotStackItemComponentSystem
    {
        [EntitySystem]
        private static void YIUIInitialize(this RedDotStackItemComponent self)
        {
        }

        [EntitySystem]
        private static void Destroy(this RedDotStackItemComponent self)
        {
        }

        #region YIUIEvent开始

        [YIUIInvoke(RedDotStackItemComponent.OnEventShowStackInvoke)]
        private static void OnEventShowStackInvoke(this RedDotStackItemComponent self)
        {
            self.u_ComStackText.text = self.RedDotStackData?.GetStackContent() ?? "";
        }

        #endregion YIUIEvent结束
    }
}
