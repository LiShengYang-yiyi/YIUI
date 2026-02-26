using UnityEngine;
using UnityEngine.U2D;
using YIUIFramework;
using UnityObject = UnityEngine.Object;

namespace ET.Client
{
    #if !YIUIMACRO_SYNCLOAD_CLOSE
    [Invoke(EYIUIInvokeType.Sync)]
    public class YIUIInvokeLoadSpriteSyncHandler : AInvokeEntityHandler<YIUIInvokeEntity_LoadSprite, Sprite>
    {
        public override Sprite Handle(Entity entity, YIUIInvokeEntity_LoadSprite args)
        {
            EntityRef<YIUILoadComponent> loadRef = entity.YIUILoad();
            if (loadRef.Entity == null)
            {
                return null;
            }

            var resName = args.ResName;
            var spriteComponent = loadRef.Entity.GetComponent<YIUIYooAssetsSpriteComponent>();
            var sprite = spriteComponent.GetSprite(resName);
            if (sprite == null)
            {
                return null;
            }

            return sprite;
        }
    }
    #endif

    [Invoke(EYIUIInvokeType.Async)]
    public class YIUIInvokeLoadSpriteAsyncHandler : AInvokeEntityHandler<YIUIInvokeEntity_LoadSprite, ETTask<Sprite>>
    {
        public override async ETTask<Sprite> Handle(Entity entity, YIUIInvokeEntity_LoadSprite args)
        {
            EntityRef<YIUILoadComponent> loadRef = entity.YIUILoad();
            if (loadRef.Entity == null)
            {
                return null;
            }

            var resName = args.ResName;
            var spriteComponent = loadRef.Entity.GetComponent<YIUIYooAssetsSpriteComponent>();
            var sprite = await spriteComponent.GetSpriteAsync(resName);
            if (sprite == null)
            {
                return null;
            }

            return sprite;
        }
    }
}