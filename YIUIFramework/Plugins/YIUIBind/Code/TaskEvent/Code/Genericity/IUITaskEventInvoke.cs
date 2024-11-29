using Cysharp.Threading.Tasks;

namespace YIUIBind
{
    public interface IUITaskEventInvoke
    {
        UniTask Invoke();
    }

    public interface IUITaskEventInvoke<P1>
    {
        UniTask Invoke(P1 p1);
    }

    public interface IUITaskEventInvoke<P1, P2>
    {
        UniTask Invoke(P1 p1, P2 p2);
    }

    public interface IUITaskEventInvoke<P1, P2, P3>
    {
        UniTask Invoke(P1 p1, P2 p2, P3 p3);
    }

    public interface IUITaskEventInvoke<P1, P2, P3, P4>
    {
        UniTask Invoke(P1 p1, P2 p2, P3 p3, P4 p4);
    }

    public interface IUITaskEventInvoke<P1, P2, P3, P4, P5>
    {
        UniTask Invoke(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5);
    }
}