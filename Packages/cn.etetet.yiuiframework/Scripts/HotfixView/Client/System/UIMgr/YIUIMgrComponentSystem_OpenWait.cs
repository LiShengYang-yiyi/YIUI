using System;
using UnityEngine;
using YIUIFramework;

namespace ET.Client
{
    public static partial class YIUIMgrComponentSystem
    {
        private static async ETTask<EHashWaitError> PanelWait(this YIUIMgrComponent self, Entity panel)
        {
            if (panel == null) return EHashWaitError.Error;
            var guid = IdGenerater.Instance.GenerateId();
            var hashWait = self.Root.GetComponent<HashWait>().Wait(guid);
            var windowComponent = panel.GetParent<YIUIChild>().GetComponent<YIUIWindowComponent>();
            var waitComponent = windowComponent.GetComponent<YIUIWaitComponent>();
            if (waitComponent == null)
            {
                waitComponent = windowComponent.AddComponent<YIUIWaitComponent, long>(guid);
            }
            else
            {
                waitComponent.Reset(guid);
            }

            return await hashWait;
        }

        internal static async ETTask<EHashWaitError> OpenPanelWaitAsync<T>(this YIUIMgrComponent self, Entity root)
                where T : Entity, IAwake, IYIUIOpen
        {
            EntityRef<YIUIMgrComponent> selfRef = self;
            var panel = await self.OpenPanelAsync<T>(root);
            self = selfRef;
            return await self.PanelWait(panel);
        }

        internal static async ETTask<EHashWaitError> OpenPanelWaitParamAsync<T>(this YIUIMgrComponent self, Entity root, params object[] paramMore)
                where T : Entity, IYIUIOpen<ParamVo>
        {
            EntityRef<YIUIMgrComponent> selfRef = self;
            var panel = await self.OpenPanelParamAsync<T>(root, paramMore);
            self = selfRef;
            return await self.PanelWait(panel);
        }

        internal static async ETTask<EHashWaitError> OpenPanelWaitAsync<T, P1>(this YIUIMgrComponent self, Entity root, P1 p1)
                where T : Entity, IYIUIOpen<P1>
        {
            EntityRef<YIUIMgrComponent> selfRef = self;
            var panel = await self.OpenPanelAsync<T, P1>(root, p1);
            self = selfRef;
            return await self.PanelWait(panel);
        }

        internal static async ETTask<EHashWaitError> OpenPanelWaitAsync<T, P1, P2>(this YIUIMgrComponent self, Entity root, P1 p1, P2 p2)
                where T : Entity, IYIUIOpen<P1, P2>
        {
            EntityRef<YIUIMgrComponent> selfRef = self;
            var panel = await self.OpenPanelAsync<T, P1, P2>(root, p1, p2);
            self = selfRef;
            return await self.PanelWait(panel);
        }

        internal static async ETTask<EHashWaitError> OpenPanelWaitAsync<T, P1, P2, P3>(this YIUIMgrComponent self, Entity root, P1 p1, P2 p2, P3 p3)
                where T : Entity, IYIUIOpen<P1, P2, P3>
        {
            EntityRef<YIUIMgrComponent> selfRef = self;
            var panel = await self.OpenPanelAsync<T, P1, P2, P3>(root, p1, p2, p3);
            self = selfRef;
            return await self.PanelWait(panel);
        }

        internal static async ETTask<EHashWaitError> OpenPanelWaitAsync<T, P1, P2, P3, P4>(this YIUIMgrComponent self, Entity root, P1 p1, P2 p2, P3 p3, P4 p4)
                where T : Entity, IYIUIOpen<P1, P2, P3, P4>
        {
            EntityRef<YIUIMgrComponent> selfRef = self;
            var panel = await self.OpenPanelAsync<T, P1, P2, P3, P4>(root, p1, p2, p3, p4);
            self = selfRef;
            return await self.PanelWait(panel);
        }

        internal static async ETTask<EHashWaitError> OpenPanelWaitAsync<T, P1, P2, P3, P4, P5>(this YIUIMgrComponent self, Entity root, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5)
                where T : Entity, IYIUIOpen<P1, P2, P3, P4, P5>
        {
            EntityRef<YIUIMgrComponent> selfRef = self;
            var panel = await self.OpenPanelAsync<T, P1, P2, P3, P4, P5>(root, p1, p2, p3, p4, p5);
            self = selfRef;
            return await self.PanelWait(panel);
        }
    }
}