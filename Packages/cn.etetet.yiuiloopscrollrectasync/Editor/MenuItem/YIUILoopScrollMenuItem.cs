#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace YIUIFramework.Editor
{
    internal static class YIUILoopScrollMenuItem
    {
        [MenuItem("GameObject/YIUI/LoopScroll/文档", false, 10000)]
        private static void LoopScrollOpenDocument()
        {
            Application.OpenURL("https://lib9kmxvq7k.feishu.cn/wiki/HPbwwkhsKi9aDik5VEXcqPhDnIh");
        }

        [MenuItem("GameObject/YIUI/LoopScroll/Horizontal", false, 10001)]
        private static void CreateLoopScrollHorizontal()
        {
            CreateLoopScroll("LoopScrollHorizontal");
        }

        [MenuItem("GameObject/YIUI/LoopScroll/Horizontal Reverse", false, 10002)]
        private static void CreateLoopScrollHorizontalReverse()
        {
            CreateLoopScroll("LoopScrollHorizontalReverse");
        }

        [MenuItem("GameObject/YIUI/LoopScroll/Horizontal Group", false, 10003)]
        private static void CreateLoopScrollHorizontalGroup()
        {
            CreateLoopScroll("LoopScrollHorizontalGroup");
        }

        [MenuItem("GameObject/YIUI/LoopScroll/Vertical", false, 10011)]
        private static void CreateLoopScrollVertical()
        {
            CreateLoopScroll("LoopScrollVertical");
        }

        [MenuItem("GameObject/YIUI/LoopScroll/Vertical Reverse", false, 10012)]
        private static void CreateLoopScrollVerticalReverse()
        {
            CreateLoopScroll("LoopScrollVerticalReverse");
        }

        [MenuItem("GameObject/YIUI/LoopScroll/Vertical Group", false, 10013)]
        private static void CreateLoopScrollVerticalGroup()
        {
            CreateLoopScroll("LoopScrollVerticalGroup");
        }

        private static GameObject CreateLoopScroll(string name)
        {
            var path = $"Packages/cn.etetet.yiuiloopscrollrectasync/Editor/TemplatePrefabs";
            return YIUICommonMenuItem.CreateTarget(path, name);
        }
    }
}
#endif