#if !YIUI_STATESYNC_DEMO
namespace ET.Client
{
    // 【武侠迁移 2026-08-18】将 LoginFinish 路由到项目自有大厅，而不是 Demo 大厅。
    // 作用：登录成功后创建大厅并执行后续进入地图流程。
    // 原因：保持登录、大厅、地图流程全部由 WuXia 包统一管理。
    [Event(SceneType.StateSync)]
    public sealed class WuXiaLoginFinish_CreateLobbyUI : AEvent<Scene, LoginFinish>
    {
        protected override async ETTask Run(Scene scene, LoginFinish args)
        {
            await scene.YIUIRoot().OpenPanelAsync<WuXiaLobbyPanelComponent>();
        }
    }
}
#endif
