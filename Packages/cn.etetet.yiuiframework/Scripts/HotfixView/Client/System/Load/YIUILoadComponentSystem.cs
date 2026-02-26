//------------------------------------------------------------
// Author: 亦亦
// Mail: 379338943@qq.com
// Data: 2023年2月12日
//------------------------------------------------------------

using System;

namespace ET.Client
{
    /// <summary>
    /// UI面板组件
    /// </summary>
    [FriendOf(typeof(YIUILoadComponent))]
    [EntitySystemOf(typeof(YIUILoadComponent))]
    public static partial class YIUILoadComponentSystem
    {
        [EntitySystem]
        private static void Awake(this YIUILoadComponent self)
        {
        }

        [EntitySystem]
        private static void Destroy(this YIUILoadComponent self)
        {
        }

        public static async ETTask<bool> Initialize(this YIUILoadComponent self)
        {
            return await EventSystem.Instance.InvokeEntity<YIUIInvokeEntity_LoadInitialize, ETTask<bool>>(self, new YIUIInvokeEntity_LoadInitialize());
        }
    }
}