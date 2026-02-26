using System;
using UnityEngine;
using YIUIFramework;
using System.Collections.Generic;

namespace ET.Client
{

    /// <summary>
    /// 由YIUI工具自动创建 请勿修改
    /// </summary>
    [YIUI(EUICodeType.Common)]
    [ComponentOf(typeof(YIUIChild))]
    public partial class GMCommandItemComponent : Entity, IDestroy, IAwake, IYIUIBind, IYIUIInitialize
    {
        public const string PkgName = "GM";
        public const string ResName = "GMCommandItem";

        public EntityRef<YIUIChild> u_UIBase;
        public YIUIChild UIBase => u_UIBase;
        public UnityEngine.UI.LoopHorizontalScrollRect u_ComParamLoop;
        public YIUIFramework.UIDataValueString u_DataName;
        public YIUIFramework.UIDataValueBool u_DataShowParamLoop;
        public YIUIFramework.UIDataValueString u_DataDesc;
        public UIEventP0 u_EventRun;
        public UIEventHandleP0 u_EventRunHandle;
        public const string OnEventRunInvoke = "GMCommandItemComponent.OnEventRunInvoke";

    }
}