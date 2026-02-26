using YIUIFramework;

namespace ET.Client
{
    [Event(SceneType.All)]
    public class YIUIEventInitializeAfterGMHandler : AEvent<Scene, YIUIEventInitializeAfter>
    {
        protected override async ETTask Run(Scene scene, YIUIEventInitializeAfter arg)
        {
            //新增 可以通过配置关闭GM功能
            if (YIUIConstHelper.Const.CloseGMCommand)
            {
                return;
            }

            //根据需求自行处理 在Editor下自动打开  也可以根据各种外围配置 或者 GM等级打开
            //#if UNITY_EDITOR

            {
                scene.AddComponent<GMCommandComponent>();
            }

            //#endif

            await ETTask.CompletedTask;
        }
    }
}