using YIUIFramework;

namespace ET.Client
{
    /// <summary>
    /// 由YIUI工具自动创建 请勿修改
    /// 当前Panel所有可用view枚举
    /// </summary>
    public enum EGMPanelViewEnum
    {
        GMView = 1,
    }
    /// <summary>
    /// 由YIUI工具自动创建 请勿修改
    /// </summary>
    [YIUI(EUICodeType.Panel, EPanelLayer.Top)]
    public partial class GMPanelComponent : Entity, IDestroy, IAwake, IYIUIBind, IYIUIInitialize, IYIUIOpen
    {
        public const string PkgName = "GM";
        public const string ResName = "GMPanel";

        public YIUIComponent UIBase;
        public YIUIWindowComponent UIWindow;
        public YIUIPanelComponent UIPanel;
        public UnityEngine.RectTransform u_ComGMButton;
        public UnityEngine.RectTransform u_ComLimitRange;
        public UIEventP0 u_EventOpenGMView;
        public UIEventHandleP0 u_EventOpenGMViewHandle;
        public UIEventP1<object> u_EventBeginDrag;
        public UIEventHandleP1<object> u_EventBeginDragHandle;
        public UIEventP1<object> u_EventEndDrag;
        public UIEventHandleP1<object> u_EventEndDragHandle;
        public UIEventP1<object> u_EventDrag;
        public UIEventHandleP1<object> u_EventDragHandle;

    }
}