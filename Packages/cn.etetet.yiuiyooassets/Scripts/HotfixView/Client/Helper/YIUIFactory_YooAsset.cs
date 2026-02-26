using UnityEngine;

namespace ET.Client
{
    public static partial class YIUIFactory
    {
        public static async ETTask<GameObject> InstantiateGameObjectAsync(Scene scene, string resName)
        {
            return await InstantiateGameObjectAsync(scene, "", resName);
        }
    }
}