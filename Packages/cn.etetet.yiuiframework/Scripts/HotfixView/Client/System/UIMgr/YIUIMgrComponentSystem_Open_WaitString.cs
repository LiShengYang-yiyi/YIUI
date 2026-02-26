using System;
using UnityEngine;
using YIUIFramework;

namespace ET.Client
{
    public static partial class YIUIMgrComponentSystem
    {
        internal static async ETTask<EHashWaitError> OpenPanelWaitAsync(this YIUIMgrComponent self, string componentName, Entity root)
        {
            EntityRef<YIUIMgrComponent> selfRef = self;
            var panel = await self.OpenPanelAsync(componentName, root);
            self = selfRef;
            return await self.PanelWait(panel);
        }

        internal static async ETTask<EHashWaitError> OpenPanelWaitParamAsync(this YIUIMgrComponent self, string componentName, Entity root, params object[] paramMore)
        {
            EntityRef<YIUIMgrComponent> selfRef = self;
            var panel = await self.OpenPanelParamAsync(componentName, root, paramMore);
            self = selfRef;
            return await self.PanelWait(panel);
        }

        internal static async ETTask<EHashWaitError> OpenPanelWaitAsync<P1>(this YIUIMgrComponent self, string componentName, Entity root, P1 p1)
        {
            EntityRef<YIUIMgrComponent> selfRef = self;
            var panel = await self.OpenPanelAsync(componentName, root, p1);
            self = selfRef;
            return await self.PanelWait(panel);
        }

        internal static async ETTask<EHashWaitError> OpenPanelWaitAsync<P1, P2>(this YIUIMgrComponent self, string componentName, Entity root, P1 p1, P2 p2)
        {
            EntityRef<YIUIMgrComponent> selfRef = self;
            var panel = await self.OpenPanelAsync(componentName, root, p1, p2);
            self = selfRef;
            return await self.PanelWait(panel);
        }

        internal static async ETTask<EHashWaitError> OpenPanelWaitAsync<P1, P2, P3>(this YIUIMgrComponent self, string componentName, Entity root, P1 p1, P2 p2, P3 p3)
        {
            EntityRef<YIUIMgrComponent> selfRef = self;
            var panel = await self.OpenPanelAsync(componentName, root, p1, p2, p3);
            self = selfRef;
            return await self.PanelWait(panel);
        }

        internal static async ETTask<EHashWaitError> OpenPanelWaitAsync<P1, P2, P3, P4>(this YIUIMgrComponent self, string componentName, Entity root, P1 p1, P2 p2, P3 p3, P4 p4)
        {
            EntityRef<YIUIMgrComponent> selfRef = self;
            var panel = await self.OpenPanelAsync(componentName, root, p1, p2, p3, p4);
            self = selfRef;
            return await self.PanelWait(panel);
        }

        internal static async ETTask<EHashWaitError> OpenPanelWaitAsync<P1, P2, P3, P4, P5>(this YIUIMgrComponent self, string componentName, Entity root, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5)
        {
            EntityRef<YIUIMgrComponent> selfRef = self;
            var panel = await self.OpenPanelAsync(componentName, root, p1, p2, p3, p4, p5);
            self = selfRef;
            return await self.PanelWait(panel);
        }
    }
}