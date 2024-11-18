namespace ET.Client
{
    [Invoke]
    public class YIUIInvokeTimerComponentWaitFrameAsyncHandler : AInvokeHandler<YIUIInvokeWaitFrameAsync, ETTask>
    {
        public override async ETTask Handle(YIUIInvokeWaitFrameAsync args)
        {
            await YIUIMgrComponent.Inst?.Root().GetComponent<TimerComponent>().WaitFrameAsync();
        }
    }

    [Invoke]
    public class YIUIInvokeTimerComponentWaitAsyncHandler : AInvokeHandler<YIUIInvokeWaitAsync, ETTask>
    {
        public override async ETTask Handle(YIUIInvokeWaitAsync args)
        {
            await YIUIMgrComponent.Inst?.Root().GetComponent<TimerComponent>().WaitAsync(args.Time);
        }
    }
}