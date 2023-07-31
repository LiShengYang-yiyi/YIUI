using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace YIUIFramework
{
    public abstract partial class BasePanel
    {
        protected async UniTask<BaseView> OpenViewAsync(string viewName)
        {
            var view = await GetView(viewName);
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

        protected async UniTask<BaseView> OpenViewAsync<P1>(string viewName, P1 p1)
        {
            var view = await GetView(viewName);
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

        protected async UniTask<BaseView> OpenViewAsync<P1, P2>(string viewName, P1 p1, P2 p2)
        {
            var view = await GetView(viewName);
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

        protected async UniTask<BaseView> OpenViewAsync<P1, P2, P3>(string viewName, P1 p1, P2 p2, P3 p3)
        {
            var view = await GetView(viewName);
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

        protected async UniTask<BaseView> OpenViewAsync<P1, P2, P3, P4>(string viewName, P1 p1, P2 p2, P3 p3, P4 p4)
        {
            var view = await GetView(viewName);
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

        protected async UniTask<BaseView> OpenViewAsync<P1, P2, P3, P4, P5>(
            string viewName, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5)
        {
            var view = await GetView(viewName);
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