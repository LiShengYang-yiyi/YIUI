#if !YIUIMACRO_SYNCLOAD_CLOSE
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
        //同步显示对象
        //尽量使用异步方式 ShowAsync
        //已知如果同步与异步同时调用加载相同资源的情况会报错 等等特殊情况 请自行处理
        public static GameObject ShowSync(this YIUI3DDisplayChild self, string resName, string cameraName = "")
        {
            if (self.UI3DDisplay == null)
            {
                Debug.LogError($"没有3D显示组件");
                return null;
            }

            var obj = self.GetDisplayObject(resName);
            if (obj == null) return null;
            var camera = string.IsNullOrEmpty(cameraName) ? self.UI3DDisplay.m_ShowCamera : self.GetCamera(obj, cameraName);
            if (camera == null) return obj;
            self.ShowByGameObject(obj, camera);
            return obj;
        }

        private static GameObject GetDisplayObject(this YIUI3DDisplayChild self, string resName)
        {
            if (!self.m_ObjPool.ContainsKey(resName))
            {
                var newObj = self.CreateObject(resName);
                if (newObj == null) return null;
                self.m_ObjPool.Add(resName, newObj);
            }

            return self.m_ObjPool[resName];
        }

        private static GameObject CreateObject(this YIUI3DDisplayChild self, string resName)
        {
            var obj = YIUIFactory.InstantiateGameObject(self.Scene(), "", resName);
            if (obj == null) return null;
            return obj;
        }
    }
}
#endif