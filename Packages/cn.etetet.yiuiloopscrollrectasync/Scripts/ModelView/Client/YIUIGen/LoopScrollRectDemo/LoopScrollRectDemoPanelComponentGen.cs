using System;
using UnityEngine;
using YIUIFramework;
using System.Collections.Generic;

namespace ET.Client
{
    /// <summary>
    /// 由YIUI工具自动创建 请勿修改
    /// 当前Panel所有可用view枚举
    /// </summary>
    public enum ELoopScrollRectDemoPanelViewEnum
    {
        LoopScrollHorizontalView = 1,
        LoopScrollVerticalView = 2,
        LoopScrollVerticalGroupView = 3,
    }
    /// <summary>
    /// 由YIUI工具自动创建 请勿修改
    /// </summary>
    [YIUI(EUICodeType.Panel, EPanelLayer.Panel)]
    [ComponentOf(typeof(YIUIChild))]
    public partial class LoopScrollRectDemoPanelComponent : Entity, IDestroy, IAwake, IYIUIBind, IYIUIInitialize, IYIUIOpen
    {
        public const string PkgName = "LoopScrollRectDemo";
        public const string ResName = "LoopScrollRectDemoPanel";

        public EntityRef<YIUIChild> u_UIBase;
        public YIUIChild UIBase => u_UIBase;
        public EntityRef<YIUIWindowComponent> u_UIWindow;
        public YIUIWindowComponent UIWindow => u_UIWindow;
        public EntityRef<YIUIPanelComponent> u_UIPanel;
        public YIUIPanelComponent UIPanel => u_UIPanel;
        public EntityRef<ET.Client.YIUICloseCommonComponent> u_UIYIUIClose_White;
        public ET.Client.YIUICloseCommonComponent UIYIUIClose_White => u_UIYIUIClose_White;
        public UITaskEventP1<int> u_EventTab;
        public UITaskEventHandleP1<int> u_EventTabHandle;
        public const string OnEventTabInvoke = "LoopScrollRectDemoPanelComponent.OnEventTabInvoke";

    }
}