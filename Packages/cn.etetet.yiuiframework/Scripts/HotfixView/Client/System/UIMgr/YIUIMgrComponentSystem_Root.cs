using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YIUIFramework;

namespace ET.Client
{
    public static partial class YIUIMgrComponentSystem
    {
        internal static async ETTask<bool> InitRoot(this YIUIMgrComponent self)
        {
            #region UICanvasRoot 查找各种组件

            EntityRef<YIUIMgrComponent> selfRef = self;
            self.UIRoot = GameObject.Find(YIUIConstHelper.Const.UIRootName);
            if (self.UIRoot == null)
            {
                self.UIRoot = await self.GetComponent<YIUILoadComponent>().LoadAssetAsyncInstantiate(YIUIConstHelper.Const.UIRootPkgName, YIUIConstHelper.Const.UIRootName);
            }

            self = selfRef;
            if (self.UIRoot == null)
            {
                Debug.LogError($"初始化错误 没有找到UIRoot");
                return false;
            }

            self.UIRoot.name = self.UIRoot.name.Replace("(Clone)", "");
            UnityEngine.Object.DontDestroyOnLoad(self.UIRoot);

            //root可修改位置防止与世界3D场景重叠导致不好编辑
            self.UIRoot.transform.position = new Vector3(YIUIConstHelper.Const.RootPosOffset, YIUIConstHelper.Const.RootPosOffset, 0);

            self.UICanvas = self.UIRoot.GetComponentInChildren<Canvas>();
            if (self.UICanvas == null)
            {
                Debug.LogError($"初始化错误 没有找到Canvas");
                return false;
            }

            self.UICanvasRoot = self.UICanvas.gameObject;

            self.UILayerRoot = self.UICanvasRoot.transform.FindChildByName(YIUIConstHelper.Const.UILayerRootName)?.GetComponent<RectTransform>();
            if (self.UILayerRoot == null)
            {
                Debug.LogError($"初始化错误 没有找到UILayerRoot");
                return false;
            }

            self.UICamera = self.UICanvasRoot.GetComponentInChildren<Camera>();
            if (self.UICamera == null)
            {
                Debug.LogError($"初始化错误 没有找到UICamera");
                return false;
            }

            var canvas = self.UICanvasRoot.GetComponent<Canvas>();
            if (canvas == null)
            {
                Debug.LogError($"初始化错误 没有找到UICanvasRoot - Canvas");
                return false;
            }

            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = self.UICamera;

            var canvasScaler = self.UICanvasRoot.GetComponent<CanvasScaler>();
            if (canvasScaler == null)
            {
                Debug.LogError($"初始化错误 没有找到UICanvasRoot - CanvasScaler");
                return false;
            }

            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScaler.referenceResolution = new Vector2(YIUIConstHelper.Const.DesignScreenWidth, YIUIConstHelper.Const.DesignScreenHeight);

            #endregion

            //分层
            const int len = (int)EPanelLayer.Count;
            self.m_AllPanelLayer.Clear();
            for (var i = len - 1; i >= 0; i--)
            {
                var layer = new GameObject($"Layer{i}-{(EPanelLayer)i}");
                var rect = layer.AddComponent<RectTransform>();
                rect.SetParent(self.UILayerRoot);
                rect.localScale = Vector3.one;
                rect.pivot = new Vector2(0.5f, 0.5f);
                rect.anchorMax = Vector2.one;
                rect.anchorMin = Vector2.zero;
                rect.sizeDelta = Vector2.zero;
                rect.localRotation = Quaternion.identity;
                rect.localPosition = new Vector3(0, 0, i * YIUIConstHelper.Const.LayerDistance);
                var rectDic = new Dictionary<RectTransform, List<PanelInfo>> { { rect, new List<PanelInfo>() } };
                self.m_AllPanelLayer.Add((EPanelLayer)i, rectDic);
            }

            self.GetLayerRect(EPanelLayer.Cache).gameObject.SetActive(false);

            self.InitAddUIBlock(); //所有层级初始化后添加一个终极屏蔽层 可根据API 定时屏蔽UI操作

            // ReSharper disable once Unity.InefficientPropertyAccess
            self.UICamera.transform.localPosition = new Vector3(self.UILayerRoot.localPosition.x, self.UILayerRoot.localPosition.y, -1000);

            self.UICamera.clearFlags = CameraClearFlags.Depth;
            self.UICamera.orthographic = true;

            //根据需求可以修改摄像机的远裁剪平面大小 没必要设置的很大
            //UICamera.farClipPlane = ((len + 1) * YIUIMgrComponent.LayerDistance) * UICanvasRoot.transform.localScale.x;

            return true;
        }

        internal static void ResetRoot(this YIUIMgrComponent self)
        {
            if (self.UILayerRoot == null) return;
            for (int i = self.UILayerRoot.transform.childCount - 1; i >= 0; i--)
            {
                UnityEngine.Object.Destroy(self.UILayerRoot.transform.GetChild(i).gameObject);
            }
        }

        internal static bool ContainsLayerPanelInfo(this YIUIMgrComponent self, EPanelLayer panelLayer, PanelInfo panelInfo)
        {
            var list = self.GetLayerPanelInfoList(panelLayer);
            return list.Contains(panelInfo);
        }

        internal static bool RemoveLayerPanelInfo(this YIUIMgrComponent self, EPanelLayer panelLayer, PanelInfo panelInfo)
        {
            var list = self.GetLayerPanelInfoList(panelLayer);
            return list != null && list.Remove(panelInfo);
        }

        internal static List<PanelInfo> GetLayerPanelInfoList(this YIUIMgrComponent self, EPanelLayer panelLayer)
        {
            self.m_AllPanelLayer.TryGetValue(panelLayer, out var rectDic);
            if (rectDic == null)
            {
                Debug.LogError($"没有这个层级 请检查 {panelLayer}");
                return null;
            }

            //只能有一个所以返回第一个
            foreach (var infoList in rectDic.Values)
            {
                return infoList;
            }

            return null;
        }

        /// <summary>
        /// 获取一个层级对象
        /// </summary>
        /// <param name="self"></param>
        /// <param name="panelLayer"></param>
        /// <returns></returns>
        public static RectTransform GetLayerRect(this YIUIMgrComponent self, EPanelLayer panelLayer)
        {
            self.m_AllPanelLayer.TryGetValue(panelLayer, out var rectDic);
            if (rectDic == null)
            {
                Debug.LogError($"没有这个层级 请检查 {panelLayer}");
                return null;
            }

            //只能有一个所以返回第一个
            foreach (var rect in rectDic.Keys)
            {
                return rect;
            }

            return null;
        }
    }
}