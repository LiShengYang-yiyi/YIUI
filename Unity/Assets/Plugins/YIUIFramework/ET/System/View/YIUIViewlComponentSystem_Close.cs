namespace ET.Client
{
    public static partial class YIUIViewComponentSystem
    {
        public static void Close(this YIUIViewComponent self, bool tween = true, bool ignoreElse = false)
        {
            self.CloseAsync(tween).Coroutine();
        }

        public static async ETTask CloseAsync(this YIUIViewComponent self, bool tween = true)
        {
            await self.UIWindow.InternalOnWindowCloseTween(tween);
            self.UIBase.SetActive(false);
        }
    }
}