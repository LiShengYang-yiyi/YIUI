using System;

namespace ET.Client
{
    public static partial class YIUIEventSystem
    {
        /// <summary>
        /// 触发堆栈时
        /// 被关闭触发 (有界面打开 当前界面被关闭)
        /// 自己被关闭
        /// </summary>
        public static async ETTask BackClose(Entity component, YIUIPanelInfo addPanelInfo)
        {
            if (component == null || component.IsDisposed)
            {
                return;
            }

            var iYIUIBackCloseSystems = EntitySystemSingleton.Instance.TypeSystems.GetSystems(component.GetType(), typeof(IYIUIBackCloseSystem));
            if (iYIUIBackCloseSystems == null)
            {
                return;
            }

            foreach (IYIUIBackCloseSystem aYIUIBackCloseSystem in iYIUIBackCloseSystems)
            {
                if (aYIUIBackCloseSystem == null)
                {
                    continue;
                }

                try
                {
                    await aYIUIBackCloseSystem.Run(component, addPanelInfo);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }
    }
}