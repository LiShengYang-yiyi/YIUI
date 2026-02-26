namespace ET.Client
{
    // 分发UI关闭之前监听
    [Event(SceneType.All)]
    public class YIUIEventPanelCloseBeforeHandler : AEvent<Scene, YIUIEventPanelCloseBefore>
    {
        protected override async ETTask Run(Scene scene, YIUIEventPanelCloseBefore arg)
        {
            if (scene == null || scene.IsDisposed)
            {
                return;
            }

            EntityRef<Scene> sceneRef = scene;
            await YIUIEventComponent.Instance.Run(scene, arg.UIComponentName, arg);
            scene = sceneRef;
            if (scene is { IsDisposed: false })
            {
                await scene.DynamicEvent(arg);
            }
        }
    }

    // 分发UI关闭之后监听
    [Event(SceneType.All)]
    public class YIUIEventPanelCloseAfterHandler : AEvent<Scene, YIUIEventPanelCloseAfter>
    {
        protected override async ETTask Run(Scene scene, YIUIEventPanelCloseAfter arg)
        {
            if (scene == null || scene.IsDisposed)
            {
                return;
            }

            EntityRef<Scene> sceneRef = scene;
            await YIUIEventComponent.Instance.Run(scene, arg.UIComponentName, arg);
            scene = sceneRef;
            if (scene is { IsDisposed: false })
            {
                await scene.DynamicEvent(arg);
            }
        }
    }

    // 分发UI被摧毁
    [Event(SceneType.All)]
    public class YIUIEventPanelDestroyHandler : AEvent<Scene, YIUIEventPanelDestroy>
    {
        protected override async ETTask Run(Scene scene, YIUIEventPanelDestroy arg)
        {
            if (scene == null || scene.IsDisposed)
            {
                return;
            }

            if (scene.YIUILoad() == null) return;
            EntityRef<Scene> sceneRef = scene;
            await YIUIEventComponent.Instance.Run(scene, arg.UIComponentName, arg);
            scene = sceneRef;
            if (scene is { IsDisposed: false })
            {
                await scene.DynamicEvent(arg);
            }
        }
    }
}