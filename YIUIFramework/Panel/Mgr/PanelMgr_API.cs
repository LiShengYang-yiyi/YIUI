namespace YIUIFramework
{
    public partial class PanelMgr
    {
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
    }
}