using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YIUIFramework;
using UnityEngine.Assertions;

namespace ET.Client
{
    /// <summary>
    /// 3DDisplay的扩展组件
    /// 文档: https://lib9kmxvq7k.feishu.cn/wiki/FhGGwVZSyiCqHCkTVQYcKHQCnKf
    /// </summary>
    public static partial class YIUI3DDisplayChildSystem
    {
        /// <summary>
        /// 显示3D
        /// 这是基础API 不建议使用
        /// 且会跳过对象池 直接创建对象
        /// 请使用封装好的传入预制名称显示的方法
        /// </summary>
        /// <param name="showObject">需要被显示的对象</param>
        /// <param name="lookCamera">摄像机参数</param>
        private static void ShowByGameObject(this YIUI3DDisplayChild self, GameObject showObject, Camera lookCamera)
        {
            if (self.UI3DDisplay == null) return;
            if (showObject == null)
            {
                Debug.LogError($"必须设置显示对象");
                return;
            }

            if (lookCamera == null)
            {
                Debug.LogError($"必须设置参考摄像机");
                return;
            }

            self.SetTemporaryRenderTexture();

            self.UpdateShowObject(showObject);

            self.UpdateLookCamera(lookCamera);
        }

        //设置所有动画
        //总是让整个角色动画化。对象即使在屏幕外也是动画的。
        //因为我们会吧对象丢到屏幕外否则动画可能会不动
        private static void SetAllAnimatorCullingMode(this YIUI3DDisplayChild self, Transform target)
        {
            var animators = YIUIFramework.ListPool<Animator>.Get();
            target.GetComponentsInChildren(true, animators);
            foreach (var animator in animators)
            {
                animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
            }

            YIUIFramework.ListPool<Animator>.Put(animators);
        }

        //设置默认UI3D显示层级
        private static void SetLayer(this YIUI3DDisplayChild self)
        {
            self.m_ShowLayer = LayerMask.NameToLayer(YIUIConstHelper.Const.YIUI3DLayer);
            if (self.m_ShowLayer != -1) return;
            Debug.LogError($"当前设定的UI层级不存在  {YIUIConstHelper.Const.YIUI3DLayer} 层级 请手动添加");
        }

        //设置碰撞体的层级
        private static void SetColliderLayer(this YIUI3DDisplayChild self, GameObject obj)
        {
            if (self.UI3DDisplay == null || !self.UI3DDisplay.m_AutoSetColliderLayer) return;
            var renderers = YIUIFramework.ListPool<Collider>.Get();
            obj.GetComponentsInChildren(true, renderers);
            foreach (var renderer in renderers)
            {
                renderer.gameObject.layer = self.m_ShowLayer;
            }

            YIUIFramework.ListPool<Collider>.Put(renderers);
        }

        /// <summary>
        /// 设置对象的显示层级
        /// </summary>
        private static void SetupShowLayerTarget(this YIUI3DDisplayChild self, Transform target)
        {
            if (self.m_ShowCameraCtrl != null && self.m_ShowCameraCtrl.ShowObject != null)
            {
                self.m_ShowCameraCtrl.SetupRenderer(target);
            }
        }

        //回收之前的对象
        private static void RecycleLastShow(this YIUI3DDisplayChild self, GameObject lastShowObject)
        {
            if (lastShowObject == null) return;
            lastShowObject.SetActive(false);
        }

