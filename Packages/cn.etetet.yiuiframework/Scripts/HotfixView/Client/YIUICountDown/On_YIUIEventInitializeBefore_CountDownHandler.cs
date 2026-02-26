namespace ET.Client
{
    [Event(SceneType.All)]
    public class On_YIUIEventInitializeBefore_CountDownHandler : AEvent<Scene, YIUIEventInitializeBefore>
    {
        protected override async ETTask Run(Scene scene, YIUIEventInitializeBefore arg)
        {
            var yiuiMgr = scene.GetComponent<YIUIMgrComponent>();
            yiuiMgr.AddComponent<CountDownMgr>();
            await ETTask.CompletedTask;
        }
    }
}