#if !YIUI_STATESYNC_DEMO
namespace ET.Client
{
    // 【武侠迁移 2026-08-18】客户端和 YIUI 初始化完成后打开项目自有登录面板。
    // 作用：把 AppStartInitFinish 转换为 WuXia 登录界面入口。
    // 原因：Demo 登录事件已停用，避免两个 LoginPanel 同时打开。
    [Event(SceneType.StateSync)]
    public sealed class WuXiaAppStartInitFinish_CreateLoginUI : AEvent<Scene, AppStartInitFinish>
    {
        protected override async ETTask Run(Scene root, AppStartInitFinish args)
        {
            await root.YIUIRoot().OpenPanelAsync<WuXiaLoginPanelComponent>();
        }
    }
}
#endif
