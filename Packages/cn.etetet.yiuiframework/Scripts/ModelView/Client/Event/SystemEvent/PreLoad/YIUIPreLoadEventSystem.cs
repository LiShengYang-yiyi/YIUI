using System;

namespace ET.Client
{
    public static partial class YIUIEventSystem
    {
        public static async ETTask PreLoad(Entity component)
        {
            var iPreLoadSystems = EntitySystemSingleton.Instance.TypeSystems.GetSystems(component.GetType(), typeof(IYIUIPreLoadSystem));
            if (iPreLoadSystems == null)
            {
                return;
            }

            EntityRef<Entity> componentRef = component;
            foreach (IYIUIPreLoadSystem aPreLoadSystem in iPreLoadSystems)
            {
                if (aPreLoadSystem == null)
                {
                    continue;
                }

                try
                {
                    await aPreLoadSystem.Run(componentRef);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }
    }
}