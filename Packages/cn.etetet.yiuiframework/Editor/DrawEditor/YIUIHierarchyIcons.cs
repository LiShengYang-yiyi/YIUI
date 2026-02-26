#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace YIUIFramework
{
    [InitializeOnLoad]
    public static class YIUIHierarchyIcons
    {
        private static Texture2D m_YIUILogo;

        public static Texture2D YIUILogo
        {
            get
            {
                return m_YIUILogo ??= (Texture2D)Resources.Load("YIUIIcon");
            }
        }

        static YIUIHierarchyIcons()
        {
            EditorApplication.hierarchyWindowItemOnGUI += ShowIcons;
        }

        private static void ShowIcons(int ID, Rect r)
        {
            var go = EditorUtility.InstanceIDToObject(ID) as GameObject;
            if (go == null)
            {
                return;
            }

            if (go.GetComponent<UIBindCDETable>() != null)
            {
                r.x     = r.xMax - 16;
                r.width = 16;
                GUI.DrawTexture(r, YIUILogo);
            }
        }
    }
}

#endif