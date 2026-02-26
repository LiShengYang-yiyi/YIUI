using UnityEngine;
using YIUIFramework;

namespace ET.Client
{
    [Invoke(EYIUIInvokeType.Sync)]
    public class YIUIInvokeGetUIRootHandler : AInvokeEntityHandler<YIUIInvokeEntity_GetUIRoot, GameObject>
    {
        public override GameObject Handle(Entity entity, YIUIInvokeEntity_GetUIRoot args)
        {
            return entity.YIUIMgr().UIRoot;
        }
    }

    [Invoke(EYIUIInvokeType.Sync)]
    public class YIUIInvokeGetUICanvasRootHandler : AInvokeEntityHandler<YIUIInvokeEntity_GetUICanvasRoot, GameObject>
    {
        public override GameObject Handle(Entity entity, YIUIInvokeEntity_GetUICanvasRoot args)
        {
            return entity.YIUIMgr().UICanvasRoot;
        }
    }

    [Invoke(EYIUIInvokeType.Sync)]
    public class YIUIInvokeGetUILayerRootHandler : AInvokeEntityHandler<YIUIInvokeEntity_GetUILayerRoot, RectTransform>
    {
        public override RectTransform Handle(Entity entity, YIUIInvokeEntity_GetUILayerRoot args)
        {
            return entity.YIUIMgr().UILayerRoot;
        }
    }

    [Invoke(EYIUIInvokeType.Sync)]
    public class YIUIInvokeGetUICameraHandler : AInvokeEntityHandler<YIUIInvokeEntity_GetUICamera, Camera>
    {
        public override Camera Handle(Entity entity, YIUIInvokeEntity_GetUICamera args)
        {
            return entity.YIUIMgr().UICamera;
        }
    }

    [Invoke(EYIUIInvokeType.Sync)]
    public class YIUIInvokeGetUICanvasHandler : AInvokeEntityHandler<YIUIInvokeEntity_GetUICanvas, Canvas>
    {
        public override Canvas Handle(Entity entity, YIUIInvokeEntity_GetUICanvas args)
        {
            return entity.YIUIMgr().UICanvas;
        }
    }
}