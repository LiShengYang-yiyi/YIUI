using System;

namespace ET
{
    public partial class YIUIInvokeSystem
    {
        public void SafetyInvoke<T>(T self, string invokeType) where T : Entity
        {
            if (!CheckInvoke(invokeType))
            {
                return;
            }

            Invoke(self, invokeType);
        }

        public void SafetyInvoke<T, T1>(T self, string invokeType, T1 arg1) where T : Entity
        {
            if (!CheckInvoke<T1>(invokeType))
            {
                return;
            }

            Invoke(self, invokeType, arg1);
        }

        public void SafetyInvoke<T, T1, T2>(T self, string invokeType, T1 arg1, T2 arg2) where T : Entity
        {
            if (!CheckInvoke<T1, T2>(invokeType))
            {
                return;
            }

            Invoke(self, invokeType, arg1, arg2);
        }

        public void SafetyInvoke<T, T1, T2, T3>(T self, string invokeType, T1 arg1, T2 arg2, T3 arg3) where T : Entity
        {
            if (!CheckInvoke<T1, T2, T3>(invokeType))
            {
                return;
            }

            Invoke(self, invokeType, arg1, arg2, arg3);
        }

        public void SafetyInvoke<T, T1, T2, T3, T4>(T self, string invokeType, T1 arg1, T2 arg2, T3 arg3, T4 arg4) where T : Entity
        {
            if (!CheckInvoke<T1, T2, T3, T4>(invokeType))
            {
                return;
            }

            Invoke(self, invokeType, arg1, arg2, arg3, arg4);
        }

        public void SafetyInvoke<T, T1, T2, T3, T4, T5>(T self, string invokeType, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) where T : Entity
        {
            if (!CheckInvoke<T1, T2, T3, T4, T5>(invokeType))
            {
                return;
            }

            Invoke(self, invokeType, arg1, arg2, arg3, arg4, arg5);
        }
    }
}