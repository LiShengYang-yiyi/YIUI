namespace YIUIFramework
{
    public partial class PanelMgr
    {
        #region 判断Panel是否存在且显示

        public bool ActiveSelf(string panelName)
        {
            var info = GetPanelInfo(panelName);
            return info?.ActiveSelf ?? false;
        }

        public bool ActiveSelf<T>() where T : BasePanel
        {
            var info = GetPanelInfo<T>();
            return info?.ActiveSelf ?? false;
        }

        #endregion

        #region 判断指定Panel的指定View是否存在且显示

        public bool ActiveSelfView(string panelName, string viewName)
        {
            var info = GetPanelInfo(panelName);
            if (info == null) return false;
            if (info.UIBasePanel == null) return false;
            var (exist, entity) = info.UIBasePanel.ExistView(viewName);
            if (!exist) return false;
            return entity.ActiveSelf;
        }

        public bool ActiveSelfView<TPanel, TView>()
            where TPanel : BasePanel
            where TView : BaseView
        {
            var info = GetPanelInfo<TPanel>();
            if (info == null) return false;
            if (info.UIBasePanel == null) return false;
            var (exist, entity) = info.UIBasePanel.ExistView<TView>();
            if (!exist) return false;
            return entity.ActiveSelf;
        }

        public bool ActiveSelfViewByViewName<TPanel>(string viewName)
            where TPanel : BasePanel
        {
            var info = GetPanelInfo<TPanel>();
            if (info == null) return false;
            if (info.UIBasePanel == null) return false;
            var (exist, entity) = info.UIBasePanel.ExistView(viewName);
            if (!exist) return false;
            return entity.ActiveSelf;
        }

        public bool ActiveSelfViewByPanelName<TView>(string panelName)
            where TView : BaseView
        {
            var info = GetPanelInfo(panelName);
            if (info == null) return false;
            if (info.UIBasePanel == null) return false;
            var (exist, entity) = info.UIBasePanel.ExistView<TView>();
            if (!exist) return false;
            return entity.ActiveSelf;
        }

        #endregion
    }
}
