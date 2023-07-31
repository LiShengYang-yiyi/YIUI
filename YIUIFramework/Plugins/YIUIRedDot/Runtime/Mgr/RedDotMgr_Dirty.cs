using System.Collections.Generic;
using YIUIBind;

namespace YIUIFramework
{
    /// <summary>
    /// 红点 管理器
    /// 脏数据管理 防止频繁调用
    /// </summary>
    public partial class RedDotMgr
    {
        //已存标记
        private HashSet<RedDotData> m_DirtyData;

        //1秒内刷新多少次
        private const int m_RedDotDirtyFrame = 2;

        private int m_LoopId;

        //初始化异步脏标
        private void InitAsyncDirty()
        {
            m_DirtyData = new HashSet<RedDotData>();
            m_LoopId    = SchedulerMgr.Loop(RedDotUpdateRefresh, 1f / m_RedDotDirtyFrame);
        }

        private void DisposeDirty()
        {
            SchedulerMgr.StopLoop(m_LoopId);
        }

        private void RedDotUpdateRefresh()
        {
            RefreshDirtyCount();
        }

        private bool TryDirtySetCount(RedDotData data, int count)
        {
            m_DirtyData.Remove(data);
            var result = data.TryDirtySetCount(count);
            if (result)
            {
                m_DirtyData.Add(data);
            }

            return result;
        }

        private void RefreshDirtyCount()
        {
            foreach (var data in m_DirtyData)
            {
                data.RefreshDirtyCount();
            }

            m_DirtyData.Clear();
        }
    }
}