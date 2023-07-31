using YIUIFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace YIUIFramework
{
    public abstract partial class BasePanel
    {
        private async UniTask<BaseView> GetView(Type viewType)
        {
            var viewName = viewType.Name;
            return await GetView(viewName);
        }

        protected async UniTask<BaseView> OpenViewAsync(Type viewType)
        {
            var view = await GetView(viewType);
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

        protected async UniTask<BaseView> OpenViewAsync<P1>(Type viewType, P1 p1)
        {
            var view = await GetView(viewType);
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

        protected async UniTask<BaseView> OpenViewAsync<P1, P2>(Type viewType, P1 p1, P2 p2)
        {
            var view = await GetView(viewType);
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

        protected async UniTask<BaseView> OpenViewAsync<P1, P2, P3>(Type viewType, P1 p1, P2 p2, P3 p3)
        {
            var view = await GetView(viewType);
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

        protected async UniTask<BaseView> OpenViewAsync<P1, P2, P3, P4>(Type viewType, P1 p1, P2 p2, P3 p3, P4 p4)
        {
            var view = await GetView(viewType);
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
            Type viewType, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5)
        {
            var view = await GetView(viewType);
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