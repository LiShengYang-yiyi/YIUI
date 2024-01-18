using Cysharp.Threading.Tasks;

namespace YIUIFramework
{
    //动画
    public abstract partial class BaseWindow
    {
        private bool m_FirstOpenTween;

        internal async UniTask InternalOnWindowOpenTween(bool tween = true)
        {
            OnOpenTweenStart();

            if (tween && !WindowBanRepetitionOpenTween || !m_FirstOpenTween)
            {
                m_FirstOpenTween = true;

                if (WindowBanAwaitOpenTween)
                {
                    SealedOnWindowOpenTween().Forget();
                }
                else
                {
                    await SealedOnWindowOpenTween();
                }
            }
            else
            {
                OnOpenTweenEnd();
            }
        }

        private bool m_FirstCloseTween;

        internal async UniTask InternalOnWindowCloseTween(bool tween = true)
        {
            OnCloseTweenStart();

            if (tween && !WindowBanRepetitionCloseTween || !m_FirstCloseTween)
            {
                m_FirstCloseTween = true;

                if (WindowBanAwaitCloseTween)
                {
                    SealedOnWindowCloseTween().Forget();
                }
                else
                {
                    await SealedOnWindowCloseTween();
                }
            }
            else
            {
                OnCloseTweenEnd();
            }
        }

        //由子类实现密封Async 调用End
        protected abstract UniTask SealedOnWindowOpenTween();

        //任何UI可重写打开与关闭动画
        protected abstract UniTask OnOpenTween();

        //有可能没有动画 也有可能动画被跳过 反正无论如何都会有动画结束回调
        protected abstract void OnOpenTweenStart();
        protected abstract void OnOpenTweenEnd();

        protected abstract UniTask SealedOnWindowCloseTween();
        protected abstract UniTask OnCloseTween();

        protected abstract void OnCloseTweenStart();
        protected abstract void OnCloseTweenEnd();
    }
}