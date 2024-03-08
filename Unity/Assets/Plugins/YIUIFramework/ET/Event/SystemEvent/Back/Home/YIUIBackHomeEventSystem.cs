using System;

namespace ET.Client
{
    public static partial class YIUIEventSystem
    {
        /// <summary>
        /// 触发堆栈时
        /// Home触发 (有其他界面打开 当前界面被关闭)
        /// 自己被关闭
        /// </summary>
        public static async ETTask BackHome(Entity component, YIUIPanelInfo homePanelInfo)
        {
            if (component == null || component.IsDisposed)
            {
                return;
            }

            var iYIUIBackHomeSystems = EntitySystemSingleton.Instance.TypeSystems.GetSystems(component.GetType(), typeof(IYIUIBackHomeSystem));
            if (iYIUIBackHomeSystems == null)
            {
                return;
            }

            foreach (IYIUIBackHomeSystem aYIUIBackHomeSystem in iYIUIBackHomeSystems)
            {
                if (aYIUIBackHomeSystem == null)
                {
                    continue;
                }

                try
                {
                    await aYIUIBackHomeSystem.Run(component, homePanelInfo);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }
    }
}