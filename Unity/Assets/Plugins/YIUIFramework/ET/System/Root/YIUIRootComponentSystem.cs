﻿//------------------------------------------------------------
// Author: 亦亦
// Mail: 379338943@qq.com
// Data: 2023年2月12日
//------------------------------------------------------------

namespace ET.Client
{
    [FriendOf(typeof (YIUIRootComponent))]
    public static partial class YIUIRootComponentSystem
    {
        [ObjectSystem]
        public class YIUIRootComponentAwakeSystem: AwakeSystem<YIUIRootComponent>
        {
            protected override void Awake(YIUIRootComponent self)
            {
            }
        }
    }
}