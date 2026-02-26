using System;

namespace ET.Client
{
    public static partial class YIUIEventSystem
    {
        /// <summary>
        /// 触发堆栈时
        /// 被关闭触发 (有界面打开 当前界面被关闭)
        /// 自己被关闭
        /// panelInfo = 是哪个界面被打开了 那个界面的一些信息
        /// </summary>
        public static async ETTask BackClose(Entity component, YIUIEventPanelInfo addPanelInfo)
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

            EntityRef<Entity> componentRef = component;
            foreach (IYIUIBackCloseSystem aYIUIBackCloseSystem in iYIUIBackCloseSystems)
            {
                if (aYIUIBackCloseSystem == null)
                {
                    continue;
                }

                try
                {
                    await aYIUIBackCloseSystem.Run(componentRef, addPanelInfo);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }
    }
}