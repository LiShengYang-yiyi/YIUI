using UnityEngine;
using YIUIFramework;

namespace ET.Client
{
    /// <summary>
    /// 界面回退功能 关闭 / 恢复 / Home
    /// </summary>
    public partial class YIUIMgrComponent
    {
        /// <summary>
        /// 打开一个同级UI时关闭其他UI 
        /// 只有Panel层才有这个逻辑
        /// </summary>
        internal async ETTask AddUICloseElse(PanelInfo info)
        {
            if (!(info.UIPanel is { Layer: EPanelLayer.Panel }))
            {
                return;
            }

            if (info.UIPanel.PanelIgnoreBack)
            {
                return;
            }

            var layerList = this.GetLayerPanelInfoList(EPanelLayer.Panel);
            var skipTween = info.UIWindow.WindowSkipOtherCloseTween;

            for (var i = layerList.Count - 1; i >= 0; i--)
            {
                var child = layerList[i];

                if (child == info)
                {
                    continue;
                }

                //防止多级时多次触发
                switch (child.UIPanel.StackOption)
                {
                    case EPanelStackOption.Visible:
                        if (!child.UIBase.ActiveSelf) 
                            continue;
                        break;
                    case EPanelStackOption.VisibleTween:
                        if (!child.UIBase.ActiveSelf) 
                            continue;
                        break;
                    case EPanelStackOption.None:
                            continue;
                    case EPanelStackOption.Omit: //此类型表示 都忽略判断
                        break;
                    default:
                        Debug.LogError($"新增类型未实现 {child.UIPanel.StackOption}");
                        if (!child.UIBase.ActiveSelf) 
                            continue;
                        break;
                }

                EventSystem.Instance.Publish(this.Root(), new YIUIEventPanelCloseBefore
                {
                    UIPkgName       = child.PkgName,
                    UIResName       = child.ResName,
                    UIComponentName = child.Name,
                    StackOption     = true,
                    PanelLayer      = child.PanelLayer,
                });

                if (child.OwnerUIEntity is IYIUIBackClose)
                {
                    await YIUIEventSystem.BackClose(child.OwnerUIEntity,new YIUIPanelInfo
                    {
                        UIPkgName = info.PkgName,
                        UIResName = info.ResName,
                        UIComponentName = info.Name,
                        PanelLayer = info.PanelLayer,
                    });
                }

                switch (child.UIPanel.StackOption)
                {
                    case EPanelStackOption.Omit:
                        if (skipTween)
                            child.UIPanel.Close(true, true);
                        else
                            await child.UIPanel.CloseAsync(true, true);
                        break;
                    case EPanelStackOption.None:
                        break;
                    case EPanelStackOption.Visible:
                        child.UIBase.SetActive(false);
                        break;
                    case EPanelStackOption.VisibleTween:
                        if (!skipTween)
                            await child.UIWindow.InternalOnWindowCloseTween();
                        child.UIBase.SetActive(false);
                        break;
                    default:
                        Debug.LogError($"新增类型未实现 {child.UIPanel.StackOption}");
                        child.UIBase.SetActive(false);
                        break;
                }

                EventSystem.Instance.Publish(this.Root(), new YIUIEventPanelCloseAfter
                {
                    UIPkgName       = child.PkgName,
                    UIResName       = child.ResName,
                    UIComponentName = child.Name,
                    StackOption     = true,
                    PanelLayer      = child.PanelLayer,
                });
            }
        }

        internal async ETTask RemoveUIAddElse(PanelInfo info)
        {
            if (!(info.UIPanel is { Layer: EPanelLayer.Panel }))
            {
                return;
            }

            if (info.UIPanel.PanelIgnoreBack)
            {
                return;
            }

            var layerList = this.GetLayerPanelInfoList(EPanelLayer.Panel);
            var skipTween = info.UIWindow.WindowSkipOtherOpenTween;

            for (var i = layerList.Count - 1; i >= 0; i--)
            {
                var child = layerList[i];

                if (child == info)
                {
                    continue;
                }

                EventSystem.Instance.Publish(this.Root(), new YIUIEventPanelOpenBefore
                {
                    UIPkgName       = child.PkgName,
                    UIResName       = child.ResName,
                    UIComponentName = child.Name,
                    StackOption     = true,
                    PanelLayer      = child.PanelLayer,
                });

                var isBreak = true;
                switch (child.UIPanel.StackOption)
                {
                    case EPanelStackOption.Omit: //不可能进入这里因为他已经被关闭了 如果进入则跳过这个界面
                        isBreak = false;
                        break;
                    case EPanelStackOption.None:
                        break;
                    case EPanelStackOption.Visible:
                        child.UIBase.SetActive(true);
                        break;
                    case EPanelStackOption.VisibleTween:
                        child.UIBase.SetActive(true);
                        if (!skipTween)
                            await child.UIWindow.InternalOnWindowOpenTween();
                        break;
                    default:
                        Debug.LogError($"新增类型未实现 {child.UIPanel.StackOption}");
                        child.UIBase.SetActive(true);
                        break;
                }

                if (child.OwnerUIEntity is IYIUIBackOpen)
                {
                    await YIUIEventSystem.BackOpen(child.OwnerUIEntity,new YIUIPanelInfo
                    {
                        UIPkgName = info.PkgName,
                        UIResName = info.ResName,
                        UIComponentName = info.Name,
                        PanelLayer = info.PanelLayer,
                    });
                }

                EventSystem.Instance.Publish(this.Root(), new YIUIEventPanelOpenAfter
                {
                    UIPkgName       = child.PkgName,
                    UIResName       = child.ResName,
                    UIComponentName = child.Name,
                    StackOption     = true,
                    PanelLayer      = child.PanelLayer,
                });

                if (isBreak)
                    break;
            }
        }

