
#if YIUI_STATESYNC_DEMO
namespace ET.Client
{
	// 【武侠迁移 2026-08-18】保留 Demo 登录事件供回滚，但正常 WuXia 运行不注册。
	// 作用：保留原代码并让 WuXia 独占 AppStartInitFinish 的界面跳转。
	// 原因：两个登录事件同时注册会创建重复的登录面板。
	[Event(SceneType.StateSync)]
	public class AppStartInitFinish_CreateLoginUI: AEvent<Scene, AppStartInitFinish>
	{
		protected override async ETTask Run(Scene root, AppStartInitFinish args)
		{
			await root.YIUIRoot().OpenPanelAsync<LoginPanelComponent>();
		}
	}
}
#endif
