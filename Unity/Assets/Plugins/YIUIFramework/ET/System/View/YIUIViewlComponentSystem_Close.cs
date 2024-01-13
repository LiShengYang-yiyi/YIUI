namespace ET.Client
{
    public static partial class YIUIViewComponentSystem
    {
        //view 关闭自己 同步
        public static void Close(this YIUIViewComponent self, bool tween = true)
        {
            self.CloseAsync(tween).Coroutine();
        }

        //view 关闭自己 异步
        public static async ETTask CloseAsync(this YIUIViewComponent self, bool tween = true)
        {
            await self.UIWindow.InternalOnWindowCloseTween(tween);
            self.UIBase.SetActive(false);
        }

        //标准view 可快捷关闭panel 需要满足panel的结构 同步
        public static void ClosePanel(this YIUIViewComponent self, bool tween = true, bool ignoreElse = false)
        {
            var panel = self?.Parent?.Parent?.Parent?.GetComponent<YIUIPanelComponent>();
            panel?.Close(tween, ignoreElse);
        }

        //标准view 可快捷关闭panel 需要满足panel的结构 异步
        public static async ETTask ClosePanelAsync(this YIUIViewComponent self, bool tween = true, bool ignoreElse = false)
        {
            var panel = self?.Parent?.Parent?.Parent?.GetComponent<YIUIPanelComponent>();
            await panel?.CloseAsync(tween, ignoreElse);
        }
    }
}