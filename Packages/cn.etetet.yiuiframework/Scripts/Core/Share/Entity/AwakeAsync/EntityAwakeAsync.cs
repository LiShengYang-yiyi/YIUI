#if !ET10
using System;

namespace ET
{
    public abstract partial class Entity
    {
        public async ETTask<Entity> AddComponentAsync(Type type, bool isFromPool = false)
        {
            if (this.components != null && this.components.ContainsKey(this.GetComponentLongHashCode(type)))
            {
                throw new Exception($"实体组件已存在： {type.FullName}");
            }

            var component = Create(type, isFromPool);
            component.Id = this.Id;
            component.ComponentParent = this;
            EntityRef<Entity> componentRef = component;
            await EntityAwakeAsyncSystem.EntityAwakeAsync(componentRef);
            return componentRef;
        }

        public async ETTask<K> AddComponentWithIdAsync<K>(long id, bool isFromPool = false) where K : Entity, IAwakeAsync, new()
        {
            var type = typeof(K);
            if (this.components != null && this.components.ContainsKey(this.GetComponentLongHashCode(type)))
            {
                throw new Exception($"实体组件已存在： {type.FullName}");
            }

            var component = Create(type, isFromPool);
            component.Id = id;
            component.ComponentParent = this;
            EntityRef<Entity> componentRef = component;
            await EntityAwakeAsyncSystem.EntityAwakeAsync(componentRef);
            return (K)componentRef.Entity;
        }

        public async ETTask<K> AddComponentWithIdAsync<K, P1>(long id, P1 p1, bool isFromPool = false) where K : Entity, IAwakeAsync<P1>, new()
        {
            var type = typeof(K);
            if (this.components != null && this.components.ContainsKey(this.GetComponentLongHashCode(type)))
            {
                throw new Exception($"实体组件已存在： {type.FullName}");
            }

            var component = Create(type, isFromPool);
            component.Id = id;
            component.ComponentParent = this;
            EntityRef<Entity> componentRef = component;
            await EntityAwakeAsyncSystem.EntityAwakeAsync(componentRef, p1);
            return (K)componentRef.Entity;
        }

        public async ETTask<K> AddComponentWithIdAsync<K, P1, P2>(long id, P1 p1, P2 p2, bool isFromPool = false) where K : Entity, IAwakeAsync<P1, P2>, new()
        {
            var type = typeof(K);
            if (this.components != null && this.components.ContainsKey(this.GetComponentLongHashCode(type)))
            {
                throw new Exception($"实体组件已存在： {type.FullName}");
            }

            var component = Create(type, isFromPool);
            component.Id = id;
            component.ComponentParent = this;
            EntityRef<Entity> componentRef = component;
            await EntityAwakeAsyncSystem.EntityAwakeAsync(componentRef, p1, p2);
            return (K)componentRef.Entity;
        }

        public async ETTask<K> AddComponentWithIdAsync<K, P1, P2, P3>(long id, P1 p1, P2 p2, P3 p3, bool isFromPool = false) where K : Entity, IAwakeAsync<P1, P2, P3>, new()
        {
            var type = typeof(K);
            if (this.components != null && this.components.ContainsKey(this.GetComponentLongHashCode(type)))
            {
                throw new Exception($"实体组件已存在： {type.FullName}");
            }

            var component = Create(type, isFromPool);
            component.Id = id;
            component.ComponentParent = this;
            EntityRef<Entity> componentRef = component;
            await EntityAwakeAsyncSystem.EntityAwakeAsync(componentRef, p1, p2, p3);
            return (K)componentRef.Entity;
        }

        public async ETTask<K> AddComponentWithIdAsync<K, P1, P2, P3, P4>(long id, P1 p1, P2 p2, P3 p3, P4 p4, bool isFromPool = false) where K : Entity, IAwakeAsync<P1, P2, P3, P4>, new()
        {
            var type = typeof(K);
            if (this.components != null && this.components.ContainsKey(this.GetComponentLongHashCode(type)))
            {
                throw new Exception($"实体组件已存在： {type.FullName}");
            }

            var component = Create(type, isFromPool);
            component.Id = id;
            component.ComponentParent = this;
            EntityRef<Entity> componentRef = component;
            await EntityAwakeAsyncSystem.EntityAwakeAsync(componentRef, p1, p2, p3, p4);
            return (K)componentRef.Entity;
        }

