#if !ET10
using System;
using System.Collections;
using System.Collections.Generic;

namespace ET
{
    public static partial class TimerComponentSystem
    {
        /// <summary>
        /// 传入的 timerAction的object 是一个 IPool 对象，需要在移除时调用 Dispose 方法
        /// </summary>
        public static bool RemovePool(this TimerComponent self, ref long id)
        {
            if (id == 0)
            {
                return false;
            }

            long i = id;
            id = 0;

            if (!self.timerActions.Remove(i, out TimerAction timerAction))
            {
                return false;
            }

            if (timerAction.Object is IPool objectPool)
            {
                objectPool.Dispose();
            }

            return true;
        }
    }
}
#endif