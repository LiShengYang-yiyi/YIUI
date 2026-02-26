using System;
using YIUIFramework;
using System.Collections.Generic;

namespace ET.Client
{
    [FriendOf(typeof(RedDotDataItemComponent))]
    public static partial class RedDotDataItemComponentSystem
    {
        [EntitySystem]
        private static void YIUIInitialize(this RedDotDataItemComponent self)
        {
        }

        [EntitySystem]
        private static void Destroy(this RedDotDataItemComponent self)
        {
        }

        public static void RefreshData(this RedDotDataItemComponent self, RedDotData data)
        {
            self.m_Data = data;
            self.u_DataCount.SetValue(data.Count);
            self.u_DataName.SetValue(RedDotMgr.Inst.GetKeyDes(data.Key));
            self.u_DataTips.SetValue(data.Tips);
            self.u_DataKeyId.SetValue((int)data.Key);
            self.u_DataParentCount.SetValue(data.ParentList.Count);
            self.u_DataChildCount.SetValue(data.ChildList.Count);
            self.u_DataSwitchTips.SetValue(data.Config.SwitchTips);
        }

        #region YIUIEvent开始

        [YIUIInvoke(RedDotDataItemComponent.OnEventTipsInvoke)]
        private static void OnEventTipsInvoke(this RedDotDataItemComponent self, bool p1)
        {
            RedDotMgr.Inst.SetTips(self.m_Data.Key, p1);
        }

        [YIUIInvoke(RedDotDataItemComponent.OnEventParentInvoke)]
        private static void OnEventParentInvoke(this RedDotDataItemComponent self)
        {
            self.DynamicEvent(new OnClickParentListEvent() { Data = self.m_Data }).NoContext();
        }

        [YIUIInvoke(RedDotDataItemComponent.OnEventClickItemInvoke)]
        private static void OnEventClickItemInvoke(this RedDotDataItemComponent self)
        {
            self.DynamicEvent(new OnClickItemEvent { Data = self.m_Data }).NoContext();
        }

        [YIUIInvoke(RedDotDataItemComponent.OnEventChildInvoke)]
        private static void OnEventChildInvoke(this RedDotDataItemComponent self)
        {
            self.DynamicEvent(new OnClickChildListEvent { Data = self.m_Data }).NoContext();
        }

        #endregion YIUIEvent结束
    }
}
