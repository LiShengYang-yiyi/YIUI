
#if YIUI_STATESYNC_DEMO
namespace ET.Client
{
	// 【武侠迁移 2026-08-18】Demo 大厅默认停用，避免与 WuXia 同时打开大厅。
	// 作用：保留原代码并用条件编译提供明确的回滚开关。
	// 原因：正常项目运行只允许一个 LoginFinish 大厅事件生效。
	[Event(SceneType.StateSync)]
	public class LoginFinish_CreateLobbyUI: AEvent<Scene, LoginFinish>
	{
		protected override async ETTask Run(Scene scene, LoginFinish args)
		{
			await scene.YIUIRoot().OpenPanelAsync<LobbyPanelComponent>();
		}
	}
}
#endif
