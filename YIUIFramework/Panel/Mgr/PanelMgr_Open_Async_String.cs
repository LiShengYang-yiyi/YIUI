using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace YIUIFramework
{
    /// <summary>
    /// 字符串异步打开
    /// </summary>
    public partial class PanelMgr
    {
        /// <summary>
        /// 获取PanelInfo
        /// 没有则创建  相当于一个打开过了 UI基础配置档
        /// 这个根据BindVo创建  为什么没有直接用VO  因为里面有Panel 实例对象
        /// 这个k 根据resName
        /// </summary>
        private PanelInfo GetPanelInfo(string panelName)
        {
            var data = UIBindHelper.GetBindVoByPanelName(panelName);
            if (data == null) return null;
            var vo = data.Value;

            if (!m_PanelCfgMap.ContainsKey(panelName))
            {
                var info = new PanelInfo()
                {
                    Name    = panelName,
                    PkgName = vo.PkgName,
                    ResName = vo.ResName,
                };

                m_PanelCfgMap.Add(panelName, info);
            }

            return m_PanelCfgMap[panelName];
        }

        /// <summary>
        /// 用字符串开启 必须保证类名与资源名一致否则无法找到
        /// 首选使用<T>泛型方法打开UI 字符串只适合于特定场合使用
        /// </summary>
        public async UniTask<BasePanel> OpenPanelAsync(string panelName, object param = null)
        {
            var info = GetPanelInfo(panelName);
            if (info == null) return default;

            var panel = await OpenPanelStartAsync(panelName);
            if (panel == null) return default;

            var success = false;

            await OpenPanelBefore(info);

            try
            {
                var p = ParamVo.Get(param);
                success = await info.UIBasePanel.Open(p);
                ParamVo.Put(p);
            }
            catch (Exception e)
            {
                Debug.LogError($"panel={info.ResName}, err={e.Message}{e.StackTrace}");
            }

            return await OpenPanelAfter(info, success);
        }

        public async UniTask<BasePanel> OpenPanelAsync(string panelName, object param1, object param2)
        {
            var paramList = ListPool<object>.Get();
            paramList.Add(param1);
            paramList.Add(param2);
            var panel = await OpenPanelAsync(panelName, paramList);
            ListPool<object>.Put(paramList);
            return panel;
        }

        public async UniTask<BasePanel> OpenPanelAsync(string panelName, object param1, object param2, object param3)
        {
            var paramList = ListPool<object>.Get();
            paramList.Add(param1);
            paramList.Add(param2);
            paramList.Add(param3);
            var panel = await OpenPanelAsync(panelName, paramList);
            ListPool<object>.Put(paramList);
            return panel;
        }

        public async UniTask<BasePanel> OpenPanelAsync(string panelName, object param1, object param2, object param3,
                                                       object param4)
        {
            var paramList = ListPool<object>.Get();
            paramList.Add(param1);
            paramList.Add(param2);
            paramList.Add(param3);
            paramList.Add(param4);
            var panel = await OpenPanelAsync(panelName, paramList);
            ListPool<object>.Put(paramList);
            return panel;
        }

        public async UniTask<BasePanel> OpenPanelAsync(string panelName, object param1, object param2, object param3,
                                                       object param4,    params object[] paramMore)
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

            var panel = await OpenPanelAsync(panelName, paramList);
            ListPool<object>.Put(paramList);
            return panel;
        }
    }
}