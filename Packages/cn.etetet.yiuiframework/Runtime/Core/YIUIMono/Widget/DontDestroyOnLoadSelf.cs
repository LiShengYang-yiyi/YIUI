using UnityEngine;

namespace YIUIFramework
{
    [AddComponentMenu("YIUIFramework/Widget/跨场景不摧毁 【DontDestroyOnLoadSelf】")]
    public class DontDestroyOnLoadSelf : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }
    }
}
