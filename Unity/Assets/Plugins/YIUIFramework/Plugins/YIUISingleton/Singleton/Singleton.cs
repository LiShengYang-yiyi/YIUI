using System;
using UnityEngine;

namespace YIUIFramework
{
    public abstract class Singleton<T> : ISingleton where T : Singleton<T>, new()
    {
        private static T g_Inst;

        /// <summary>
        /// 是否存在
        /// </summary>
        public static bool Exist => g_Inst != null;

        /// <summary>
        /// 是否已释放
        /// </summary>
        private bool m_Disposed;

        public bool Disposed => m_Disposed;

        protected Singleton()
        {
            if (g_Inst != null)
            {
                #if UNITY_EDITOR
                throw new Exception(this + "是单例类，不能实例化两次");
                #endif
            }
        }

        public static T Inst
        {
            get
            {
                if (g_Inst == null)
                {
                    if (SingletonMgr.Disposing)
                    {
                        Debug.LogError($" {typeof (T).Name} 单例管理器已释放或未初始化 禁止使用");
                        return null;
                    }
                    
                    g_Inst = new T();
                    g_Inst.OnInitSingleton();
                    SingletonMgr.Add(g_Inst);
                }

                g_Inst.OnUseSingleton();
                return g_Inst;
            }
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

        //释放方法1: 对象释放
        public bool Dispose()
        {
            if (m_Disposed)
            {
                return false;
            }

            SingletonMgr.Remove(g_Inst);
            g_Inst     = default;
            m_Disposed = true;
            OnDispose();
            return true;
        }

        /// <summary>
        /// 处理释放相关事情
        /// </summary>
        protected virtual void OnDispose()
        {
        }

        //初始化回调
        protected virtual void OnInitSingleton()
        {
        }

        //每次使用前回调
        protected virtual void OnUseSingleton()
        {
        }
    }
}