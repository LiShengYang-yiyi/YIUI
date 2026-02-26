using UnityEditor;
using UnityEngine;

namespace YIUIFramework.Editor
{
    public static class MenuItemYIUICommon
    {
        [MenuItem("Assets/YIUI/Create UICommon", false, 3)]
        static void CreateYIUICommonByFolder()
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

            var saveName = $"{YIUIConstHelper.Const.UIProjectName}{YIUIConstHelper.Const.UICommonName}";
            var savePath = $"{path}/{saveName}.prefab";

            if (AssetDatabase.LoadAssetAtPath(savePath, typeof(Object)) != null)
            {
                UnityTipsHelper.ShowError($"已存在 请先重命名 {saveName}");
                return;
            }

            var createCommon = CreateYIUICommon();
            PrefabUtility.SaveAsPrefabAsset(createCommon, savePath);
            Object.DestroyImmediate(createCommon);
            UIMenuItemHelper.SelectAssetAtPath(savePath);
        }

        [MenuItem("GameObject/YIUI/Create UICommon", false, 3)]
        static void CreateYIUICommonByGameObject()
        {
            var activeObject = Selection.activeObject as GameObject;
            if (activeObject == null)
            {
                UnityTipsHelper.ShowError($"请选择一个目标");
                return;
            }

            Selection.activeObject = CreateYIUICommon(activeObject);
        }

        public static GameObject CreateYIUICommon(GameObject activeObject = null)
        {
            var commonObject = new GameObject();
            commonObject.name = $"{YIUIConstHelper.Const.UIProjectName}{YIUIConstHelper.Const.UICommonName}";
            var viewRect = commonObject.GetOrAddComponent<RectTransform>();
            commonObject.GetOrAddComponent<CanvasRenderer>();
            var cdeTable = commonObject.GetOrAddComponent<UIBindCDETable>();
            cdeTable.UICodeType = EUICodeType.Common;
            if (activeObject != null)
            {
                viewRect.SetParent(activeObject.transform, false);
                EditorUtility.SetDirty(activeObject);
            }

            commonObject.SetLayerRecursively(LayerMask.NameToLayer("UI"));
            return commonObject;
        }
    }
}