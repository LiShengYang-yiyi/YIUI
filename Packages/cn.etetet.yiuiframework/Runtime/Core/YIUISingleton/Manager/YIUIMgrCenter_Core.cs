#define YIUIMACRO_SINGLETON_LOG
using System;
using System.Collections.Generic;
using System.Diagnostics;
using ET;
using Debug = UnityEngine.Debug;

namespace YIUIFramework
{
    public partial class YIUIMgrCenter
    {
        //内部类 核心管理
        private class MgrCore
        {
            private List<IYIUIManager>            m_MgrList            = new List<IYIUIManager>();
            private List<IYIUIManagerUpdate>      m_MgrUpdateList      = new List<IYIUIManagerUpdate>();
            private List<IYIUIManagerLateUpdate>  m_MgrLateUpdateList  = new List<IYIUIManagerLateUpdate>();
            private List<IYIUIManagerFixedUpdate> m_MgrFixedUpdateList = new List<IYIUIManagerFixedUpdate>();
            private HashSet<IYIUIManager>         m_CacheInitMgr       = new HashSet<IYIUIManager>();

            public async ETTask<bool> Add(IYIUIManager manager)
            {
                if (m_MgrList.Contains(manager))
                {
                    Debug.LogError($"已存在Mgr {manager.GetType().Name} 请勿重复注册");
                    return false;
                }

                if (m_CacheInitMgr.Contains(manager))
                {
                    Debug.LogError($"{manager.GetType().Name} 请等待异步初始化中的管理器  请检查是否调用错误");
                    return false;
                }

                m_CacheInitMgr.Add(manager);

                if (manager is IYIUIManagerAsyncInit initialize)
                {
                    #if YIUIMACRO_SINGLETON_LOG
                    var sw = new Stopwatch();
                    sw.Start();
                    #endif

                    var result = await initialize.ManagerAsyncInit();

                    #if YIUIMACRO_SINGLETON_LOG
                    sw.Stop();
                    Debug.Log($"<color=green>MgrCenter: 管理器[<color=Brown>{manager.GetType().Name}</color>]初始化耗时 {sw.ElapsedMilliseconds} 毫秒</color>");
                    #endif

                    if (!result)
                    {
                        #if YIUIMACRO_SINGLETON_LOG
                        Debug.Log($"<color=red>MgrCenter: 管理器[<color=Brown>{manager.GetType().Name}</color>]初始化失败</color>");
                        #endif
                        return false; //初始化失败的管理器 不添加
                    }
                }

                m_CacheInitMgr.Remove(manager);

                #if YIUIMACRO_SINGLETON_LOG
                Debug.Log($"<color=navy>MgrCenter: 管理器[<color=Brown>{manager.GetType().Name}</color>]启动完毕</color>");
                #endif

                m_MgrList.Add(manager);

                //YIUIDisposerMonoSingleton 类型的单例 因为继承UnityMono 其实是可以使用自带的Update
                //也可以使用YIUI提供的独立Update 2种有区分 2选一即可

                if (manager is IYIUIManagerUpdate update)
                {
                    m_MgrUpdateList.Add(update);
                }

                if (manager is IYIUIManagerLateUpdate lateUpdate)
                {
                    m_MgrLateUpdateList.Add(lateUpdate);
                }

                if (manager is IYIUIManagerFixedUpdate fixedUpdate)
                {
                    m_MgrFixedUpdateList.Add(fixedUpdate);
                }

                return true;
            }

            public void Update()
            {
                for (int i = 0; i < m_MgrUpdateList.Count; i++)
                {
                    IYIUIManagerUpdate manager = m_MgrUpdateList[i];

                    if (manager.Disposed) continue;
                    if (!manager.Enabled) continue;

                    try
                    {
                        manager.ManagerUpdate();
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"管理器={manager.GetType().Name}, err={e.Message}{e.StackTrace}");
                    }
                }

                CheckDisposed();
            }

            public void LateUpdate()
            {
                for (int i = 0; i < m_MgrLateUpdateList.Count; i++)
                {
                    IYIUIManagerLateUpdate manager = m_MgrLateUpdateList[i];

                    if (manager.Disposed) continue;
                    if (!manager.Enabled) continue;

                    try
                    {
                        manager.ManagerLateUpdate();
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"管理器={manager.GetType().Name}, err={e.Message}{e.StackTrace}");
                    }
                }
            }

            public void FixedUpdate()
            {
                for (int i = 0; i < m_MgrFixedUpdateList.Count; i++)
                {
                    IYIUIManagerFixedUpdate manager = m_MgrFixedUpdateList[i];

                    if (manager.Disposed) continue;
                    if (!manager.Enabled) continue;

                    try
                    {
                        manager.ManagerFixedUpdate();
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"管理器={manager.GetType().Name}, err={e.Message}{e.StackTrace}");
                    }
                }
            }

            private void CheckDisposed()
            {
                for (int i = m_MgrList.Count - 1; i >= 0; i--)
                {
                    IYIUIManager manager = m_MgrList[i];
                    if (manager.Disposed)
                    {
                        Remove(manager);
                    }
                }
            }

            private void Remove(IYIUIManager manager)
            {
                if (manager == null)
                {
                    return;
                }

                #if YIUIMACRO_SINGLETON_LOG
                Debug.Log($"<color=navy>MgrCenter: 管理器[<color=Brown>{manager.GetType().Name}</color>]移除</color>");
                #endif

                m_MgrList.Remove(manager);

                if (manager is IYIUIManagerUpdate update)
                {
                    m_MgrUpdateList.Remove(update);
                }

                if (manager is IYIUIManagerLateUpdate lateUpdate)
                {
                    m_MgrLateUpdateList.Remove(lateUpdate);
                }

                if (manager is IYIUIManagerFixedUpdate fixedUpdate)
                {
                    m_MgrFixedUpdateList.Remove(fixedUpdate);
                }
            }

            public void Dispose()
            {
                #if YIUIMACRO_SINGLETON_LOG
                Debug.Log("<color=navy>MgrCenter: 关闭MgrCenter</color>");
                #endif

                //倒过来dispose
                for (int i = m_MgrList.Count - 1; i >= 0; i--)
                {
                    IYIUIManager manager = m_MgrList[i];
                    manager.Dispose();
                }
            }
        }
    }
}
