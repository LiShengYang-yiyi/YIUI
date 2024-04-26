using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace YIUIFramework
{
    //小型异步锁
    public class AsyncLockMgr : MgrSingleton<AsyncLockMgr>
    {
        private readonly Dictionary<long, AsyncLockComponent> m_SemaphoreSlims = new Dictionary<long, AsyncLockComponent>();

        private readonly Dictionary<long, int> m_SemaphoreSlimsRefCount = new Dictionary<long, int>();

        internal bool Release(long key)
        {
            if (m_SemaphoreSlimsRefCount.ContainsKey(key))
            {
                m_SemaphoreSlimsRefCount[key] -= 1;
                if (m_SemaphoreSlimsRefCount[key] <= 0)
                {
                    m_SemaphoreSlimsRefCount.Remove(key);
                    m_SemaphoreSlims.Remove(key);
                    return true;
                }
            }

            return false;
        }

        //这就是一个小型的异步锁
        //不考虑其他非常丰富的功能
        //只能锁定一个key 注意一个对象你要同时开多个不同类型锁时 需要用不同的key
        //推荐使用GetHashCode做Key 但是一个对象只能同时一个
        public async UniTask<AsyncLockComponent> Wait(long key, int millisecondsTimeout = -1)
        {
            if (!m_SemaphoreSlims.TryGetValue(key, out var semaphoreSlimComponent))
            {
                semaphoreSlimComponent = RefPool.Get<AsyncLockComponent>();
                semaphoreSlimComponent.Reset(key, millisecondsTimeout);
                m_SemaphoreSlims.Add(key, semaphoreSlimComponent);
                m_SemaphoreSlimsRefCount.Add(key, 0);
            }

            m_SemaphoreSlimsRefCount[key] += 1;
            return await semaphoreSlimComponent.WaitAsync();
        }
    }
}