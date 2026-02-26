using System;

namespace ET
{
    /// <summary>
    /// 额外增加的异步助手 用于等待完成
    /// </summary>
    public static class ETTaskHelperExtend
    {
        public static async ETTask WaitUntil(this Entity self, Func<bool> func)
        {
            var timer = self?.Root()?.GetComponent<TimerComponent>();
            if (timer == null)
            {
                return;
            }

            await timer.WaitUntil(func);
        }

        public static async ETTask WaitUntil(this TimerComponent self, Func<bool> func)
        {
            EntityRef<TimerComponent> timer = self;

            while (true)
            {
                if (timer.Entity == null)
                {
                    return;
                }

                await timer.Entity.WaitFrameAsync();

                if (func == null)
                {
                    return;
                }

                try
                {
                    if (func.Invoke())
                    {
                        return;
                    }
                }
                catch (Exception e)
                {
                    Log.Error($"WaitUntil Error: {e}");
                    return;
                }
            }
        }
    }
}