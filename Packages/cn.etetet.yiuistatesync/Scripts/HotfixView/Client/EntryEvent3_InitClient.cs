#if YIUI_STATESYNC_DEMO
using System;
using System.Collections.Generic;
using System.IO;
using YIUIFramework;

namespace ET.Client
{
    // 【武侠迁移 2026-08-18】项目已切换到 cn.etetet.wuxia，Demo 启动入口默认停用。
    // 作用：保留原代码并通过条件编译提供回滚和调试开关。
    // 原因：同时注册两个 EntryEvent3 会重复添加根组件并初始化两次 YIUI。
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
#endif
