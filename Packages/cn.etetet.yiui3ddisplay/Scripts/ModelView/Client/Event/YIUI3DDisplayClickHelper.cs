using System;
using UnityEngine;
using YIUIFramework;

namespace ET.Client
{
    public static class YIUI3DDisplayClickHelper
    {
        /// <summary>
        /// 点击事件
        /// </summary>
        /// <param name="systemType"></param>
        /// <param name="self"></param>
        /// <param name="display">一个组件下多个3DDisplay时 用于区分</param>
        /// <param name="target">被点击的对象</param>
        /// <param name="root">他的最终父级是谁(显示对象)</param>
        public static void OnClick(Type systemType, Entity self, UI3DDisplay display, GameObject target, GameObject root)
        {
            var iEventSystems = EntitySystemSingleton.Instance.TypeSystems.GetSystems(self.GetType(), systemType);
            if (iEventSystems is not { Count: > 0 })
            {
                Log.Error($"类:{self.GetType().Name} 没有具体实现的事件 YIUI3DDisplayClick 请检查");
                return;
            }

            foreach (IYIUI3DDisplayClick eventSystem in iEventSystems)
            {
                try
                {
                    eventSystem.OnClick(self, display, target, root);
                    return;
                }
                catch (Exception e)
                {
                    Log.Error($"类:{self.GetType().Name} 事件回调错误 YIUI3DDisplayClick 请检查 {e}");
                }
            }
        }
    }
}