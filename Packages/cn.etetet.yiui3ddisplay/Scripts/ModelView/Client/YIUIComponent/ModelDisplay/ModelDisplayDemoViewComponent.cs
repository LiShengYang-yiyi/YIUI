using System;
using UnityEngine;
using YIUIFramework;
using System.Collections.Generic;

namespace ET.Client
{
    /// <summary>
    /// 显示模型的Demo
    /// </summary>
    public partial class ModelDisplayDemoViewComponent : Entity, IYIUIOpen<ParamVo>
    {
        public EntityRef<YIUI3DDisplayChild> m_Display;
        public YIUI3DDisplayChild            Display => m_Display;
    }
}