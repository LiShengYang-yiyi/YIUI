using UnityEngine;
using YIUIFramework;

namespace ET.Client
{
    public static partial class YIUIMgrComponentSystem
    {
        #region 判断Panel是否关闭

        /*
         * 1. 如果Panel不存在，返回true 表示已关闭
         * 2. 如果存在,不在最前面 = 已关闭
         * 3. 如果存在,在最前面 = 未关闭
         * 因为Panel层同时只会显示一个 所以只要不是在前面就算关闭 哪怕你现在是显示状态
         */
        public static bool IsClose(this YIUIMgrComponent self, string panelName)
        {
            var info = self.GetPanelInfo(panelName);
            if (info.UIBase == null) return true;
            return self.IsClose(info.UIPanel);
        }

        public static bool IsClose<T>(this YIUIMgrComponent self) where T : Entity
        {
            var info = self.GetPanelInfo<T>();
            if (info.UIBase == null) return true;
            return self.IsClose(info.UIPanel);
        }

        public static bool IsClose(this YIUIMgrComponent self, YIUIPanelComponent panel)
        {
            var layerList = self.GetLayerPanelInfoList(EPanelLayer.Panel);
            if (layerList is not { Count: > 0 }) return true;
            var currentPanel = layerList[^1];
            if (currentPanel.UIPanel == null) return true;
            return currentPanel.UIPanel != panel;
        }

        #endregion

        /// <summary>
        /// 关闭一个窗口
        /// </summary>
        /// <param name="panelName">名称</param>
        /// <param name="tween">是否调用关闭动画</param>
        /// <param name="ignoreElse">忽略堆栈操作 -- 不要轻易忽略除非你明白 </param>
        /// <param name="ignoreLock">忽略锁 -- 不要轻易忽略除非你明白 </param>
        public static async ETTask<bool> ClosePanelAsync(this YIUIMgrComponent self, string panelName, bool tween = true, bool ignoreElse = false, bool ignoreLock = false)
        {
            if (YIUISingletonHelper.IsQuitting || self.IsDisposed) return true;

            #if YIUIMACRO_PANEL_OPENCLOSE
            Debug.Log($"<color=yellow> 关闭UI: {panelName} </color>");
            #endif

            self.m_PanelCfgMap.TryGetValue(panelName, out var info);
            if (info?.UIBase == null) return true; //没有也算成功关闭

            EntityRef<YIUIMgrComponent> selfRef = self;
            var coroutineLockCode = info.PanelLayer == EPanelLayer.Panel ? YIUIConstHelper.Const.UIProjectName.GetHashCode() : panelName.GetHashCode();

            using var _ = ignoreLock ? null : await self.Root().GetComponent<CoroutineLockComponent>()?.Wait(CoroutineLockType.YIUIPanel, coroutineLockCode);

            if (info.UIPanel == null) return true;

            self = selfRef;

            await EventSystem.Instance?.PublishAsync(self.Root(), new YIUIEventPanelCloseBefore
            {
                UIPkgName = info.PkgName, UIResName = info.ResName, UIComponentName = info.Name,
                PanelLayer = info.PanelLayer,
            });

            if (info.UIPanel.PanelOption.HasFlag(EPanelOption.DisClose))
            {
                var allowClose = false; //是否允许关闭

                //如果继承禁止关闭接口 可返回是否允许关闭自行处理
                if (info.OwnerUIEntity is IYIUIDisClose)
                {
                    allowClose = await YIUIEventSystem.DisClose(info.OwnerUIEntity);
                }

                if (!allowClose)
                {
                    Debug.LogError($"{panelName} 这个界面禁止被关闭 请检查");
                    return false;
                }
            }

            var successPanel = true;

            if (info.OwnerUIEntity is IYIUIClose)
            {
                successPanel = await YIUIEventSystem.Close(info.OwnerUIEntity);
            }

            if (info.UIWindow is { WindowCloseTweenBefore: true })
            {
                await YIUIEventSystem.WindowClose(info.UIWindow, successPanel);
            }

            if (!successPanel)
            {
                Log.Info($"<color=yellow> 关闭事件返回不允许关闭Panel UI: {panelName} </color>");
                return false;
            }

            var isCloseTriggerTween = info.UIWindow is { WindowCloseTriggerTween: true };

            var ignoreTween = false;

            if (!isCloseTriggerTween)
            {
                self = selfRef;
                ignoreTween = self.IsClose(info.UIPanel);
            }

            if (!ignoreTween && info.UIWindow is { WindowLastClose: false })
            {
                await info.UIPanel.CloseAllView(tween);
                await info.UIWindow.InternalOnWindowCloseTween(tween);
            }

            if (!ignoreTween && !ignoreElse)
            {
                self = selfRef;
                await self.RemoveUIAddElse(info);
            }

            if (!ignoreTween && info.UIWindow is { WindowLastClose: true })
            {
                await info.UIPanel.CloseAllView(tween);
                await info.UIWindow.InternalOnWindowCloseTween(tween);
            }

            if (info.UIWindow is { WindowCloseTweenBefore: false })
            {
                await YIUIEventSystem.WindowClose(info.UIWindow, true);
            }

            self = selfRef;
            self.RemoveUI(info);

            return true;
        }