        //更新显示的对象
        //父级 位置 旋转
        private static void UpdateShowObject(this YIUI3DDisplayChild self, GameObject showObject)
        {
            if (self.UI3DDisplay == null) return;

            if (showObject != self.UI3DDisplay.m_ShowObject)
            {
                self.RecycleLastShow(self.UI3DDisplay.m_ShowObject);
            }

            showObject.SetActive(true);
            self.SetColliderLayer(showObject);

            self.UI3DDisplay.m_ShowObject = showObject;

            self.m_DragRotation = 0f;

            self.m_DragTarge = self.MultipleTargetMode ? null : self.UI3DDisplay.m_ShowObject;

            var showTransform = self.UI3DDisplay.m_ShowObject.transform;
            if (self.UI3DDisplay.m_FitScaleRoot != null)
            {
                self.UI3DDisplay.m_FitScaleRoot.localScale = Vector3.one;
                showTransform.SetParent(self.UI3DDisplay.m_FitScaleRoot, true);
            }
            else
            {
                showTransform.SetParent(self.UI3DDisplay.transform, true);
            }

            self.m_ShowCameraCtrl ??= self.UI3DDisplay.m_ShowCamera.GetOrAddComponent<UI3DDisplayCamera>();
            if (self.m_ShowCameraCtrl == null)
            {
                Debug.LogError($"必须有 UI3DDisplayCamera 组件 请检查");
                return;
            }

            //对象层级
            self.m_ShowCameraCtrl.ShowLayer  = self.m_ShowLayer;
            self.m_ShowCameraCtrl.ShowObject = self.UI3DDisplay.m_ShowObject;

            //动画屏幕外也可动
            self.SetAllAnimatorCullingMode(self.UI3DDisplay.m_ShowObject.transform);

            //位置大小旋转
            var showRotation = Quaternion.Euler(self.UI3DDisplay.m_ShowRotation);
            var showUp       = showRotation * Vector3.up;
            showRotation                *= Quaternion.AngleAxis(self.m_DragRotation, showUp);
            showTransform.localRotation =  showRotation;
            showTransform.localScale    =  self.UI3DDisplay.m_ShowScale;
            showTransform.localPosition =  self.m_ModelGlobalOffset + self.UI3DDisplay.m_ShowOffset;
            self.m_ShowPosition         =  showTransform.localPosition;

            //镜面反射
            if (self.UI3DDisplay.m_ReflectionPlane != null)
            {
                self.UI3DDisplay.m_ReflectionPlane.position = showTransform.position;
                self.UI3DDisplay.m_ReflectionPlane.rotation = showTransform.rotation;
            }

            //阴影
            self.DisableMeshRectShadow();
            if (self.UI3DDisplay.m_ShadowPlane != null)
            {
                self.EnableMeshRectShadow(self.UI3DDisplay.m_ShowObject.transform);
                self.UI3DDisplay.m_ShadowPlane.position = showTransform.position;
                self.UI3DDisplay.m_ShadowPlane.rotation = showTransform.rotation;
            }

            if (self.UI3DDisplay.m_FitScaleRoot != null)
            {
                var lossyScale = self.UI3DDisplay.m_FitScaleRoot.lossyScale;
                var localScale = self.UI3DDisplay.transform.localScale;
                self.UI3DDisplay.m_FitScaleRoot.localScale = new Vector3(1f / lossyScale.x * localScale.x, 1f / lossyScale.y * localScale.y, 1f / lossyScale.z * localScale.z);
            }
        }

