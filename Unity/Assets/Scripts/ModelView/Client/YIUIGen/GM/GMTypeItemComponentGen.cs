using YIUIFramework;

namespace ET.Client
{

    /// <summary>
    /// 由YIUI工具自动创建 请勿修改
    /// </summary>
    [YIUI(EUICodeType.Common)]
    public partial class GMTypeItemComponent : Entity, IDestroy, IAwake, IYIUIBind, IYIUIInitialize
    {
        public const string PkgName = "GM";
        public const string ResName = "GMTypeItem";

        public YIUIComponent UIBase;
        public YIUIFramework.UIDataValueString u_DataTypeName;
        public YIUIFramework.UIDataValueBool u_DataSelect;
        public UIEventP0 u_EventSelect;
        public UIEventHandleP0 u_EventSelectHandle;

    }
}