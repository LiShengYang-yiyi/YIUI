using UnityEditor;
using UnityEngine;

namespace YIUIFramework.Editor
{
    internal static partial class YIUICommonMenuItem
    {
        public static GameObject CreateTarget(string path, string targetName)
        {
            var activeObject = Selection.activeObject as GameObject;
            if (activeObject == null)
            {
                UnityTipsHelper.ShowError($"请选择一个对象 右键创建");
                return null;
            }

            var clonePath = $"{path}/{targetName}.prefab";
            var obj       = UIMenuItemHelper.CloneGameObjectByPath(clonePath, activeObject.transform);
            Selection.activeObject = obj;
            return obj;
        }

        internal static GameObject CreateTarget(string targetName)
        {
            var path = $"Packages/cn.etetet.yiuiframework/Editor/TemplatePrefabs/YIUI";
            return CreateTarget(path, targetName);
        }

        [MenuItem("GameObject/YIUI/UIBlockBG", false, 100000)]
        private static void CreateText_UIBlockBG()
        {
            CreateTarget("UIBlockBG");
        }

        [MenuItem("GameObject/YIUI/Text_NoRaycast", false, 100001)]
        private static void CreateText_NoRaycast()
        {
            CreateTarget("YIUIText_NoRaycast");
        }

        [MenuItem("GameObject/YIUI/Text (TMP)", false, 100002)]
        private static void CreateTextTMP()
        {
            CreateTarget("YIUIText (TMP)");
        }

        [MenuItem("GameObject/YIUI/Image_NoRaycast", false, 100003)]
        private static void CreateImage_NoRaycast()
        {
            CreateTarget("YIUIImage_NoRaycast");
        }

        [MenuItem("GameObject/YIUI/Button", false, 100004)]
        private static void CreateButton()
        {
            CreateTarget("YIUIButton");
        }

        [MenuItem("GameObject/YIUI/Button_NoText", false, 100005)]
        private static void CreateButton_NoText()
        {
            CreateTarget("YIUIButton_NoText");
        }

        [MenuItem("GameObject/YIUI/Click_Event", false, 110001)]
        private static void CreateClick_Event()
        {
            var obj = CreateTarget("UIBlockBG");
            obj.name = "ClickEvent";
            obj.AddComponent<YIUIClickEffect>();
            obj.AddComponent<UIEventBindClick>();
        }

        [MenuItem("GameObject/YIUI/Click_Task", false, 110002)]
        private static void CreateClick_Task()
        {
            var obj = CreateTarget("UIBlockBG");
            obj.name = "ClickTask";
            obj.AddComponent<YIUIClickEffect>();
            obj.AddComponent<UITaskEventBindClick>();
        }
    }
}