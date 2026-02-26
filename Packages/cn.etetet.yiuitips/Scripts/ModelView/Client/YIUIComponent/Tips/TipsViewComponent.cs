namespace ET.Client
{
    [ComponentOf(typeof(YIUIWindowComponent))]
    public partial class TipsViewComponent : Entity, IAwake, IDestroy, IYIUIWindowClose
    {
        public bool m_IsFromTips;
    }

    /// <summary>
    /// 通用弹窗view关闭事件
    /// </summary>
    public struct EventPutTipsView
    {
        public EntityRef<Entity> View;
        public bool   Destroy;
    }
}