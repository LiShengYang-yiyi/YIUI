using System;
using System.Collections.Generic;

namespace ET.Client
{
    /// <summary>
    /// UI通用等待
    /// </summary>
    [ComponentOf(typeof(YIUIWindowComponent))]
    public class YIUIWaitComponent : Entity, IAwake<long>, IDestroy, IYIUIWindowClose
    {
        public bool m_IsWaitCompleted;
        public long m_WaitId;
    }
}