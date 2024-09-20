//------------------------------------------------------------
// Author: 亦亦
// Mail: 379338943@qq.com
// Data: 2023年2月12日
//------------------------------------------------------------

using UnityEngine;
using Cysharp.Threading.Tasks;

namespace YIUIFramework
{
    public static partial class YIUIFactory
    {
        /// <summary> 单纯的实例化一个预制体 </summary>
        /// 与UI框架无关，由自己管理回收
        public static GameObject InstantiateGameObject(string pkgName, string resName)
        {
            var obj = YIUILoadHelper.LoadAssetInstantiate(pkgName, resName);
            if (obj == null)
            {
                Debug.LogError($"没有加载到这个资源 {pkgName}/{resName}");
                return null;
            }
            
            // TIP: 想办法在预制体销毁的时候调用回收
            obj.AddComponent<YIUIReleaseInstantiate>();

            return obj;
        }

        /// <summary> 单纯的实例化一个预制体 </summary>
        /// 与UI框架无关，由自己管理回收
        public static async UniTask<GameObject> InstantiateGameObjectAsync(string pkgName, string resName)
        {
            var obj = await YIUILoadHelper.LoadAssetAsyncInstantiate(pkgName, resName);
            if (obj == null)
            {
                Debug.LogError($"没有加载到这个资源 {pkgName}/{resName}");
                return null;
            }

            // TIP: 想办法在预制体销毁的时候调用回收
            obj.AddComponent<YIUIReleaseInstantiate>();

            return obj;
        }

        /// <summary> 回收游戏对象 </summary>
        public static void Destroy(GameObject obj)
        {
            YIUILoadHelper.ReleaseInstantiate(obj);
        }
    }
}