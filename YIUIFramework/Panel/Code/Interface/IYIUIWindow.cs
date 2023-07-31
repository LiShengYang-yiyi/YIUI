using Cysharp.Threading.Tasks;

namespace YIUIFramework
{
    public interface IYIUIWindow
    {
        UniTask<bool> Open();
        UniTask<bool> Open(ParamVo                param);
        UniTask<bool> Open<P1>(P1                 p1);
        UniTask<bool> Open<P1, P2>(P1             p1, P2 p2);
        UniTask<bool> Open<P1, P2, P3>(P1         p1, P2 p2, P3 p3);
        UniTask<bool> Open<P1, P2, P3, P4>(P1     p1, P2 p2, P3 p3, P4 p4);
        UniTask<bool> Open<P1, P2, P3, P4, P5>(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5);

        /// <summary>
        /// 窗口选项
        /// </summary>
        EWindowOption WindowOption { get; }

        /// <summary>
        /// 显隐状态
        /// </summary>
        bool ActiveSelf { get; }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        void Close(bool tween = true, bool ignoreElse = false);
    }
}