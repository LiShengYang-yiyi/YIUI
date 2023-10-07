using System;

namespace ET
{
    /// <summary>
    /// 主要是无法使用泛型的创建 用于YIUI添加能保证一定是Entity
    /// </summary>
    public partial class Entity
    {
        //为YIUI扩展的
        public Entity AddYIUIChild(Type childType, bool isFromPool = false)
        {
            var component = Create(childType, isFromPool);
            component.Id     = IdGenerater.Instance.GenerateId();
            component.Parent = this;

            EventSystem.Instance.Awake(component);
            return component;
        }

        public Entity AddYIUIChild<A>(Type childType, A a, bool isFromPool = false)
        {
            var component = Create(childType, isFromPool);
            component.Id     = IdGenerater.Instance.GenerateId();
            component.Parent = this;

            EventSystem.Instance.Awake(component,a);
            return component;
        }
    }
}