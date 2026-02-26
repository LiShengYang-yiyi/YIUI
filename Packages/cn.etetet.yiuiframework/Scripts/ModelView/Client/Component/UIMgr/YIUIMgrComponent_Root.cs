using System.Collections.Generic;
using UnityEngine;
using YIUIFramework;

namespace ET.Client
{
    public partial class YIUIMgrComponent
    {
        public GameObject UIRoot { get; set; }
        public GameObject UICanvasRoot { get; set; }
        public RectTransform UILayerRoot { get; set; }
        public Camera UICamera { get; set; }
        public Canvas UICanvas { get; set; }

        //K1 = 层级枚举 V1 = 层级对应的rect
        //List = 当前层级中的当前所有UI 前面的代表这个UI在前面以此类推
        public readonly Dictionary<EPanelLayer, Dictionary<RectTransform, List<PanelInfo>>> m_AllPanelLayer = new();

        #region 快捷获取层级

        private RectTransform m_UICache;

        public RectTransform UICache
        {
            get
            {
                if (m_UICache == null)
                {
                    m_AllPanelLayer.TryGetValue(EPanelLayer.Cache, out var rectDic);
                    if (rectDic == null)
                    {
                        Log.Error($"没有这个层级 请检查 {EPanelLayer.Cache}");
                        return null;
                    }

                    foreach (var rect in rectDic)
                    {
                        m_UICache = rect.Key;
                        break;
                    }

                    return m_UICache;
                }

                return m_UICache;
            }
        }

        private RectTransform m_UIPanel;

        public RectTransform UIPanel
        {
            get
            {
                if (m_UIPanel == null)
                {
                    m_AllPanelLayer.TryGetValue(EPanelLayer.Panel, out var rectDic);
                    if (rectDic == null)
                    {
                        Log.Error($"没有这个层级 请检查 {EPanelLayer.Panel}");
                        return null;
                    }

                    foreach (var rect in rectDic)
                    {
                        m_UIPanel = rect.Key;
                        break;
                    }

                    return m_UIPanel;
                }

                return m_UIPanel;
            }
        }

        #endregion
    }
}