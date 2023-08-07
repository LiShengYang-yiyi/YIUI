#if UNITY_EDITOR

using Sirenix.OdinInspector;

namespace YIUIFramework.Editor
{
    /// <summary>
    /// 基类 自动创建模块
    /// 由其他模块继承后实现
    /// </summary>
    [HideReferenceObjectPicker]
    public abstract class BaseCreateModule
    {
        internal virtual void Initialize()
        {
        }

        internal virtual void OnDestroy()
        {
        }
    }
}
#endif