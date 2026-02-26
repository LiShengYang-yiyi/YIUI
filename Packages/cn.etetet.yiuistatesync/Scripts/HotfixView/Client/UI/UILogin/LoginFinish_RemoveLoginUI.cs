namespace ET.Client
{
    [Event(SceneType.StateSync)]
    public class LoginFinish_RemoveLoginUI : AEvent<Scene, LoginFinish>
    {
        protected override async ETTask Run(Scene scene, LoginFinish args)
        {
            await scene.YIUIMgr().ClosePanelAsync<LoginPanelComponent>();
        }
    }
}