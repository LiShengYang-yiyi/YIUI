using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using YIUIBind;

namespace YIUIFramework
{
    /// <summary>
    /// 3DDisplay的扩展
    /// 自带创建与对象池 基础需求使用更简单
    /// </summary>
    public partial class YIUI3DDisplayExtend
    {
        public async UniTaskVoid ShowAsync(string resName, string cameraName = "Camera")
        {
            var obj = await GetDisplayObjectAsync(resName);
            if (obj == null) return;
            var camera = GetCamera(obj, cameraName);
            if (camera == null) return;
            m_UI3DDisplay.Show(obj, camera);
        }

        private async UniTask<GameObject> GetDisplayObjectAsync(string resName)
        {
            if (!m_ObjPool.ContainsKey(resName))
            {
                var newObj = await CreateObjectAsync(resName);
                m_ObjPool.Add(resName, newObj);
            }

            return m_ObjPool[resName];
        }

        private async UniTask<GameObject> CreateObjectAsync(string resName)
        {
            var obj = await YIUIFactory.InstantiateGameObjectAsync("", resName);
            if (obj == null)
            {
                Debug.LogError($"实例化失败 {resName}  请检查为何没有加载成功 是否配置");
            }

            return obj;
        }
    }
}