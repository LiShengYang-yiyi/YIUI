namespace YIUIFramework
{
    /// <summary>
    /// 面板基类, 编辑器创建面板代码时自动带入
    /// </summary>
    public abstract partial class BasePanel : BaseWindow, IYIUIPanel
    {
        /// <summary>
        /// 所在层级
        /// </summary>
        public virtual EPanelLayer Layer => EPanelLayer.Panel;

        /// <summary>
        /// 界面选项
        /// </summary>
        public virtual EPanelOption PanelOption => EPanelOption.None;

        /// <summary>
        /// 堆栈操作
        /// </summary>
        public virtual EPanelStackOption StackOption => EPanelStackOption.Visible;

        /// <summary>
        /// 优先级，用于同层级排序,
        /// 大的在前 小的在后
        /// 相同时 后添加的在前
        /// </summary>
        public virtual int Priority => 0;

        #region 密封生命周期

        protected sealed override void SealedInitialize()
        {
            InitPanelViewData();
        }

        protected sealed override void SealedStart()
        {
        }

        protected sealed override void SealedOnDestroy()
        {
        }

        #endregion
    }
}