        public async ETTask<K> AddComponentWithIdAsync<K, P1, P2, P3, P4, P5>(long id, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, bool isFromPool = false) where K : Entity, IAwakeAsync<P1, P2, P3, P4, P5>, new()
        {
            var type = typeof(K);
            if (this.components != null && this.components.ContainsKey(this.GetComponentLongHashCode(type)))
            {
                throw new Exception($"实体组件已存在： {type.FullName}");
            }

            var component = Create(type, isFromPool);
            component.Id = id;
            component.ComponentParent = this;
            EntityRef<Entity> componentRef = component;
            await EntityAwakeAsyncSystem.EntityAwakeAsync(componentRef, p1, p2, p3, p4, p5);
            return (K)componentRef.Entity;
        }

        public async ETTask<K> AddComponentAsync<K>(bool isFromPool = false) where K : Entity, IAwakeAsync, new()
        {
            return await this.AddComponentWithIdAsync<K>(this.Id, isFromPool);
        }

        public async ETTask<K> AddComponentAsync<K, P1>(P1 p1, bool isFromPool = false) where K : Entity, IAwakeAsync<P1>, new()
        {
            return await this.AddComponentWithIdAsync<K, P1>(this.Id, p1, isFromPool);
        }

        public async ETTask<K> AddComponentAsync<K, P1, P2>(P1 p1, P2 p2, bool isFromPool = false) where K : Entity, IAwakeAsync<P1, P2>, new()
        {
            return await this.AddComponentWithIdAsync<K, P1, P2>(this.Id, p1, p2, isFromPool);
        }

        public async ETTask<K> AddComponentAsync<K, P1, P2, P3>(P1 p1, P2 p2, P3 p3, bool isFromPool = false) where K : Entity, IAwakeAsync<P1, P2, P3>, new()
        {
            return await this.AddComponentWithIdAsync<K, P1, P2, P3>(this.Id, p1, p2, p3, isFromPool);
        }

        public async ETTask<K> AddComponentAsync<K, P1, P2, P3, P4>(P1 p1, P2 p2, P3 p3, P4 p4, bool isFromPool = false) where K : Entity, IAwakeAsync<P1, P2, P3, P4>, new()
        {
            return await this.AddComponentWithIdAsync<K, P1, P2, P3, P4>(this.Id, p1, p2, p3, p4, isFromPool);
        }

        public async ETTask<K> AddComponentAsync<K, P1, P2, P3, P4, P5>(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, bool isFromPool = false) where K : Entity, IAwakeAsync<P1, P2, P3, P4, P5>, new()
        {
            return await this.AddComponentWithIdAsync<K, P1, P2, P3, P4, P5>(this.Id, p1, p2, p3, p4, p5, isFromPool);
        }

        public async ETTask<Entity> AddChildAsync(Type type, bool isFromPool = false)
        {
            return await AddChildWithIdAsync(type, IdGenerater.Instance.GenerateId(), isFromPool);
        }

        public async ETTask<T> AddChildAsync<T>(bool isFromPool = false) where T : Entity, IAwakeAsync
        {
            return await AddChildWithIdAsync<T>(IdGenerater.Instance.GenerateId(), isFromPool);
        }

        public async ETTask<T> AddChildAsync<T, A>(A a, bool isFromPool = false) where T : Entity, IAwakeAsync<A>
        {
            return await AddChildWithIdAsync<T, A>(IdGenerater.Instance.GenerateId(), a, isFromPool);
        }

        public async ETTask<T> AddChildAsync<T, A, B>(A a, B b, bool isFromPool = false) where T : Entity, IAwakeAsync<A, B>
        {
            return await AddChildWithIdAsync<T, A, B>(IdGenerater.Instance.GenerateId(), a, b, isFromPool);
        }

        public async ETTask<T> AddChildAsync<T, A, B, C>(A a, B b, C c, bool isFromPool = false) where T : Entity, IAwakeAsync<A, B, C>
        {
            return await AddChildWithIdAsync<T, A, B, C>(IdGenerater.Instance.GenerateId(), a, b, c, isFromPool);
        }

