using System;
using UnityEngine;
using YIUIFramework;

namespace ET.Client
{
    public static partial class YIUIRootComponentSystem
    {
        public static async ETTask<Entity> OpenPanelAsync(this YIUIRootComponent self, string componentName)
        {
            return await YIUIMgrComponent.Inst.OpenPanelAsync(componentName, self);
        }

        public static async ETTask<Entity> OpenPanelParamAsync(this YIUIRootComponent self, string componentName, params object[] paramMore)
        {
            return await YIUIMgrComponent.Inst.OpenPanelParamAsync(componentName, self, paramMore);
        }

        public static async ETTask<Entity> OpenPanelAsync<P1>(
        this YIUIRootComponent self, string componentName, P1 p1)
        {
            return await YIUIMgrComponent.Inst.OpenPanelAsync(componentName, self, p1);
        }

        public static async ETTask<Entity> OpenPanelAsync<P1, P2>(
        this YIUIRootComponent self, string componentName, P1 p1, P2 p2)
        {
            return await YIUIMgrComponent.Inst.OpenPanelAsync(componentName, self, p1, p2);
        }

        public static async ETTask<Entity> OpenPanelAsync<P1, P2, P3>(
        this YIUIRootComponent self, string componentName, P1 p1, P2 p2, P3 p3)
        {
            return await YIUIMgrComponent.Inst.OpenPanelAsync(componentName, self, p1, p2, p3);
        }

        public static async ETTask<Entity> OpenPanelAsync<P1, P2, P3, P4>(
        this YIUIRootComponent self, string componentName, P1 p1, P2 p2, P3 p3, P4 p4)
        {
            return await YIUIMgrComponent.Inst.OpenPanelAsync(componentName, self, p1, p2, p3, p4);
        }

        public static async ETTask<Entity> OpenPanelAsync<P1, P2, P3, P4, P5>(
        this YIUIRootComponent self, string componentName, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5)
        {
            return await YIUIMgrComponent.Inst.OpenPanelAsync(componentName, self, p1, p2, p3, p4, p5);
        }
    }
}