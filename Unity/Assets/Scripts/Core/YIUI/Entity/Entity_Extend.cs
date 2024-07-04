using System;
using System.Collections.Generic;

namespace ET
{
    /// <summary>
    /// 以下都是为YIUI扩展的
    /// 其他地方如果你无法保证正确性 请不要使用
    /// </summary>
    public partial class Entity
    {
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
                Log.Error($"错误 this.components == null");
                return null;
            }

            Entity component;
            if (!this.components.TryGetValue(typeof (K), out component))
            {
                Log.Error($"{this.GetType().Name} 目标没有这个组件 {typeof (K).Name}");
                return default;
            }

            // 如果有IGetComponent接口，则触发GetComponentSystem
            if (this is IGetComponent)
            {
                EventSystem.Instance.GetComponent(this, component);
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
            // 清理Children
            if (this.children != null)
            {
                var tempChildren = ObjectPool.Instance.Fetch<Dictionary<long, Entity>>();

                foreach (var child in this.children.Values)
                {
                    tempChildren.Add(child.Id, child);
                }

                this.children.Clear();
                ObjectPool.Instance.Recycle(this.children);
                this.children = null;

                foreach (Entity child in tempChildren.Values)
                {
                    child.Dispose();
                }

                if (this.childrenDB != null)
                {
                    this.childrenDB.Clear();
                    if (this.IsNew)
                    {
                        ObjectPool.Instance.Recycle(this.childrenDB);
                        this.childrenDB = null;
                    }
                }

                tempChildren.Clear();
                ObjectPool.Instance.Recycle(tempChildren);
            }
        }
    }
}