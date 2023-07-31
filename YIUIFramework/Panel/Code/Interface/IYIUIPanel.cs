namespace YIUIFramework
{
    /// <summary>
    /// 面板接口
    /// </summary>
    public interface IYIUIPanel : IYIUIWindow
    {
        /// <summary>
        /// 得到窗口所在的层
        /// </summary>
        EPanelLayer Layer { get; }

        /// <summary>
        /// 界面的各种选项
        /// </summary>
        EPanelOption PanelOption { get; }

        /// <summary>
        /// 面板堆栈操作
        /// </summary>
        EPanelStackOption StackOption { get; }

        /// <summary>
        /// 同层级，优先级高的在前面
        /// </summary>
        int Priority { get; }
    }
}