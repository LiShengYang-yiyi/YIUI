using Cysharp.Threading.Tasks;

namespace YIUIFramework
{
    public partial class BaseView
    {
        public void Close(bool tween = true, bool ignoreElse = false)
        {
            CloseAsync(tween).Forget();
        }

        public async UniTask CloseAsync(bool tween = true)
        {
            await InternalOnWindowCloseTween(tween);
            OnClose();
        }
    }
}