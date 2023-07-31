using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace YIUIFramework
{
    //打开泛型 异步
    public abstract partial class BasePanel
    {
        protected async UniTask<T> OpenViewAsync<T>()
            where T : BaseView, new()
        {
            var view = await GetView<T>();
            if (view == null) return default;

            var success = false;

            await OpenViewBefore(view);

            try
            {
                success = await view.Open();
            }
            catch (Exception e)
            {
                Debug.LogError($"ResName={view.UIResName}, err={e.Message}{e.StackTrace}");
            }

            await OpenViewAfter(view, success);

            return view;
        }

        protected async UniTask<T> OpenViewAsync<T, P1>(P1 p1)
            where T : BaseView, IYIUIOpen<P1>, new()
        {
            var view = await GetView<T>();
            if (view == null) return default;

            var success = false;

            await OpenViewBefore(view);

            try
            {
                success = await view.Open(p1);
            }
            catch (Exception e)
            {
                Debug.LogError($"ResName={view.UIResName}, err={e.Message}{e.StackTrace}");
            }

            await OpenViewAfter(view, success);

            return view;
        }

        protected async UniTask<T> OpenViewAsync<T, P1, P2>(P1 p1, P2 p2)
            where T : BaseView, IYIUIOpen<P1, P2>, new()
        {
            var view = await GetView<T>();
            if (view == null) return default;

            var success = false;

            await OpenViewBefore(view);

            try
            {
                success = await view.Open(p1, p2);
            }
            catch (Exception e)
            {
                Debug.LogError($"ResName={view.UIResName}, err={e.Message}{e.StackTrace}");
            }

            await OpenViewAfter(view, success);

            return view;
        }

        protected async UniTask<T> OpenViewAsync<T, P1, P2, P3>(P1 p1, P2 p2, P3 p3)
            where T : BaseView, IYIUIOpen<P1, P2, P3>, new()
        {
            var view = await GetView<T>();
            if (view == null) return default;

            var success = false;

            await OpenViewBefore(view);

            try
            {
                success = await view.Open(p1, p2, p3);
            }
            catch (Exception e)
            {
                Debug.LogError($"ResName={view.UIResName}, err={e.Message}{e.StackTrace}");
            }

            await OpenViewAfter(view, success);

            return view;
        }

        protected async UniTask<T> OpenViewAsync<T, P1, P2, P3, P4>(P1 p1, P2 p2, P3 p3, P4 p4)
            where T : BaseView, IYIUIOpen<P1, P2, P3, P4>, new()
        {
            var view = await GetView<T>();
            if (view == null) return default;

            var success = false;

            await OpenViewBefore(view);

            try
            {
                success = await view.Open(p1, p2, p3, p4);
            }
            catch (Exception e)
            {
                Debug.LogError($"ResName={view.UIResName}, err={e.Message}{e.StackTrace}");
            }

            await OpenViewAfter(view, success);

            return view;
        }

        protected async UniTask<T> OpenViewAsync<T, P1, P2, P3, P4, P5>(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5)
            where T : BaseView, IYIUIOpen<P1, P2, P3, P4, P5>, new()
        {
            var view = await GetView<T>();
            if (view == null) return default;

            var success = false;

            await OpenViewBefore(view);

            try
            {
                success = await view.Open(p1, p2, p3, p4, p5);
            }
            catch (Exception e)
            {
                Debug.LogError($"ResName={view.UIResName}, err={e.Message}{e.StackTrace}");
            }

            await OpenViewAfter(view, success);

            return view;
        }
    }
}