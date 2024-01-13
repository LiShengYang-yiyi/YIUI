using System;
using YIUIFramework;
using System.Collections.Generic;
using UnityEngine;

namespace ET.Client
{
    [FriendOf(typeof (MessageTipsViewComponent))]
    public static partial class MessageTipsViewComponentSystem
    {
        [ObjectSystem]
        public class MessageTipsViewComponentInitializeSystem: YIUIInitializeSystem<MessageTipsViewComponent>
        {
            protected override void YIUIInitialize(MessageTipsViewComponent self)
            {
            }
        }

        [ObjectSystem]
        public class MessageTipsViewComponentDestroySystem: DestroySystem<MessageTipsViewComponent>
        {
            protected override void Destroy(MessageTipsViewComponent self)
            {
            }
        }

        [ObjectSystem]
        public class MessageTipsViewComponentOpenSystem: YIUIOpenSystem<MessageTipsViewComponent>
        {
            protected override async ETTask<bool> YIUIOpen(MessageTipsViewComponent self)
            {
                await ETTask.CompletedTask;
                return true;
            }
        }

        [ObjectSystem]
        public class MessageTipsViewComponentOpenTweenSystem: YIUIOpenTweenSystem<MessageTipsViewComponent>
        {
            protected override async ETTask YIUIOpenTween(MessageTipsViewComponent self)
            {
                await WindowFadeAnim.In(self.UIBase);
            }
        }

        [ObjectSystem]
        public class MessageTipsViewComponentCloseTweenSystem: YIUICloseTweenSystem<MessageTipsViewComponent>
        {
            protected override async ETTask YIUICloseTween(MessageTipsViewComponent self)
            {
                await WindowFadeAnim.Out(self.UIBase);
            }
        }

        [ObjectSystem]
        public class MessageTipsViewComponentOpenParamVoSystem: YIUIOpenSystem<MessageTipsViewComponent, ParamVo>
        {
            protected override async ETTask<bool> YIUIOpen(MessageTipsViewComponent self, ParamVo vo)
            {
                await ETTask.CompletedTask;
                var content = vo.Get<string>();
                if (string.IsNullOrEmpty(content))
                {
                    Debug.LogError($"MessageTipsView 必须有消息内容 请检查");
                    return false;
                }
                self.ExtraData = vo.Get(1,new MessageTipsExtraData());
                self.u_DataMessageContent.SetValue(content);
                self.u_DataShowCancel.SetValue(self.ExtraData.CancelCallBack != null);
                self.u_DataShowClose.SetValue(self.ExtraData.CloseCallBack != null);
                self.u_DataConfirmName.SetValue(string.IsNullOrEmpty(self.ExtraData.ConfirmName)? "确定" : self.ExtraData.ConfirmName);
                self.u_DataCancelName.SetValue(string.IsNullOrEmpty(self.ExtraData.CancelName)? "取消" : self.ExtraData.CancelName);
                return true;
            }
        }
        
        #region YIUIEvent开始

        private static void OnEventConfirmAction(this MessageTipsViewComponent self)
        {
            self.ExtraData.ConfirmCallBack?.Invoke();
            TipsHelper.CloseTipsViewSync(self);
        }

        private static void OnEventCancelAction(this MessageTipsViewComponent self)
        {
            self.ExtraData.CancelCallBack?.Invoke();
            TipsHelper.CloseTipsView(self).Coroutine();
        }

        private static void OnEventCloseAction(this MessageTipsViewComponent self)
        {
            self.ExtraData.CloseCallBack?.Invoke();
            TipsHelper.CloseTipsView(self).Coroutine();
        }

        #endregion YIUIEvent结束
    }
}