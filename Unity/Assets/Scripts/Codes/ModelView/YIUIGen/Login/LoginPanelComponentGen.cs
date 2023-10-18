using YIUIFramework;

namespace ET.Client
{

    /// <summary>
    /// 由YIUI工具自动创建 请勿修改
    /// </summary>
    [YIUI(EUICodeType.Panel, EPanelLayer.Popup)]
    public partial class LoginPanelComponent : Entity, IDestroy, IAwake, IYIUIBind, IYIUIInitialize, IYIUIOpen
    {
        public const string PkgName = "Login";
        public const string ResName = "LoginPanel";

        public YIUIComponent UIBase;
        public YIUIWindowComponent UIWindow;
        public YIUIPanelComponent UIPanel;
        public UIEventP0 u_EventLogin;
        public UIEventHandleP0 u_EventLoginHandle;
        public UIEventP1<string> u_EventAccount;
        public UIEventHandleP1<string> u_EventAccountHandle;
        public UIEventP1<string> u_EventPassword;
        public UIEventHandleP1<string> u_EventPasswordHandle;

    }
}