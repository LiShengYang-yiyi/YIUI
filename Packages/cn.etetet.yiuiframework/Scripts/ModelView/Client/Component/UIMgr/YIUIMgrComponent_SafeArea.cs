using System;
using UnityEngine;

namespace ET.Client
{
    /// <summary>
    /// 当前使用  UnitysafeArea API 进行UI刘海屏自适应
    /// UnitysafeArea API 也不太靠谱 可以在这里扩展自己的东西
    /// </summary>
    public partial class YIUIMgrComponent
    {
        //安全区
        [StaticField]
        public static Rect g_SafeArea;

        /// <summary>
        /// 横屏设置时，界面左边离屏幕的距离
        /// </summary>
        [StaticField]
        public static float SafeAreaLeft => Screen.orientation == ScreenOrientation.LandscapeRight
                ? Screen.width - g_SafeArea.xMax
                : g_SafeArea.x;

        [StaticField]
        internal static ScreenOrientation ScreenOrientation = Screen.orientation;
    }
}
