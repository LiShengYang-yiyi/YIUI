using YIUIFramework;

namespace ET.Client
{

    /// <summary>
    /// 由YIUI工具自动创建 请勿修改
    /// </summary>
    [YIUI(EUICodeType.Common)]
    public partial class GMParamItemComponent: Entity, IDestroy, IAwake, IYIUIBind, IYIUIInitialize
    {
        public const string PkgName = "GM";
        public const string ResName = "GMParamItem";

        public EntityRef<YIUIComponent> u_UIBase;
        public YIUIComponent UIBase => u_UIBase;
        public TMPro.TMP_InputField u_ComInputField;
        public UnityEngine.UI.Toggle u_ComToggle;
        public TMPro.TMP_Dropdown u_ComDropdown;
        public YIUIFramework.UIDataValueString u_DataParamDesc;
        public YIUIFramework.UIDataValueInt u_DataTypeValue;
        public UIEventP1<string> u_EventInput;
        public UIEventHandleP1<string> u_EventInputHandle;
        public UIEventP1<bool> u_EventToggle;
        public UIEventHandleP1<bool> u_EventToggleHandle;
        public UIEventP1<int> u_EventDropdown;
        public UIEventHandleP1<int> u_EventDropdownHandle;

    }
}