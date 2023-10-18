using System;
using System.IO;
using YIUIFramework;

namespace ET.Client
{
    [Event(SceneType.Process)]
    public class EntryEvent3_InitClient: AEvent<ET.EventType.EntryEvent3>
    {
        protected override async ETTask Run(Scene scene, ET.EventType.EntryEvent3 args)
        {
            // 加载配置
            Root.Instance.Scene.AddComponent<ResourcesComponent>();

            Root.Instance.Scene.AddComponent<GlobalComponent>();

            await ResourcesComponent.Instance.LoadBundleAsync("unit.unity3d");

            Scene clientScene = await SceneFactory.CreateClientScene(1, "Game");

            YIUIBindHelper.InternalGameGetUIBindVoFunc = YIUICodeGenerated.YIUIBindProvider.Get;
            await clientScene.AddComponent<YIUIMgrComponent>().Initialize();

            #region 根据需求自行处理
            //在editor下自动打开  也可以根据各种外围配置 或者 GM等级打开
            #if UNITY_EDITOR
            clientScene.AddComponent<GMCommandComponent>();
            #endif
            #endregion

            await EventSystem.Instance.PublishAsync(clientScene, new EventType.AppStartInitFinish());
        }
    }
}