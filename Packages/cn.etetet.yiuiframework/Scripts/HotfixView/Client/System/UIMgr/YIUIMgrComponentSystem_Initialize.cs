using YIUIFramework;

namespace ET.Client
{
    public static partial class YIUIMgrComponentSystem
    {
        //YIUI 初始化
        //不要在这个类中增加逻辑,应该用其他消息来实现
        //如果觉得消息不够顺序不对可扩展
        public static async ETTask<bool> Initialize(this YIUIMgrComponent self)
        {
            EntityRef<YIUIMgrComponent> selfRef = self;

            //YIUI资源管理
            var loadComponent = self.AddComponent<YIUILoadComponent>();
            var loadResult = await loadComponent.Initialize();
            if (!loadResult) return false;

            //YIUI常量管理
            self = selfRef;
            var constResult = await YIUIConstHelper.LoadAsset(self.Scene());
            if (!constResult) return false;

            //初始化UI绑定
            self = selfRef;
            var bindComponent = self.AddComponent<YIUIBindComponent>();
            var bindResult = bindComponent.InitAllBind(YIUICodeGenerated.YIUIBindProvider.Get());
            if (!bindResult) return false;

            //初始化其他UI框架中的管理器
            self = selfRef;
            await EventSystem.Instance.PublishAsync(self.Scene(), new YIUIEventInitializeBefore());

            //初始化所有YIUI相关 单例
            self = selfRef;
            await YIUISingletonHelper.InitializeAll(self);

            //初始化YIUIRoot
            self = selfRef;
            var rootResult = await self.InitRoot();
            if (!rootResult) return false;
            self = selfRef;
            self.InitSafeArea();

            //其他模块各自初始化
            await EventSystem.Instance.PublishAsync(self.Scene(), new YIUIEventInitializeAfter());

            return true;
        }
    }
}