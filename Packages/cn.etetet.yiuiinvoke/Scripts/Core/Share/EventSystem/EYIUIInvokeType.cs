namespace ET
{
    /// <summary>
    /// YIUI公共Invoke事件类型
    /// </summary>
    [UniqueId]
    public static partial class EYIUIInvokeType
    {
        /*
         为了让一个invoke 定义实现多个效果而设计
         优点: 不需要重复设计
         缺点: 调用者与响应者需要一一对应 防止出错
         这里是因为YIUI底层设计
         调用时清楚调用
         如果不清楚的禁止使用
         请自行实现

         const 作为枚举使用 方便部类扩展
         防止以后改ID 为了不受影响 所以非0开始 (默认不使用0)
         */

        //    [Invoke(EYIUIInvokeType.Sync)] 
        public const long Sync           = 1000; //同步 
        public const long SyncHandler_1  = 1001; //同步返回类型X 由Handler后缀标识
        public const long SyncHandler_2  = 1002;
        public const long SyncHandler_3  = 1003;
        public const long SyncHandler_4  = 1004;
        public const long SyncHandler_5  = 1005;
        public const long SyncHandler_6  = 1006;
        public const long SyncHandler_7  = 1007;
        public const long SyncHandler_8  = 1008;
        public const long SyncHandler_9  = 1009;
        public const long SyncHandler_10 = 1010;

        //    [Invoke(EYIUIInvokeType.Async)]
        public const long Async           = 2000; //异步
        public const long AsyncHandler_1  = 2001; //异步返回类型X 由Handler后缀标识
        public const long AsyncHandler_2  = 2002;
        public const long AsyncHandler_3  = 2003;
        public const long AsyncHandler_4  = 2004;
        public const long AsyncHandler_5  = 2005;
        public const long AsyncHandler_6  = 2006;
        public const long AsyncHandler_7  = 2007;
        public const long AsyncHandler_8  = 2008;
        public const long AsyncHandler_9  = 2009;
        public const long AsyncHandler_10 = 2010;
    }
}