        //更新摄像机配置根据传入的摄像机
        private static void UpdateLookCamera(this YIUI3DDisplayChild self, Camera lookCamera)
        {
            Assert.IsNotNull(self.UI3DDisplay.m_ShowCamera);

            lookCamera.gameObject.SetActive(false);
            self.UI3DDisplay.m_ShowCamera.orthographic     = lookCamera.orthographic;
            self.UI3DDisplay.m_ShowCamera.orthographicSize = lookCamera.orthographicSize;
            self.UI3DDisplay.m_ShowCamera.fieldOfView      = lookCamera.fieldOfView;
            self.UI3DDisplay.m_ShowCamera.nearClipPlane    = lookCamera.nearClipPlane;
            self.UI3DDisplay.m_ShowCamera.farClipPlane     = lookCamera.farClipPlane;
            self.UI3DDisplay.m_ShowCamera.orthographic     = lookCamera.orthographic;
            self.UI3DDisplay.m_ShowCamera.orthographicSize = lookCamera.orthographicSize;
            self.UI3DDisplay.m_ShowCamera.clearFlags       = CameraClearFlags.SolidColor;
            self.UI3DDisplay.m_ShowCamera.backgroundColor  = self.UI3DDisplay.m_UseLookCameraColor ? lookCamera.backgroundColor : Color.clear;
            self.m_OrthographicSize                        = lookCamera.orthographicSize;
            self.UI3DDisplay.m_LookCamera                  = lookCamera;

            self.UI3DDisplay.m_ShowCamera.cullingMask = 1 << self.m_ShowLayer;

            if (self.UI3DDisplay.m_ShowLight)
            {
                self.UI3DDisplay.m_ShowLight.cullingMask = self.UI3DDisplay.m_ShowCamera.cullingMask;
            }

            var lookCameraTsf = lookCamera.transform;
            if (lookCamera == self.UI3DDisplay.m_ShowCamera)
            {
                //当使用默认相机作为显示相机时 需要处理每个显示物体的额外偏移
                lookCamera.transform.localPosition = self.m_ShowCameraDefPos + self.m_ModelGlobalOffset;
            }

            self.UI3DDisplay.m_ShowCamera.transform.SetPositionAndRotation(lookCameraTsf.position, lookCameraTsf.rotation);

            self.UI3DDisplay.m_ShowCamera.enabled = true;
            self.UI3DDisplay.m_ShowCamera.gameObject.SetActive(true);
        }

        //RawImage 用的RenderTexture就是这样动态创建的
        private static void SetTemporaryRenderTexture(this YIUI3DDisplayChild self)
        {
            Assert.IsNotNull(self.UI3DDisplay.m_ShowImage);

            if (self.m_ShowTexture != null)
            {
                RenderTexture.ReleaseTemporary(self.m_ShowTexture);
            }

            self.m_ShowTexture                          = RenderTexture.GetTemporary(self.UI3DDisplay.m_ResolutionX, self.UI3DDisplay.m_ResolutionY, self.UI3DDisplay.m_RenderTextureDepthBuffer);
            self.UI3DDisplay.m_ShowImage.texture        = self.m_ShowTexture;
            self.UI3DDisplay.m_ShowCamera.targetTexture = self.m_ShowTexture;
            self.UI3DDisplay.m_ShowImage.enabled        = true;
        }

        //阴影采集
        private static void EnableMeshRectShadow(this YIUI3DDisplayChild self, Transform goNode)
        {
            var render = goNode.GetComponent<SkinnedMeshRenderer>();
            if (render != null)
            {
                if (render.shadowCastingMode == UnityEngine.Rendering.ShadowCastingMode.Off)
                {
                    render.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                    self.m_RenderList.Add(render);
                }
            }

            for (var i = 0; i < goNode.childCount; ++i)
            {
                self.EnableMeshRectShadow(goNode.GetChild(i));
            }
        }

        //关闭所有已采集的阴影
        private static void DisableMeshRectShadow(this YIUI3DDisplayChild self)
        {
            foreach (var render in self.m_RenderList)
            {
                render.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            }

            self.m_RenderList.Clear();
        }

        //交换显示的rawImage  一般不需要
        private static void ExchangeShowImage(this YIUI3DDisplayChild self, RawImage img)
        {
            self.UI3DDisplay.m_ShowImage = img;
            if (null != self.m_ShowTexture)
            {
                self.UI3DDisplay.m_ShowImage.texture = self.m_ShowTexture;
            }
            else
            {
                self.m_ShowTexture                          = RenderTexture.GetTemporary(self.UI3DDisplay.m_ResolutionX, self.UI3DDisplay.m_ResolutionY, self.UI3DDisplay.m_RenderTextureDepthBuffer);
                self.UI3DDisplay.m_ShowImage.texture        = self.m_ShowTexture;
                self.UI3DDisplay.m_ShowCamera.targetTexture = self.m_ShowTexture;
            }
        }
    }
}
