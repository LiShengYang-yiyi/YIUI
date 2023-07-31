using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace YIUIFramework
{
    /// <summary>
    /// 界面回退功能 关闭 / 恢复 / Home
    /// </summary>
    public partial class PanelMgr
    {
        /// <summary>
        /// 打开一个同级UI时关闭其他UI 
        /// 只有Panel层才有这个逻辑
        /// </summary>
        private async UniTask AddUICloseElse(PanelInfo info)
        {
            if (!(info.UIBasePanel is { Layer: EPanelLayer.Panel }))
            {
                return;
            }

            if (info.UIBasePanel.PanelIgnoreBack)
            {
                return;
            }

            var layerList = GetLayerPanelInfoList(EPanelLayer.Panel);
            var skipTween = info.UIBasePanel.WindowSkipOtherCloseTween;

            for (var i = layerList.Count - 1; i >= 0; i--)
            {
                var child = layerList[i];

                if (child == info)
                {
                    continue;
                }

                if (child.UIBasePanel is IYIUIBack back)
                {
                    back.DoBackClose(info);
                }

                switch (child.UIBasePanel.StackOption)
                {
                    case EPanelStackOption.Omit:
                        if (skipTween)
                            child.UIBasePanel.Close(true, true);
                        else
                            await child.UIBasePanel.CloseAsync(true, true);
                        break;
                    case EPanelStackOption.None:
                        break;
                    case EPanelStackOption.Visible:
                        child.UIBasePanel.SetActive(false);
                        break;
                    case EPanelStackOption.VisibleTween:
                        if (!skipTween)
                            await child.UIBasePanel.InternalOnWindowCloseTween();
                        child.UIBasePanel.SetActive(false);
                        break;
                    default:
                        Debug.LogError($"新增类型未实现 {child.UIBasePanel.StackOption}");
                        child.UIBasePanel.SetActive(false);
                        break;
                }
            }
        }

        private async UniTask RemoveUIAddElse(PanelInfo info)
        {
            if (!(info.UIBasePanel is { Layer: EPanelLayer.Panel }))
            {
                return;
            }

            if (info.UIBasePanel.PanelIgnoreBack)
            {
                return;
            }

            var layerList = GetLayerPanelInfoList(EPanelLayer.Panel);
            var skipTween = info.UIBasePanel.WindowSkipOtherOpenTween;

            for (var i = layerList.Count - 1; i >= 0; i--)
            {
                var child = layerList[i];

                if (child == info)
                {
                    continue;
                }

                if (child.UIBasePanel is IYIUIBack back)
                {
                    back.DoBackAdd(info);
                }

                var isBreak = true;
                switch (child.UIBasePanel.StackOption)
                {
                    case EPanelStackOption.Omit: //不可能进入这里因为他已经被关闭了 如果进入则跳过这个界面
                        isBreak = false;
                        break;
                    case EPanelStackOption.None:
                        break;
                    case EPanelStackOption.Visible:
                        child.UIBasePanel.SetActive(true);
                        break;
                    case EPanelStackOption.VisibleTween:
                        child.UIBasePanel.SetActive(true);
                        if (!skipTween)
                            await child.UIBasePanel.InternalOnWindowOpenTween();
                        break;
                    default:
                        Debug.LogError($"新增类型未实现 {child.UIBasePanel.StackOption}");
                        child.UIBasePanel.SetActive(true);
                        break;
                }

                if (isBreak)
                    break;
            }
        }

        private async UniTask RemoveUIToHome(PanelInfo home, bool tween = true)
        {
            if (!(home.UIBasePanel is { Layer: EPanelLayer.Panel }))
            {
                return;
            }

            var layerList           = GetLayerPanelInfoList(EPanelLayer.Panel);
            var skipOtherCloseTween = home.UIBasePanel.WindowSkipOtherCloseTween;
            var skipHomeOpenTween   = home.UIBasePanel.WindowSkipHomeOpenTween;

            for (var i = layerList.Count - 1; i >= 0; i--)
            {
                var child = layerList[i];

                if (child != home)
                {
                    if (child.UIBasePanel is IYIUIBack back)
                    {
                        back.DoBackHome(home);
                    }

                    if (skipOtherCloseTween)
                    {
                        ClosePanel(child.Name, false, true);
                    }
                    else
                    {
                        await ClosePanelAsync(child.Name, tween, true);
                    }

                    continue;
                }

                switch (child.UIBasePanel.StackOption)
                {
                    case EPanelStackOption.Omit:
                    case EPanelStackOption.None:
                    case EPanelStackOption.Visible:
                        child.UIBasePanel.SetActive(true);
                        break;
                    case EPanelStackOption.VisibleTween:
                        child.UIBasePanel.SetActive(true);
                        if (tween && !skipHomeOpenTween)
                            await child.UIBasePanel.InternalOnWindowOpenTween();
                        break;
                    default:
                        Debug.LogError($"新增类型未实现 {child.UIBasePanel.StackOption}");
                        child.UIBasePanel.SetActive(true);
                        break;
                }
            }
        }
    }
}