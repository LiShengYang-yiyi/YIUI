using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace YIUIFramework.Editor
{
    public class UnityIconsWindow : EditorWindow
    {
        public static void ShowWindow()
        {
            var window                           = GetWindow<UnityIconsWindow>();
            window.minSize      = window.maxSize = new Vector2(1280, 720);
            window.titleContent = new GUIContent("YIUI Unity内置图标", EditorGUIUtility.IconContent("d_UnityLogo").image);
        }

        public void CreateGUI()
        {
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Packages/cn.etetet.yiuiframework/Editor/Resources/USS/UnityIconsWindow.uss");
            rootVisualElement.styleSheets.Add(styleSheet);

            var texture2Ds = Resources.FindObjectsOfTypeAll<Texture2D>();

            var container = new VisualElement();
            container.AddToClassList("container");

            Debug.unityLogger.logEnabled = false;
            for (int i = 0; i < texture2Ds.Length; i++)
            {
                var iconName = texture2Ds[i].name;
                if (string.IsNullOrEmpty(iconName)) continue;

                var texture = EditorGUIUtility.IconContent(iconName)?.image;
                if (texture is Texture2D texture2D)
                {
                    var image = new VisualElement();
                    image.style.backgroundImage = new StyleBackground(texture2D);
                    image.AddToClassList("container-item");
                    container.Add(image);

                    image.RegisterCallback<ClickEvent>(_ => { CopyToClipboard(iconName); });
                }
            }

            Debug.unityLogger.logEnabled = true;
            var scrollView = new ScrollView();
            scrollView.Add(container);
            rootVisualElement.Add(scrollView);
        }

        private void CopyToClipboard(string str)
        {
            var copyStr = $"EditorGUIUtility.IconContent({str}).image";
            GUIUtility.systemCopyBuffer = copyStr;
            Debug.Log($"已复制到剪切板:{str}");
        }
    }
}