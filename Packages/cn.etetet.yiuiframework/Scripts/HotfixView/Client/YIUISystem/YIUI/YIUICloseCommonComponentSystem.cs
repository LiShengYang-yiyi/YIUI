using System;
using UnityEngine;
using YIUIFramework;
using System.Collections.Generic;

namespace ET.Client
{
    [FriendOf(typeof(YIUICloseCommonComponent))]
    public static partial class YIUICloseCommonComponentSystem
    {
        [EntitySystem]
        private static void YIUIInitialize(this YIUICloseCommonComponent self)
        {
        }

        [EntitySystem]
        private static void Destroy(this YIUICloseCommonComponent self)
        {
        }

        private static async ETTask<bool> CloseUI(this YIUICloseCommonComponent self, EntityRef<Entity> parent)
        {
            if (parent.Entity.Parent == null)
            {
                Debug.LogError($"结构错误 无法找到可关闭UI {self.UIBase.OwnerGameObject}", self.UIBase.OwnerGameObject);
                return false;
            }

            EntityRef<YIUICloseCommonComponent> selfRef = self;
            if (parent.Entity.Parent is YIUIChild yiuiChild)
            {
                var panelComponent = yiuiChild.GetComponent<YIUIPanelComponent>();
                if (panelComponent != null)
                {
                    return await panelComponent.CloseAsync();
                }

                var viewComponent = yiuiChild.GetComponent<YIUIViewComponent>();
                if (viewComponent != null)
                {
                    return await viewComponent.CloseAsync();
                }
            }

            self = selfRef;
            return await self.CloseUI(parent.Entity.Parent);
        }

        #region YIUIEvent开始

        [YIUIInvoke(YIUICloseCommonComponent.OnEventCloseInvoke)]
        private static async ETTask OnEventCloseInvoke(this YIUICloseCommonComponent self)
        {
            await self.CloseUI(self.Parent);
        }

        #endregion YIUIEvent结束
    }
}