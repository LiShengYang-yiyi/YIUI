using System;

namespace ET.Client
{
    public static partial class YIUIEventSystem
    {
        /// <summary>
        /// 触发堆栈时
        /// HomeOpen触发 (当前界面home打开 其他全部关闭)
        /// 自己被打开
        /// </summary>
        public static async ETTask BackHomeOpen(Entity component)
        {
            if (component == null || component.IsDisposed)
            {
                return;
            }

            var iYIUIBackHomeOpenSystems = EntitySystemSingleton.Instance.TypeSystems.GetSystems(component.GetType(), typeof(IYIUIBackHomeOpenSystem));
            if (iYIUIBackHomeOpenSystems == null)
            {
                return;
            }

            EntityRef<Entity> componentRef = component;
            foreach (IYIUIBackHomeOpenSystem aYIUIBackHomeOpenSystem in iYIUIBackHomeOpenSystems)
            {
                if (aYIUIBackHomeOpenSystem == null)
                {
                    continue;
                }

                try
                {
                    await aYIUIBackHomeOpenSystem.Run(componentRef);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }
    }
}