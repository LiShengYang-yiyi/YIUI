#if ET10
namespace ET
{
    /// <summary>
    /// 特殊情况下使用的 提前检查一个InvokeEntity是否存在
    /// </summary>
    public static class EventSystem_Check_Invoke_Entity
    {
        public static bool CheckInvokeEntity<A>(this EventSystem self, long type) where A : struct
        {
            return self.GetInvoker<AInvokeEntityHandler<A>, A>(type) != null;
        }

        public static bool CheckInvokeEntity<A, T>(this EventSystem self, long type) where A : struct
        {
            return self.GetInvoker<AInvokeEntityHandler<A, T>, A>(type) != null;
        }

        public static bool CheckInvokeEntity<A>(this EventSystem self) where A : struct
        {
            return self.CheckInvokeEntity<A>(0);
        }

        public static bool CheckInvokeEntity<A, T>(this EventSystem self) where A : struct
        {
            return self.CheckInvokeEntity<A, T>(0);
        }
    }
}
#else
namespace ET
{
    /// <summary>
    /// 特殊情况下使用的 提前检查一个InvokeEntity是否存在
    /// </summary>
    public partial class EventSystem
    {
        public bool CheckInvokeEntity<A>(long type) where A : struct
        {
            if (!this.allInvokers.TryGetValue(typeof(A), out var invokeHandlers))
            {
                return false;
            }

            if (!invokeHandlers.TryGetValue(type, out var invokeHandler))
            {
                return false;
            }

            var aInvokeHandler = invokeHandler as AInvokeEntityHandler<A>;
            if (aInvokeHandler == null)
            {
                return false;
            }

            return true;
        }

        public bool CheckInvokeEntity<A, T>(long type) where A : struct
        {
            if (!this.allInvokers.TryGetValue(typeof(A), out var invokeHandlers))
            {
                return false;
            }

            if (!invokeHandlers.TryGetValue(type, out var invokeHandler))
            {
                return false;
            }

            var aInvokeHandler = invokeHandler as AInvokeEntityHandler<A, T>;
            if (aInvokeHandler == null)
            {
                return false;
            }

            return true;
        }

        public bool CheckInvokeEntity<A>() where A : struct
        {
            return CheckInvokeEntity<A>(0);
        }

        public bool CheckInvokeEntity<A, T>() where A : struct
        {
            return CheckInvokeEntity<A, T>(0);
        }
    }
}
#endif