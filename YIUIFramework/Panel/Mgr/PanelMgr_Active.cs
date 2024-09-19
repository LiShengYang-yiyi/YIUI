namespace YIUIFramework
{
    public partial class PanelMgr
    {
        /// <summary> 判断指定Panel是否显示 </summary>
        public bool ActiveSelf(string panelName)
        {
            var info = GetPanelInfo(panelName);
            return info?.ActiveSelf ?? false;
        }

        /// <summary> 判断指定Panel是否显示 </summary>
        public bool ActiveSelf<T>() where T : BasePanel
        {
            var info = GetPanelInfo<T>();
            return info?.ActiveSelf ?? false;
        }

        /// <summary> 判断指定Panel下的View是否显示 </summary>
        public bool ActiveSelfView(string panelName, string viewName)
        {
            var info = GetPanelInfo(panelName);
            if (info == null) return false;
            if (info.UIBasePanel == null) return false;
            var (exist, entity) = info.UIBasePanel.ExistView(viewName);
            if (!exist) return false;
            return entity.ActiveSelf;
        }

        /// <summary> 判断指定Panel下的View是否显示 </summary>
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

        /// <summary> 判断指定Panel下的View是否显示 </summary>
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

        /// <summary> 判断指定Panel下的View是否显示 </summary>
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
    }
}
