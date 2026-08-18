#if !YIUI_STATESYNC_DEMO
namespace ET.Client
{
    // 【武侠迁移 2026-08-18】EnterMapHelper 完成场景切换后打开项目自有 HUD。
    // 作用：进入地图后创建主界面占位面板。
    // 原因：Demo 的 UIHelp 事件已停用，避免地图 HUD 重复创建。
    [Event(SceneType.Current)]
    public sealed class WuXiaSceneChangeFinish_CreateMainUI : AEvent<Scene, SceneChangeFinish>
    {
        protected override async ETTask Run(Scene scene, SceneChangeFinish args)
        {
            await scene.YIUIRoot().OpenPanelAsync<WuXiaMainPanelComponent>();
        }
    }
}
#endif
