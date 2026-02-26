using System;

namespace ET.Client
{
    /// <summary>
    /// 关闭事件 如果没有则算成功
    /// 代表UI正在关闭中  在关闭前来的消息 可以根据实际需求使用
    /// 因为是异步的 所以在过程中注意屏蔽玩家操作之类的
    /// 防止玩家的操作打断你的异步关闭准备工作
    /// 大概率不需要使用这个
    /// UI被关闭
    /// 与OnDisable 不同  Disable 只是显影操作不代表被关闭
    /// 与OnDestroy 不同  Destroy 是摧毁 但是因为有缓存界面的原因 当被缓存时 OnDestroy是不会来的
    /// 这个时候你想要知道是不是被关闭了就必须通过OnClose
    /// </summary>
    public static partial class YIUIEventSystem
    {
        //返回结果
        //true = 界面可以被关闭 (99%的情况都返回true)
        //false = 界面不允许关闭 需要自行处理各种突发情况 (false 可能会遇到各种界面未关闭的情况)
        //这个事件建议没有特殊情况不要用 特别返回值不要返回false 关闭失败你要处理非常多的情况 确定你Hold住
        public static async ETTask<bool> Close(Entity component)
        {
            if (component == null || component.IsDisposed)
            {
                return true;
            }

            var iYIUICloseSystems = EntitySystemSingleton.Instance.TypeSystems.GetSystems(component.GetType(), typeof(IYIUICloseSystem));
            if (iYIUICloseSystems is not { Count: > 0 }) return true;

            EntityRef<Entity> componentRef = component;
            foreach (IYIUICloseSystem aYIUICloseSystem in iYIUICloseSystems)
            {
                if (aYIUICloseSystem == null)
                {
                    continue;
                }

                try
                {
                    return await aYIUICloseSystem.Run(componentRef);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }

            return true;
        }
    }
}