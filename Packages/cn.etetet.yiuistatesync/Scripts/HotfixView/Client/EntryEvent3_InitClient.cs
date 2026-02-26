using System;
using System.Collections.Generic;
using System.IO;
using YIUIFramework;

namespace ET.Client
{
    [Event(SceneType.StateSync)]
    public class EntryEvent3_InitClient : AEvent<Scene, EntryEvent3>
    {
        protected override async ETTask Run(Scene root, EntryEvent3 args)
        {
            root.AddComponent<GlobalComponent>();
            root.AddComponent<ResourcesLoaderComponent>();
            root.AddComponent<PlayerComponent>();
            root.AddComponent<CurrentScenesComponent>();

            var result = await root.AddComponent<YIUIMgrComponent>().Initialize();
            if (!result)
            {
                Log.Error("初始化UI失败");
                return;
            }

            await EventSystem.Instance.PublishAsync(root, new AppStartInitFinish());
        }
    }
}