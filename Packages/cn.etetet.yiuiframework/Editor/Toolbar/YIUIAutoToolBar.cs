#if YIUI
using UnityEditor;
using UnityEngine;

namespace YIUIFramework.Editor
{
    [InitializeOnLoad]
    public static class YIUIAutoToolBar
    {
        private static Texture _cachedIcon;

        static YIUIAutoToolBar()
        {
            YIUIToolbarExtender.AddLeftToolbarGUI(OnYIUIAutoToolbarGUI, 100);
        }

        private static void OnYIUIAutoToolbarGUI()
        {
            _cachedIcon ??= AssetDatabase.LoadAssetAtPath<Texture>("Packages/cn.etetet.yiuiframework/Editor/Resources/YIUIIcon16.png");

            GUILayout.Space(5);
            GUIContent iconContent = new(string.Empty, _cachedIcon);
            iconContent.tooltip = "YIUIAutoTool";
            if (GUILayout.Button(iconContent))
            {
                YIUIAutoTool.OpenWindow();
                EditorApplication.delayCall += () =>
                {
                    var window = EditorWindow.GetWindow<YIUIAutoTool>();
                    if (window != null)
                    {
                        window.SelectModule("发布");
                    }
                };
            }

            GUILayout.Space(5);
        }
    }
}
#endif