        /// <summary>
        /// 关闭一个窗口 (被直接摧毁的)
        /// 注意被直接摧毁的Panel 无法触发动画 回退等异步操作
        /// </summary>
        internal static bool DestroyPanel(this YIUIMgrComponent self, string panelName)
        {
            if (YIUISingletonHelper.IsQuitting || self.IsDisposed) return true;

            if (!self.m_PanelCfgMap.TryGetValue(panelName, out var info)) return false;

            if (!self.ContainsLayerPanelInfo(info.PanelLayer, info)) return false;

            #if YIUIMACRO_PANEL_OPENCLOSE
            Debug.Log($"<color=yellow> 关闭一个窗口(被直接摧毁的): {panelName} </color>");
            #endif

            EventSystem.Instance?.Publish(self.Root(),
                new YIUIEventPanelCloseBefore
                {
                    UIPkgName = info.PkgName, UIResName = info.ResName, UIComponentName = info.Name,
                    PanelLayer = info.PanelLayer,
                });

            self.DestroyRemoveUI(info);

            return true;
        }

        public static void ClosePanel(this YIUIMgrComponent self, string panelName, bool tween = true, bool ignoreElse = false, bool ignoreLock = false)
        {
            self.ClosePanelAsync(panelName, tween, ignoreElse, ignoreLock).NoContext();
        }

        /// <summary>
        /// 关闭一个窗口
        /// 异步等待关闭动画
        /// </summary>
        public static async ETTask<bool> ClosePanelAsync<T>(this YIUIMgrComponent self, bool tween = true, bool ignoreElse = false, bool ignoreLock = false) where T : Entity
        {
            return await self.ClosePanelAsync(self.GetPanelName<T>(), tween, ignoreElse, ignoreLock);
        }

        /// <summary>
        /// 同步关闭窗口
        /// 无法等待关闭动画
        /// </summary>
        public static void ClosePanel<T>(this YIUIMgrComponent self, bool tween = true, bool ignoreElse = false, bool ignoreLock = false) where T : Entity
        {
            self.ClosePanelAsync(self.GetPanelName<T>(), tween, ignoreElse, ignoreLock).NoContext();
        }

        /// <summary>
        /// 回到指定的界面 其他界面全部关闭
        /// </summary>
        /// <param name="homeName">需要被打开的界面 且这个UI是存在的 否则无法打开</param>
        /// <param name="tween">动画</param>
        /// <param name="forceHome">如果不存在则 强制打开 被强制打开的无法触发Back Home消息 只会触发常规的open close</param>
        public static async ETTask<bool> HomePanel(this YIUIMgrComponent self, string homeName, bool tween = true, Scene forceHome = null)
        {
            #if YIUIMACRO_PANEL_OPENCLOSE
            Debug.Log($"<color=yellow> Home关闭其他所有Panel UI: {homeName} </color>");
            #endif

            self.m_PanelCfgMap.TryGetValue(homeName, out var homeInfo);
            if (homeInfo?.UIBase != null)
            {
                return await self.RemoveUIToHome(homeInfo, tween);
            }
            else
            {
                if (forceHome != null)
                {
                    EntityRef<Scene> rootRef = forceHome;
                    await self.CloseAll(EPanelLayer.Panel, EPanelOption.IgnoreClose, tween);
                    return await EventSystem.Instance?.YIUIInvokeEntityAsync<YIUIInvokeEntity_SceneOpenPanel, ETTask<bool>>(rootRef, new YIUIInvokeEntity_SceneOpenPanel
                    {
                        PanelName = homeName
                    });
                }
            }

            return false;
        }

        public static async ETTask<bool> HomePanel<T>(this YIUIMgrComponent self, bool tween = true, Scene forceHome = null) where T : Entity
        {
            return await self.HomePanel(typeof(T).Name, tween, forceHome);
        }
    }
}