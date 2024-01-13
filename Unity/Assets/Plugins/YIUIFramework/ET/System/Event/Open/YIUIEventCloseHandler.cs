namespace ET.Client
{
    // 分发UI关闭之前监听
    [Event(SceneType.None)]
    public class YIUIEventPanelCloseBeforeHandler: AEvent<YIUIEventPanelCloseBefore>
    {
        protected override async ETTask Run(Scene scene, YIUIEventPanelCloseBefore arg)
        {
            await YIUIEventComponent.Inst.Run(arg.UIComponentName, arg);
            await YIUIEventSystem.Event(arg);
        }
    }

    // 分发UI关闭之后监听
    [Event(SceneType.None)]
    public class YIUIEventPanelCloseAfterHandler: AEvent<YIUIEventPanelCloseAfter>
    {
        protected override async ETTask Run(Scene scene, YIUIEventPanelCloseAfter arg)
        {
            await YIUIEventComponent.Inst.Run(arg.UIComponentName, arg);
            await YIUIEventSystem.Event(arg);
        }
    }

    // 分发UI被摧毁
    [Event(SceneType.None)]
    public class YIUIEventPanelDestroyHandler: AEvent<YIUIEventPanelDestroy>
    {
        protected override async ETTask Run(Scene scene, YIUIEventPanelDestroy arg)
        {
            await YIUIEventComponent.Inst.Run(arg.UIComponentName, arg);
            await YIUIEventSystem.Event(arg);
        }
    }
}