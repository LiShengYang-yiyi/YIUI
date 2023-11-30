using System;
using YIUIFramework;
using System.Collections.Generic;
using UnityEngine;

namespace ET.Client
{
    [FriendOf(typeof (TextTipsViewComponent))]
    public static partial class TextTipsViewComponentSystem
    {
        [EntitySystem]
        public static void YIUIInitialize(this TextTipsViewComponent self)
        {
        }

        [EntitySystem]
        public static void Awake(this TextTipsViewComponent self)
        {
        }

        [EntitySystem]
        public static void Destroy(this TextTipsViewComponent self)
        {
        }

        [EntitySystem]
        public static async ETTask<bool> YIUIOpen(this TextTipsViewComponent self, ParamVo vo)
        {
            await ETTask.CompletedTask;
            var content = vo.Get<string>();
            if (string.IsNullOrEmpty(content))
            {
                Debug.LogError($"TextTipsView 必须有消息内容 请检查");
                return false;
            }

            self.u_DataMessageContent.SetValue(content);
            self.PlayAnimation().Coroutine();
            return true;
        }

        private static async ETTask PlayAnimation(this TextTipsViewComponent self)
        {
            self.u_ComAnimation.Play(self.u_ComAnimation.clip.name);
            await self.Fiber().TimerComponent.WaitAsync((long)(self.u_ComAnimation.clip.length * 1000));
            await TipsHelper.CloseTipsView(self);
        }

        #region YIUIEvent开始

        #endregion YIUIEvent结束
    }
}