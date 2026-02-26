using System;
using System.Collections.Generic;
using UnityEngine;
using YIUIFramework;

namespace ET.Client
{
    /// <summary>
    /// 3DDisplay的扩展组件
    /// 文档: https://lib9kmxvq7k.feishu.cn/wiki/FhGGwVZSyiCqHCkTVQYcKHQCnKf
    /// </summary>
    public static partial class YIUI3DDisplayChildSystem
    {
        //可动态设置改变多目标 但是尽量不要
        public static void ResetMultipleTargetMode(this YIUI3DDisplayChild self, bool value)
        {
            self.MultipleTargetMode = value;
            if (value)
            {
                self.InitRotationData();
            }
            else
            {
                self.ClearMultipleData();
            }
        }

        //初始化
        private static void InitRotationData(this YIUI3DDisplayChild self)
        {
            if (self.m_InitMultipleData) return;

            self.m_AllMultipleTarget = new List<GameObject>();
            self.m_MultipleCache     = new Dictionary<GameObject, GameObject>();
            self.m_InitMultipleData  = true;
        }

        //清除
        private static void ClearMultipleData(this YIUI3DDisplayChild self)
        {
            self.m_AllMultipleTarget = null;
            self.m_MultipleCache     = null;
            self.m_InitMultipleData  = false;
            self.m_DragTarge         = self.UI3DDisplay.m_ShowObject;
        }

        //添加目标
        public static void AddMultipleTarget(this YIUI3DDisplayChild self, GameObject obj, Camera lookCamera = null, Transform parent = null)
        {
            if (self.UI3DDisplay.m_ShowObject == null)
            {
                Debug.LogError($"多目标 使用前 需要一个父级对象");
                return;
            }

            if (!self.m_InitMultipleData)
            {
                Debug.LogError("多目标模式 未初始化");
                return;
            }

            SetPosAndRotAndParent(obj.transform, lookCamera, parent);

            self.SetAllAnimatorCullingMode(obj.transform);

            self.SetColliderLayer(obj);

            self.SetupShowLayerTarget(obj.transform);

            self.m_AllMultipleTarget.Add(obj);
        }

        //移除目标
        public static void RemoveMultipleTarget(this YIUI3DDisplayChild self, GameObject obj)
        {
            if (!self.m_InitMultipleData)
            {
                Debug.LogError("多目标模式 未初始化");
                return;
            }

            self.m_AllMultipleTarget.Remove(obj);
        }

        //获取点击目标的父级对象
        private static GameObject GetMultipleTargetByClick(this YIUI3DDisplayChild self, GameObject child)
        {
            var obj = self.GetMultipleCache(child);
            if (obj != null)
                return obj;

            if (self.IsMultipleTarget(child))
                return child;

            var parent = child.transform.parent;
            if (parent)
                return self.GetMultipleTargetByClick(parent.gameObject);

            Debug.LogError($"没有找到这个对象 {child.name}");
            return null;
        }

        private static GameObject GetMultipleCache(this YIUI3DDisplayChild self, GameObject obj)
        {
            self.m_MultipleCache.TryGetValue(obj, out var rotationObj);
            return rotationObj;
        }

        private static bool IsMultipleTarget(this YIUI3DDisplayChild self, GameObject obj)
        {
            foreach (var item in self.m_AllMultipleTarget)
            {
                if (item == obj)
                {
                    self.m_MultipleCache.Add(obj, item);
                    return true;
                }
            }

            return false;
        }

        //多目标下 设置位置旋转父级
        private static void SetPosAndRotAndParent(Transform target, Camera lookCamera = null, Transform parent = null)
        {
            if (parent)
                target.SetParent(parent, false);

            target.localPosition = Vector3.zero;

            target.localScale = Vector3.one;

            target.localRotation = lookCamera ? lookCamera.transform.localRotation : Quaternion.identity;
        }
    }
}
