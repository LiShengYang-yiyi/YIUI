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
    public partial class GMTypeItemComponent : Entity, IDestroy, IAwake, IYIUIBind, IYIUIInitialize
    {
        public const string PkgName = "GM";
        public const string ResName = "GMTypeItem";

        public EntityRef<YIUIChild> u_UIBase;
        public YIUIChild UIBase => u_UIBase;
        public YIUIFramework.UIDataValueString u_DataTypeName;
        public YIUIFramework.UIDataValueBool u_DataSelect;
        public UIEventP0 u_EventSelect;
        public UIEventHandleP0 u_EventSelectHandle;
        public const string OnEventSelectInvoke = "GMTypeItemComponent.OnEventSelectInvoke";

    }
}