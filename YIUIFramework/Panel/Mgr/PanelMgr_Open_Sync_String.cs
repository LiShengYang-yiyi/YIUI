using Cysharp.Threading.Tasks;

namespace YIUIFramework
{
    public partial class PanelMgr
    {
        //非特殊需求 应该尽量使用异步操作
        //同步 无法获得返回值
        public void OpenPanel(string panelName, object param = null)
        {
            OpenPanelAsync(panelName, param).Forget();
        }

        public void OpenPanel(string panelName, object param1, object param2)
        {
            OpenPanelAsync(panelName, param1, param2).Forget();
        }

        public void OpenPanel(string panelName, object param1, object param2, object param3)
        {
            OpenPanelAsync(panelName, param1, param2, param3).Forget();
        }

        public void OpenPanel(string panelName, object param1, object param2, object param3, object param4)
        {
            OpenPanelAsync(panelName, param1, param2, param3, param4).Forget();
        }

        public void OpenPanel(string          panelName, object param1, object param2, object param3, object param4,
                              params object[] paramMore)
        {
            OpenPanelAsync(panelName, param1, param2, param3, param4, paramMore).Forget();
        }
    }
}