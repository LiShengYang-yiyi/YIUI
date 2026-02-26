using UnityEditor;
using UnityEngine;

namespace YIUIFramework.Editor
{
    public static class MenuItemYIUIPanelSource
    {
        [MenuItem("Assets/YIUI/Create UIPanelSource", false, 0)]
        static void CreateYIUIPanelByFolder()
        {
            var activeObject = Selection.activeObject as DefaultAsset;
            if (activeObject == null)
            {
                UnityTipsHelper.ShowError($"请在路径 {YIUIConstHelper.Const.UIProjectResPath}/xxx/{YIUIConstHelper.Const.UISource} 下右键创建");
                return;
            }

            var path = AssetDatabase.GetAssetPath(Selection.activeObject);

            if (activeObject.name != YIUIConstHelper.Const.UISource ||
                !path.Contains(YIUIConstHelper.Const.UIProjectResPath))
            {
                UnityTipsHelper.ShowError($"请在路径 {YIUIConstHelper.Const.UIProjectResPath}/xxx/{YIUIConstHelper.Const.UISource} 下右键创建");
                return;
            }

            CreateYIUIPanelByPath(path);
        }

        internal static void CreateYIUIPanelByPath(string path, string name = null)
        {
            if (!path.Contains(YIUIConstHelper.Const.UIProjectResPath))
            {
                UnityTipsHelper.ShowError($"请在路径 {YIUIConstHelper.Const.UIProjectResPath}/xxx/{YIUIConstHelper.Const.UISource} 下右键创建");
                return;
            }

            var saveName = string.IsNullOrEmpty(name)
                    ? YIUIConstHelper.Const.UIYIUIPanelSourceName
                    : $"{name}{YIUIConstHelper.Const.UIPanelSourceName}";
            var savePath = $"{path}/{saveName}.prefab";

            if (AssetDatabase.LoadAssetAtPath(savePath, typeof(Object)) != null)
            {
                UnityTipsHelper.ShowError($"已存在 请先重命名 {saveName}");
                return;
            }

            var createPanel = CreateYIUIPanel();
            PrefabUtility.SaveAsPrefabAsset(createPanel, savePath);
            Object.DestroyImmediate(createPanel);
            UIMenuItemHelper.SelectAssetAtPath(savePath);
        }

        public static GameObject CreateYIUIPanel(GameObject activeObject = null)
        {
            //panel
            var panelObject = new GameObject();
            var panelRect   = panelObject.GetOrAddComponent<RectTransform>();
            panelObject.GetOrAddComponent<CanvasRenderer>();
            var cdeTable = panelObject.GetOrAddComponent<UIBindCDETable>();
            cdeTable.UICodeType  =  EUICodeType.Panel;
            cdeTable.IsSplitData =  true;
            cdeTable.PanelOption |= EPanelOption.TimeCache;

            var panelEditorData = cdeTable.PanelSplitData;
            panelEditorData.Panel = panelObject;
            panelObject.name      = YIUIConstHelper.Const.UIYIUIPanelSourceName;
            if (activeObject != null)
                panelRect.SetParent(activeObject.transform, false);
            panelRect.ResetToFullScreen();

            //阻挡图
            var backgroundObject = new GameObject();
            var backgroundRect   = backgroundObject.GetOrAddComponent<RectTransform>();
            backgroundObject.GetOrAddComponent<CanvasRenderer>();
            backgroundObject.GetOrAddComponent<UIBlock>();
            backgroundObject.name = "UIBlockBG";
            backgroundRect.SetParent(panelRect, false);
            backgroundRect.ResetToFullScreen();

            // 添加allView节点
            var allViewObject = new GameObject();
            var allViewRect   = allViewObject.GetOrAddComponent<RectTransform>();
            allViewObject.name = YIUIConstHelper.Const.UIAllViewParentName;
            allViewRect.SetParent(panelRect, false);
            allViewRect.ResetToFullScreen();
            panelEditorData.AllViewParent = allViewRect;

            // 添加AllPopupView节点
            var allPopupViewObject = new GameObject();
            var allPopupViewRect   = allPopupViewObject.GetOrAddComponent<RectTransform>();
            allPopupViewObject.name = YIUIConstHelper.Const.UIAllPopupViewParentName;
            allPopupViewRect.SetParent(panelRect, false);
            allPopupViewRect.ResetToFullScreen();
            panelEditorData.AllPopupViewParent = allPopupViewRect;

            panelObject.SetLayerRecursively(LayerMask.NameToLayer("UI"));

            return panelObject;
        }
    }
}