#if ET10
namespace ET
{
    /// <summary>
    /// 特殊情况下使用的 提前检查一个Invoke是否存在
    /// </summary>
    public static class EventSystemCheck_Invoke
    {
        public static bool CheckInvoke<A>(this EventSystem self, long type) where A : struct
        {
            return self.GetInvoker<AInvokeHandler<A>, A>(type) != null;
        }

        public static bool CheckInvoke<A, T>(this EventSystem self, long type) where A : struct
        {
            return self.GetInvoker<AInvokeHandler<A, T>, A>(type) != null;
        }

        public static bool CheckInvoke<A>(this EventSystem self) where A : struct
        {
            return self.CheckInvoke<A>(0);
        }

        public static bool CheckInvoke<A, T>(this EventSystem self) where A : struct
        {
            return self.CheckInvoke<A, T>(0);
        }
    }
}
#else
namespace ET
{
    /// <summary>
    /// 特殊情况下使用的 提前检查一个Invoke是否存在
    /// </summary>
    public partial class EventSystem
    {
        public bool CheckInvoke<A>(long type) where A : struct
        {
            if (!this.allInvokers.TryGetValue(typeof(A), out var invokeHandlers))
            {
                return false;
            }

            if (!invokeHandlers.TryGetValue(type, out var invokeHandler))
            {
                return false;
            }

            var aInvokeHandler = invokeHandler as AInvokeHandler<A>;
            if (aInvokeHandler == null)
            {
                return false;
            }

            return true;
        }

        public bool CheckInvoke<A, T>(long type) where A : struct
        {
            if (!this.allInvokers.TryGetValue(typeof(A), out var invokeHandlers))
            {
                return false;
            }

            if (!invokeHandlers.TryGetValue(type, out var invokeHandler))
            {
                return false;
            }

            var aInvokeHandler = invokeHandler as AInvokeHandler<A, T>;
            if (aInvokeHandler == null)
            {
                return false;
            }

            return true;
        }

        public bool CheckInvoke<A>() where A : struct
        {
            return CheckInvoke<A>(0);
        }

        public bool CheckInvoke<A, T>() where A : struct
        {
            return CheckInvoke<A, T>(0);
        }
    }
}
#endif