namespace YIUIFramework
{
    public abstract partial class BasePanel
    {
        /// <summary>
        /// 当前已打开的UI 不包含弹窗
        /// </summary>
        protected BaseView u_CurrentOpenView;

        /// <summary>
        /// 外界可判断的当前打开的view名字
        /// </summary>
        public string CurrentOpenViewName => u_CurrentOpenView?.UIResName ?? "";
    }
}