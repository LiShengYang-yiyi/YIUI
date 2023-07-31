using Cysharp.Threading.Tasks;

namespace YIUIFramework
{
    public interface IYIUIOpen
    {
    }

    public interface IYIUIOpen<P1> : IYIUIOpen
    {
        UniTask<bool> OnOpen(P1 p1);
    }

    public interface IYIUIOpen<P1, P2> : IYIUIOpen
    {
        UniTask<bool> OnOpen(P1 p1, P2 p2);
    }

    public interface IYIUIOpen<P1, P2, P3> : IYIUIOpen
    {
        UniTask<bool> OnOpen(P1 p1, P2 p2, P3 p3);
    }

    public interface IYIUIOpen<P1, P2, P3, P4> : IYIUIOpen
    {
        UniTask<bool> OnOpen(P1 p1, P2 p2, P3 p3, P4 p4);
    }

    public interface IYIUIOpen<P1, P2, P3, P4, P5> : IYIUIOpen
    {
        UniTask<bool> OnOpen(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5);
    }
}