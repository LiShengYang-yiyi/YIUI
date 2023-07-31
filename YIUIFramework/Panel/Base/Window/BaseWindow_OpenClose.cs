using Cysharp.Threading.Tasks;

namespace YIUIFramework
{
    //打开关闭
    public abstract partial class BaseWindow
    {
        //打开有3种
        //1 无参打开 OnOpen
        //2 ParamVo参数打开 也叫 ParamOpen 打开内部是List<obj>参数 所以可以支持无限长度
        //3 继承接口IOpen的 也叫 IOpen 打开 最高支持5个泛型参数
        //前2种都会自带 第三种根据需求自己继承

        /// <summary>
        /// 打开UI 无参
        /// </summary>
        protected virtual async UniTask<bool> OnOpen()
        {
            await UniTask.CompletedTask;
            return true;
        }

        /// <summary>
        /// 打开UI 带参数的
        /// 需要自己解析参数
        /// 适合用字符串开启的 参数可字符串配置那种
        /// 默认会调用onopen
        /// </summary>
        protected virtual async UniTask<bool> OnOpen(ParamVo param)
        {
            return await OnOpen();
        }

        /// <summary>
        /// UI被关闭
        /// 与OnDisable 不同  Disable 只是显影操作不代表被关闭
        /// 与OnDestroy 不同  Destroy 是摧毁 但是因为有缓存界面的原因 当被缓存时 OnDestroy是不会来的
        /// 这个时候你想要知道是不是被关闭了就必须通过OnClose
        /// baseView除外 因为view的关闭就是隐藏 所以 view的 OnDisable = OnClose
        /// </summary>
        internal virtual void OnClose()
        {
        }
    }
}