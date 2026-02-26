using System;
using System.Collections.Generic;

namespace ET
{
    [ComponentOf]
    public class HashWait : Entity, IAwake, IDestroy
    {
        public Dictionary<long, ETTask<EHashWaitError>> m_HashWaitTasks = new();
    }
}