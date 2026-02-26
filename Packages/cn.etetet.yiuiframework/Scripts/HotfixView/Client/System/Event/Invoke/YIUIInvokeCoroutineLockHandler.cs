using UnityEngine;
using YIUIFramework;

namespace ET.Client
{
    [Invoke(EYIUIInvokeType.Async)]
    public class YIUIInvokeCoroutineLockHandler : AInvokeEntityHandler<YIUIInvokeEntity_CoroutineLock, ETTask<Entity>>
    {
        public override async ETTask<Entity> Handle(Entity entity, YIUIInvokeEntity_CoroutineLock args)
        {
            var root = entity?.Root();
            if (root == null)
            {
                Log.Error($"没有找到root {entity.IsDisposed}");
                return null;
            }

            var lockType = args.LockType;
            if (lockType <= 0)
            {
                lockType = CoroutineLockType.YIUIInvokeCoroutineLock;
            }

            return await root.GetComponent<CoroutineLockComponent>().Wait(lockType, args.Lock);
        }
    }
}