        public async ETTask<T> AddChildAsync<T, A, B, C, D>(A a, B b, C c, D d, bool isFromPool = false) where T : Entity, IAwakeAsync<A, B, C, D>
        {
            return await AddChildWithIdAsync<T, A, B, C, D>(IdGenerater.Instance.GenerateId(), a, b, c, d, isFromPool);
        }

        public async ETTask<T> AddChildAsync<T, A, B, C, D, E>(A a, B b, C c, D d, E e, bool isFromPool = false) where T : Entity, IAwakeAsync<A, B, C, D, E>
        {
            return await AddChildWithIdAsync<T, A, B, C, D, E>(IdGenerater.Instance.GenerateId(), a, b, c, d, e, isFromPool);
        }

        public async ETTask<Entity> AddChildWithIdAsync(Type type, long id, bool isFromPool = false)
        {
            var component = Create(type, isFromPool);
            component.Id = id;
            component.Parent = this;
            EntityRef<Entity> componentRef = component;
            await EntityAwakeAsyncSystem.EntityAwakeAsync(componentRef);
            return componentRef.Entity;
        }

        public async ETTask<T> AddChildWithIdAsync<T>(long id, bool isFromPool = false) where T : Entity, IAwakeAsync
        {
            var type = typeof(T);
            var component = Create(type, isFromPool);
            component.Id = id;
            component.Parent = this;
            EntityRef<Entity> componentRef = component;
            await EntityAwakeAsyncSystem.EntityAwakeAsync(componentRef);
            return (T)componentRef.Entity;
        }

        public async ETTask<T> AddChildWithIdAsync<T, A>(long id, A a, bool isFromPool = false) where T : Entity, IAwakeAsync<A>
        {
            var type = typeof(T);
            var component = Create(type, isFromPool);
            component.Id = id;
            component.Parent = this;
            EntityRef<Entity> componentRef = component;
            await EntityAwakeAsyncSystem.EntityAwakeAsync(componentRef, a);
            return (T)componentRef.Entity;
        }

        public async ETTask<T> AddChildWithIdAsync<T, A, B>(long id, A a, B b, bool isFromPool = false) where T : Entity, IAwakeAsync<A, B>
        {
            var type = typeof(T);
            var component = Create(type, isFromPool);
            component.Id = id;
            component.Parent = this;
            EntityRef<Entity> componentRef = component;
            await EntityAwakeAsyncSystem.EntityAwakeAsync(componentRef, a, b);
            return (T)componentRef.Entity;
        }

        public async ETTask<T> AddChildWithIdAsync<T, A, B, C>(long id, A a, B b, C c, bool isFromPool = false) where T : Entity, IAwakeAsync<A, B, C>
        {
            var type = typeof(T);
            var component = Create(type, isFromPool);
            component.Id = id;
            component.Parent = this;
            EntityRef<Entity> componentRef = component;
            await EntityAwakeAsyncSystem.EntityAwakeAsync(componentRef, a, b, c);
            return (T)componentRef.Entity;
        }

        public async ETTask<T> AddChildWithIdAsync<T, A, B, C, D>(long id, A a, B b, C c, D d, bool isFromPool = false) where T : Entity, IAwakeAsync<A, B, C, D>
        {
            var type = typeof(T);
            var component = Create(type, isFromPool);
            component.Id = id;
            component.Parent = this;
            EntityRef<Entity> componentRef = component;
            await EntityAwakeAsyncSystem.EntityAwakeAsync(componentRef, a, b, c, d);
            return (T)componentRef.Entity;
        }

        public async ETTask<T> AddChildWithIdAsync<T, A, B, C, D, E>(long id, A a, B b, C c, D d, E e, bool isFromPool = false) where T : Entity, IAwakeAsync<A, B, C, D, E>
        {
            var type = typeof(T);
            var component = Create(type, isFromPool);
            component.Id = id;
            component.Parent = this;
            EntityRef<Entity> componentRef = component;
            await EntityAwakeAsyncSystem.EntityAwakeAsync(componentRef, a, b, c, d, e);
            return (T)componentRef.Entity;
        }
    }
}
#endif