using System;

namespace ET.Client
{
    public static partial class YIUIEventSystem
    {
        /// <summary>
        /// 触发堆栈时
        /// 被添加触发 (有其他界面关闭 当前界面被打开)
        /// 自己被打开
        /// panelInfo = 是哪个界面被关闭了 那个界面的一些信息
        /// </summary>
        public static async ETTask BackOpen(Entity component, YIUIPanelInfo closePanelInfo)
        {
            if (component == null || component.IsDisposed)
            {
                return;
            }

            var iYIUIBackOpenSystems = EntitySystemSingleton.Instance.TypeSystems.GetSystems(component.GetType(), typeof(IYIUIBackOpenSystem));
            if (iYIUIBackOpenSystems == null)
            {
                return;
            }

            foreach (IYIUIBackOpenSystem aYIUIBackOpenSystem in iYIUIBackOpenSystems)
            {
                if (aYIUIBackOpenSystem == null)
                {
                    continue;
                }

                try
                {
                    await aYIUIBackOpenSystem.Run(component, closePanelInfo);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }
    }
}