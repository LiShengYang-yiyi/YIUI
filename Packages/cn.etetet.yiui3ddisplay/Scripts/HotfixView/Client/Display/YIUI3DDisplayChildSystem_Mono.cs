using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YIUIFramework;

namespace ET.Client
{
    /// <summary>
    /// 3DDisplay的扩展组件
    /// 文档: https://lib9kmxvq7k.feishu.cn/wiki/FhGGwVZSyiCqHCkTVQYcKHQCnKf
    /// </summary>
    public static partial class YIUI3DDisplayChildSystem
    {
        private static void Awake3DDisplay(this YIUI3DDisplayChild self)
        {
            self.SetLayer();

            self.UI3DDisplay.m_ShowImage ??= self.UI3DDisplay.GetComponent<RawImage>();

            if (self.UI3DDisplay.m_ShowImage != null)
            {
                if (self.UI3DDisplay.m_ShowObject == null)
                    self.UI3DDisplay.m_ShowImage.enabled = false;
            }

            if (self.UI3DDisplay.m_ShowCamera != null)
            {
                if (self.UI3DDisplay.m_ShowObject == null)
                    self.UI3DDisplay.m_ShowCamera.enabled = false;
                self.m_ShowCameraCtrl   = self.UI3DDisplay.m_ShowCamera.GetOrAddComponent<UI3DDisplayCamera>();
                self.m_ShowCameraDefPos = self.UI3DDisplay.m_ShowCamera.transform.localPosition;
            }
            else
            {
                Debug.LogError($"{self.UI3DDisplay.gameObject.name} ShowCamera == null 这是不允许的 请检查 建议直接使用默认预制 不要自己修改");
            }

            YIUI3DDisplayChild.g_DisPlayUIIndex++;
            var offsetY = YIUI3DDisplayChild.g_DisPlayUIIndex * 100.0f;
            if (YIUI3DDisplayChild.g_DisPlayUIIndex >= 2147)
                YIUI3DDisplayChild.g_DisPlayUIIndex = 0;

            self.m_ModelGlobalOffset = new Vector3(0, offsetY, 0);

            if (self.UI3DDisplay.m_MultipleTargetMode)
                self.InitRotationData();

            //如果提前设置显示对象 在非多模式下 自动设置
            if (!self.UI3DDisplay.m_MultipleTargetMode && self.UI3DDisplay.m_ShowObject != null && self.UI3DDisplay.m_LookCamera != null)
            {
                self.ShowByGameObject(self.UI3DDisplay.m_ShowObject, self.UI3DDisplay.m_LookCamera);
            }
        }

        private static void Destroy3DDisplay(this YIUI3DDisplayChild self)
        {
            if (self.m_ShowTexture != null)
            {
                RenderTexture.ReleaseTemporary(self.m_ShowTexture);
                self.m_ShowTexture                          = null;
                self.UI3DDisplay.m_ShowImage.texture        = null;
                self.UI3DDisplay.m_ShowCamera.targetTexture = null;
                self.UI3DDisplay.m_ShowCamera.enabled       = false;
            }

            self.DisableMeshRectShadow();

            if (self.m_ShowCameraCtrl != null)
            {
                self.m_ShowCameraCtrl.ShowObject = null;
            }
        }

        [EntitySystem]
        private static void LateUpdate(this YIUI3DDisplayChild self)
        {
            if (!self.UI3DDisplay.m_AutoSyncLookCamera)
                return;

            //自动同步摄像机位置旋转 ShowCamera的位置 = LookCamera的位置
            if (self.UI3DDisplay.m_AutoSyncLookCamera && self.UI3DDisplay.m_LookCamera)
            {
                var tsf = self.UI3DDisplay.m_LookCamera.transform;
                self.UI3DDisplay.m_ShowCamera.transform.SetPositionAndRotation(tsf.position, tsf.rotation);
            }
        }

        [EntitySystem]
        private static void YIUIEnable(this YIUI3DDisplayChild self)
        {
            if (self.UI3DDisplay.m_ShowObject == null || self.m_ShowTexture != null) return;

            self.m_ShowTexture = RenderTexture.GetTemporary(self.UI3DDisplay.m_ResolutionX, self.UI3DDisplay.m_ResolutionY, self.UI3DDisplay.m_RenderTextureDepthBuffer);

            if (self.UI3DDisplay.m_ShowImage != null)
            {
                self.UI3DDisplay.m_ShowImage.texture = self.m_ShowTexture;
                self.UI3DDisplay.m_ShowImage.enabled = true;
            }

            if (self.UI3DDisplay.m_ShowCamera != null)
            {
                self.UI3DDisplay.m_ShowCamera.targetTexture = self.m_ShowTexture;
                self.UI3DDisplay.m_ShowCamera.enabled       = true;
            }
        }

        [EntitySystem]
        private static void YIUIDisable(this YIUI3DDisplayChild self)
        {
            if (self.m_ShowTexture == null) return;

            RenderTexture.ReleaseTemporary(self.m_ShowTexture);
            self.m_ShowTexture = null;

            if (self.UI3DDisplay.m_ShowImage != null)
            {
                self.UI3DDisplay.m_ShowImage.texture = null;
                self.UI3DDisplay.m_ShowImage.enabled = false;
            }

            if (self.UI3DDisplay.m_ShowCamera != null)
            {
                self.UI3DDisplay.m_ShowCamera.targetTexture = null;
                self.UI3DDisplay.m_ShowCamera.enabled       = false;
            }
        }
    }
}
