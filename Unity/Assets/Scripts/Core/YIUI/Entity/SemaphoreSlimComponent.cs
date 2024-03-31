using System;
using System.Threading;

namespace ET
{
    public class SemaphoreSlimComponent : IDisposable, IPool
    {
        public bool IsFromPool { get; set; }
        private readonly SemaphoreSlim m_Semaphore = new(1, 1);
        private long m_Key;
        private int m_MillisecondsTimeout;

        public SemaphoreSlimComponent()
        {
        }

        public async ETTask<SemaphoreSlimComponent> WaitAsync()
        {
            await m_Semaphore.WaitAsync(m_MillisecondsTimeout);
            return this;
        }

        public static SemaphoreSlimComponent Get(long key, int millisecondsTimeout = -1)
        {
            var semaphoreSlimComponent = ObjectPool.Instance.Fetch<SemaphoreSlimComponent>();
            semaphoreSlimComponent.m_Key = key;
            semaphoreSlimComponent.m_MillisecondsTimeout = millisecondsTimeout;
            return semaphoreSlimComponent;
        }

        public void Dispose()
        {
            m_Semaphore.Release();
            if (SemaphoreSlimSingleton.Instance.Release(m_Key))
            {
                m_Key = 0;
                m_MillisecondsTimeout = -1;
                ObjectPool.Instance.Recycle(this);
            }
        }
    }
}