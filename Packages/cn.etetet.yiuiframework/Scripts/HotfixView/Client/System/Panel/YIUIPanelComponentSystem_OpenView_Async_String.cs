using System;
using UnityEngine;
using YIUIFramework;

namespace ET.Client
{
    public static partial class YIUIPanelComponentSystem
    {
        //这里传入的是ResName 不要传错
        //可以用自动生成的枚举转字符串
        public static async ETTask<Entity> OpenViewAsync(this YIUIPanelComponent self, string resName)
        {
            EntityRef<YIUIPanelComponent> selfRef = self;
            EntityRef<Entity> view = await self.GetView(resName);
            if (view.Entity == null) return default;
            self = selfRef;
            var success = false;
            EntityRef<YIUIViewComponent> viewComponent = view.Entity.GetParent<YIUIChild>().GetComponent<YIUIViewComponent>();
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

            return view;
        }

        public static async ETTask<Entity> OpenViewParamAsync(this YIUIPanelComponent self, string resName, params object[] paramMore)
        {
            EntityRef<YIUIPanelComponent> selfRef = self;
            EntityRef<Entity> view = await self.GetView(resName);
            if (view.Entity == null) return default;
            self = selfRef;
            var success = false;
            EntityRef<YIUIViewComponent> viewComponent = view.Entity.GetParent<YIUIChild>().GetComponent<YIUIViewComponent>();
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

            return view;
        }

        public static async ETTask<Entity> OpenViewAsync<P1>(this YIUIPanelComponent self, string resName, P1 p1)
        {
            EntityRef<YIUIPanelComponent> selfRef = self;
            EntityRef<Entity> view = await self.GetView(resName);
            if (view.Entity == null) return default;
            self = selfRef;
            var success = false;
            EntityRef<YIUIViewComponent> viewComponent = view.Entity.GetParent<YIUIChild>().GetComponent<YIUIViewComponent>();
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

            return view;
        }

        public static async ETTask<Entity> OpenViewAsync<P1, P2>(this YIUIPanelComponent self, string resName, P1 p1, P2 p2)
        {
            EntityRef<YIUIPanelComponent> selfRef = self;
            EntityRef<Entity> view = await self.GetView(resName);
            if (view.Entity == null) return default;
            self = selfRef;
            var success = false;
            EntityRef<YIUIViewComponent> viewComponent = view.Entity.GetParent<YIUIChild>().GetComponent<YIUIViewComponent>();
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

            return view;
        }

        public static async ETTask<Entity> OpenViewAsync<P1, P2, P3>(this YIUIPanelComponent self, string resName, P1 p1, P2 p2, P3 p3)
        {
            EntityRef<YIUIPanelComponent> selfRef = self;
            EntityRef<Entity> view = await self.GetView(resName);
            if (view.Entity == null) return default;
            self = selfRef;
            var success = false;
            EntityRef<YIUIViewComponent> viewComponent = view.Entity.GetParent<YIUIChild>().GetComponent<YIUIViewComponent>();
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

            return view;
        }

        public static async ETTask<Entity> OpenViewAsync<P1, P2, P3, P4>(this YIUIPanelComponent self, string resName, P1 p1, P2 p2, P3 p3, P4 p4)
        {
            EntityRef<YIUIPanelComponent> selfRef = self;
            EntityRef<Entity> view = await self.GetView(resName);
            if (view.Entity == null) return default;
            self = selfRef;
            var success = false;
            EntityRef<YIUIViewComponent> viewComponent = view.Entity.GetParent<YIUIChild>().GetComponent<YIUIViewComponent>();
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

            return view;
        }

        public static async ETTask<Entity> OpenViewAsync<P1, P2, P3, P4, P5>(this YIUIPanelComponent self, string resName, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5)
        {
            EntityRef<YIUIPanelComponent> selfRef = self;
            EntityRef<Entity> view = await self.GetView(resName);
            if (view.Entity == null) return default;
            self = selfRef;
            var success = false;
            EntityRef<YIUIViewComponent> viewComponent = view.Entity.GetParent<YIUIChild>().GetComponent<YIUIViewComponent>();
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

            return view;
        }
    }
}