namespace ET
{
    public partial class YIUIInvokeSystem
    {
        public bool CheckInvokeReturn<R>(string invokeType)
        {
            var invoker = GetInvoker<IYIUIInvokeReturnHandler<R>>(invokeType, false);
            return invoker != null;
        }

        public bool CheckInvokeReturn<T1, R>(string invokeType)
        {
            var invoker = GetInvoker<IYIUIInvokeReturnHandler<T1, R>>(invokeType, false);
            return invoker != null;
        }

        public bool CheckInvokeReturn<T1, T2, R>(string invokeType)
        {
            var invoker = GetInvoker<IYIUIInvokeReturnHandler<T1, T2, R>>(invokeType, false);
            return invoker != null;
        }

        public bool CheckInvokeReturn<T1, T2, T3, R>(string invokeType)
        {
            var invoker = GetInvoker<IYIUIInvokeReturnHandler<T1, T2, T3, R>>(invokeType, false);
            return invoker != null;
        }

        public bool CheckInvokeReturn<T1, T2, T3, T4, R>(string invokeType)
        {
            var invoker = GetInvoker<IYIUIInvokeReturnHandler<T1, T2, T3, T4, R>>(invokeType, false);
            return invoker != null;
        }

        public bool CheckInvokeReturn<T1, T2, T3, T4, T5, R>(string invokeType)
        {
            var invoker = GetInvoker<IYIUIInvokeReturnHandler<T1, T2, T3, T4, T5, R>>(invokeType, false);
            return invoker != null;
        }

        public bool CheckInvokeTask(string invokeType)
        {
            var invoker = GetInvoker<IYIUIInvokeReturnHandler<ETTask>>(invokeType, false);
            return invoker != null;
        }

        public bool CheckInvokeTask<T1>(string invokeType)
        {
            var invoker = GetInvoker<IYIUIInvokeReturnHandler<T1, ETTask>>(invokeType, false);
            return invoker != null;
        }

        public bool CheckInvokeTask<T1, T2>(string invokeType)
        {
            var invoker = GetInvoker<IYIUIInvokeReturnHandler<T1, T2, ETTask>>(invokeType, false);
            return invoker != null;
        }

        public bool CheckInvokeTask<T1, T2, T3>(string invokeType)
        {
            var invoker = GetInvoker<IYIUIInvokeReturnHandler<T1, T2, T3, ETTask>>(invokeType, false);
            return invoker != null;
        }

        public bool CheckInvokeTask<T1, T2, T3, T4>(string invokeType)
        {
            var invoker = GetInvoker<IYIUIInvokeReturnHandler<T1, T2, T3, T4, ETTask>>(invokeType, false);
            return invoker != null;
        }

        public bool CheckInvokeTask<T1, T2, T3, T4, T5>(string invokeType)
        {
            var invoker = GetInvoker<IYIUIInvokeReturnHandler<T1, T2, T3, T4, T5, ETTask>>(invokeType, false);
            return invoker != null;
        }
    }
}