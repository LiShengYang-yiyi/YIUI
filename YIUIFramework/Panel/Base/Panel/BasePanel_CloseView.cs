using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace YIUIFramework
{
    public abstract partial class BasePanel
    {
        public void CloseView<T>(bool tween = true)
            where T : BaseView
        {
            CloseViewAsync<T>(tween).Forget();
        }

        public void CloseView(string resName, bool tween = true)
        {
            CloseViewAsync(resName, tween).Forget();
        }

        public async UniTask<bool> CloseViewAsync<TView>(bool tween = true)
            where TView : BaseView
        {
            var (exist, entity) = ExistView<TView>();
            if (!exist) return false;
            return await CloseViewAsync(entity, tween);
        }

        public async UniTask<bool> CloseViewAsync(string resName, bool tween = true)
        {
            var (exist, entity) = ExistView(resName);
            if (!exist) return false;
            return await CloseViewAsync(entity, tween);
        }

        private async UniTask<bool> CloseViewAsync(BaseView view, bool tween)
        {
            if (view == null) return false;
            await view.CloseAsync(tween);
            return true;
        }
    }
}
