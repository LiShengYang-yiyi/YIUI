using System;
using System.Collections.Generic;

namespace ET.Client
{
    public static partial class YIUIEventSystem
    {
        /// <summary>
        /// 你可以理解为OOP中的 子类重写Close方法 先触发base.Close()
        /// 然后触发现在的IYIUIWindowClose事件
        /// 切记如果base.Close()中吧自己给摧毁了，那么这里就不会触发了
        /// 一个YIUIWindowComponent 可以重写多个ViewClose 但是无法保证先后顺序
        /// 实现方式为 在对应YIUIWindowComponent中 AddComponent你的重写组件 然后实现IYIUIWindowClose接口
        /// 参考 TipsViewComponent
        /// </summary>
        /// <param name="component">实体对应的那个具体UI</param>
        /// <param name="viewCloseResult">base.Close()的返回值</param>
        public static async ETTask WindowClose(Entity component, bool viewCloseResult)
        {
            if (component == null || component.IsDisposed)
            {
                return;
            }

            var windowComponent = component.GetParent<YIUIChild>()?.GetComponent<YIUIWindowComponent>();
            if (windowComponent != null)
            {
                foreach (var view in windowComponent.Components.Values)
                {
                    await WindowCloseSystem(view, viewCloseResult);
                }
            }
        }

        public static async ETTask WindowClose(YIUIWindowComponent windowComponent, bool viewCloseResult)
        {
            if (windowComponent != null)
            {
                foreach (var view in windowComponent.Components.Values)
                {
                    await WindowCloseSystem(view, viewCloseResult);
                }
            }
        }

        private static async ETTask WindowCloseSystem(Entity component, bool viewCloseResult)
        {
            if (component == null || component.IsDisposed)
            {
                return;
            }

            if (component is not IYIUIWindowClose)
            {
                return;
            }

            var iYIUICloseSystems = EntitySystemSingleton.Instance.TypeSystems.GetSystems(component.GetType(), typeof(IYIUIWindowCloseSystem));
            if (iYIUICloseSystems is not { Count: > 0 }) return;

            EntityRef<Entity> componentRef = component;
            foreach (IYIUIWindowCloseSystem aYIUICloseSystem in iYIUICloseSystems)
            {
                if (aYIUICloseSystem == null)
                {
                    continue;
                }

                try
                {
                    await aYIUICloseSystem.Run(componentRef, viewCloseResult);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }
    }
}