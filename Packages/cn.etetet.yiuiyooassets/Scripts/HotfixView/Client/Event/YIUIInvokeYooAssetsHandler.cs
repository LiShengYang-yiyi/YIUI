using YIUIFramework;
using YooAsset;

namespace ET.Client
{
    [Invoke]
    public class YIUIInvokeYooAssetsHandler : AInvokeEntityHandler<YIUIInvokeEntity_LoadInitialize, ETTask<bool>>
    {
        public override async ETTask<bool> Handle(Entity entity, YIUIInvokeEntity_LoadInitialize args)
        {
            EntityRef<YIUILoadComponent> loadComponentRef = (YIUILoadComponent)entity;
            if (loadComponentRef.Entity == null)
            {
                Log.Error($"LoadComponent is null!");
                return false;
            }

            return await loadComponentRef.Entity.AddComponent<YIUIYooAssetsLoadComponent>().Initialize();
        }
    }

    [Invoke(EYIUIInvokeType.Sync)]
    public class YIUIInvokeGetAssetsInfoSyncHandler : AInvokeEntityHandler<YIUIInvokeEntity_GetAssetInfo, AssetInfo>
    {
        public override AssetInfo Handle(Entity entity, YIUIInvokeEntity_GetAssetInfo args)
        {
            return entity.YIUILoad()?.GetComponent<YIUIYooAssetsLoadComponent>().GetAssetInfo(args.Location, args.AssetType);
        }
    }

    [Invoke(EYIUIInvokeType.Sync)]
    public class YIUIInvokeGetAssetInfoByGUIDSyncHandler : AInvokeEntityHandler<YIUIInvokeEntity_GetAssetInfoByGUID, AssetInfo>
    {
        public override AssetInfo Handle(Entity entity, YIUIInvokeEntity_GetAssetInfoByGUID args)
        {
            return entity.YIUILoad()?.GetComponent<YIUIYooAssetsLoadComponent>().GetAssetInfoByGUID(args.AssetGUID, args.AssetType);
        }
    }
}