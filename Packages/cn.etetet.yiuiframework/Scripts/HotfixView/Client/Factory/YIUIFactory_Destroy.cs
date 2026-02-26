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
        internal static void Destroy(Scene scene, GameObject obj)
        {
            if (obj == null) return;
            UnityEngine.Object.Destroy(obj);
            EventSystem.Instance?.YIUIInvokeEntitySyncSafety(scene, new YIUIInvokeEntity_ReleaseInstantiate
            {
                obj = obj
            });
        }
    }
}