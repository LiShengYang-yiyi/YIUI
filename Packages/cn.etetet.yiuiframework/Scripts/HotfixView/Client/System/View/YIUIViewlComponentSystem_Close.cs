namespace ET.Client
{
    public static partial class YIUIViewComponentSystem
    {
        //关闭自己 同步
        public static void Close(this YIUIViewComponent self, bool tween = true)
        {
            self.CloseAsync(tween).NoContext();
        }

        //关闭自己 异步
        public static async ETTask<bool> CloseAsync(this YIUIViewComponent self, bool tween = true)
        {
            EntityRef<YIUIViewComponent> selfRef = self;
            var view = self?.OwnerUIEntity;
            if (view != null)
            {
                var success = true;

                if (view is IYIUIClose)
                {
                    success = await YIUIEventSystem.Close(view);
                }

                self = selfRef;
                if (self.UIWindow is { WindowCloseTweenBefore: true })
                {
                    await YIUIEventSystem.WindowClose(self.UIWindow, success);
                }

                if (!success)
                {
                    self = selfRef;
                    Log.Info($"<color=yellow> 关闭事件返回不允许关闭View UI: {self.UIBase?.OwnerGameObject.name} </color>");
                    return false;
                }
            }

            self = selfRef;

            await self.UIWindow.InternalOnWindowCloseTween(tween);

            self = selfRef;

            self.UIBase.SetActive(false);

            if (self.UIWindow is { WindowCloseTweenBefore: false })
            {
                await YIUIEventSystem.WindowClose(self.UIWindow, true);
            }

            return true;
        }

        //标准view 可快捷关闭panel 需要满足panel的结构 同步
        public static void ClosePanel(this YIUIViewComponent self, bool tween = true, bool ignoreElse = false)
        {
            EventSystem.Instance?.YIUIInvokeEntitySync(self, new YIUIInvokeEntity_ViewClosePanel
            {
                Tween = tween,
                IgnoreElse = ignoreElse
            });
        }

        //标准view 可快捷关闭panel 需要满足panel的结构 异步
        public static async ETTask<bool> ClosePanelAsync(this YIUIViewComponent self, bool tween = true, bool ignoreElse = false)
        {
            return await EventSystem.Instance?.YIUIInvokeEntityAsync<YIUIInvokeEntity_ViewClosePanel, ETTask<bool>>(self, new YIUIInvokeEntity_ViewClosePanel
            {
                Tween = tween,
                IgnoreElse = ignoreElse
            });
        }
    }
}