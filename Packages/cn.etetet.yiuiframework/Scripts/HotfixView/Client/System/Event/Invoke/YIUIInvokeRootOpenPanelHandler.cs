namespace ET.Client
{
    [Invoke(EYIUIInvokeType.Async)]
    public class YIUIInvokeRootOpenPanelAsyncHandler : AInvokeEntityHandler<YIUIInvokeEntity_SceneOpenPanel, ETTask<bool>>
    {
        public override async ETTask<bool> Handle(Entity entity, YIUIInvokeEntity_SceneOpenPanel args)
        {
            return await entity.YIUISceneRoot().OpenPanelAsync(args.PanelName) != null;
        }
    }

    [Invoke(EYIUIInvokeType.Sync)]
    public class YIUIInvokeRootOpenPanelSyncHandler : AInvokeEntityHandler<YIUIInvokeEntity_SceneOpenPanel>
    {
        public override void Handle(Entity entity, YIUIInvokeEntity_SceneOpenPanel args)
        {
            entity.YIUISceneRoot().OpenPanelAsync(args.PanelName).NoContext();
        }
    }
}