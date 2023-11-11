using YIUIFramework;

namespace ET.Client
{

    /// <summary>
    /// 由YIUI工具自动创建 请勿修改
    /// </summary>
    [YIUI(EUICodeType.View)]
    public partial class TextTipsViewComponent : Entity, IDestroy, IAwake, IYIUIBind, IYIUIInitialize, IYIUIOpen
    {
        public const string PkgName = "Tips";
        public const string ResName = "TextTipsView";

        public YIUIComponent UIBase;
        public YIUIWindowComponent UIWindow;
        public YIUIViewComponent UIView;
        public UnityEngine.RectTransform u_ComContent;
        public UnityEngine.Animation u_ComAnimation;
        public YIUIFramework.UIDataValueString u_DataMessageContent;

    }
}