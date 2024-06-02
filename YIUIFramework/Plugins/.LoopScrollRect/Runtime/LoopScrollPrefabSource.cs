using Cysharp.Threading.Tasks;

namespace UnityEngine.UI
{
    public interface LoopScrollPrefabAsyncSource
    {
        UniTask<GameObject> GetObject(int index);

        void ReturnObject(Transform trans);
    }
}
