﻿using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace YIUIFramework
{
    /// <summary>
    ///   打开相关
    /// </summary>
    public partial class PanelMgr
    {
        /// <summary>
        /// 所有已经打开过的UI
        /// K = C#文件名
        /// 主要是作为缓存PanelInfo
        /// </summary>
        private Dictionary<string, PanelInfo> m_PanelCfgMap = new Dictionary<string, PanelInfo>();

        /// <summary>
        /// 获取PanelInfo
        /// 没有则创建  相当于一个打开过了 UI基础配置档
        /// 这个k 根据resName
        /// </summary>
        private PanelInfo GetPanelInfo<T>() where T : BasePanel => GetPanelInfo(typeof(T).Name);

        /// <summary>
        /// 获取UI名称 用字符串开界面 不用类型 减少GC
        /// 另外也方便之后有可能需要的扩展 字符串会更好使用
        /// </summary>
        private string GetPanelName<T>() where T : BasePanel
        {
            var panelInfo = GetPanelInfo<T>();
            return panelInfo?.Name;
        }

        /// <summary>
        /// 打开之前
        /// </summary>
        private async UniTask OpenPanelBefore(PanelInfo info)
        {
            if (!info.UIBasePanel.WindowFirstOpen)
            {
                await AddUICloseElse(info);
            }
        }

        /// <summary>
        /// 打开之后
        /// </summary>
        private async UniTask<BasePanel> OpenPanelAfter(PanelInfo info, bool success)
        {
            if (success)
            {
                if (info.UIBasePanel.WindowFirstOpen)
                {
                    await AddUICloseElse(info);
                }
            }
            else
            {
                //如果打开失败直接屏蔽
                info?.UIBasePanel?.SetActive(false);
                info?.UIBasePanel?.Close();
            }

            return info?.UIBasePanel;
        }
    }
}