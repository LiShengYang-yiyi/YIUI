using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace YIUIFramework
{
    /// <summary>
    /// 部类 界面拆分数据
    /// </summary>
    public abstract partial class BasePanel
    {
        private async UniTask<BaseView> GetView(string viewName)
        {
            var parent = GetViewParent(viewName);
            if (parent == null)
            {
                Debug.LogError($"不存在这个View  请检查 {viewName}");
                return null;
            }

            if (m_ExistView.ContainsKey(viewName))
            {
                return m_ExistView[viewName];
            }

            var value = UIBindHelper.GetBindVoByPath(UIPkgName, viewName);
            if (value == null) return null;
            var bindVo = value.Value;

            if (ViewIsOpening(viewName))
            {
                Debug.LogError($"请检查 {viewName} 正在异步打开中 请勿重复调用 请检查代码是否一瞬间频繁调用");
                return null;
            }

            AddOpening(viewName);
            var view = (BaseView)await YIUIFactory.InstantiateAsync(bindVo, parent);
            RemovOpening(viewName);

            m_ExistView.Add(viewName, view);

            return view;
        }

        #region 打开泛型

        protected async UniTask<BaseView> OpenViewAsync(string viewName, object param = null)
        {
            var view = await GetView(viewName);
            if (view == null) return null;

            var success = false;

            await OpenViewBefore(view);

            try
            {
                var p = ParamVo.Get(param);
                success = await view.Open(p);
                ParamVo.Put(p);
            }
            catch (Exception e)
            {
                Debug.LogError($"ResName={view.UIResName}, err={e.Message}{e.StackTrace}");
            }

            await OpenViewAfter(view, success);

            return view;
        }

        protected async UniTask<BaseView> OpenViewAsync(string viewName, object param1, object param2)
        {
            var paramList = ListPool<object>.Get();
            paramList.Add(param1);
            paramList.Add(param2);
            var view = await OpenViewAsync(viewName, paramList);
            ListPool<object>.Put(paramList);
            return view;
        }

        protected async UniTask<BaseView> OpenViewAsync(string viewName, object param1, object param2, object param3)
        {
            var paramList = ListPool<object>.Get();
            paramList.Add(param1);
            paramList.Add(param2);
            paramList.Add(param3);
            var view = await OpenViewAsync(viewName, paramList);
            ListPool<object>.Put(paramList);
            return view;
        }

        protected async UniTask<BaseView> OpenViewAsync(string viewName, object param1, object param2, object param3,
                                                        object param4)
        {
            var paramList = ListPool<object>.Get();
            paramList.Add(param1);
            paramList.Add(param2);
            paramList.Add(param3);
            paramList.Add(param4);
            var view = await OpenViewAsync(viewName, paramList);
            ListPool<object>.Put(paramList);
            return view;
        }

        protected async UniTask<BaseView> OpenViewAsync(string viewName, object param1, object param2, object param3,
                                                        object param4,   params object[] paramMore)
        {
            var paramList = ListPool<object>.Get();
            paramList.Add(param1);
            paramList.Add(param2);
            paramList.Add(param3);
            paramList.Add(param4);
            if (paramMore.Length > 0)
            {
                paramList.AddRange(paramMore);
            }

            var view = await OpenViewAsync(viewName, paramList);
            ListPool<object>.Put(paramList);
            return view;
        }

        #endregion
    }
}