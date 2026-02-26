#if ET10
using System.Collections.Generic;

namespace ET
{
    public static class ETTaskHelper
    {
        public static async ETTask WaitAny(List<ETTask> tasks)
        {
            await ETTask.WaitAny(tasks);
        }

        public static async ETTask WaitAny(ETTask[] tasks)
        {
            await ETTask.WaitAny(tasks);
        }

        public static async ETTask WaitAll(ETTask[] tasks)
        {
            await ETTask.WaitAll(tasks);
        }

        public static async ETTask WaitAll(List<ETTask> tasks)
        {
            await ETTask.WaitAll(tasks);
        }

        public static async ETTask<T> GetContextAsync<T>() where T : class
        {
            return await ETTask.GetContextAsync<T>();
        }

        public static async ETTask<object> GetContextAsync()
        {
            return await ETTask.GetContextAsync();
        }
    }
}
#endif