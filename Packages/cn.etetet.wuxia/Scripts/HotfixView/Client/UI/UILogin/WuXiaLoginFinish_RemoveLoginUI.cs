#if !YIUI_STATESYNC_DEMO
namespace ET.Client
{
    // 【武侠迁移 2026-08-18】LoginHelper 成功后只关闭项目自有登录面板。
    // 作用：清理登录界面并把控制权交给大厅流程。
    // 原因：Demo 登录面板不参与 WuXia 正常运行，不能关闭错误的面板类型。
    [Event(SceneType.StateSync)]
    public sealed class WuXiaLoginFinish_RemoveLoginUI : AEvent<Scene, LoginFinish>
    {
        protected override async ETTask Run(Scene scene, LoginFinish args)
        {
            await scene.YIUIMgr().ClosePanelAsync<WuXiaLoginPanelComponent>();
        }
    }
}
#endif
