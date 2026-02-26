using System;
using YIUIFramework;
using System.Collections.Generic;
using UnityEngine.UI;

namespace ET.Client
{
    [FriendOf(typeof(GMCommandItemComponent))]
    public static partial class GMCommandItemComponentSystem
    {
        [EntitySystem]
        private static void YIUIInitialize(this GMCommandItemComponent self)
        {
            self.m_GMParamLoop = self.AddChild<YIUILoopScrollChild, LoopScrollRect, Type>(self.u_ComParamLoop, typeof(GMParamItemComponent));
        }

        [EntitySystem]
        private static void YIUILoopRenderer(this GMCommandItemComponent self, GMParamItemComponent item, GMParamInfo data, int index, bool select)
        {
            item.ResetItem(data);
        }

        [EntitySystem]
        private static void Destroy(this GMCommandItemComponent self)
        {
        }

        public static void ResetItem(this GMCommandItemComponent self, GMCommandComponent commandComponent, GMCommandInfo info)
        {
            self.m_CommandComponent = commandComponent;
            self.Info               = info;
            self.u_DataName.SetValue(info.GMName);
            self.u_DataDesc.SetValue(info.GMDesc);
            self.u_DataShowParamLoop.SetValue(info.ParamInfoList.Count >= 1);
            self.WaitRefresh().NoContext();
        }

        private static async ETTask WaitRefresh(this GMCommandItemComponent self)
        {
            await self.GMParamLoop.SetDataRefresh(self.Info.ParamInfoList);
        }

        #region YIUIEvent开始

        [YIUIInvoke(GMCommandItemComponent.OnEventRunInvoke)]
        private static void OnEventRunInvoke(this GMCommandItemComponent self)
        {
            self.CommandComponent?.Run(self.Info).NoContext();
        }

        #endregion YIUIEvent结束
    }
}