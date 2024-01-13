using System;
using YIUIFramework;
using System.Collections.Generic;

namespace ET.Client
{
    [FriendOf(typeof (GMTypeItemComponent))]
    public static partial class GMTypeItemComponentSystem
    {
        [ObjectSystem]
        public class GMTypeItemComponentInitializeSystem: YIUIInitializeSystem<GMTypeItemComponent>
        {
            protected override void YIUIInitialize(GMTypeItemComponent self)
            {
            }
        }

        [ObjectSystem]
        public class GMTypeItemComponentDestroySystem: DestroySystem<GMTypeItemComponent>
        {
            protected override void Destroy(GMTypeItemComponent self)
            {
            }
        }
        
        public static void ResetItem(this GMTypeItemComponent self,string name, EGMType data)
        {
            self.u_DataTypeName.SetValue(name);
        }

        public static void SelectItem(this GMTypeItemComponent self, bool value)
        {
            self.u_DataSelect.SetValue(value);
        }

        #region YIUIEvent开始

        private static void OnEventSelectAction(this GMTypeItemComponent self)
        {
        }

        #endregion YIUIEvent结束
    }
}