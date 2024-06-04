using System;
using YIUIBind;
using YIUIFramework;
using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;

namespace MizuYIUI.RedDot
{
    /// <summary>
    /// Author  YIUI
    /// Date    2024.6.2
    /// </summary>
    public sealed partial class RedDotPanel : RedDotPanelBase
    {
    
        #region 生命周期
        
        

        protected override async UniTask<bool> OnOpen()
        {
            await UniTask.CompletedTask;
            return true;
        }
        
        #endregion

        private YIUILoopScroll<RedDotData, RedDotDataItem> m_SearchScroll;

        private List<RedDotData> m_CurrentDataList = new List<RedDotData>();

        protected override void Initialize()
        {
            InitInfo();
            m_SearchScroll = new YIUILoopScroll<RedDotData, RedDotDataItem>(u_ComSearchScroll, SearchRenderer);
            InitDropdownSearchDic();
        }

        private void SearchRenderer(int index, RedDotData data, RedDotDataItem item, bool select)
        {
            item.RefreshData(this, data);
        }

        private Dictionary<int, ERedDotKeyType> m_AllDropdownSearchDic = new Dictionary<int, ERedDotKeyType>();
        private List<TMP_Dropdown.OptionData>   m_DropdownOptionData   = new List<TMP_Dropdown.OptionData>();

        private void InitDropdownSearchDic()
        {
            u_ComDropdownSearch.ClearOptions();
            m_AllDropdownSearchDic.Clear();
            m_DropdownOptionData.Clear();
            var index = 0;
            foreach (ERedDotKeyType key in Enum.GetValues(typeof(ERedDotKeyType)))
            {
                if (key == ERedDotKeyType.None) continue;
                var des = RedDotMgr.Inst.GetKeyDes(key);
                if (string.IsNullOrEmpty(des)) continue;

                m_DropdownOptionData.Add(new TMP_Dropdown.OptionData(des));
                m_AllDropdownSearchDic.Add(index, key);
                index++;
            }

            u_ComDropdownSearch.AddOptions(m_DropdownOptionData);
        }

        private void RefreshSearchKey(ERedDotKeyType key)
        {
            var data = RedDotMgr.Inst.GetData(key);
            if (data == null) return;

            m_CurrentDataList.Clear();
            m_CurrentDataList.Add(data);
            RefreshSearchScroll();
            ResetStackInfo(data);
        }

        public void OnClickParentList(RedDotData data)
        {
            if (data.ParentList.Count <= 0) return;
            m_CurrentDataList.Clear();

            foreach (var parentData in data.ParentList)
            {
                m_CurrentDataList.Add(parentData);
            }

            RefreshSearchScroll();
        }

        private void RefreshSearchScroll()
        {
            m_SearchScroll.SetDataRefresh(m_CurrentDataList);
        }

        public void OnClickChildList(RedDotData data)
        {
            if (data.ChildList.Count <= 0) return;
            m_CurrentDataList.Clear();

            foreach (var childData in data.ChildList)
            {
                m_CurrentDataList.Add(childData);
            }

            RefreshSearchScroll();
        }

        public void OnClickItem(RedDotData data)
        {
            ResetStackInfo(data);
        }

        protected override void OnDestroy()
        {
            RemoveInfoChanged();
        }

        #region Event开始

        protected override void OnEventCloseAction()
        {
            Close();
        }

        protected override void OnEventInputSearchEndAction(string p1)
        {
            if (string.IsNullOrEmpty(p1))
            {
                return;
            }

            var result = Enum.TryParse(p1, out ERedDotKeyType key);
            if (!result)
            {
                Debug.LogError($"没有找到这个枚举 {p1}");
                return;
            }

            RefreshSearchKey(key);
        }

        protected override void OnEventDropdownSearchAction(int p1)
        {
            if (!m_AllDropdownSearchDic.TryGetValue(p1, out var key))
            {
                Debug.LogError($"没有找到这个key {p1}");
                return;
            }

            RefreshSearchKey(key);
        }

        protected override void OnEventChangeCountAction(string p1)
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

            ChangeInfoCount(value);
        }

        protected override void OnEventChangeToggleYIUIFrameworkAction(bool p1)
        {
            RedDotStackHelper.StackHideYIUIFramework = p1;
            u_DataToggleYIUIFramework.SetValue(p1);
            ChangeToggleRefreshStack();
        }

        protected override void OnEventChangeToggleUnityEngineAction(bool p1)
        {
            RedDotStackHelper.StackHideUnityEngine = p1;
            u_DataToggleUnityEngine.SetValue(p1);
            ChangeToggleRefreshStack();
        }

        protected override void OnEventChangeToggleYIUIBindAction(bool p1)
        {
            RedDotStackHelper.StackHideYIUIBind = p1;
            u_DataToggleYIUIBind.SetValue(p1);
            ChangeToggleRefreshStack();
        }

        protected override void OnEventChangeToggleShowStackIndexAction(bool p1)
        {
            RedDotStackHelper.ShowStackIndex = p1;
            u_DataToggleShowIndex.SetValue(p1);
            ChangeToggleRefreshStack();
        }

        protected override void OnEventChangeToggleShowFileNameAction(bool p1)
        {
            RedDotStackHelper.ShowFileNameStack = p1;
            u_DataToggleShowFileName.SetValue(p1);
            ChangeToggleRefreshStack();
        }

        protected override void OnEventChangeToggleShowFilePathAction(bool p1)
        {
            RedDotStackHelper.ShowFilePath = p1;
            u_DataToggleShowFilePath.SetValue(p1);
            ChangeToggleRefreshStack();
        }

        #endregion Event结束

    }
}