//------------------------------------------------------------
// Author: 亦亦
// Mail: 379338943@qq.com
// Data: 2023年2月12日
//------------------------------------------------------------

using UnityEngine;
using YIUIFramework;

namespace ET.Client
{
    public static partial class YIUIFactory
    {
        #if !YIUIMACRO_SYNCLOAD_CLOSE
        //普通的UI预制体 创建与摧毁 一定要成对
        //为了防止忘记 所以默认自动回收
        public static GameObject InstantiateGameObject(Scene scene, string pkgName, string resName)
        {
            var obj = scene.YIUILoad()?.LoadAssetInstantiate(pkgName, resName);
            if (obj == null)
            {
                Debug.LogError($"没有加载到这个资源 {pkgName}/{resName}");
                return null;
            }

            //强制添加 既然你要使用这个方法那就必须接受 否则请使用其他方式
            //被摧毁时 自动回收 无需调用 UIFactory.Destroy
            obj.AddComponent<YIUIReleaseInstantiate>().m_EntityRef = scene;

            return obj;
        }
        #endif

        public static async ETTask<GameObject> InstantiateGameObjectAsync(Scene scene, string pkgName, string resName)
        {
            EntityRef<Scene> sceneRef = scene;
            var obj = await scene.YIUILoad()?.LoadAssetAsyncInstantiate(pkgName, resName);
            if (obj == null)
            {
                Debug.LogError($"没有加载到这个资源 {pkgName}/{resName}");
                return null;
            }

            obj.AddComponent<YIUIReleaseInstantiate>().m_EntityRef = sceneRef.Entity;

            return obj;
        }
    }
}