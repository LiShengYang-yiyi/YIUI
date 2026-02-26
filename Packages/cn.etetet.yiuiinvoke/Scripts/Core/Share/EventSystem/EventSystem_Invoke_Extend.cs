namespace ET
{
    public static class EventSystem_YIUIExtension
    {
        public static void YIUIInvoke<A>(this EventSystem self, long invokeType, A args) where A : struct
        {
            self?.Invoke(invokeType, args);
        }

        public static T YIUIInvoke<A, T>(this EventSystem self, long invokeType, A args) where A : struct
        {
            return self.Invoke<A, T>(invokeType, args);
        }

        public static void YIUIInvokeSync<A>(this EventSystem self, A args) where A : struct
        {
            self.YIUIInvoke(EYIUIInvokeType.Sync, args);
        }

        public static T YIUIInvokeSync<A, T>(this EventSystem self, A args) where A : struct
        {
            return self.YIUIInvoke<A, T>(EYIUIInvokeType.Sync, args);
        }

        public static void YIUIInvokeAsync<A>(this EventSystem self, A args) where A : struct
        {
            self.YIUIInvoke(EYIUIInvokeType.Async, args);
        }

        public static T YIUIInvokeAsync<A, T>(this EventSystem self, A args) where A : struct
        {
            return self.YIUIInvoke<A, T>(EYIUIInvokeType.Async, args);
        }
    }
}