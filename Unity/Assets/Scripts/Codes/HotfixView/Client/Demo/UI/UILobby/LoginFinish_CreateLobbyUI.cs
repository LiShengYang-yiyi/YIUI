namespace ET.Client
{
	[Event(SceneType.Client)]
	public class LoginFinish_CreateLobbyUI: AEvent<EventType.LoginFinish>
	{
		protected override async ETTask Run(Scene scene, EventType.LoginFinish args)
		{
			await YIUIMgrComponent.Inst.OpenPanelAsync<LobbyPanelComponent>();
			//await UIHelper.Create(scene, UIType.UILobby, UILayer.Mid);
		}
	}
}
