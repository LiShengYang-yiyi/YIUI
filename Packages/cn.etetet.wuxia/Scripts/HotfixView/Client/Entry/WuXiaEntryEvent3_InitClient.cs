#if !YIUI_STATESYNC_DEMO
using YIUIFramework;

namespace ET.Client
{
    // 【武侠迁移 2026-08-18】项目自有 StateSync 客户端入口替代 Demo 入口。
    // 作用：由 cn.etetet.wuxia 接管客户端和 YIUI 初始化。
    // 原因：避免 Demo 包升级后重新夺取启动入口，导致两个 EntryEvent3 重复初始化。
    [Event(SceneType.StateSync)]
    public sealed class WuXiaEntryEvent3_InitClient : AEvent<Scene, EntryEvent3>
    {
        protected override async ETTask Run(Scene root, EntryEvent3 args)
        {
            root.AddComponent<GlobalComponent>();
            root.AddComponent<ResourcesLoaderComponent>();
            root.AddComponent<PlayerComponent>();
            root.AddComponent<CurrentScenesComponent>();

            bool initialized = await root.AddComponent<YIUIMgrComponent>().Initialize();
            if (!initialized)
            {
                Log.Error("WuXia YIUI initialization failed");
                return;
            }

            await EventSystem.Instance.PublishAsync(root, new AppStartInitFinish());
        }
    }
}
#endif
