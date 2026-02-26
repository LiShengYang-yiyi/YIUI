using System;
using YIUIFramework;
using System.Collections.Generic;

namespace ET.Client
{
    [FriendOf(typeof(GMTypeItemComponent))]
    public static partial class GMTypeItemComponentSystem
    {
        [EntitySystem]
        private static void YIUIInitialize(this GMTypeItemComponent self)
        {
        }

        [EntitySystem]
        private static void Destroy(this GMTypeItemComponent self)
        {
        }

        public static void ResetItem(this GMTypeItemComponent self, int data)
        {
            self.u_DataTypeName.SetValue(GMKeyHelper.GetDesc(data));
        }

        public static void SelectItem(this GMTypeItemComponent self, bool value)
        {
            self.u_DataSelect.SetValue(value);
        }

        #region YIUIEvent开始

        [YIUIInvoke(GMTypeItemComponent.OnEventSelectInvoke)]
        private static void OnEventSelectInvoke(this GMTypeItemComponent self)
        {
        }

        #endregion YIUIEvent结束
    }
}