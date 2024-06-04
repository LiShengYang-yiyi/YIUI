using System;
using YIUIBind;
using YIUIFramework;
using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace MizuYIUI.RedDot
{
    /// <summary>
    /// Author  YIUI
    /// Date    2024.6.2
    /// </summary>
    public sealed partial class RedDotDataItem : RedDotDataItemBase
    {
    
        #region 生命周期
        
        
        
        #endregion

        private RedDotPanel m_RedDotPanel;
        private RedDotData  m_Data;

        public void RefreshData(RedDotPanel panel, RedDotData data)
        {
            m_RedDotPanel = panel;
            m_Data        = data;
            u_DataCount.SetValue(data.Count);
            u_DataName.SetValue(RedDotMgr.Inst.GetKeyDes(data.Key));
            u_DataTips.SetValue(data.Tips);
            u_DataKeyId.SetValue((int)data.Key);
            u_DataParentCount.SetValue(data.ParentList.Count);
            u_DataChildCount.SetValue(data.ChildList.Count);
            u_DataSwitchTips.SetValue(data.Config.SwitchTips);
        }

        #region Event开始

        protected override void OnEventParentAction()
        {
            m_RedDotPanel.OnClickParentList(m_Data);
        }

        protected override void OnEventChildAction()
        {
            m_RedDotPanel.OnClickChildList(m_Data);
        }

        protected override void OnEventTipsAction(bool p1)
        {
            RedDotMgr.Inst.SetTips(m_Data.Key, p1);
        }

        protected override void OnEventClickItemAction()
        {
            m_RedDotPanel.OnClickItem(m_Data);
        }

        #endregion Event结束
    }
}