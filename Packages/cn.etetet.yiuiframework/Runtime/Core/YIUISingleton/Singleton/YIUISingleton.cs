using System;
using ET;
using UnityEngine;

namespace YIUIFramework
{
    /// <summary>
    /// 单例
    /// </summary>
    public abstract class YIUISingleton<T> : IYIUIManagerAsyncInit where T : YIUISingleton<T>, new()
    {
        private EntityRef<Entity> EntityRef;

        protected Entity Entity => EntityRef;

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

        protected YIUISingleton()
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
                    if (YIUISingletonHelper.Disposing)
                    {
                        Debug.LogError($" {typeof(T).Name} 单例管理器已释放或未初始化 禁止使用");
                        return null;
                    }

                    g_Inst = new T();
                    g_Inst.EntityRef = YIUISingletonHelper.YIUIMgr;
                    g_Inst.OnInitSingleton();
                    YIUISingletonHelper.Add(g_Inst);
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

            YIUISingletonHelper.Remove(g_Inst);
            g_Inst = default;
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

        //初始化
        protected virtual void OnInitSingleton()
        {
        }

        //每次使用前
        protected virtual void OnUseSingleton()
        {
        }

        private bool m_Enabled;

        public bool Enabled => m_Enabled;

        public void SetEnabled(bool value)
        {
            m_Enabled = value;
        }

        private bool m_InitedSucceed;

        public bool InitedSucceed => m_InitedSucceed;

        public async ETTask<bool> ManagerAsyncInit()
        {
            if (m_InitedSucceed)
            {
                Debug.LogError($"{typeof(T).Name}已成功初始化过 请勿重复初始化");
                return true;
            }

            var result = await MgrAsyncInit();
            if (!result)
            {
                Debug.LogError($"{typeof(T).Name} 初始化失败");
            }
            else
            {
                //成功初始化才记录
                m_InitedSucceed = true;
            }

            m_Enabled = true;
            return result;
        }

        protected virtual async ETTask<bool> MgrAsyncInit()
        {
            await ETTask.CompletedTask;
            return true;
        }
    }
}