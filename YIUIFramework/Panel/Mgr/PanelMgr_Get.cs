namespace YIUIFramework
{
    public partial class PanelMgr
    {
        //字符串类型获取 需要自己转换 建议不要使用
        public BasePanel GetPanel(string panelName)
        {
            var info = GetPanelInfo(panelName);
            return info?.UIBasePanel;
        }

        /// <summary>
        /// 获取一个panel
        /// 必须是存在的 但不一定是打开的有可能是隐藏
        /// 这个只能表示对象存在
        /// 不应该滥用 UI与UI之间还是应该使用消息通信 这个是为了解决部分问题
        /// </summary>
        public T GetPanel<T>() where T : BasePanel
        {
            var info = GetPanelInfo<T>();
            if (info is { UIBasePanel: { } })
            {
                return (T)info.UIBasePanel;
            }
            return null;
        }
    }
}