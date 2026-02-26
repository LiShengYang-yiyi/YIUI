
namespace ET.Client
{
	[Event(SceneType.StateSync)]
	public class AppStartInitFinish_CreateLoginUI: AEvent<Scene, AppStartInitFinish>
	{
		protected override async ETTask Run(Scene root, AppStartInitFinish args)
		{
			await root.YIUIRoot().OpenPanelAsync<LoginPanelComponent>();
		}
	}
}
