namespace ET.Client
{
    [Invoke(EYIUIInvokeType.Sync)]
    public class YIUIInvokeHomePanelHandler : AInvokeEntityHandler<YIUIInvokeEntity_HomePanel>
    {
        public override void Handle(Entity entity, YIUIInvokeEntity_HomePanel args)
        {
            entity.YIUIMgr().HomePanel(args.PanelName, args.Tween, args.ForceHome).NoContext();
        }
    }

    [Invoke(EYIUIInvokeType.Async)]
    public class YIUIInvokeHomePanelWaitHandler : AInvokeEntityHandler<YIUIInvokeEntity_HomePanel, ETTask<bool>>
    {
        public override async ETTask<bool> Handle(Entity entity, YIUIInvokeEntity_HomePanel args)
        {
            var yiuiMgr = entity.YIUIMgr();
            if (yiuiMgr == null) return false;
            return await yiuiMgr.HomePanel(args.PanelName, args.Tween, args.ForceHome);
        }
    }
}