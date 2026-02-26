using UnityEngine;
using YIUIFramework;
using UnityGameObject = UnityEngine.GameObject;

namespace ET.Client
{
    [Invoke(EYIUIInvokeType.Async)]
    public class YIUIInvokeLoadInstantiateAsyncHandler : AInvokeEntityHandler<YIUIInvokeEntity_LoadInstantiate, ETTask<Entity>>
    {
        public override async ETTask<Entity> Handle(Entity entity, YIUIInvokeEntity_LoadInstantiate args)
        {
            return await YIUIFactory.InstantiateAsync(entity.Scene(), args.LoadType, args.ParentEntity, args.ParentTransform);
        }
    }

    #if !YIUIMACRO_SYNCLOAD_CLOSE
    [Invoke(EYIUIInvokeType.Sync)]
    public class YIUIInvokeLoadInstantiateSyncHandler : AInvokeEntityHandler<YIUIInvokeEntity_LoadInstantiate, Entity>
    {
        public override Entity Handle(Entity entity, YIUIInvokeEntity_LoadInstantiate args)
        {
            return YIUIFactory.Instantiate(entity.Scene(), args.LoadType, args.ParentEntity, args.ParentTransform);
        }
    }
    #endif

    [Invoke(EYIUIInvokeType.Async)]
    public class YIUIInvokeLoadInstantiateByVoAsyncHandler : AInvokeEntityHandler<YIUIInvokeEntity_LoadInstantiateByVo, ETTask<Entity>>
    {
        public override async ETTask<Entity> Handle(Entity entity, YIUIInvokeEntity_LoadInstantiateByVo args)
        {
            return await YIUIFactory.InstantiateAsync(entity.Scene(), args.BindVo, args.ParentEntity, args.ParentTransform);
        }
    }

    #if !YIUIMACRO_SYNCLOAD_CLOSE
    [Invoke(EYIUIInvokeType.Sync)]
    public class YIUIInvokeLoadInstantiateByVoSyncHandler : AInvokeEntityHandler<YIUIInvokeEntity_LoadInstantiateByVo, Entity>
    {
        public override Entity Handle(Entity entity, YIUIInvokeEntity_LoadInstantiateByVo args)
        {
            return YIUIFactory.Instantiate(entity.Scene(), args.BindVo, args.ParentEntity, args.ParentTransform);
        }
    }

    [Invoke(EYIUIInvokeType.Sync)]
    public class YIUIInvokeInstantiateGameObjectSyncHandler : AInvokeEntityHandler<YIUIInvokeEntity_InstantiateGameObject, UnityGameObject>
    {
        public override UnityGameObject Handle(Entity entity, YIUIInvokeEntity_InstantiateGameObject args)
        {
            return YIUIFactory.InstantiateGameObject(entity.Scene(), args.PkgName, args.ResName);
        }
    }
    #endif

    [Invoke(EYIUIInvokeType.Async)]
    public class YIUIInvokeInstantiateGameObjectAsyncHandler : AInvokeEntityHandler<YIUIInvokeEntity_InstantiateGameObject, ETTask<UnityGameObject>>
    {
        public override async ETTask<UnityGameObject> Handle(Entity entity, YIUIInvokeEntity_InstantiateGameObject args)
        {
            return await YIUIFactory.InstantiateGameObjectAsync(entity.Scene(), args.PkgName, args.ResName);
        }
    }
}