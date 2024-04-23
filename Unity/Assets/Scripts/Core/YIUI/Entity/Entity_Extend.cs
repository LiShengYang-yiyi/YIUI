using System;

namespace ET
{
    /// <summary>
    /// 为YIUI扩展的
    /// 主要是无法使用泛型的创建 用于YIUI添加能保证一定是Entity
    /// </summary>
    public partial class Entity
    {
        public Entity AddYIUIChild(Type childType, bool isFromPool = false)
        {
            var component = Create(childType, isFromPool);
            component.Id     = IdGenerater.Instance.GenerateId();
            component.Parent = this;
            EntitySystemSingleton.Instance.Awake(component);
            return component;
        }

        public Entity AddYIUIChild<A>(Type childType, A a, bool isFromPool = false)
        {
            var component = Create(childType, isFromPool);
            component.Id     = IdGenerater.Instance.GenerateId();
            component.Parent = this;
            EntitySystemSingleton.Instance.Awake(component, a);
            return component;
        }

        //虽然使用 AddChild也可以实现 但是为了保持一致性 还是写一个方法
        //另外就是跳过分析器检查 这是YIUI使用的 确定不会有问题
        public void SetParent(Entity target)
        {
            if (target == null) return;
            if (this.Parent == target) return;
            this.Parent = target;
        }

        //自定义扩展 获取不到就报错 不管你log是true 还是false
        //不想报错就别调用这个方法
        public K GetComponent<K>(bool log) where K : Entity
        {
            if (this.components == null)
            {
                return null;
            }

            Entity component;
            if (!this.components.TryGetValue(this.GetLongHashCode(typeof(K)), out component))
            {
                Log.Error($"{this.GetType().Name} 目标没有这个组件 {typeof(K).Name}");
                return default;
            }

            // 如果有IGetComponent接口，则触发GetComponentSystem
            if (this is IGetComponentSys)
            {
                EntitySystemSingleton.Instance.GetComponentSys(this, typeof(K));
            }

            return (K)component;
        }

        //任意类型 添加组件 跳过分析器检查
        public K AddComponentByEntity<K>() where K : Entity, IAwake, new()
        {
            return this.AddComponent<K>();
        }

        public K GetOrAddComponent<K>() where K : Entity, IAwake, new()
        {
            return this.GetComponent<K>() ?? this.AddComponent<K>();
        }

        // 清理Children
        public void DisposeChildren()
        {
            if (this.children != null)
            {
                var tempChildren = this.children;
                this.children.Clear();
                foreach (Entity child in tempChildren.Values)
                {
                    child.Dispose();
                }

                if (this.childrenDB != null)
                {
                    this.childrenDB.Clear();
                }
            }
        }
    }
}