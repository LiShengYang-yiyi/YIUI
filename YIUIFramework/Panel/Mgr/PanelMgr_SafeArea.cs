using System;
using UnityEngine;

namespace YIUIFramework
{
    /// <summary>
    /// 安全区 刘海屏
    /// </summary>
    public partial class PanelMgr
    {
        /// <summary>
        /// 在刘海屏机子时，是否打开黑边
        /// </summary>
        public const bool OpenBlackBorder = false;

        //启用2倍安全 则左右2边都会裁剪
        public const bool DoubleSafe = false;

        //安全区
        private static Rect g_SafeArea;

        /// <summary>
        /// 安全区
        /// </summary>
        public static Rect SafeArea => g_SafeArea;

        /// <summary>
        /// 横屏设置时，界面左边离屏幕的距离
        /// </summary>
        public static float SafeAreaLeft => Screen.orientation == ScreenOrientation.LandscapeRight
            ? Screen.width - g_SafeArea.xMax
            : g_SafeArea.x;

        private ScreenOrientation ScreenOrientation = Screen.orientation;

        private void InitSafeArea()
        {
            var safeAreaX = Math.Max(Screen.safeArea.x, Screen.width - Screen.safeArea.xMax);
            var safeAreaY = Math.Max(Screen.safeArea.y, Screen.height - Screen.safeArea.yMax);

            #if UNITY_EDITOR

            //safeAreaX = 100;
            //safeAreaY = 100;
            #endif

            g_SafeArea = new Rect(
                safeAreaX,
                safeAreaY,
                DesignScreenWidth_F - GetSafeValue(safeAreaX),
                DesignScreenHeight_F - GetSafeValue(safeAreaY));

            InitUISafeArea();
        }

        private float GetSafeValue(float safeValue)
        {
            return DoubleSafe ? safeValue * 2 : safeValue;
        }

        private void InitUISafeArea()
        {
            UILayerRoot.anchoredPosition = new Vector2(SafeArea.x, -SafeArea.y);
            if (DoubleSafe)
            {
                UILayerRoot.offsetMax = new Vector2(-SafeArea.x, UILayerRoot.offsetMax.y);
                UILayerRoot.offsetMin = new Vector2(UILayerRoot.offsetMin.x, SafeArea.y);
            }
            else
            {
                //TODO 单边时需要考虑手机是左还是右
                UILayerRoot.offsetMax = new Vector2(0, UILayerRoot.offsetMax.y);
                UILayerRoot.offsetMin = new Vector2(UILayerRoot.offsetMin.x, 0);
            }
        }
    }
}