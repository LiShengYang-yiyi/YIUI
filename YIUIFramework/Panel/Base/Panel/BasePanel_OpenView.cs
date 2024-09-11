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

        /// <summary> 已经存在的视图 </summary>
        private Dictionary<string, BaseView> m_ExistView = new Dictionary<string, BaseView>();

        /// <summary> 所有视图对应的父级映射 </summary>
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

        /// <summary> 创建公共View窗口 </summary>
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

                //通用创建 这个时候通用UI一定是没有创建的 否则就有问题
                var viewBase = YIUIFactory.CreateCommon(UIPkgName, viewName, viewTsf.gameObject);
                if (viewBase == null)continue;

                viewTsf.gameObject.SetActive(false);

                switch (viewBase)
                {
                    case BaseView baseView:
                        m_ExistView.Add(viewName, baseView);
                        break;
                    default:
                        Debug.LogError($"{viewName} 不应该存在的错误 当前创建的View 不是BaseView");
                        break;
                }
            }
        }

        /// <summary> 添加视图父级 </summary>
        private void AddViewParent(List<RectTransform> listParent)
        {
            foreach (var parent in listParent)
            {
                var viewName = parent.name.Replace(UIStaticHelper.UIParentName, "");
                m_ViewParent.Add(viewName, parent);
            }
        }
        
        private RectTransform GetViewParent(string viewName)
        {
            m_ViewParent.TryGetValue(viewName, out var value);
            return value;
        }

        private async UniTask<T> GetView<T>() where T : BaseView
        {
            var viewName = typeof(T).Name;
            var parent   = GetViewParent(viewName);
            if (parent == null)
            {
                Debug.LogError($"不存在这个View  请检查 {viewName}");
                return null;
            }

            using var asyncLock = await AsyncLockMgr.Inst.Wait(viewName.GetHashCode());

            if (m_ExistView.TryGetValue(viewName, out var value))
            {
                return (T)value;
            }

            var view = await YIUIFactory.InstantiateAsync<T>(parent);

            m_ExistView.Add(viewName, view);

            return view;
        }

        public (bool, BaseView) ExistView<T>() where T : BaseView
        {
            var data = UIBindHelper.GetBindVoByType<T>();
            return data == null ? (false, null) : ExistView(data.Value.ResName);
        }

        public (bool, BaseView) ExistView(string viewName)
        {
            var viewParent = GetViewParent(viewName);
            if (viewParent == null)
            {
                Debug.LogError($"不存在这个View  请检查 {viewName}");
                return (false, null);
            }

            if (m_ExistView.TryGetValue(viewName, out var baseView))
            {
                return (true, baseView);
            }

            return (false, null);
        }

        /// <summary>
        /// 打开之前
        /// </summary>
        private async UniTask OpenViewBefore(BaseView view)
        {
            // 如果窗口选项不是先开，则先关闭上一个窗口
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
                // 如果窗口选项为先开，则在打开完关闭上一个窗口
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
        /// <param name="nextView">接下来要打开的视图</param>
        private async UniTask CloseLastView(BaseView nextView)
        {
            //其他需要被忽略
            if (nextView.ViewWindowType != EViewWindowType.View)
            {
                return;
            }

            //View只有切换没有关闭
            var skipTween = nextView.WindowSkipOtherCloseTween;

            if (u_CurrentOpenView != null && u_CurrentOpenView != nextView)
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

            u_CurrentOpenView = nextView;
        }
    }
}