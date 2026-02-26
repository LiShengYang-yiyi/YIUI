namespace ET.Client
{
    [Invoke(EYIUIInvokeType.Sync)]
    public class YIUIInvokeClosePanelSyncHandler : AInvokeEntityHandler<YIUIInvokeEntity_ClosePanel>
    {
        public override void Handle(Entity entity, YIUIInvokeEntity_ClosePanel args)
        {
            entity.YIUIMgr()?.ClosePanel(args.PanelName, args.Tween, args.IgnoreElse, args.IgnoreLock);
        }
    }

    [Invoke(EYIUIInvokeType.Async)]
    public class YIUIInvokeClosePanelAsyncHandler : AInvokeEntityHandler<YIUIInvokeEntity_ClosePanel, ETTask<bool>>
    {
        public override async ETTask<bool> Handle(Entity entity, YIUIInvokeEntity_ClosePanel args)
        {
            var yiuiMgr = entity.YIUIMgr();
            if (yiuiMgr == null) return false;
            return await yiuiMgr.ClosePanelAsync(args.PanelName, args.Tween, args.IgnoreElse, args.IgnoreLock);
        }
    }

    [Invoke(EYIUIInvokeType.Sync)]
    public class YIUIInvokeViewClosePanelSyncHandler : AInvokeEntityHandler<YIUIInvokeEntity_ViewClosePanel>
    {
        public override void Handle(Entity entity, YIUIInvokeEntity_ViewClosePanel args)
        {
            var panel = entity?.Parent?.Parent?.Parent?.GetComponent<YIUIPanelComponent>();
            if (panel == null)
            {
                Log.Error($"错误当前view的结构不满足标准 无法向上找到Panel 并且关闭 请检查 {entity}");
                return;
            }

            panel.Close(args.Tween, args.IgnoreElse);
        }
    }

    [Invoke(EYIUIInvokeType.Async)]
    public class YIUIInvokeViewClosePanelAsyncHandler : AInvokeEntityHandler<YIUIInvokeEntity_ViewClosePanel, ETTask<bool>>
    {
        public override async ETTask<bool> Handle(Entity entity, YIUIInvokeEntity_ViewClosePanel args)
        {
            var panel = entity?.Parent?.Parent?.Parent?.GetComponent<YIUIPanelComponent>();
            if (panel == null)
            {
                Log.Error($"错误当前view的结构不满足标准 无法向上找到Panel 并且关闭 请检查 {entity}");
                return false;
            }

            return await panel.CloseAsync(args.Tween, args.IgnoreElse);
        }
    }
}