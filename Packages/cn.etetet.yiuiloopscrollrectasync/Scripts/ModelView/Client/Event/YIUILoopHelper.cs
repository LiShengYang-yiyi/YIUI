using System;

namespace ET.Client
{
    public static class YIUILoopHelper
    {
        public static void Renderer(Type rendererType, Entity self, Entity item, object data, int index, bool select)
        {
            var iEventSystems = EntitySystemSingleton.Instance.TypeSystems.GetSystems(self.GetType(), rendererType);
            if (iEventSystems is not { Count: > 0 })
            {
                Log.Error($"类:{self.GetType().Name} Item:{item.GetType().Name} Data:{data.GetType().Name} 没有具体实现的事件 IYIUILoopRenderer 请检查");
                return;
            }

            foreach (IYIUILoopRenderer eventSystem in iEventSystems)
            {
                try
                {
                    eventSystem.Renderer(self, item, data, index, select);
                    return;
                }
                catch (Exception e)
                {
                    Log.Error($"类:{self.GetType().Name} Item:{item.GetType().Name} Data:{data.GetType().Name} 事件回调错误 IYIUILoopRenderer 请检查 {e.Message}");
                }
            }
        }

        public static void OnClick(Type onclickType, Entity self, Entity item, object data, int index, bool select)
        {
            var iEventSystems = EntitySystemSingleton.Instance.TypeSystems.GetSystems(self.GetType(), onclickType);
            if (iEventSystems is not { Count: > 0 })
            {
                Log.Error($"类:{self.GetType().Name} Item:{item.GetType().Name} Data:{data.GetType().Name} 没有具体实现的事件 IYIUILoopOnClick 请检查");
                return;
            }

            foreach (IYIUILoopOnClick eventSystem in iEventSystems)
            {
                try
                {
                    eventSystem.OnClick(self, item, data, index, select);
                    return;
                }
                catch (Exception e)
                {
                    Log.Error($"类:{self.GetType().Name} Item:{item.GetType().Name} Data:{data.GetType().Name} 事件回调错误 IYIUILoopOnClick 请检查 {e.Message}");
                }
            }
        }

        public static bool OnClickCheck(Type onclickCheckType, Entity self, Entity item, object data, int index, bool select)
        {
            var iEventSystems = EntitySystemSingleton.Instance.TypeSystems.GetSystems(self.GetType(), onclickCheckType);
            if (iEventSystems is not { Count: > 0 })
            {
                Log.Error($"类:{self.GetType().Name} Item:{item.GetType().Name} Data:{data.GetType().Name} 没有具体实现的事件 IYIUILoopOnClickCheck 请检查");
                return false;
            }

            foreach (IYIUILoopOnClickCheck eventSystem in iEventSystems)
            {
                try
                {
                    return eventSystem.OnClickCheck(self, item, data, index, select);
                }
                catch (Exception e)
                {
                    Log.Error($"类:{self.GetType().Name} Item:{item.GetType().Name} Data:{data.GetType().Name} 事件回调错误 IYIUILoopOnClick 请检查 {e.Message}");
                    return false;
                }
            }

            return false;
        }
    }
}