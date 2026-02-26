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
    [FriendOf(typeof(YIUI3DDisplayChild))]
    [EntitySystemOf(typeof(YIUI3DDisplayChild))]
    public static partial class YIUI3DDisplayChildSystem
    {
        [EntitySystem]
        private static void Awake(this YIUI3DDisplayChild self, UI3DDisplay ui3DDisplay)
        {
            self.m_UI3DDisplay                       = ui3DDisplay;
            self.m_OnClickedEntity                   = self.Parent;
            self.UI3DDisplay.m_YIUI3DDisplayChildRef = self;
            self.Awake3DDisplay();
        }

        [EntitySystem]
        private static void Destroy(this YIUI3DDisplayChild self)
        {
            self.Destroy3DDisplay();
            self.UI3DDisplay.m_YIUI3DDisplayChildRef = default;
            self.m_UI3DDisplay                       = null;
            self.m_OnClickedEntity                   = default;
        }
    }
}
