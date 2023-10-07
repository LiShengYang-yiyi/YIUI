namespace ET.Client
{
    // 分发UI打开之前监听
    [Event(SceneType.None)]
    public class YIUIEventPanelOpenBeforeHandler: AEvent<YIUIEventPanelOpenBefore>
    {
        protected override async ETTask Run(Scene scene, YIUIEventPanelOpenBefore arg)
        {
            YIUIEventComponent.Inst.Run(typeof (YIUIEventPanelOpenBefore), arg.UIComponentName, arg);
            YIUIEventSystem.Event(arg);
            await ETTask.CompletedTask;
        }
    }

    // 分发UI打开之后监听
    [Event(SceneType.None)]
    public class YIUIEventPanelOpenAfterHandler: AEvent<YIUIEventPanelOpenAfter>
    {
        protected override async ETTask Run(Scene scene, YIUIEventPanelOpenAfter arg)
        {
            YIUIEventComponent.Inst.Run(typeof (YIUIEventPanelOpenAfter), arg.UIComponentName, arg);
            YIUIEventSystem.Event(arg);
            await ETTask.CompletedTask;
        }
    }
}