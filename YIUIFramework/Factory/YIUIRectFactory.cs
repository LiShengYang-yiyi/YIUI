using UnityEngine;

namespace YIUIFramework
{
    /// <summary>
    /// 创建一个UI空对象时
    /// </summary>
    public static class YIUIRectFactory
    {
        /// <summary> 重置为全屏自适应UI </summary>
        public static void ResetToFullScreen(this RectTransform self)
        {
            self.anchorMin          = Vector2.zero;
            self.anchorMax          = Vector2.one;
            self.anchoredPosition3D = Vector3.zero;
            self.pivot              = new Vector2(0.5f, 0.5f);
            self.offsetMax          = Vector2.zero;
            self.offsetMin          = Vector2.zero;
            self.sizeDelta          = Vector2.zero;
            self.localEulerAngles   = Vector3.zero;
            self.localScale         = Vector3.one;
        }

        /// <summary> 重置位置与旋转 </summary>
        public static void ResetLocalPosAndRot(this RectTransform self)
        {
            self.localPosition = Vector3.zero;
            self.localRotation = Quaternion.identity;
        }

        /// <summary> 自动重置 </summary>
        public static void AutoReset(this RectTransform self)
        {
            // 全屏的情况
            if (self.anchorMax == Vector2.one && self.anchorMin == Vector2.zero)
            {
                self.ResetToFullScreen();
            }
            
            self.localScale = Vector3.one;
            self.ResetLocalPosAndRot();
        }
    }
}