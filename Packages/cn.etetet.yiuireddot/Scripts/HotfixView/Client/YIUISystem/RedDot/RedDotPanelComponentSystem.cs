using System;
using YIUIFramework;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    /// <summary>
    /// 文档: https://lib9kmxvq7k.feishu.cn/wiki/XzyawmryHitNVNk9QVtcDAftn5O
    /// </summary>
    [FriendOf(typeof(RedDotPanelComponent))]
    [FriendOf(typeof(RedDotStackItemComponent))]
    public static partial class RedDotPanelComponentSystem
    {
        [EntitySystem]
        private static void YIUIInitialize(this RedDotPanelComponent self)
        {
            self.RemoveInfoChanged();
            self.InitInfo();
            self.m_SearchScroll = self.AddChild<YIUILoopScrollChild, LoopScrollRect, Type>(self.u_ComSearchScroll, typeof(RedDotDataItemComponent));
            self.InitDropdownSearchDic();
        }

        [EntitySystem]
        private static void Destroy(this RedDotPanelComponent self)
        {
            self.RemoveInfoChanged();
        }

        [EntitySystem]
        private static async ETTask<bool> YIUIOpen(this RedDotPanelComponent self)
        {
            await ETTask.CompletedTask;
            return true;
        }

        [EntitySystem]
        private static async ETTask DynamicEvent(this RedDotPanelComponent self, OnClickParentListEvent message)
        {
            await ETTask.CompletedTask;
            self.OnClickParentList(message.Data);
        }

        [EntitySystem]
        private static async ETTask DynamicEvent(this RedDotPanelComponent self, OnClickChildListEvent message)
        {
            await ETTask.CompletedTask;
            self.OnClickChildList(message.Data);
        }

        [EntitySystem]
        private static async ETTask DynamicEvent(this RedDotPanelComponent self, OnClickItemEvent message)
        {
            await ETTask.CompletedTask;
            self.OnClickItem(message.Data);
        }

        #region Search

        [EntitySystem]
        private static void YIUILoopRenderer(this RedDotPanelComponent self, RedDotDataItemComponent item, RedDotData data, int index, bool select)
        {
            item.RefreshData(data);
        }

        private static void InitDropdownSearchDic(this RedDotPanelComponent self)
        {
            self.u_ComDropdownSearch.ClearOptions();
            self.m_AllDropdownSearchDic.Clear();
            self.m_DropdownOptionData.Clear();
            var index = 0;

            foreach (int key in RedDotKeyHelper.GetKeys())
            {
                if (key == ERedDotKeyType.None) continue;
                var des = RedDotMgr.Inst.GetKeyDes(key);
                if (string.IsNullOrEmpty(des)) continue;

                self.m_DropdownOptionData.Add(new Dropdown.OptionData(des));
                self.m_AllDropdownSearchDic.Add(index, key);
                index++;
            }

            self.u_ComDropdownSearch.AddOptions(self.m_DropdownOptionData);
        }

        private static void RefreshSearchKey(this RedDotPanelComponent self, int key)
        {
            var data = RedDotMgr.Inst.GetData(key);
            if (data == null) return;

            self.m_CurrentDataList.Clear();
            self.m_CurrentDataList.Add(data);
            self.RefreshSearchScroll();
            self.ResetStackInfo(data);
        }

        private static void OnClickParentList(this RedDotPanelComponent self, RedDotData data)
        {
            if (data.ParentList.Count <= 0) return;
            self.m_CurrentDataList.Clear();

            foreach (var parentData in data.ParentList)
            {
                self.m_CurrentDataList.Add(parentData);
            }

            self.RefreshSearchScroll();
        }

        private static void RefreshSearchScroll(this RedDotPanelComponent self)
        {
            self.SearchScroll.SetDataRefresh(self.m_CurrentDataList).NoContext();
        }

        private static void OnClickChildList(this RedDotPanelComponent self, RedDotData data)
        {
            if (data.ChildList.Count <= 0) return;
            self.m_CurrentDataList.Clear();

            foreach (var childData in data.ChildList)
            {
                self.m_CurrentDataList.Add(childData);
            }

            self.RefreshSearchScroll();
        }

        private static void OnClickItem(this RedDotPanelComponent self, RedDotData data)
        {
            self.ResetStackInfo(data);
        }

        #endregion

        #region Info

        private static void InitInfo(this RedDotPanelComponent self)
        {
            self.u_DataInfoName.SetValue("");
            self.u_ComInputChangeCount.text = "";
            self.m_StackScroll = self.AddChild<YIUILoopScrollChild, LoopScrollRect, Type>(self.u_ComStackScroll, typeof(RedDotStackItemComponent));
            self.u_DataToggleUnityEngine.SetValue(RedDotStackHelper.StackHideUnityEngine);
            self.u_DataToggleYIUIFramework.SetValue(RedDotStackHelper.StackHideYIUIFramework);
            self.u_DataToggleYIUIBind.SetValue(RedDotStackHelper.StackHideYIUIBind);
            self.u_DataToggleShowIndex.SetValue(RedDotStackHelper.ShowStackIndex);
            self.u_DataToggleShowFileName.SetValue(RedDotStackHelper.ShowFileNameStack);
            self.u_DataToggleShowFilePath.SetValue(RedDotStackHelper.ShowFilePath);
        }

        [EntitySystem]
        private static void YIUILoopRenderer(this RedDotPanelComponent self, RedDotStackItemComponent item, RedDotStack data, int index, bool select)
        {
            item.u_DataId.SetValue(data.Id);
            item.u_DataTime.SetValue(data.GetTime());
            item.u_DataOs.SetValue(data.GetOS(self.m_InfoData));
            item.u_DataSource.SetValue(data.GetSource());
            item.u_DataShowStack.SetValue(false);
            item.RedDotStackData = data;
        }

        private static void ResetStackInfo(this RedDotPanelComponent self, RedDotData data)
        {
            if (self.m_InfoData == data)
            {
                return;
            }

            self.RemoveInfoChanged();
            self.m_InfoData = data;
            RedDotMgr.Inst.AddChanged(self.m_InfoData.Key, self.OnInfoChangeCount);
            self.u_DataInfoName.SetValue($"{(int)data.Key} {RedDotMgr.Inst.GetKeyDes(data.Key)}");
            self.u_ComInputChangeCount.text = self.m_InfoData.Count.ToString();
            self.RefreshInfoScroll();
        }

        private static void RemoveInfoChanged(this RedDotPanelComponent self)
        {
            if (self.m_InfoData != null)
            {
                RedDotMgr.Inst.RemoveChanged(self.m_InfoData.Key, self.OnInfoChangeCount);
            }
        }

        private static void RefreshInfoScroll(this RedDotPanelComponent self)
        {
            if (self.m_InfoData == null)
            {
                return;
            }

            self.StackScroll.SetDataRefresh(self.m_InfoData.StackList).NoContext();
        }

        private static void OnInfoChangeCount(this RedDotPanelComponent self, int obj)
        {
            self.RefreshInfoScroll();
            self.RefreshSearchScroll();
        }

        private static void ChangeInfoCount(this RedDotPanelComponent self, int count)
        {
            if (self.m_InfoData == null)
            {
                return;
            }

            RedDotMgr.Inst.SetCount(self.m_InfoData.Key, count);
        }

        private static void ChangeToggleRefreshStack(this RedDotPanelComponent self)
        {
            self.RefreshInfoScroll();
        }

        #endregion

        #region YIUIEvent开始

        [YIUIInvoke(RedDotPanelComponent.OnEventInputSearchEndInvoke)]
        private static void OnEventInputSearchEndInvoke(this RedDotPanelComponent self, string p1)
        {
            if (string.IsNullOrEmpty(p1))
            {
                return;
            }

            var result = int.TryParse(p1, out int key);
            if (!result)
            {
                Debug.LogError($"没有找到这个枚举 {p1}");
                return;
            }

            self.RefreshSearchKey(key);
        }

        [YIUIInvoke(RedDotPanelComponent.OnEventDropdownSearchInvoke)]
        private static void OnEventDropdownSearchInvoke(this RedDotPanelComponent self, int p1)
        {
            if (!self.m_AllDropdownSearchDic.TryGetValue(p1, out var key))
            {
                Debug.LogError($"没有找到这个key {p1}");
                return;
            }

            self.RefreshSearchKey(key);
        }

        [YIUIInvoke(RedDotPanelComponent.OnEventChangeToggleYIUIFrameworkInvoke)]
        private static void OnEventChangeToggleYIUIFrameworkInvoke(this RedDotPanelComponent self, bool p1)
        {
            RedDotStackHelper.StackHideYIUIFramework = p1;
            self.u_DataToggleYIUIFramework.SetValue(p1);
            self.ChangeToggleRefreshStack();
        }

        [YIUIInvoke(RedDotPanelComponent.OnEventChangeToggleYIUIBindInvoke)]
        private static void OnEventChangeToggleYIUIBindInvoke(this RedDotPanelComponent self, bool p1)
        {
            RedDotStackHelper.StackHideYIUIBind = p1;
            self.u_DataToggleYIUIBind.SetValue(p1);
            self.ChangeToggleRefreshStack();
        }

        [YIUIInvoke(RedDotPanelComponent.OnEventChangeToggleUnityEngineInvoke)]
        private static void OnEventChangeToggleUnityEngineInvoke(this RedDotPanelComponent self, bool p1)
        {
            RedDotStackHelper.StackHideUnityEngine = p1;
            self.u_DataToggleUnityEngine.SetValue(p1);
            self.ChangeToggleRefreshStack();
        }

        [YIUIInvoke(RedDotPanelComponent.OnEventChangeToggleShowStackIndexInvoke)]
        private static void OnEventChangeToggleShowStackIndexInvoke(this RedDotPanelComponent self, bool p1)
        {
            RedDotStackHelper.ShowStackIndex = p1;
            self.u_DataToggleShowIndex.SetValue(p1);
            self.ChangeToggleRefreshStack();
        }

        [YIUIInvoke(RedDotPanelComponent.OnEventChangeToggleShowFilePathInvoke)]
        private static void OnEventChangeToggleShowFilePathInvoke(this RedDotPanelComponent self, bool p1)
        {
            RedDotStackHelper.ShowFilePath = p1;
            self.u_DataToggleShowFilePath.SetValue(p1);
            self.ChangeToggleRefreshStack();
        }

        [YIUIInvoke(RedDotPanelComponent.OnEventChangeToggleShowFileNameInvoke)]
        private static void OnEventChangeToggleShowFileNameInvoke(this RedDotPanelComponent self, bool p1)
        {
            RedDotStackHelper.ShowFileNameStack = p1;
            self.u_DataToggleShowFileName.SetValue(p1);
            self.ChangeToggleRefreshStack();
        }

        [YIUIInvoke(RedDotPanelComponent.OnEventChangeCountInvoke)]
        private static void OnEventChangeCountInvoke(this RedDotPanelComponent self, string p1)
        {
            if (string.IsNullOrEmpty(p1))
            {
                return;
            }

            var result = int.TryParse(p1, out var value);
            if (!result)
            {
                Debug.LogError($"转换失败 {p1}");
                return;
            }

            self.ChangeInfoCount(value);
        }

        #endregion YIUIEvent结束
    }
}
