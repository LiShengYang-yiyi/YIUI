using YIUIFramework;

namespace ET.Client
{

    /// <summary>
    /// 由YIUI工具自动创建 请勿修改
    /// </summary>
    [YIUI(EUICodeType.View)]
    public partial class GMViewComponent : Entity, IDestroy, IAwake, IYIUIBind, IYIUIInitialize, IYIUIOpen
    {
        public const string PkgName = "GM";
        public const string ResName = "GMView";

        public YIUIComponent UIBase;
        public YIUIWindowComponent UIWindow;
        public YIUIViewComponent UIView;
        public UnityEngine.UI.LoopVerticalScrollRect u_ComGMTypeLoop;
        public UnityEngine.UI.LoopVerticalScrollRect u_ComGMCommandLoop;
        public UIEventP0 u_EventClose;
        public UIEventHandleP0 u_EventCloseHandle;

    }
}