using YIUIFramework;

namespace ET.Client
{
    public static partial class YIUIRootComponentSystem
    {
        public static async ETTask<bool> PreLoadPanelAsync<T>(this YIUIRootComponent self, bool loadEntity = true)
                where T : Entity, IAwake, IYIUIOpen
        {
            return await self.YIUIMgr.PreLoadPanelAsync<T>(self, loadEntity);
        }

        public static async ETTask<bool> PreLoadPanelAsync(this YIUIRootComponent self, string panelName, bool loadEntity = true)
        {
            return await self.YIUIMgr.PreLoadPanelAsync(panelName, self, loadEntity);
        }
    }
}