using System;
using YIUIFramework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ET.Client
{
    [FriendOf(typeof(GMPanelComponent))]
    public static partial class GMPanelComponentSystem
    {
        [EntitySystem]
        private static void YIUIInitialize(this GMPanelComponent self)
        {
            self._OpenGMViewKey = YIUIConstHelper.Const.OpenGMViewKey;
        }

        [EntitySystem]
        private static void Destroy(this GMPanelComponent self)
        {
        }

        [EntitySystem]
        private static async ETTask<bool> YIUIOpen(this GMPanelComponent self)
        {
            var rect = self.u_ComLimitRange.rect;
            self._LimitSize = new Vector2(rect.width, rect.height);
            var InitX = Mathf.Clamp(self._GMBtn_Pos_X.Value, 0, self._LimitSize.x);
            var InitY = Mathf.Clamp(self._GMBtn_Pos_Y.Value, 0, self._LimitSize.y);
            if (InitY == 0)
                InitY = self._LimitSize.y;
            self.u_ComGMButton.anchoredPosition = new Vector2(InitX, InitY);
            await ETTask.CompletedTask;
            return true;
        }

        [EntitySystem]
        private static void Update(this GMPanelComponent self)
        {
            if (self._OpenGMViewKey == KeyCode.None) return;
            if (Input.GetKeyDown(self._OpenGMViewKey))
            {
                if (!self.UIPanel.CurrentOpenViewActiveSelf)
                {
                    self.OnEventOpenGMViewInvoke();
                }
                else
                {
                    if (self.UIPanel.CurrentOpenView != null)
                    {
                        var view = self.UIPanel.CurrentOpenView.GetParent<YIUIChild>().GetComponent<YIUIViewComponent>();
                        view.Close();
                    }
                }
            }
        }

        #region YIUIEvent开始

        [YIUIInvoke(GMPanelComponent.OnEventDragInvoke)]
        private static void OnEventDragInvoke(this GMPanelComponent self, object p1)
        {
            var data = (PointerEventData)p1;
            self.u_ComGMButton.anchoredPosition = data.position + self._Offset;
        }

        [YIUIInvoke(GMPanelComponent.OnEventEndDragInvoke)]
        private static void OnEventEndDragInvoke(this GMPanelComponent self, object p1)
        {
            var endPos = self.u_ComGMButton.anchoredPosition;
            endPos.x                            = Mathf.Clamp(endPos.x, 0, self._LimitSize.x);
            endPos.y                            = Mathf.Clamp(endPos.y, 0, self._LimitSize.y);
            self.u_ComGMButton.anchoredPosition = endPos;
            self._GMBtn_Pos_X.Value             = endPos.x;
            self._GMBtn_Pos_Y.Value             = endPos.y;
        }

        [YIUIInvoke(GMPanelComponent.OnEventBeginDragInvoke)]
        private static void OnEventBeginDragInvoke(this GMPanelComponent self, object p1)
        {
            var data = (PointerEventData)p1;
            self._Offset = self.u_ComGMButton.anchoredPosition - data.position;
        }

        [YIUIInvoke(GMPanelComponent.OnEventOpenGMViewInvoke)]
        private static void OnEventOpenGMViewInvoke(this GMPanelComponent self)
        {
            self.UIPanel.OpenViewAsync<GMViewComponent>().NoContext();
        }

        #endregion YIUIEvent结束
    }
}