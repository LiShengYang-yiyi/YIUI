#if ET10
using System;

namespace ET
{
    /// <summary>
    /// InvokeEntity事件
    /// </summary>
    public static class YIUIEventSystem
    {
        public static void InvokeEntity<A>(this EventSystem self, Entity entity, long type, A args) where A : struct
        {
            var aInvokeHandler = self.GetInvoker<AInvokeEntityHandler<A>, A>(type);
            if (aInvokeHandler == null)
            {
                throw new Exception($"Invoke error3, not AInvokeHandler: {type} {typeof(A).FullName}");
            }

            aInvokeHandler.Handle(entity, args);
        }

        public static T InvokeEntity<A, T>(this EventSystem self, Entity entity, long type, A args) where A : struct
        {
            var aInvokeHandler = self.GetInvoker<AInvokeEntityHandler<A, T>, A>(type);
            if (aInvokeHandler == null)
            {
                throw new Exception($"Invoke error6, not AInvokeHandler: {type} {typeof(A).FullName} {typeof(T).FullName} ");
            }

            return aInvokeHandler.Handle(entity, args);
        }

        public static void InvokeEntity<A>(this EventSystem self, Entity entity, A args) where A : struct
        {
            self.InvokeEntity(entity, 0, args);
        }

        public static T InvokeEntity<A, T>(this EventSystem self, Entity entity, A args) where A : struct
        {
            return self.InvokeEntity<A, T>(entity, 0, args);
        }
    }
}
#else
using System;

namespace ET
{
    /// <summary>
    /// InvokeEntity事件
    /// </summary>
    public partial class EventSystem
    {
        public void InvokeEntity<A>(Entity entity, long type, A args) where A : struct
        {
            if (!this.allInvokers.TryGetValue(typeof(A), out var invokeHandlers))
            {
                throw new Exception($"Invoke error1: {type} {typeof(A).FullName}");
            }

            if (!invokeHandlers.TryGetValue(type, out var invokeHandler))
            {
                throw new Exception($"Invoke error2: {type} {typeof(A).FullName}");
            }

            var aInvokeHandler = invokeHandler as AInvokeEntityHandler<A>;
            if (aInvokeHandler == null)
            {
                throw new Exception($"Invoke error3, not AInvokeHandler: {type} {typeof(A).FullName}");
            }

            aInvokeHandler.Handle(entity, args);
        }

        public T InvokeEntity<A, T>(Entity entity, long type, A args) where A : struct
        {
            if (!this.allInvokers.TryGetValue(typeof(A), out var invokeHandlers))
            {
                throw new Exception($"Invoke error4: {type} {typeof(A).FullName}");
            }

            if (!invokeHandlers.TryGetValue(type, out var invokeHandler))
            {
                throw new Exception($"Invoke error5: {type} {typeof(A).FullName}");
            }

            var aInvokeHandler = invokeHandler as AInvokeEntityHandler<A, T>;
            if (aInvokeHandler == null)
            {
                throw new Exception($"Invoke error6, not AInvokeHandler: {type} {typeof(A).FullName} {typeof(T).FullName} ");
            }

            return aInvokeHandler.Handle(entity, args);
        }

        public void InvokeEntity<A>(Entity entity, A args) where A : struct
        {
            InvokeEntity(entity, 0, args);
        }

        public T InvokeEntity<A, T>(Entity entity, A args) where A : struct
        {
            return InvokeEntity<A, T>(entity, 0, args);
        }
    }
}
#endif