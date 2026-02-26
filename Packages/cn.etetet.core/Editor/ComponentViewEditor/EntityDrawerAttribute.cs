using System;

namespace ET
{
    public class EntityDrawerAttribute : Attribute
    {
        public bool SkipTypeDrawer { get; }

        public int Order { get; } //优先级 如果觉得官方的绘制不好看的可以通过更高优先级重写

        public EntityDrawerAttribute(bool skipTypeDrawer = false, int order = 0)
        {
            SkipTypeDrawer = skipTypeDrawer;
            Order          = order;
        }
    }
}