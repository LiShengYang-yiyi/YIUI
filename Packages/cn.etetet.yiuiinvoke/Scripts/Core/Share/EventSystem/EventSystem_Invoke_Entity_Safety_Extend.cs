namespace ET
{
    public static class EventSystem_InvokeEntity_YIUISafetyExtension
    {
        public static void YIUIInvokeEntitySafety<A>(this EventSystem self, Entity entity, long invokeType, A args) where A : struct
        {
            if (self == null || entity == null || entity.IsDisposed) return;
            self.InvokeEntity(entity, invokeType, args);
        }

        public static T YIUIInvokeEntitySafety<A, T>(this EventSystem self, Entity entity, long invokeType, A args) where A : struct
        {
            if (self == null || entity == null || entity.IsDisposed) return default;
            return self.InvokeEntity<A, T>(entity, invokeType, args);
        }

        public static void YIUIInvokeEntitySyncSafety<A>(this EventSystem self, Entity entity, A args) where A : struct
        {
            if (self == null || entity == null || entity.IsDisposed) return;
            self.YIUIInvokeEntity(entity, EYIUIInvokeType.Sync, args);
        }

        public static T YIUIInvokeEntitySyncSafety<A, T>(this EventSystem self, Entity entity, A args) where A : struct
        {
            if (self == null || entity == null || entity.IsDisposed) return default;
            return self.YIUIInvokeEntity<A, T>(entity, EYIUIInvokeType.Sync, args);
        }

        public static void YIUIInvokeEntityAsyncSafety<A>(this EventSystem self, Entity entity, A args) where A : struct
        {
            if (self == null || entity == null || entity.IsDisposed) return;
            self.YIUIInvokeEntity(entity, EYIUIInvokeType.Async, args);
        }

        public static T YIUIInvokeEntityAsyncSafety<A, T>(this EventSystem self, Entity entity, A args) where A : struct
        {
            if (self == null || entity == null || entity.IsDisposed) return default;
            return self.YIUIInvokeEntity<A, T>(entity, EYIUIInvokeType.Async, args);
        }
    }
}