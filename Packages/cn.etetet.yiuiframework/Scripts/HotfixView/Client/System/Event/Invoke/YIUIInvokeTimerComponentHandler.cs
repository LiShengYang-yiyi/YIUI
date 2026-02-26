using YIUIFramework;

namespace ET.Client
{
    [Invoke(EYIUIInvokeType.Async)]
    public class YIUIInvokeTimerComponentWaitFrameAsyncHandler : AInvokeEntityHandler<YIUIInvokeEntity_WaitFrameAsync, ETTask>
    {
        public override async ETTask Handle(Entity entity, YIUIInvokeEntity_WaitFrameAsync args)
        {
            await entity.Root().GetComponent<TimerComponent>().WaitFrameAsync();
        }
    }

    [Invoke(EYIUIInvokeType.Async)]
    public class YIUIInvokeTimerComponentWaitAsyncHandler : AInvokeEntityHandler<YIUIInvokeEntity_WaitAsync, ETTask>
    {
        public override async ETTask Handle(Entity entity, YIUIInvokeEntity_WaitAsync args)
        {
            if (args.CancellationToken == null)
            {
                await entity.Root().GetComponent<TimerComponent>().WaitAsync(args.Time);
            }
            else
            {
                await entity.Root().GetComponent<TimerComponent>().WaitAsync(args.Time).NewContext(args.CancellationToken);
            }
        }
    }

    [Invoke(EYIUIInvokeType.Async)]
    public class YIUIInvokeTimerComponentWaitSecondAsyncHandler : AInvokeEntityHandler<YIUIInvokeEntity_WaitSecondAsync, ETTask>
    {
        public override async ETTask Handle(Entity entity, YIUIInvokeEntity_WaitSecondAsync args)
        {
            if (args.CancellationToken == null)
            {
                await entity.Root().GetComponent<TimerComponent>().WaitAsync((long)(args.Time * 1000));
            }
            else
            {
                await entity.Root().GetComponent<TimerComponent>().WaitAsync((long)(args.Time * 1000)).NewContext(args.CancellationToken);
            }
        }
    }
}