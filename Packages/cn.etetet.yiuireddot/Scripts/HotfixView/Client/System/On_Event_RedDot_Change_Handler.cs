using YIUIFramework;

namespace ET.Client
{
    [Event(SceneType.All)]
    public class On_Event_RedDot_Change_Handler : AEvent<Scene, Event_RedDot_Change>
    {
        protected override async ETTask Run(Scene currentScene, Event_RedDot_Change args)
        {
            RedDotMgr.Inst.SetCount(args.RedDotId, args.Count);
            await ETTask.CompletedTask;
        }
    }
}