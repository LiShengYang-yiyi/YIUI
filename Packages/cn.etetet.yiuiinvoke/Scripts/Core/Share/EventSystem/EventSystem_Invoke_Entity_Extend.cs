namespace ET
{
    public static class EventSystem_InvokeEntity_YIUIExtension
    {
        public static void YIUIInvokeEntity<A>(this EventSystem self, Entity entity, long invokeType, A args) where A : struct
        {
            self.InvokeEntity(entity, invokeType, args);
        }

        public static T YIUIInvokeEntity<A, T>(this EventSystem self, Entity entity, long invokeType, A args) where A : struct
        {
            return self.InvokeEntity<A, T>(entity, invokeType, args);
        }

        public static void YIUIInvokeEntitySync<A>(this EventSystem self, Entity entity, A args) where A : struct
        {
            self.YIUIInvokeEntity(entity, EYIUIInvokeType.Sync, args);
        }

        public static T YIUIInvokeEntitySync<A, T>(this EventSystem self, Entity entity, A args) where A : struct
        {
            return self.YIUIInvokeEntity<A, T>(entity, EYIUIInvokeType.Sync, args);
        }

        public static void YIUIInvokeEntityAsync<A>(this EventSystem self, Entity entity, A args) where A : struct
        {
            self.YIUIInvokeEntity(entity, EYIUIInvokeType.Async, args);
        }

        public static T YIUIInvokeEntityAsync<A, T>(this EventSystem self, Entity entity, A args) where A : struct
        {
            return self.YIUIInvokeEntity<A, T>(entity, EYIUIInvokeType.Async, args);
        }
    }
}