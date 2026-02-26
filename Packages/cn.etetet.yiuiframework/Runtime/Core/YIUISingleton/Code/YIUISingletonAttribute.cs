using CommandLine;

namespace YIUIFramework
{
    /// <summary>
    /// 自启动特性
    /// </summary>
    public class YIUISingletonAttribute : BaseAttribute
    {
        /// <summary>
        /// 启动优先级
        /// 数字越小，优先级越高
        /// </summary>
        public int Order { get; set; }

        public YIUISingletonAttribute(int order = 0)
        {
            Order = order;
        }
    }
}
