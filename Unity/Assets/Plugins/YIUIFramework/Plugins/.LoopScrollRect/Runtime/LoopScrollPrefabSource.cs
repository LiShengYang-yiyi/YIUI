using UnityEngine;
using System.Collections;
using ET;

namespace UnityEngine.UI
{
    public interface LoopScrollPrefabAsyncSource
    {
        ETTask<GameObject> GetObject(int index);

        void ReturnObject(Transform trans);
    }
}
