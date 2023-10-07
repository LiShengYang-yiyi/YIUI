//------------------------------------------------------------
// Author: 亦亦
// Mail: 379338943@qq.com
// Data: 2023年2月12日
//------------------------------------------------------------

using UnityEngine;
using YIUIFramework;

namespace ET.Client
{
    /// <summary>
    /// UI主体
    /// </summary>
    [FriendOf(typeof (YIUIComponent))]
    public static partial class YIUIComponentSystem
    {
        [ObjectSystem]
        public class YIUIComponentAwakeSystem: AwakeSystem<YIUIComponent, YIUIBindVo, GameObject>
        {
            protected override void Awake(YIUIComponent self, YIUIBindVo uiBindVo, GameObject obj)
            {
                self.InitUIBase(uiBindVo, obj);
            }
        }

        [ObjectSystem]
        public class YIUIComponentDestroySystem: DestroySystem<YIUIComponent>
        {
            protected override void Destroy(YIUIComponent self)
            {
                if (self.OwnerGameObject != null)
                    UnityEngine.Object.Destroy(self.OwnerGameObject);
            }
        }
    }
}