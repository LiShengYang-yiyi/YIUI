#if YIUI_STATESYNC_DEMO
namespace ET.Client
{
    // 【武侠迁移 2026-08-18】默认停用，正常运行仅由 cn.etetet.wuxia 处理 LoginFinish。
    // 作用：保留原 Demo 关闭登录面板代码，定义 YIUI_STATESYNC_DEMO 时可回滚。
    // 原因：避免两个包同时关闭不同的登录面板造成状态错乱。
    [Event(SceneType.StateSync)]
    public class LoginFinish_RemoveLoginUI : AEvent<Scene, LoginFinish>
    {
        protected override async ETTask Run(Scene scene, LoginFinish args)
        {
            await scene.YIUIMgr().ClosePanelAsync<LoginPanelComponent>();
        }
    }
}
#endif
