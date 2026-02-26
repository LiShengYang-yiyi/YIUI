using YIUIFramework;

namespace ET.Client
{
    public static partial class YIUIRootComponentSystem
    {
        public static async ETTask<EHashWaitError> OpenPanelWaitAsync<T>(this YIUIRootComponent self)
                where T : Entity, IAwake, IYIUIOpen
        {
            return await self.YIUIMgr.OpenPanelWaitAsync<T>(self);
        }

        public static async ETTask<EHashWaitError> OpenPanelWaitParamAsync<T>(this YIUIRootComponent self, params object[] paramMore)
                where T : Entity, IYIUIOpen<ParamVo>
        {
            return await self.YIUIMgr.OpenPanelWaitParamAsync<T>(self, paramMore);
        }

        public static async ETTask<EHashWaitError> OpenPanelWaitAsync<T, P1>(this YIUIRootComponent self, P1 p1)
                where T : Entity, IYIUIOpen<P1>
        {
            return await self.YIUIMgr.OpenPanelWaitAsync<T, P1>(self, p1);
        }

        public static async ETTask<EHashWaitError> OpenPanelWaitAsync<T, P1, P2>(this YIUIRootComponent self, P1 p1, P2 p2)
                where T : Entity, IYIUIOpen<P1, P2>
        {
            return await self.YIUIMgr.OpenPanelWaitAsync<T, P1, P2>(self, p1, p2);
        }

        public static async ETTask<EHashWaitError> OpenPanelWaitAsync<T, P1, P2, P3>(this YIUIRootComponent self, P1 p1, P2 p2, P3 p3)
                where T : Entity, IYIUIOpen<P1, P2, P3>
        {
            return await self.YIUIMgr.OpenPanelWaitAsync<T, P1, P2, P3>(self, p1, p2, p3);
        }

        public static async ETTask<EHashWaitError> OpenPanelWaitAsync<T, P1, P2, P3, P4>(this YIUIRootComponent self, P1 p1, P2 p2, P3 p3, P4 p4)
                where T : Entity, IYIUIOpen<P1, P2, P3, P4>
        {
            return await self.YIUIMgr.OpenPanelWaitAsync<T, P1, P2, P3, P4>(self, p1, p2, p3, p4);
        }

        public static async ETTask<EHashWaitError> OpenPanelWaitAsync<T, P1, P2, P3, P4, P5>(this YIUIRootComponent self, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5)
                where T : Entity, IYIUIOpen<P1, P2, P3, P4, P5>
        {
            return await self.YIUIMgr.OpenPanelWaitAsync<T, P1, P2, P3, P4, P5>(self, p1, p2, p3, p4, p5);
        }
    }
}