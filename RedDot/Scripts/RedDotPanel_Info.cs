using System;
using YIUIBind;
using YIUIFramework;
using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace MizuYIUI.RedDot
{
    public sealed partial class RedDotPanel
    {
        private RedDotData m_InfoData;

        private YIUILoopScroll<RedDotStack, RedDotStackItem> m_StackScroll;

        private void InitInfo()
        {
            m_StackScroll = new YIUILoopScroll<RedDotStack, RedDotStackItem>(u_ComStackScroll, StackRenderer);
            u_DataToggleUnityEngine.SetValue(RedDotStackHelper.StackHideUnityEngine);
            u_DataToggleYIUIFramework.SetValue(RedDotStackHelper.StackHideYIUIFramework);
            u_DataToggleYIUIBind.SetValue(RedDotStackHelper.StackHideYIUIBind);
            u_DataToggleShowIndex.SetValue(RedDotStackHelper.ShowStackIndex);
            u_DataToggleShowFileName.SetValue(RedDotStackHelper.ShowFileNameStack);
            u_DataToggleShowFilePath.SetValue(RedDotStackHelper.ShowFilePath);
        }

        private void StackRenderer(int index, RedDotStack data, RedDotStackItem item, bool select)
        {
            item.u_DataId.SetValue(data.Id);
            item.u_DataTime.SetValue(data.GetTime());
            item.u_DataOs.SetValue(data.GetOS(m_InfoData));
            item.u_DataSource.SetValue(data.GetSource());
            item.u_DataShowStack.SetValue(false);
            item.ShowStackAction = () => { ShowStackInfo(data, item); };
        }

        private void ShowStackInfo(RedDotStack data, RedDotStackItem item)
        {
            item.u_ComStackText.text = data.GetStackContent();
        }

        private void ResetStackInfo(RedDotData data)
        {
            if (m_InfoData == data)
            {
                return;
            }

            RemoveInfoChanged();
            m_InfoData = data;
            RedDotMgr.Inst.AddChanged(m_InfoData.Key, OnInfoChangeCount);
            u_DataInfoName.SetValue($"{(int)data.Key} {RedDotMgr.Inst.GetKeyDes(data.Key)}");
            u_ComInputChangeCount.text = m_InfoData.Count.ToString();
            RefreshInfoScroll();
        }

        private void RemoveInfoChanged()
        {
            if (m_InfoData != null)
            {
                RedDotMgr.Inst.RemoveChanged(m_InfoData.Key, OnInfoChangeCount);
            }
        }

        private void RefreshInfoScroll()
        {
            if (m_InfoData == null)
            {
                return;
            }

            m_StackScroll.SetDataRefresh(m_InfoData.StackList);
        }

        private void OnInfoChangeCount(int obj)
        {
            RefreshInfoScroll();
            RefreshSearchScroll();
        }

        private void ChangeInfoCount(int count)
        {
            if (m_InfoData == null)
            {
                return;
            }

            RedDotMgr.Inst.SetCount(m_InfoData.Key, count);
        }

       
        private void ChangeToggleRefreshStack()
        {
            RefreshInfoScroll();
        }
        
    }
}