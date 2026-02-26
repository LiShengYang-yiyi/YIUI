using YIUIFramework;

namespace ET.Client
{
    [Invoke(EYIUIInvokeType.Sync)]
    public class YIUIInvokeReleaseSpriteSyncHandler : AInvokeEntityHandler<YIUIInvokeEntity_ReleaseSprite>
    {
        public override void Handle(Entity entity, YIUIInvokeEntity_ReleaseSprite args)
        {
            EntityRef<YIUILoadComponent> loadRef = entity.YIUILoad();
            if (loadRef.Entity == null)
            {
                return;
            }

            loadRef.Entity.GetComponent<YIUIYooAssetsSpriteComponent>()?.ReleaseSprite(args.obj);
        }
    }
}