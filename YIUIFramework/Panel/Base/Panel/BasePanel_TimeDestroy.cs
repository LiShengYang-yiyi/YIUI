using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace YIUIFramework
{
    /// <summary>
    /// 倒计时摧毁面板 适用于倒计时缓存界面
    /// </summary>
    public abstract partial class BasePanel
    {
        protected virtual float CachePanelTime => 10;

        private CancellationTokenSource m_Cts;

        internal void CacheTimeCountDownDestroyPanel()
        {
            StopCountDownDestroyPanel();
            m_Cts = new CancellationTokenSource();
            DoCountDownDestroyPanel(m_Cts.Token).Forget();
        }

        internal void StopCountDownDestroyPanel()
        {
            if (m_Cts == null) return;

            m_Cts.Cancel();
            m_Cts.Dispose();
            m_Cts = null;
        }

        /// <summary> 当关闭面板后倒计时结束销毁面板 </summary>
        private async UniTaskVoid DoCountDownDestroyPanel(CancellationToken token)
        {
            var cancelled = await UniTask.Delay(TimeSpan.FromSeconds(CachePanelTime), cancellationToken: token)
                .SuppressCancellationThrow();
            // 被取消力
            if (cancelled)
            {
                return;
            }

            m_Cts = null;
            UnityEngine.Object.Destroy(OwnerGameObject);
            PanelMgr.Inst.RemoveUIReset(UIResName);
        }
    }
}