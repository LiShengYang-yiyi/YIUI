using YIUIFramework;

namespace ET.Client
{
    [Invoke(EYIUIInvokeType.Sync)]
    public class YIUIInvokeBanLayerOptionForeverHandler : AInvokeEntityHandler<YIUIInvokeEntity_BanLayerOptionForever, long>
    {
        public override long Handle(Entity entity, YIUIInvokeEntity_BanLayerOptionForever args)
        {
            return entity.YIUIMgr().BanLayerOptionForever();
        }
    }

    [Invoke(EYIUIInvokeType.Sync)]
    public class YIUIInvokeRecoverLayerOptionForeverHandler : AInvokeEntityHandler<YIUIInvokeEntity_RecoverLayerOptionForever>
    {
        public override void Handle(Entity entity, YIUIInvokeEntity_RecoverLayerOptionForever args)
        {
            entity.YIUIMgr().RecoverLayerOptionForever(args.ForeverCode);
        }
    }
}