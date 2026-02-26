using System;

namespace ET.Client
{
    public static partial class YIUIEventSystem
    {
        /// <summary>
        /// 当一个界面 EPanelOption.DisClose 时 (禁止关闭)
        /// 且又被调用时 则会触发 可根据需求继承
        /// 根据需求返回 是否可以被关闭
        /// 返回true 就是可以被关闭
        /// </summary>
        public static async ETTask<bool> DisClose(Entity component)
        {
            if (component == null || component.IsDisposed)
            {
                return false;
            }

            var iYIUIBanCloseSystems = EntitySystemSingleton.Instance.TypeSystems.GetSystems(component.GetType(), typeof(IYIUIDisCloseSystem));
            if (iYIUIBanCloseSystems == null)
            {
                return false;
            }

            EntityRef<Entity> componentRef = component;
            foreach (IYIUIDisCloseSystem aYIUIBanCloseSystem in iYIUIBanCloseSystems)
            {
                if (aYIUIBanCloseSystem == null)
                {
                    continue;
                }

                try
                {
                    return await aYIUIBanCloseSystem.Run(componentRef);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }

            return false;
        }
    }
}