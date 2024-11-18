namespace ET
{
    //等一帧 (1毫秒)
    public struct YIUIInvokeWaitFrameAsync
    {
    }

    //等指定毫秒
    public struct YIUIInvokeWaitAsync
    {
        public long Time;
    }

    //协程锁
    public struct YIUIInvokeCoroutineLock
    {
        public int LockType;
        public long Lock;
    }
}