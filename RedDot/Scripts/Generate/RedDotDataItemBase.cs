using System;
using YIUIBind;
using YIUIFramework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace MizuYIUI.RedDot
{



    /// <summary>
    /// 由YIUI工具自动创建 请勿手动修改
    /// </summary>
    public abstract class RedDotDataItemBase : BaseComponent
    {
        public const string PkgName = "RedDot";
        public const string ResName = "RedDotDataItem";
        
        public YIUIBind.UIDataValueInt u_DataKeyId { get; private set; }
        public YIUIBind.UIDataValueInt u_DataCount { get; private set; }
        public YIUIBind.UIDataValueString u_DataName { get; private set; }
        public YIUIBind.UIDataValueBool u_DataTips { get; private set; }
        public YIUIBind.UIDataValueInt u_DataParentCount { get; private set; }
        public YIUIBind.UIDataValueInt u_DataChildCount { get; private set; }
        public YIUIBind.UIDataValueBool u_DataShowType { get; private set; }
        public YIUIBind.UIDataValueBool u_DataSwitchTips { get; private set; }
        protected UIEventP0 u_EventParent { get; private set; }
        protected UIEventHandleP0 u_EventParentHandle { get; private set; }
        protected UIEventP0 u_EventChild { get; private set; }
        protected UIEventHandleP0 u_EventChildHandle { get; private set; }
        protected UIEventP1<bool> u_EventTips { get; private set; }
        protected UIEventHandleP1<bool> u_EventTipsHandle { get; private set; }
        protected UIEventP0 u_EventClickItem { get; private set; }
        protected UIEventHandleP0 u_EventClickItemHandle { get; private set; }

        
        protected sealed override void UIBind()
        {
            u_DataKeyId = DataTable.FindDataValue<YIUIBind.UIDataValueInt>("u_DataKeyId");
            u_DataCount = DataTable.FindDataValue<YIUIBind.UIDataValueInt>("u_DataCount");
            u_DataName = DataTable.FindDataValue<YIUIBind.UIDataValueString>("u_DataName");
            u_DataTips = DataTable.FindDataValue<YIUIBind.UIDataValueBool>("u_DataTips");
            u_DataParentCount = DataTable.FindDataValue<YIUIBind.UIDataValueInt>("u_DataParentCount");
            u_DataChildCount = DataTable.FindDataValue<YIUIBind.UIDataValueInt>("u_DataChildCount");
            u_DataShowType = DataTable.FindDataValue<YIUIBind.UIDataValueBool>("u_DataShowType");
            u_DataSwitchTips = DataTable.FindDataValue<YIUIBind.UIDataValueBool>("u_DataSwitchTips");
            u_EventParent = EventTable.FindEvent<UIEventP0>("u_EventParent");
            u_EventParentHandle = u_EventParent.Add(OnEventParentAction);
            u_EventChild = EventTable.FindEvent<UIEventP0>("u_EventChild");
            u_EventChildHandle = u_EventChild.Add(OnEventChildAction);
            u_EventTips = EventTable.FindEvent<UIEventP1<bool>>("u_EventTips");
            u_EventTipsHandle = u_EventTips.Add(OnEventTipsAction);
            u_EventClickItem = EventTable.FindEvent<UIEventP0>("u_EventClickItem");
            u_EventClickItemHandle = u_EventClickItem.Add(OnEventClickItemAction);

        }

        protected sealed override void UnUIBind()
        {
            u_EventParent.Remove(u_EventParentHandle);
            u_EventChild.Remove(u_EventChildHandle);
            u_EventTips.Remove(u_EventTipsHandle);
            u_EventClickItem.Remove(u_EventClickItemHandle);

        }
     
        protected virtual void OnEventParentAction(){}
        protected virtual void OnEventChildAction(){}
        protected virtual void OnEventTipsAction(bool p1){}
        protected virtual void OnEventClickItemAction(){}
   
   
    }
}