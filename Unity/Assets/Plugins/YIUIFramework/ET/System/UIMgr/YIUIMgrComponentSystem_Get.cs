namespace ET.Client
{
    public static partial class YIUIMgrComponentSystem
    {
        #region 获取指定Panel 必须是存在的 但不一定是打开的有可能是隐藏

        //字符串类型获取 需要自己转换 建议不要使用
        public static Entity GetPanel(this YIUIMgrComponent self, string panelName)
        {
            var info = self.GetPanelInfo(panelName);
            return info?.OwnerUIEntity;
        }

        /// <summary>
        /// 获取一个panel
        /// 必须是存在的 但不一定是打开的有可能是隐藏
        /// 这个只能表示对象存在
        /// 不应该滥用 UI与UI之间还是应该使用消息通信 这个是为了解决部分问题
        /// </summary>
        public static T GetPanel<T>(this YIUIMgrComponent self) where T : Entity
        {
            var info = self.GetPanelInfo<T>();
            if (info is { OwnerUIEntity: not null })
            {
                return (T)info.OwnerUIEntity;
            }

            return null;
        }

        #endregion

        #region 获取指定Panel的指定View 必须是存在的 但不一定是打开的有可能是隐藏

        //字符串类型获取 需要自己转换 建议不要使用
        public static Entity GetPanelView(this YIUIMgrComponent self, string panelName, string viewName)
        {
            var info = self.GetPanelInfo(panelName);
            if (info == null) return null;
            if (info.UIPanel == null) return null;
            var (exist, entity) = info.UIPanel.ExistView(viewName);
            if (exist == false) return null;
            return entity;
        }

        /// <summary>
        /// 获取指定Panel的指定View
        /// 必须是存在的 但不一定是打开的有可能是隐藏
        /// 这个只能表示对象存在
        /// 不应该滥用 UI与UI之间还是应该使用消息通信 这个是为了解决部分问题
        /// </summary>
        public static TView GetPanelView<TPanel, TView>(this YIUIMgrComponent self)
                where TPanel : Entity
                where TView : Entity
        {
            var info = self.GetPanelInfo<TPanel>();
            if (info == null) return null;
            if (info.UIPanel == null) return null;
            var (exist, entity) = info.UIPanel.ExistView<TView>();
            if (exist == false) return null;
            return (TView)entity;
        }

        //字符串类型获取 需要自己转换 建议不要使用
        public static Entity GetPanelViewByViewName<TPanel>(this YIUIMgrComponent self, string viewName)
                where TPanel : Entity
        {
            var info = self.GetPanelInfo<TPanel>();
            if (info == null) return null;
            if (info.UIPanel == null) return null;
            var (exist, entity) = info.UIPanel.ExistView(viewName);
            if (!exist) return null;
            return entity;
        }

        /// <summary>
        /// 获取指定Panel的指定View
        /// 必须是存在的 但不一定是打开的有可能是隐藏
        /// 这个只能表示对象存在
        /// 不应该滥用 UI与UI之间还是应该使用消息通信 这个是为了解决部分问题
        /// </summary>
        public static TView GetPanelViewByPanelName<TView>(this YIUIMgrComponent self, string panelName)
                where TView : Entity
        {
            var info = self.GetPanelInfo(panelName);
            if (info == null) return null;
            if (info.UIPanel == null) return null;
            var (exist, entity) = info.UIPanel.ExistView<TView>();
            if (exist == false) return null;
            return (TView)entity;
        }

        #endregion
    }
}
