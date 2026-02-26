namespace ET.Client
{
    public static partial class YIUIPanelComponentSystem
    {
        public static void Close(this YIUIPanelComponent self, bool tween = true, bool ignoreElse = false, bool ignoreLock = false)
        {
            self.CloseAsync(tween, ignoreElse, ignoreLock).NoContext();
        }

        public static async ETTask<bool> CloseAsync(this YIUIPanelComponent self, bool tween = true, bool ignoreElse = false, bool ignoreLock = false)
        {
            return await EventSystem.Instance?.YIUIInvokeEntityAsync<YIUIInvokeEntity_ClosePanel, ETTask<bool>>(self, new YIUIInvokeEntity_ClosePanel
            {
                PanelName = self.UIBindVo.ComponentType.Name,
                Tween = tween,
                IgnoreElse = ignoreElse,
                IgnoreLock = ignoreLock
            });
        }

        public static async ETTask<bool> Home<T>(this YIUIPanelComponent self, bool tween = true) where T : Entity
        {
            return await EventSystem.Instance?.YIUIInvokeEntityAsync<YIUIInvokeEntity_HomePanel, ETTask<bool>>(self, new YIUIInvokeEntity_HomePanel
            {
                PanelName = typeof(T).Name,
                Tween = tween
            });
        }

        public static async ETTask<bool> Home(this YIUIPanelComponent self, string homeName, bool tween = true)
        {
            return await EventSystem.Instance?.YIUIInvokeEntityAsync<YIUIInvokeEntity_HomePanel, ETTask<bool>>(self, new YIUIInvokeEntity_HomePanel
            {
                PanelName = homeName,
                Tween = tween
            });
        }
    }
}