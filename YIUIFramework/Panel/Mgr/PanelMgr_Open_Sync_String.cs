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
            var paramList = ListPool<object>.Get();
            paramList.Add(param1);
            paramList.Add(param2);
            OpenPanel(panelName, paramList);
            ListPool<object>.Put(paramList);
        }

        public void OpenPanel(string panelName, object param1, object param2, object param3)
        {
            var paramList = ListPool<object>.Get();
            paramList.Add(param1);
            paramList.Add(param2);
            paramList.Add(param3);
            OpenPanel(panelName, paramList);
            ListPool<object>.Put(paramList);
        }

        public void OpenPanel(string panelName, object param1, object param2, object param3, object param4)
        {
            var paramList = ListPool<object>.Get();
            paramList.Add(param1);
            paramList.Add(param2);
            paramList.Add(param3);
            paramList.Add(param4);
            OpenPanel(panelName, paramList);
            ListPool<object>.Put(paramList);
        }

        public void OpenPanel(string          panelName, object param1, object param2, object param3, object param4,
                              params object[] paramMore)
        {
            var paramList = ListPool<object>.Get();
            paramList.Add(param1);
            paramList.Add(param2);
            paramList.Add(param3);
            paramList.Add(param4);
            if (paramMore.Length > 0)
            {
                paramList.AddRange(paramMore);
            }

            OpenPanel(panelName, paramList);
            ListPool<object>.Put(paramList);
        }
    }
}