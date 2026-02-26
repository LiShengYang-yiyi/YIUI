namespace ET.Client
{
    [Invoke(EYIUIInvokeType.Sync)]
    public class YIUIInvokeDestroyPanelHandler : AInvokeEntityHandler<YIUIInvokeEntity_DestroyPanel>
    {
        public override void Handle(Entity entity, YIUIInvokeEntity_DestroyPanel args)
        {
            entity.YIUIMgr().DestroyPanel(args.PanelName);
        }
    }
}