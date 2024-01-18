using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using YIUIBind;

namespace YIUIFramework
{
    /// <summary>
    /// 部类 界面拆分数据
    /// </summary>
    public abstract partial class BasePanel
    {
        private UIPanelSplitData m_PanelSplitData;

        private Dictionary<string, BaseView> m_ExistView = new Dictionary<string, BaseView>();

        private Dictionary<string, RectTransform> m_ViewParent = new Dictionary<string, RectTransform>();

        private void InitPanelViewData()
        {
            m_ExistView.Clear();
            m_ViewParent.Clear();
            m_PanelSplitData = CDETable.PanelSplitData;
            CreateCommonView();
            AddViewParent(m_PanelSplitData.AllCommonView);
            AddViewParent(m_PanelSplitData.AllCreateView);
            AddViewParent(m_PanelSplitData.AllPopupView);
        }

        private void AddViewParent(List<RectTransform> listParent)
        {
            foreach (var parent in listParent)
            {
                var viewName = parent.name.Replace(UIStaticHelper.UIParentName, "");
                m_ViewParent.Add(viewName, parent);
            }
        }

        private void CreateCommonView()
        {
            foreach (var commonParentView in m_PanelSplitData.AllCommonView)
            {
                var viewName = commonParentView.name.Replace(UIStaticHelper.UIParentName, "");

                //通用view的名称是不允许修改的 如果修改了 那么就创建一个新的
                var viewTsf = commonParentView.FindChildByName(viewName);
                if (viewTsf == null)
                {
                    Debug.LogError($"{viewName} 当前通用View 不存在于父级下 所以无法自动创建 将会动态创建");
                    continue;
                }

                //查看本地是否已经创建
                var viewBase = CDETable.FindUIBase<UIBase>(viewName);

                //如果没有则通用重新创建
                viewBase ??= YIUIFactory.CreateCommon(UIPkgName, viewName, viewTsf.gameObject);
                switch (viewBase)
                {
                    case null:
                        continue;
                    case BaseView baseView:
                        m_ExistView.Add(viewName, baseView);
                        break;
                    default:
                        Debug.LogError($"{viewName} 不应该存在的错误 当前创建的View 不是BaseView");
                        break;
                }
            }
        }

        private RectTransform GetViewParent(string viewName)
        {
            m_ViewParent.TryGetValue(viewName, out var value);
            return value;
        }

        private async UniTask<T> GetView<T>() where T : BaseView, new()
        {
            var viewName = typeof(T).Name;
            var parent   = GetViewParent(viewName);
            if (parent == null)
            {
                Debug.LogError($"不存在这个View  请检查 {viewName}");
                return null;
            }

            if (m_ExistView.ContainsKey(viewName))
            {
                return (T)m_ExistView[viewName];
            }

            if (ViewIsOpening(viewName))
            {
                Debug.LogError($"请检查 {viewName} 正在异步打开中 请勿重复调用 请检查代码是否一瞬间频繁调用");
                return null;
            }

            AddOpening(viewName);
            var view = await YIUIFactory.InstantiateAsync<T>(parent);
            RemovOpening(viewName);

            m_ExistView.Add(viewName, view);

            return view;
        }

        /// <summary>
        /// 打开之前
        /// </summary>
        private async UniTask OpenViewBefore(BaseView view)
        {
            if (!view.WindowFirstOpen)
            {
                await CloseLastView(view);
            }
        }

        /// <summary>
        /// 打开之后
        /// </summary>
        private async UniTask OpenViewAfter(BaseView view, bool success)
        {
            if (success)
            {
                if (view.WindowFirstOpen)
                {
                    await CloseLastView(view);
                }
            }
            else
            {
                view.Close(false);
            }
        }

        /// <summary>
        /// 关闭上一个
        /// </summary>
        /// <param name="view">当前</param>
        private async UniTask CloseLastView(BaseView view)
        {
            //其他需要被忽略
            if (view.ViewWindowType != EViewWindowType.View)
            {
                return;
            }

            //View只有切换没有关闭
            var skipTween = view.WindowSkipOtherCloseTween;

            if (u_CurrentOpenView != null && u_CurrentOpenView != view)
            {
                //View 没有自动回退功能  比如AView 关闭 自动吧上一个BView 给打开 没有这种需求 也不能有这个需求
                //只能有 打开一个新View 上一个View的自动处理 99% 都是吧上一个隐藏即可
                //外部就只需要关心 打开 A B C 即可
                //因为这是View  不是 Panel
                switch (u_CurrentOpenView.StackOption)
                {
                    case EViewStackOption.None:
                        break;
                    case EViewStackOption.Visible:
                        u_CurrentOpenView.SetActive(false);
                        break;
                    case EViewStackOption.VisibleTween:
                        await u_CurrentOpenView.CloseAsync(!skipTween);
                        break;
                    default:
                        Debug.LogError($"新增类型未实现 {u_CurrentOpenView.StackOption}");
                        u_CurrentOpenView.SetActive(false);
                        break;
                }
            }

            u_CurrentOpenView = view;
        }
    }
}