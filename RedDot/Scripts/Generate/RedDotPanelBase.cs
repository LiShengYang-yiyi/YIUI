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
    public abstract class RedDotPanelBase : BasePanel
    {
        public const string PkgName = "RedDot";
        public const string ResName = "RedDotPanel";
        
        public override EWindowOption WindowOption => EWindowOption.None;
        public override EPanelLayer Layer => EPanelLayer.Tips;
        public override EPanelOption PanelOption => EPanelOption.TimeCache;
        public override EPanelStackOption StackOption => EPanelStackOption.VisibleTween;
        public override int Priority => 1000;
        protected override float CachePanelTime => 10;

        public UnityEngine.UI.LoopVerticalScrollRect u_ComSearchScroll { get; private set; }
        public TMPro.TMP_Dropdown u_ComDropdownSearch { get; private set; }
        public UnityEngine.UI.LoopVerticalScrollRect u_ComStackScroll { get; private set; }
        public TMPro.TMP_InputField u_ComInputChangeCount { get; private set; }
        public YIUIBind.UIDataValueBool u_DataDropdownSearch { get; private set; }
        public YIUIBind.UIDataValueString u_DataInfoName { get; private set; }
        public YIUIBind.UIDataValueBool u_DataToggleUnityEngine { get; private set; }
        public YIUIBind.UIDataValueBool u_DataToggleYIUIBind { get; private set; }
        public YIUIBind.UIDataValueBool u_DataToggleYIUIFramework { get; private set; }
        public YIUIBind.UIDataValueBool u_DataToggleShowIndex { get; private set; }
        public YIUIBind.UIDataValueBool u_DataToggleShowFileName { get; private set; }
        public YIUIBind.UIDataValueBool u_DataToggleShowFilePath { get; private set; }
        protected UIEventP0 u_EventClose { get; private set; }
        protected UIEventHandleP0 u_EventCloseHandle { get; private set; }
        protected UIEventP1<string> u_EventInputSearchEnd { get; private set; }
        protected UIEventHandleP1<string> u_EventInputSearchEndHandle { get; private set; }
        protected UIEventP1<int> u_EventDropdownSearch { get; private set; }
        protected UIEventHandleP1<int> u_EventDropdownSearchHandle { get; private set; }
        protected UIEventP1<string> u_EventChangeCount { get; private set; }
        protected UIEventHandleP1<string> u_EventChangeCountHandle { get; private set; }
        protected UIEventP1<bool> u_EventChangeToggleYIUIFramework { get; private set; }
        protected UIEventHandleP1<bool> u_EventChangeToggleYIUIFrameworkHandle { get; private set; }
        protected UIEventP1<bool> u_EventChangeToggleUnityEngine { get; private set; }
        protected UIEventHandleP1<bool> u_EventChangeToggleUnityEngineHandle { get; private set; }
        protected UIEventP1<bool> u_EventChangeToggleYIUIBind { get; private set; }
        protected UIEventHandleP1<bool> u_EventChangeToggleYIUIBindHandle { get; private set; }
        protected UIEventP1<bool> u_EventChangeToggleShowStackIndex { get; private set; }
        protected UIEventHandleP1<bool> u_EventChangeToggleShowStackIndexHandle { get; private set; }
        protected UIEventP1<bool> u_EventChangeToggleShowFileName { get; private set; }
        protected UIEventHandleP1<bool> u_EventChangeToggleShowFileNameHandle { get; private set; }
        protected UIEventP1<bool> u_EventChangeToggleShowFilePath { get; private set; }
        protected UIEventHandleP1<bool> u_EventChangeToggleShowFilePathHandle { get; private set; }

        
        protected sealed override void UIBind()
        {
            u_ComSearchScroll = ComponentTable.FindComponent<UnityEngine.UI.LoopVerticalScrollRect>("u_ComSearchScroll");
            u_ComDropdownSearch = ComponentTable.FindComponent<TMPro.TMP_Dropdown>("u_ComDropdownSearch");
            u_ComStackScroll = ComponentTable.FindComponent<UnityEngine.UI.LoopVerticalScrollRect>("u_ComStackScroll");
            u_ComInputChangeCount = ComponentTable.FindComponent<TMPro.TMP_InputField>("u_ComInputChangeCount");
            u_DataDropdownSearch = DataTable.FindDataValue<YIUIBind.UIDataValueBool>("u_DataDropdownSearch");
            u_DataInfoName = DataTable.FindDataValue<YIUIBind.UIDataValueString>("u_DataInfoName");
            u_DataToggleUnityEngine = DataTable.FindDataValue<YIUIBind.UIDataValueBool>("u_DataToggleUnityEngine");
            u_DataToggleYIUIBind = DataTable.FindDataValue<YIUIBind.UIDataValueBool>("u_DataToggleYIUIBind");
            u_DataToggleYIUIFramework = DataTable.FindDataValue<YIUIBind.UIDataValueBool>("u_DataToggleYIUIFramework");
            u_DataToggleShowIndex = DataTable.FindDataValue<YIUIBind.UIDataValueBool>("u_DataToggleShowIndex");
            u_DataToggleShowFileName = DataTable.FindDataValue<YIUIBind.UIDataValueBool>("u_DataToggleShowFileName");
            u_DataToggleShowFilePath = DataTable.FindDataValue<YIUIBind.UIDataValueBool>("u_DataToggleShowFilePath");
            u_EventClose = EventTable.FindEvent<UIEventP0>("u_EventClose");
            u_EventCloseHandle = u_EventClose.Add(OnEventCloseAction);
            u_EventInputSearchEnd = EventTable.FindEvent<UIEventP1<string>>("u_EventInputSearchEnd");
            u_EventInputSearchEndHandle = u_EventInputSearchEnd.Add(OnEventInputSearchEndAction);
            u_EventDropdownSearch = EventTable.FindEvent<UIEventP1<int>>("u_EventDropdownSearch");
            u_EventDropdownSearchHandle = u_EventDropdownSearch.Add(OnEventDropdownSearchAction);
            u_EventChangeCount = EventTable.FindEvent<UIEventP1<string>>("u_EventChangeCount");
            u_EventChangeCountHandle = u_EventChangeCount.Add(OnEventChangeCountAction);
            u_EventChangeToggleYIUIFramework = EventTable.FindEvent<UIEventP1<bool>>("u_EventChangeToggleYIUIFramework");
            u_EventChangeToggleYIUIFrameworkHandle = u_EventChangeToggleYIUIFramework.Add(OnEventChangeToggleYIUIFrameworkAction);
            u_EventChangeToggleUnityEngine = EventTable.FindEvent<UIEventP1<bool>>("u_EventChangeToggleUnityEngine");
            u_EventChangeToggleUnityEngineHandle = u_EventChangeToggleUnityEngine.Add(OnEventChangeToggleUnityEngineAction);
            u_EventChangeToggleYIUIBind = EventTable.FindEvent<UIEventP1<bool>>("u_EventChangeToggleYIUIBind");
            u_EventChangeToggleYIUIBindHandle = u_EventChangeToggleYIUIBind.Add(OnEventChangeToggleYIUIBindAction);
            u_EventChangeToggleShowStackIndex = EventTable.FindEvent<UIEventP1<bool>>("u_EventChangeToggleShowStackIndex");
            u_EventChangeToggleShowStackIndexHandle = u_EventChangeToggleShowStackIndex.Add(OnEventChangeToggleShowStackIndexAction);
            u_EventChangeToggleShowFileName = EventTable.FindEvent<UIEventP1<bool>>("u_EventChangeToggleShowFileName");
            u_EventChangeToggleShowFileNameHandle = u_EventChangeToggleShowFileName.Add(OnEventChangeToggleShowFileNameAction);
            u_EventChangeToggleShowFilePath = EventTable.FindEvent<UIEventP1<bool>>("u_EventChangeToggleShowFilePath");
            u_EventChangeToggleShowFilePathHandle = u_EventChangeToggleShowFilePath.Add(OnEventChangeToggleShowFilePathAction);

        }

        protected sealed override void UnUIBind()
        {
            u_EventClose.Remove(u_EventCloseHandle);
            u_EventInputSearchEnd.Remove(u_EventInputSearchEndHandle);
            u_EventDropdownSearch.Remove(u_EventDropdownSearchHandle);
            u_EventChangeCount.Remove(u_EventChangeCountHandle);
            u_EventChangeToggleYIUIFramework.Remove(u_EventChangeToggleYIUIFrameworkHandle);
            u_EventChangeToggleUnityEngine.Remove(u_EventChangeToggleUnityEngineHandle);
            u_EventChangeToggleYIUIBind.Remove(u_EventChangeToggleYIUIBindHandle);
            u_EventChangeToggleShowStackIndex.Remove(u_EventChangeToggleShowStackIndexHandle);
            u_EventChangeToggleShowFileName.Remove(u_EventChangeToggleShowFileNameHandle);
            u_EventChangeToggleShowFilePath.Remove(u_EventChangeToggleShowFilePathHandle);

        }
     
        protected virtual void OnEventCloseAction(){}
        protected virtual void OnEventInputSearchEndAction(string p1){}
        protected virtual void OnEventDropdownSearchAction(int p1){}
        protected virtual void OnEventChangeCountAction(string p1){}
        protected virtual void OnEventChangeToggleYIUIFrameworkAction(bool p1){}
        protected virtual void OnEventChangeToggleUnityEngineAction(bool p1){}
        protected virtual void OnEventChangeToggleYIUIBindAction(bool p1){}
        protected virtual void OnEventChangeToggleShowStackIndexAction(bool p1){}
        protected virtual void OnEventChangeToggleShowFileNameAction(bool p1){}
        protected virtual void OnEventChangeToggleShowFilePathAction(bool p1){}
   
   
    }
}