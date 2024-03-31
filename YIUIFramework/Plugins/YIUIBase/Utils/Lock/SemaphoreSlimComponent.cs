using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace YIUIFramework
{
    public class SemaphoreSlimComponent : IDisposable, IRefPool
    {
        private readonly SemaphoreSlim m_Semaphore = new SemaphoreSlim(1, 1);
        private long m_Key;
        private int m_MillisecondsTimeout;

        public SemaphoreSlimComponent()
        {
        }

        public void Reset(long key, int millisecondsTimeout = -1)
        {
            m_Key = key;
            m_MillisecondsTimeout = millisecondsTimeout;
        }

        public async UniTask<SemaphoreSlimComponent> WaitAsync()
        {
            await m_Semaphore.WaitAsync(m_MillisecondsTimeout);
            return this;
        }

        public void Dispose()
        {
            m_Semaphore.Release();
            if (SemaphoreSlimSingleton.Inst.Release(m_Key))
            {
                RefPool.Put(this);
            }
        }

        public void Recycle()
        {
            Reset(0);
        }
    }
}