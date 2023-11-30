using System;
using YIUIFramework;
using System.Collections.Generic;

namespace ET.Client
{
    [FriendOf(typeof (RedDotDataItemComponent))]
    public static partial class RedDotDataItemComponentSystem
    {
        [ObjectSystem]
        public class RedDotDataItemComponentInitializeSystem: YIUIInitializeSystem<RedDotDataItemComponent>
        {
            protected override void YIUIInitialize(RedDotDataItemComponent self)
            {
            }
        }

        [ObjectSystem]
        public class RedDotDataItemComponentDestroySystem: DestroySystem<RedDotDataItemComponent>
        {
            protected override void Destroy(RedDotDataItemComponent self)
            {
            }
        }

        public static void RefreshData(this RedDotDataItemComponent self, RedDotData data)
        {
            self.m_Data        = data;
            self.u_DataCount.SetValue(data.Count);
            self.u_DataName.SetValue(RedDotMgr.Inst.GetKeyDes(data.Key));
            self.u_DataTips.SetValue(data.Tips);
            self.u_DataKeyId.SetValue((int)data.Key);
            self.u_DataParentCount.SetValue(data.ParentList.Count);
            self.u_DataChildCount.SetValue(data.ChildList.Count);
            self.u_DataSwitchTips.SetValue(data.Config.SwitchTips);
        }

        #region YIUIEvent开始

        private static void OnEventTipsAction(this RedDotDataItemComponent self, bool p1)
        {
            RedDotMgr.Inst.SetTips(self.m_Data.Key, p1);
        }

        private static void OnEventParentAction(this RedDotDataItemComponent self)
        {
            YIUIEventSystem.Event(new OnClickParentListEvent() { Data = self.m_Data }).Coroutine();
        }

        private static void OnEventClickItemAction(this RedDotDataItemComponent self)
        {
            YIUIEventSystem.Event(new OnClickItemEvent { Data = self.m_Data }).Coroutine();
        }

        private static void OnEventChildAction(this RedDotDataItemComponent self)
        {
            YIUIEventSystem.Event(new OnClickChildListEvent { Data = self.m_Data }).Coroutine();
        }

        #endregion YIUIEvent结束
    }
}