        internal async ETTask<bool> RemoveUIToHome(PanelInfo home, bool tween = true)
        {
            if (!(home.UIPanel is { Layer: EPanelLayer.Panel }))
            {
                return false; //home的UI必须在panel层
            }

            var layerList           = this.GetLayerPanelInfoList(EPanelLayer.Panel);
            var skipOtherCloseTween = home.UIWindow.WindowSkipOtherCloseTween;
            var skipHomeOpenTween   = home.UIWindow.WindowSkipHomeOpenTween;
            var skipHomeBack   = home.UIWindow.WindowSkipHomeBack;

            for (var i = layerList.Count - 1; i >= 0; i--)
            {
                var child = layerList[i];

                if (child != home)
                {
                    EventSystem.Instance.Publish(this.Root(), new YIUIEventPanelCloseBefore
                    {
                        UIPkgName       = child.PkgName,
                        UIResName       = child.ResName,
                        UIComponentName = child.Name,
                        StackOption     = true,
                        PanelLayer      = child.PanelLayer,
                    });

                    if (!skipHomeBack && child.OwnerUIEntity is IYIUIBackClose)
                    {
                        await YIUIEventSystem.BackClose(child.OwnerUIEntity,new YIUIPanelInfo
                        {
                            UIPkgName = home.PkgName,
                            UIResName = home.ResName,
                            UIComponentName = home.Name,
                            PanelLayer = home.PanelLayer,
                        });
                    }

                    if (child.OwnerUIEntity is IYIUIBackHomeClose)
                    {
                        await YIUIEventSystem.BackHomeClose(child.OwnerUIEntity,new YIUIPanelInfo
                        {
                            UIPkgName = home.PkgName,
                            UIResName = home.ResName,
                            UIComponentName = home.Name,
                            PanelLayer = home.PanelLayer,
                        });
                    }

                    if (skipOtherCloseTween)
                    {
                        this.ClosePanel(child.Name, false, true);
                    }
                    else
                    {
                        var success = await this.ClosePanelAsync(child.Name, tween, true);
                        if (!success)
                        {
                            return false;
                        }
                    }

                    EventSystem.Instance.Publish(this.Root(), new YIUIEventPanelCloseAfter
                    {
                        UIPkgName       = child.PkgName,
                        UIResName       = child.ResName,
                        UIComponentName = child.Name,
                        StackOption     = true,
                        PanelLayer      = child.PanelLayer,
                    });

                    continue;
                }

                EventSystem.Instance.Publish(this.Root(), new YIUIEventPanelOpenBefore
                {
                    UIPkgName       = child.PkgName,
                    UIResName       = child.ResName,
                    UIComponentName = child.Name,
                    StackOption     = true,
                    PanelLayer      = child.PanelLayer,
                });

                switch (child.UIPanel.StackOption)
                {
                    case EPanelStackOption.Omit:
                    case EPanelStackOption.None:
                    case EPanelStackOption.Visible:
                        child.UIBase.SetActive(true);
                        break;
                    case EPanelStackOption.VisibleTween:
                        child.UIBase.SetActive(true);
                        if (tween && !skipHomeOpenTween)
                            await child.UIWindow.InternalOnWindowOpenTween();
                        break;
                    default:
                        Debug.LogError($"新增类型未实现 {child.UIPanel.StackOption}");
                        child.UIBase.SetActive(true);
                        break;
                }

                if (!skipHomeBack && child.OwnerUIEntity is IYIUIBackOpen)
                {
                    await YIUIEventSystem.BackOpen(child.OwnerUIEntity,new YIUIPanelInfo
                    {
                        UIPkgName = child.PkgName,
                        UIResName = child.ResName,
                        UIComponentName = child.Name,
                        PanelLayer = child.PanelLayer,
                    });
                }

                if (child.OwnerUIEntity is IYIUIBackHomeOpen)
                {
                    await YIUIEventSystem.BackHomeOpen(child.OwnerUIEntity);
                }

                EventSystem.Instance.Publish(this.Root(), new YIUIEventPanelOpenAfter
                {
                    UIPkgName       = child.PkgName,
                    UIResName       = child.ResName,
                    UIComponentName = child.Name,
                    StackOption     = true,
                    PanelLayer      = child.PanelLayer,
                });
            }

            return true;
        }
    }
}