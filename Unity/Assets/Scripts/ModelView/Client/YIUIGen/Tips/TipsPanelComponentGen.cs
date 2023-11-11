using YIUIFramework;

namespace ET.Client
{

    /// <summary>
    /// 由YIUI工具自动创建 请勿修改
    /// </summary>
    [YIUI(EUICodeType.Panel, EPanelLayer.Tips)]
    public partial class TipsPanelComponent : Entity, IDestroy, IAwake, IYIUIBind, IYIUIInitialize, IYIUIOpen
    {
        public const string PkgName = "Tips";
        public const string ResName = "TipsPanel";

        public YIUIComponent UIBase;
        public YIUIWindowComponent UIWindow;
        public YIUIPanelComponent UIPanel;

    }
}