namespace ET.Client
{
    public static partial class YIUIViewComponentSystem
    {
        //标准view(Panel下的view)(非独立View)
        //可快捷获取panel实例 (不推荐使用)!!!
        //这里只是告诉你有这么个方式
        public static T GetPanel<T>(this YIUIViewComponent self) where T : IYIUIOpen
        {
            if (self?.Parent?.Parent is T panel)
            {
                return panel;
            }

            Log.Error($"获取失败 {self.GetType().Name} 没有找到父级 {typeof(T).Name} 请检查结构");
            return default;
        }

        //同上 需要标准View
        //这里拿到的不是panel 而是panel同级的YIUIPanelComponent
        public static YIUIPanelComponent GetPanelComponent(this YIUIViewComponent self)
        {
            if (self?.Parent?.Parent?.Parent is YIUIChild uiBase)
            {
                return uiBase.GetComponent<YIUIPanelComponent>();
            }

            Log.Error($"获取失败 {self.GetType().Name} 没有找到 YIUIPanelComponent 请检查结构");
            return default;
        }

        //同上 需要标准View
        //这里拿到的不是panel 而是panel同级的YIUIWindowComponent
        public static YIUIWindowComponent GetWindowComponent(this YIUIViewComponent self)
        {
            if (self?.Parent?.Parent?.Parent is YIUIChild uiBase)
            {
                return uiBase.GetComponent<YIUIWindowComponent>();
            }

            Log.Error($"获取失败 {self.GetType().Name} 没有找到 YIUIWindowComponent 请检查结构");
            return default;
        }
    }
}