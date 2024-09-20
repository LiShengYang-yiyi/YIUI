using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;


namespace YIUIFramework
{
    /// <summary>
    /// UIRoot
    /// </summary>
    public partial class PanelMgr
    {
        /// <summary> Panel Group </summary>
        private struct PanelGroup
        {
            /// <summary> 当前Layer下所有Panel面板Root </summary>
            public readonly RectTransform PanelRoot;
            public readonly List<PanelInfo> AllPanel;

            public PanelGroup(RectTransform panelRoot)
            {
                PanelRoot = panelRoot;
                AllPanel = new List<PanelInfo>();
            }
        }
        
        public       GameObject    UIRoot;
        public       GameObject    UICanvasRoot;
        public       RectTransform UILayerRoot;
        public       Camera        UICamera;
        public       Canvas        UICanvas;
        public const int           DesignScreenWidth    = 1920;
        public const int           DesignScreenHeight   = 1080;
        public const float         DesignScreenWidth_F  = 1920f;
        public const float         DesignScreenHeight_F = 1080f;

        private const int RootPosOffset = 1000; // 修改可设置UI根位置
        private const int LayerDistance = 1000; // UI每个层级之间z轴的间距

        public const string UIRootName      = "YIUIRoot";
        public const string UILayerRootName = "YIUILayerRoot";
        public const string UIRootPkgName   = "Common";

        //K1 = 层级枚举 V1 = 层级对应的rectRoot And PanelList
        //List = 当前层级中的当前所有UI 前面的代表这个UI在前面以此类推
        private readonly Dictionary<EPanelLayer, PanelGroup>
            m_AllPanelLayer = new Dictionary<EPanelLayer, PanelGroup>();
        
        private async UniTask<bool> InitRoot()
        {
            #region UICanvasRoot 查找各种组件

            UIRoot = GameObject.Find(UIRootName);
            if (UIRoot == null)
            {
                UIRoot = await YIUILoadHelper.LoadAssetAsyncInstantiate(UIRootPkgName, UIRootName);
            }

            if (UIRoot == null)
            {
                Debug.LogError($"初始化错误 没有找到UIRoot");
                return false;
            }

            UIRoot.name = UIRoot.name.Replace("(Clone)", "");
            Object.DontDestroyOnLoad(UIRoot);
            UIRoot.transform.position = new Vector3(RootPosOffset, RootPosOffset, 0); //root可修改位置防止与世界3D场景重叠导致不好编辑

            UICanvas = UIRoot.GetComponentInChildren<Canvas>();
            if (UICanvas == null)
            {
                Debug.LogError($"初始化错误 没有找到Canvas");
                return false;
            }

            UICanvasRoot = UICanvas.gameObject;

            UILayerRoot = UICanvasRoot.transform.FindChildByName(UILayerRootName)?.GetComponent<RectTransform>();
            if (UILayerRoot == null)
            {
                Debug.LogError($"初始化错误 没有找到UILayerRoot");
                return false;
            }

            UICamera = UICanvasRoot.GetComponentInChildren<Camera>();
            if (UICamera == null)
            {
                Debug.LogError($"初始化错误 没有找到UICamera");
                return false;
            }

            var canvas = UICanvasRoot.GetComponent<Canvas>();
            if (canvas == null)
            {
                Debug.LogError($"初始化错误 没有找到UICanvasRoot - Canvas");
                return false;
            }

            canvas.renderMode  = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = UICamera;

            var canvasScaler = UICanvasRoot.GetComponent<CanvasScaler>();
            if (canvasScaler == null)
            {
                Debug.LogError($"初始化错误 没有找到UICanvasRoot - CanvasScaler");
                return false;
            }

            canvasScaler.uiScaleMode         = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScaler.referenceResolution = new Vector2(DesignScreenWidth, DesignScreenHeight);

            #endregion

            //分层
            const int len = (int)EPanelLayer.Count;
            m_AllPanelLayer.Clear();
            for (var i = len - 1; i >= 0; i--)
            {
                var layer = new GameObject($"Layer{i}-{(EPanelLayer)i}");
                var rect  = layer.AddComponent<RectTransform>();
                rect.SetParent(UILayerRoot);
                rect.localScale    = Vector3.one;
                rect.pivot         = new Vector2(0.5f, 0.5f);
                rect.anchorMax     = Vector2.one;
                rect.anchorMin     = Vector2.zero;
                rect.sizeDelta     = Vector2.zero;
                rect.localRotation = Quaternion.identity;
                rect.localPosition = new Vector3(0, 0, i * LayerDistance); //这个是为了3D模型时穿插的问题
                m_AllPanelLayer.Add((EPanelLayer)i, new PanelGroup(rect));
            }

            InitAddUIBlock(); //所有层级初始化后添加一个终极屏蔽层 可根据API 定时屏蔽UI操作
            
            UICamera.transform.localPosition =
                new Vector3(UILayerRoot.localPosition.x, UILayerRoot.localPosition.y, -LayerDistance);

            UICamera.clearFlags   = CameraClearFlags.Depth;
            UICamera.orthographic = true;
            
            //根据需求可以修改摄像机的远剪裁平面大小 没必要设置的很大
            // UICamera.farClipPlane = ((len + 1) * LayerDistance) * UICanvasRoot.transform.localScale.x; 
            return true;
        }

        /// <summary> 移除所有面板对象 </summary>
        private void ResetRoot()
        {
            for (int i = UILayerRoot.transform.childCount - 1; i >= 0; i--)
            {
                UnityEngine.Object.Destroy(UILayerRoot.transform.GetChild(i).gameObject);
            }
        }

        #region 快捷获取层级

        private RectTransform m_UICache;

        public RectTransform UICache
        {
            get
            {
                if (m_UICache == null)
                {
                    m_UICache = GetLayerRect(EPanelLayer.Cache);
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
                    m_UIPanel = GetLayerRect(EPanelLayer.Panel);
                }

                return m_UIPanel;
            }
        }

        #endregion

        /// <summary> 获取层级的根对象 Root </summary>
        public RectTransform GetLayerRect(EPanelLayer panelLayer)
        {
            if (m_AllPanelLayer.TryGetValue(panelLayer, out var panelGroup))
            {
                return panelGroup.PanelRoot;    
            }

            Debug.LogError($"没有这个层级 请检查 {panelLayer}");
            return null;
        }

        /// <summary> 获取指定层下所有面板信息 </summary>
        private List<PanelInfo> GetLayerPanelInfoList(EPanelLayer panelLayer)
        {
            if (m_AllPanelLayer.TryGetValue(panelLayer, out var panelGroup))
            {
                return panelGroup.AllPanel;    
            }
            
            Debug.LogError($"没有这个层级 请检查 {panelLayer}");
            return null;
        }

        /// <summary> 移除面板信息 </summary>
        private bool RemoveLayerPanelInfo(EPanelLayer panelLayer, PanelInfo panelInfo)
        {
            var list = GetLayerPanelInfoList(panelLayer);
            return list != null && list.Remove(panelInfo);
        }
    }
}