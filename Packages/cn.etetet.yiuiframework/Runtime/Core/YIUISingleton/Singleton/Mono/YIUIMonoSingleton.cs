using System;
using ET;
using UnityEngine;

namespace YIUIFramework
{
    /// <summary>
    /// 组件的单例类基类
    /// 注意：如果这个单例实现不能存在于场景,会自动在场景上创建一个GO，并把类挂在他下面
    /// 如果不希望自动创建，请使用MonoSceneSingleton。
    /// 它默认会设置DontDestroyOnLoad， 如果有其它需求，请覆写CanDestroyOnLoad。
    /// 它默认会给go一个合适的名字，如果有其它需求，请覆写CreateName。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class YIUIMonoSingleton<T> : YIUIDisposerMonoSingleton where T : YIUIMonoSingleton<T>
    {
        private EntityRef<Entity> EntityRef;
        protected Entity Entity => EntityRef;

        private static T g_Inst;

        /// <summary>
        /// 是否存在
        /// </summary>
        public static bool Exist => g_Inst != null;

        /// <summary>
        /// 得到单例
        /// </summary>
        public static T Inst
        {
            get
            {
                if (g_Inst == null)
                {
                    if (!UIOperationHelper.IsPlaying())
                    {
                        Debug.LogError($"非运行时 禁止调用");
                        return null;
                    }

                    if (YIUISingletonHelper.Disposing)
                    {
                        Debug.LogError($" {typeof(T).Name} 单例管理器已释放或未初始化 禁止使用");
                        return null;
                    }

                    var go = new GameObject();
                    g_Inst = go.AddComponent<T>();
                    g_Inst.EntityRef = YIUISingletonHelper.YIUIMgr;
                    go.name = g_Inst.GetCreateName();
                    if (g_Inst.GetDontDestroyOnLoad())
                    {
                        DontDestroyOnLoad(go);
                    }

                    if (g_Inst.GetHideAndDontSave())
                    {
                        go.hideFlags = HideFlags.HideAndDontSave;
                    }

                    YIUISingletonHelper.Add(g_Inst);
                    g_Inst.OnInitSingleton();
                }

                g_Inst.OnUseSingleton();
                return g_Inst;
            }
        }

        private string GetCreateName()
        {
            return $"[Singleton]{typeof(T).Name}";
        }

        protected override bool GetHideAndDontSave()
        {
            return true;
        }

        //子类如果使用这个生命周期记得调用base
        //推荐使用 重写 OnDispose
        protected override void OnDestroy()
        {
            base.OnDestroy();
            YIUISingletonHelper.Remove(g_Inst);
            g_Inst = null;
        }

        //释放方法2: 静态释放
        public static bool DisposeInst()
        {
            if (g_Inst == null)
            {
                return true;
            }

            return g_Inst.Dispose();
        }
    }
}