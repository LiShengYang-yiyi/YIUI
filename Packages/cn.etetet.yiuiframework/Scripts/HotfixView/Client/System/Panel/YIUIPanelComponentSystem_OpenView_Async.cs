using System;
using UnityEngine;
using YIUIFramework;

namespace ET.Client
{
    public static partial class YIUIPanelComponentSystem
    {
        public static async ETTask<T> OpenViewAsync<T>(this YIUIPanelComponent self) where T : Entity
        {
            EntityRef<YIUIPanelComponent> selfRef = self;
            EntityRef<Entity> view = await self.GetView<T>();
            if (view.Entity == null) return default;
            var success = false;
            EntityRef<YIUIViewComponent> viewComponent = view.Entity.GetParent<YIUIChild>().GetComponent<YIUIViewComponent>();
            self = selfRef;
            await self.OpenViewBefore(view);

            try
            {
                success = await viewComponent.Entity.Open();
            }
            catch (Exception e)
            {
                Debug.LogError($"err={e.Message}{e.StackTrace}");
            }

            self = selfRef;

            await self.OpenViewAfter(view, success);

            return (T)view.Entity;
        }

        public static async ETTask<T> OpenViewParamAsync<T>(this YIUIPanelComponent self, params object[] paramMore) where T : Entity, IYIUIOpen<ParamVo>
        {
            EntityRef<YIUIPanelComponent> selfRef = self;
            EntityRef<Entity> view = await self.GetView<T>();
            if (view.Entity == null) return default;
            var success = false;
            EntityRef<YIUIViewComponent> viewComponent = view.Entity.GetParent<YIUIChild>().GetComponent<YIUIViewComponent>();
            self = selfRef;
            await self.OpenViewBefore(view);

            var p = ParamVo.Get(paramMore);

            try
            {
                success = await viewComponent.Entity.Open(p);
            }
            catch (Exception e)
            {
                Debug.LogError($"err={e.Message}{e.StackTrace}");
            }

            self = selfRef;

            await self.OpenViewAfter(view, success);

            ParamVo.Put(p);

            return (T)view.Entity;
        }

        public static async ETTask<T> OpenViewAsync<T, P1>(this YIUIPanelComponent self, P1 p1) where T : Entity, IYIUIOpen<P1>
        {
            EntityRef<YIUIPanelComponent> selfRef = self;
            EntityRef<Entity> view = await self.GetView<T>();
            if (view.Entity == null) return default;
            var success = false;
            EntityRef<YIUIViewComponent> viewComponent = view.Entity.GetParent<YIUIChild>().GetComponent<YIUIViewComponent>();
            self = selfRef;
            await self.OpenViewBefore(view);

            try
            {
                success = await viewComponent.Entity.Open(p1);
            }
            catch (Exception e)
            {
                Debug.LogError($"err={e.Message}{e.StackTrace}");
            }

            self = selfRef;

            await self.OpenViewAfter(view, success);

            return (T)view.Entity;
        }

        public static async ETTask<T> OpenViewAsync<T, P1, P2>(this YIUIPanelComponent self, P1 p1, P2 p2) where T : Entity, IYIUIOpen<P1, P2>
        {
            EntityRef<YIUIPanelComponent> selfRef = self;
            EntityRef<Entity> view = await self.GetView<T>();
            if (view.Entity == null) return default;
            var success = false;
            EntityRef<YIUIViewComponent> viewComponent = view.Entity.GetParent<YIUIChild>().GetComponent<YIUIViewComponent>();
            self = selfRef;
            await self.OpenViewBefore(view);

            try
            {
                success = await viewComponent.Entity.Open(p1, p2);
            }
            catch (Exception e)
            {
                Debug.LogError($"err={e.Message}{e.StackTrace}");
            }

            self = selfRef;

            await self.OpenViewAfter(view, success);

            return (T)view.Entity;
        }

        public static async ETTask<T> OpenViewAsync<T, P1, P2, P3>(this YIUIPanelComponent self, P1 p1, P2 p2, P3 p3) where T : Entity, IYIUIOpen<P1, P2, P3>
        {
            EntityRef<YIUIPanelComponent> selfRef = self;
            EntityRef<Entity> view = await self.GetView<T>();
            if (view.Entity == null) return default;
            var success = false;
            EntityRef<YIUIViewComponent> viewComponent = view.Entity.GetParent<YIUIChild>().GetComponent<YIUIViewComponent>();
            self = selfRef;
            await self.OpenViewBefore(view);

            try
            {
                success = await viewComponent.Entity.Open(p1, p2, p3);
            }
            catch (Exception e)
            {
                Debug.LogError($"err={e.Message}{e.StackTrace}");
            }

            self = selfRef;

            await self.OpenViewAfter(view, success);

            return (T)view.Entity;
        }

        public static async ETTask<T> OpenViewAsync<T, P1, P2, P3, P4>(this YIUIPanelComponent self, P1 p1, P2 p2, P3 p3, P4 p4) where T : Entity, IYIUIOpen<P1, P2, P3, P4>
        {
            EntityRef<YIUIPanelComponent> selfRef = self;
            EntityRef<Entity> view = await self.GetView<T>();
            if (view.Entity == null) return default;
            var success = false;
            EntityRef<YIUIViewComponent> viewComponent = view.Entity.GetParent<YIUIChild>().GetComponent<YIUIViewComponent>();
            self = selfRef;
            await self.OpenViewBefore(view);

            try
            {
                success = await viewComponent.Entity.Open(p1, p2, p3, p4);
            }
            catch (Exception e)
            {
                Debug.LogError($"err={e.Message}{e.StackTrace}");
            }

            self = selfRef;

            await self.OpenViewAfter(view, success);

            return (T)view.Entity;
        }

        public static async ETTask<T> OpenViewAsync<T, P1, P2, P3, P4, P5>(this YIUIPanelComponent self, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5) where T : Entity, IYIUIOpen<P1, P2, P3, P4, P5>
        {
            EntityRef<YIUIPanelComponent> selfRef = self;
            EntityRef<Entity> view = await self.GetView<T>();
            if (view.Entity == null) return default;
            var success = false;
            EntityRef<YIUIViewComponent> viewComponent = view.Entity.GetParent<YIUIChild>().GetComponent<YIUIViewComponent>();
            self = selfRef;
            await self.OpenViewBefore(view);

            try
            {
                success = await viewComponent.Entity.Open(p1, p2, p3, p4, p5);
            }
            catch (Exception e)
            {
                Debug.LogError($"err={e.Message}{e.StackTrace}");
            }

            self = selfRef;

            await self.OpenViewAfter(view, success);

            return (T)view.Entity;
        }
    }
}