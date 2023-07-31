using Cysharp.Threading.Tasks;

namespace YIUIFramework
{
    /// <summary>
    /// 打开泛型 同步方法 内部还是异步打开
    /// 不想提供快捷操作可以删除此类
    /// </summary>
    public abstract partial class BasePanel
    {
        protected void OpenView<T>()
            where T : BaseView, new()
        {
            OpenViewAsync<T>().Forget();
        }

        protected void OpenView<T, P1>(P1 p1)
            where T : BaseView, IYIUIOpen<P1>, new()
        {
            OpenViewAsync<T, P1>(p1).Forget();
        }

        protected void OpenView<T, P1, P2>(P1 p1, P2 p2)
            where T : BaseView, IYIUIOpen<P1, P2>, new()
        {
            OpenViewAsync<T, P1, P2>(p1, p2).Forget();
        }

        protected void OpenView<T, P1, P2, P3>(P1 p1, P2 p2, P3 p3)
            where T : BaseView, IYIUIOpen<P1, P2, P3>, new()
        {
            OpenViewAsync<T, P1, P2, P3>(p1, p2, p3).Forget();
        }

        protected void OpenView<T, P1, P2, P3, P4>(P1 p1, P2 p2, P3 p3, P4 p4)
            where T : BaseView, IYIUIOpen<P1, P2, P3, P4>, new()
        {
            OpenViewAsync<T, P1, P2, P3, P4>(p1, p2, p3, p4).Forget();
        }

        protected void OpenView<T, P1, P2, P3, P4, P5>(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5)
            where T : BaseView, IYIUIOpen<P1, P2, P3, P4, P5>, new()
        {
            OpenViewAsync<T, P1, P2, P3, P4, P5>(p1, p2, p3, p4, p5).Forget();
        }
    }
}