using System;
using YIUIFramework;
using System.Collections.Generic;
using UnityEngine;

namespace ET.Client
{
    [FriendOf(typeof (GMCommandItemComponent))]
    public static partial class GMCommandItemComponentSystem
    {
        [ObjectSystem]
        public class GMCommandItemComponentInitializeSystem: YIUIInitializeSystem<GMCommandItemComponent>
        {
            protected override void YIUIInitialize(GMCommandItemComponent self)
            {
                self.GMParamLoop = new YIUILoopScroll<GMParamInfo, GMParamItemComponent>(self, self.u_ComParamLoop, self.GMParamRenderer);
            }
        }

        [ObjectSystem]
        public class GMCommandItemComponentDestroySystem: DestroySystem<GMCommandItemComponent>
        {
            protected override void Destroy(GMCommandItemComponent self)
            {
            }
        }

        public static void ResetItem(this GMCommandItemComponent self, GMCommandComponent commandComponent, GMCommandInfo info)
        {
            self.CommandComponent = commandComponent;
            self.Info             = info;
            self.u_DataName.SetValue(info.GMName);
            self.u_DataDesc.SetValue(info.GMDesc);
            self.u_DataShowParamLoop.SetValue(info.ParamInfoList.Count >= 1);
            self.GMParamLoop.SetDataRefresh(info.ParamInfoList);
            self.WaitRefresh().Coroutine();
        }

        private static async ETTask WaitRefresh(this GMCommandItemComponent self)
        {
            await TimerComponent.Instance.WaitAsync(500);
            self.GMParamLoop.RefreshCells();
        }

        private static void GMParamRenderer(this GMCommandItemComponent self, int index, GMParamInfo data, GMParamItemComponent item, bool select)
        {
            item.ResetItem(data);
        }

        #region YIUIEvent开始

        private static void OnEventRunAction(this GMCommandItemComponent self)
        {
            self.CommandComponent.Run(self.Info).Coroutine();
        }

        #endregion YIUIEvent结束
    }
}