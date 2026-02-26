#if ET10
namespace ET
{
    public static class ETTask_Extend
    {
        public static void NoContext(this ETTask self)
        {
            self.Coroutine();
        }

        public static void WithContext(this ETTask self, object context)
        {
            self.Coroutine(context);
        }

        public static void NoContext<T>(this ETTask<T> self)
        {
            self.Coroutine();
        }

        public static void WithContext<T>(this ETTask<T> self, object context)
        {
            self.Coroutine(context);
        }
    }
}
#endif