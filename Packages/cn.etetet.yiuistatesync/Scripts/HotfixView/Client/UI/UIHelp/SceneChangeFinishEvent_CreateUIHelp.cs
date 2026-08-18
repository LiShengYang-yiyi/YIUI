
#if YIUI_STATESYNC_DEMO
namespace ET.Client
{
    // 【武侠迁移 2026-08-18】地图切换后的 HUD 已由 WuXia 接管。
    // 作用：保留原 Demo 事件，仅在定义 YIUI_STATESYNC_DEMO 回滚时注册。
    // 原因：避免 Demo 和 WuXia 同时创建地图 HUD。
    [Event(SceneType.Current)]
    public class SceneChangeFinishEvent_CreateUIHelp : AEvent<Scene, SceneChangeFinish>
    {
        protected override async ETTask Run(Scene scene, SceneChangeFinish args)
        {
            await scene.YIUIRoot().OpenPanelAsync<MainPanelComponent>();
        }
    }
}
#endif
