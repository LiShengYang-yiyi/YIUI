using Cysharp.Threading.Tasks;

namespace YIUIFramework
{
    /// <summary>
    /// 关闭
    /// </summary>
    public abstract partial class BasePanel
    {
        public void Close(bool tween = true, bool ignoreElse = false)
        {
            CloseAsync(tween, ignoreElse).Forget();
        }

        public async UniTask CloseAsync(bool tween = true, bool ignoreElse = false)
        {
            await m_PanelMgr.ClosePanelAsync(UIResName, tween, ignoreElse);
        }

        protected void Home<T>(bool tween = true) where T : BasePanel
        {
            m_PanelMgr.HomePanel<T>(tween).Forget();
        }

        protected void Home(string homeName, bool tween = true)
        {
            m_PanelMgr.HomePanel(homeName, tween).Forget();
        }
    }
}