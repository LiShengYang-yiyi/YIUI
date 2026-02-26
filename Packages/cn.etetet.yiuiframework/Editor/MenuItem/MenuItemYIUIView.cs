using UnityEditor;
using UnityEngine;

namespace YIUIFramework.Editor
{
    public static class MenuItemYIUIView
    {
        [MenuItem("Assets/YIUI/Create UIView", false, 2)]
        static void CreateYIUIViewByFolder()
        {
            var activeObject = Selection.activeObject as DefaultAsset;
            if (activeObject == null)
            {
                UnityTipsHelper.ShowError($"请在路径 {YIUIConstHelper.Const.UIProjectResPath}/xxx/{YIUIConstHelper.Const.UIPrefabs} 下右键创建");
                return;
            }

            var path = AssetDatabase.GetAssetPath(Selection.activeObject);

            if (!path.Contains(YIUIConstHelper.Const.UIProjectResPath))
            {
                UnityTipsHelper.ShowError($"请在路径 {YIUIConstHelper.Const.UIProjectResPath}/xxx/{YIUIConstHelper.Const.UIPrefabs} 下右键创建");
                return;
            }

            var saveName = $"{YIUIConstHelper.Const.UIProjectName}{YIUIConstHelper.Const.UIViewName}";
            var savePath = $"{path}/{saveName}.prefab";

            if (AssetDatabase.LoadAssetAtPath(savePath, typeof(Object)) != null)
            {
                UnityTipsHelper.ShowError($"已存在 请先重命名 {saveName}");
                return;
            }

            var createView = CreateYIUIView();
            PrefabUtility.SaveAsPrefabAsset(createView, savePath);
            Object.DestroyImmediate(createView);
            UIMenuItemHelper.SelectAssetAtPath(savePath);
        }

        [MenuItem("GameObject/YIUI/Create UIView", false, 2)]
        static void CreateYIUIViewByGameObject()
        {
            var activeObject = Selection.activeObject as GameObject;
            if (activeObject == null)
            {
                UnityTipsHelper.ShowError($"请选择ViewParent 右键创建");
                return;
            }

            var panelCdeTable = activeObject.transform.parent.GetComponentInParent<UIBindCDETable>();
            if (panelCdeTable == null)
            {
                UnityTipsHelper.ShowError($"只能在AllViewParent / AllPopupViewParent 下使用 快捷创建View");
                return;
            }

            if (panelCdeTable.UICodeType != EUICodeType.Panel)
            {
                UnityTipsHelper.ShowError($"必须是Panel 下使用 快捷创建View");
                return;
            }

            var panelEditorData = panelCdeTable.PanelSplitData;

            if (activeObject != panelEditorData.AllViewParent?.gameObject &&
                activeObject != panelEditorData.AllPopupViewParent?.gameObject)
            {
                UnityTipsHelper.ShowError($"只能在AllViewParent / AllPopupViewParent 下使用 快捷创建View");
                return;
            }

            //ViewParent
            var viewParentObject = new GameObject();
            var viewParentRect = viewParentObject.GetOrAddComponent<RectTransform>();
            viewParentObject.name = YIUIConstHelper.Const.UIYIUIViewParentName;
            viewParentRect.SetParent(activeObject.transform, false);
            viewParentRect.ResetToFullScreen();

            //View
            var viewObject = CreateYIUIView(viewParentObject);
            var cdeTable = viewObject.GetOrAddComponent<UIBindCDETable>();
            if (activeObject == panelEditorData.AllViewParent.gameObject)
            {
                panelEditorData.AllCreateView.Add(viewParentRect);
                cdeTable.ViewWindowType = EViewWindowType.View;
            }
            else if (activeObject == panelEditorData.AllPopupViewParent.gameObject)
            {
                panelEditorData.AllPopupView.Add(viewParentRect);
                cdeTable.ViewWindowType = EViewWindowType.Popup;
            }

            viewParentObject.SetLayerRecursively(LayerMask.NameToLayer("UI"));
            Selection.activeObject = viewParentObject;
        }

        public static GameObject CreateYIUIView(GameObject activeObject = null)
        {
            var viewObject = new GameObject();
            viewObject.name = $"{YIUIConstHelper.Const.UIProjectName}{YIUIConstHelper.Const.UIViewName}";
            var viewRect = viewObject.GetOrAddComponent<RectTransform>();
            viewObject.GetOrAddComponent<CanvasRenderer>();
            var cdeTable = viewObject.GetOrAddComponent<UIBindCDETable>();
            cdeTable.UICodeType = EUICodeType.View;
            if (activeObject != null)
            {
                viewRect.SetParent(activeObject.transform, false);
                EditorUtility.SetDirty(activeObject);
            }
            viewRect.ResetToFullScreen();
            viewObject.SetLayerRecursively(LayerMask.NameToLayer("UI"));
            return viewObject;
        }
    }
}