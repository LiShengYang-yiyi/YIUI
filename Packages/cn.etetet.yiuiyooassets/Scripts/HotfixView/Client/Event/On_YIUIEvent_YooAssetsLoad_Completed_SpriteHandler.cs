using System;

namespace ET.Client
{
    [Event(SceneType.All)]
    public class On_YIUIEvent_YooAssetsLoad_Completed_SpriteHandler : AEvent<Scene, YIUIEvent_YooAssetsLoad_Completed>
    {
        protected override async ETTask Run(Scene scene, YIUIEvent_YooAssetsLoad_Completed args)
        {
            var yiuiAtlas = args.YooAssetsLoadRef.Entity.GetParent<YIUILoadComponent>().AddComponent<YIUIYooAssetsSpriteComponent>();
            await yiuiAtlas.LoadAtlasAsync();
        }
    }
}