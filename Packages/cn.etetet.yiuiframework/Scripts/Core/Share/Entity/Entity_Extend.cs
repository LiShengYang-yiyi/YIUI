using System;
using System.Linq;

namespace ET
{
    public static class Entity_Extend
    {
        //自定义扩展 获取不到就报错 不管你log是true 还是false
        //不想报错就别调用这个方法
        public static K GetComponent<K>(this Entity self, bool log) where K : Entity
        {
            var component = self.GetComponent<K>();
            if (component == null)
            {
                Log.Error($"{self.GetType().Name} 目标没有这个组件 {typeof(K).Name}");
                return default;
            }

            return component;
        }

        //虽然使用 AddChild也可以实现
        //但是为了保持一致性 跟第一直觉 还是写一个方法
        //且相同时不报错
        public static void SetParent(this Entity self, Entity parent)
        {
            if (parent == null) return;
            if (self.Parent == parent) return;
            parent.AddChild(self);
        }

        //跳过分析器用的 慎用 除非你知道你在做什么
        public static K GetParentComponent<K>(this Entity self) where K : Entity
        {
            return self.Parent.GetComponent<K>();
        }

        //跳过分析器用的 慎用 除非你知道你在做什么
        public static K GetParentChild<K>(this Entity self, int id) where K : Entity
        {
            return self.Parent.GetChild<K>(id);
        }

        //快捷移除所有子类
        public static void RemoveAllChild(this Entity self)
        {
            if (self.ChildrenCount() <= 0) return;
            foreach (long unitId in self.Children.Keys.ToArray())
            {
                self.RemoveChild(unitId);
            }
        }
    }
}