using YIUIFramework;

namespace ET.Client
{
    [Invoke(EYIUIInvokeType.Sync)]
    public class YIUIInvokeReleaseSyncHandler : AInvokeEntityHandler<YIUIInvokeEntity_Release>
    {
        public override void Handle(Entity entity, YIUIInvokeEntity_Release args)
        {
            entity.YIUILoad().Release(args.obj);
        }
    }

    [Invoke(EYIUIInvokeType.Sync)]
    public class YIUIInvokeReleaseInstantiateSyncHandler : AInvokeEntityHandler<YIUIInvokeEntity_ReleaseInstantiate>
    {
        public override void Handle(Entity entity, YIUIInvokeEntity_ReleaseInstantiate args)
        {
            entity.YIUILoad().ReleaseInstantiate(args.obj);
        }
    }
}