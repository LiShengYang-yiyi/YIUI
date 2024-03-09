using System;

namespace ET.Client
{
    public static partial class YIUIEventSystem
    {
        /// <summary>
        /// 触发堆栈时
        /// HomeClose触发 (有其他界面打开 当前界面被关闭)
        /// 自己被关闭
        /// </summary>
        public static async ETTask BackHomeClose(Entity component, YIUIPanelInfo HomeClosePanelInfo)
        {
            if (component == null || component.IsDisposed)
            {
                return;
            }

            var iYIUIBackHomeCloseSystems = EntitySystemSingleton.Instance.TypeSystems.GetSystems(component.GetType(), typeof(IYIUIBackHomeCloseSystem));
            if (iYIUIBackHomeCloseSystems == null)
            {
                return;
            }

            foreach (IYIUIBackHomeCloseSystem aYIUIBackHomeCloseSystem in iYIUIBackHomeCloseSystems)
            {
                if (aYIUIBackHomeCloseSystem == null)
                {
                    continue;
                }

                try
                {
                    await aYIUIBackHomeCloseSystem.Run(component, HomeClosePanelInfo);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }
    }
}