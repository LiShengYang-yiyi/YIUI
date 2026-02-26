using System;
using UnityEngine;
using YIUIFramework;
using System.Collections.Generic;

namespace ET.Client
{
    public partial class LoopScrollVerticalGroupViewComponent : Entity
    {
        public EntityRef<YIUILoopScrollChild> m_Loop;
        public YIUILoopScrollChild Loop => m_Loop;
    }
}