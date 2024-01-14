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

        private async UniTaskVoid DoCountDownDestroyPanel(CancellationToken token)
        {
            try
            {
                await UniTask.Delay(TimeSpan.FromSeconds(CachePanelTime), cancellationToken: token);
            }
            catch (OperationCanceledException e)
            {
                //取消倒计时 正常操作不需要日志
                return;
            }

            UnityEngine.Object.Destroy(OwnerGameObject);
            PanelMgr.Inst.RemoveUIReset(UIResName);
        }
    }
}