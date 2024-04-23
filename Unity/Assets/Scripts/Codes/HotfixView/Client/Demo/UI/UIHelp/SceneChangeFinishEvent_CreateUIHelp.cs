﻿namespace ET.Client
{
    [Event(SceneType.Current)]
    public class SceneChangeFinishEvent_CreateUIHelp : AEvent<EventType.SceneChangeFinish>
    {
        protected override async ETTask Run(Scene scene, EventType.SceneChangeFinish args)
        {
            await scene.GetComponent<YIUIRootComponent>().OpenPanelAsync<MainPanelComponent>();
            //await UIHelper.Create(scene, UIType.UIHelp, UILayer.Mid);
        }
    }
}
