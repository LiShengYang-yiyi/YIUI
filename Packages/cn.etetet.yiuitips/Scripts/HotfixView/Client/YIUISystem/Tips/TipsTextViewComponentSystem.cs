using System;
using YIUIFramework;
using UnityEngine;

namespace ET.Client
{
    [FriendOf(typeof(TipsTextViewComponent))]
    public static partial class TipsTextViewComponentSystem
    {
        [EntitySystem]
        private static void YIUIInitialize(this TipsTextViewComponent self)
        {
        }

        [EntitySystem]
        private static void Destroy(this TipsTextViewComponent self)
        {
        }

        [EntitySystem]
        private static async ETTask<bool> YIUIOpen(this TipsTextViewComponent self)
        {
            await ETTask.CompletedTask;
            return true;
        }

        [EntitySystem]
        private static async ETTask<bool> YIUIOpen(this TipsTextViewComponent self, ParamVo vo)
        {
            await ETTask.CompletedTask;
            var content = vo.Get<string>();
            if (string.IsNullOrEmpty(content))
            {
                Debug.LogError($"TextTipsView 必须有消息内容 请检查");
                return false;
            }

            self.u_DataMessageContent.SetValue(content);
            self.PlayAnimation().NoContext();
            return true;
        }

        private static async ETTask PlayAnimation(this TipsTextViewComponent self)
        {
            EntityRef<TipsTextViewComponent> selfRef = self;
            self.u_ComAnimation.Play(self.u_ComAnimation.clip.name);
            await self.Root().GetComponent<TimerComponent>().WaitAsync((long)(self.u_ComAnimation.clip.length * 1000));
            self = selfRef;
            await self.UIView.CloseAsync();
        }

        #region YIUIEvent开始

        #endregion YIUIEvent结束
    }
}