using System;

namespace ET
{
    /// <summary>
    /// 额外增加的异步助手 用于等待完成
    /// </summary>
    public static class ETTaskHelperExtend
    {
        public static async ETTask WaitUntil(Func<bool> func)
        {
            while (true)
            {
                await TimerComponent.Instance.WaitFrameAsync();
                if (func == null || func.Invoke()) return;
            }
        }
    }
}