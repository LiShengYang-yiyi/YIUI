namespace ET.Client
{
    [Invoke(EYIUIInvokeType.Sync)]
    public class YIUIInvokeRemoveUIResetHandler : AInvokeEntityHandler<YIUIInvokeEntity_RemoveUIReset>
    {
        public override void Handle(Entity entity, YIUIInvokeEntity_RemoveUIReset args)
        {
            entity.YIUIMgr().RemoveUIReset(args.PanelName);
        }
    }
}