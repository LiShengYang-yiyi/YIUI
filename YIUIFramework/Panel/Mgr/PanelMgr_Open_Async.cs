using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace YIUIFramework
{
    /// <summary>
    /// 异步打开
    /// </summary>
    public partial class PanelMgr
    {
        private async UniTask<PanelInfo> OpenPanelStartAsync(string panelName)
        {
            #if YIUIMACRO_PANEL_OPENCLOSE
            Debug.Log($"<color=yellow> 打开UI: {panelName} </color>");
            #endif

            if (string.IsNullOrEmpty(panelName))
            {
                Debug.LogError($"<color=red> 无法打开 这是一个空名称 </color>");
                return null;
            }

            if (!m_PanelCfgMap.TryGetValue(panelName, out var info))
            {
                Debug.LogError($"请检查 {panelName} 没有获取到PanelInfo  1. 必须继承IPanel 的才可行  2. 检查是否没有注册上");
                return null;
            }

            if (info.UIBasePanel == null)
            {
                if (PanelIsOpening(panelName))
                {
                    Debug.LogError($"请检查 {panelName} 正在异步打开中 请勿重复调用 请检查代码是否一瞬间频繁调用");
                    return null;
                }

                AddOpening(panelName);
                var uiBase = await YIUIFactory.CreatePanelAsync(info);
                RemovOpening(panelName);
                if (uiBase == null)
                {
                    Debug.LogError($"面板[{panelName}]没有创建成功，packName={info.PkgName}, resName={info.ResName}");
                    return null;
                }

                uiBase.SetActive(false);
                info.Reset(uiBase);
            }

            AddUI(info);

            return info;
        }

        public async UniTask<T> OpenPanelAsync<T>()
            where T : BasePanel, new()
        {
            var info = await OpenPanelStartAsync(GetPanelName<T>());
            if (info == null) return default;

            var success = false;

            await OpenPanelBefore(info);

            try
            {
                success = await info.UIBasePanel.Open();
            }
            catch (Exception e)
            {
                Debug.LogError($"panel={info.ResName}, err={e.Message}{e.StackTrace}");
            }

            return (T)await OpenPanelAfter(info, success);
        }

        public async UniTask<T> OpenPanelAsync<T, P1>(P1 p1)
            where T : BasePanel, IYIUIOpen<P1>, new()
        {
            var info = await OpenPanelStartAsync(GetPanelName<T>());
            if (info == null) return default;

            var success = false;

            await OpenPanelBefore(info);

            try
            {
                success = await info.UIBasePanel.Open(p1);
            }
            catch (Exception e)
            {
                Debug.LogError($"panel={info.ResName}, err={e.Message}{e.StackTrace}");
            }

            return (T)await OpenPanelAfter(info, success);
        }

        public async UniTask<T> OpenPanelAsync<T, P1, P2>(P1 p1, P2 p2)
            where T : BasePanel, IYIUIOpen<P1, P2>, new()
        {
            var info = await OpenPanelStartAsync(GetPanelName<T>());
            if (info == null) return default;

            var success = false;

            await OpenPanelBefore(info);

            try
            {
                success = await info.UIBasePanel.Open(p1, p2);
            }
            catch (Exception e)
            {
                Debug.LogError($"panel={info.ResName}, err={e.Message}{e.StackTrace}");
            }

            return (T)await OpenPanelAfter(info, success);
        }

        public async UniTask<T> OpenPanelAsync<T, P1, P2, P3>(P1 p1, P2 p2, P3 p3)
            where T : BasePanel, IYIUIOpen<P1, P2, P3>, new()
        {
            var info = await OpenPanelStartAsync(GetPanelName<T>());
            if (info == null) return default;

            var success = false;

            await OpenPanelBefore(info);

            try
            {
                success = await info.UIBasePanel.Open(p1, p2, p3);
            }
            catch (Exception e)
            {
                Debug.LogError($"panel={info.ResName}, err={e.Message}{e.StackTrace}");
            }

            return (T)await OpenPanelAfter(info, success);
        }

        public async UniTask<T> OpenPanelAsync<T, P1, P2, P3, P4>(P1 p1, P2 p2, P3 p3, P4 p4)
            where T : BasePanel, IYIUIOpen<P1, P2, P3, P4>, new()
        {
            var info = await OpenPanelStartAsync(GetPanelName<T>());
            if (info == null) return default;

            var success = false;

            await OpenPanelBefore(info);

            try
            {
                success = await info.UIBasePanel.Open(p1, p2, p3, p4);
            }
            catch (Exception e)
            {
                Debug.LogError($"panel={info.ResName}, err={e.Message}{e.StackTrace}");
            }

            return (T)await OpenPanelAfter(info, success);
        }

        public async UniTask<T> OpenPanelAsync<T, P1, P2, P3, P4, P5>(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5)
            where T : BasePanel, IYIUIOpen<P1, P2, P3, P4, P5>, new()
        {
            var info = await OpenPanelStartAsync(GetPanelName<T>());
            if (info == null) return default;

            var success = false;

            await OpenPanelBefore(info);

            try
            {
                success = await info.UIBasePanel.Open(p1, p2, p3, p4, p5);
            }
            catch (Exception e)
            {
                Debug.LogError($"panel={info.ResName}, err={e.Message}{e.StackTrace}");
            }

            return (T)await OpenPanelAfter(info, success);
        }
    }
}