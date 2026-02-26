namespace ET.Client
{
    public static partial class YIUIMgrComponentSystem
    {
        #region 判断Panel是否存在且显示

        public static bool ActiveSelf(this YIUIMgrComponent self, string panelName)
        {
            var info = self.GetPanelInfo(panelName);
            return info?.ActiveSelf ?? false;
        }

        public static bool ActiveSelf<T>(this YIUIMgrComponent self) where T : Entity
        {
            var info = self.GetPanelInfo<T>();
            return info?.ActiveSelf ?? false;
        }

        #endregion

        #region 判断指定Panel的指定View是否存在且显示

        public static bool ActiveSelfView(this YIUIMgrComponent self, string panelName, string viewName)
        {
            var info = self.GetPanelInfo(panelName);
            if (info == null) return false;
            if (info.UIPanel == null) return false;
            var (exist, entity) = info.UIPanel.ExistView(viewName);
            if (!exist) return false;
            return entity.GetParent<YIUIChild>()?.ActiveSelf ?? false;
        }

        public static bool ActiveSelfView<TPanel, TView>(this YIUIMgrComponent self)
                where TPanel : Entity
                where TView : Entity
        {
            var info = self.GetPanelInfo<TPanel>();
            if (info == null) return false;
            if (info.UIPanel == null) return false;
            var (exist, entity) = info.UIPanel.ExistView<TView>();
            if (!exist) return false;
            return entity.GetParent<YIUIChild>()?.ActiveSelf ?? false;
        }

        public static bool ActiveSelfViewByViewName<TPanel>(this YIUIMgrComponent self, string viewName)
                where TPanel : Entity
        {
            var info = self.GetPanelInfo<TPanel>();
            if (info == null) return false;
            if (info.UIPanel == null) return false;
            var (exist, entity) = info.UIPanel.ExistView(viewName);
            if (!exist) return false;
            return entity.GetParent<YIUIChild>()?.ActiveSelf ?? false;
        }

        public static bool ActiveSelfViewByPanelName<TView>(this YIUIMgrComponent self, string panelName)
                where TView : Entity
        {
            var info = self.GetPanelInfo(panelName);
            if (info == null) return false;
            if (info.UIPanel == null) return false;
            var (exist, entity) = info.UIPanel.ExistView<TView>();
            if (!exist) return false;
            return entity.GetParent<YIUIChild>()?.ActiveSelf ?? false;
        }

        #endregion
    }
}