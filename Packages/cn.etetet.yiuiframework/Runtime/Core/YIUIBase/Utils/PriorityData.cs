using System;
using System.Collections.Generic;

namespace ET
{
    public interface IPriorityData<out Key>
    {
        /// <summary>
        /// 对应的优先级
        /// </summary>
        int Priority { get; }

        /// <summary>
        /// 对应的数据唯一值
        /// </summary>
        Key DataKey { get; }
    }

    /// <summary>
    /// 优先级数据
    /// </summary>
    public class PriorityData<Key, Data> where Data : class, IPriorityData<Key>
    {
        public PriorityData() { }

        public PriorityData(Action<Data> handler)
        {
            OnActiveDataChangedHandler = handler;
        }

        /// <summary>
        /// 当前数据
        /// </summary>
        protected readonly Dictionary<int, List<Data>> m_CurrentDatas = new Dictionary<int, List<Data>>();

        /// <summary>
        /// 当前级别
        /// </summary>
        public int CurrentPriority { get; protected set; } = int.MinValue;

        /// <summary>
        /// 当前生效的数据
        /// </summary>
        internal Data CurrentActiveData { get; set; }

        /// <summary>
        /// 激活数据发生改变(如果没有数据了，激活的数据为null)
        /// </summary>
        private Action<Data> OnActiveDataChangedHandler;

        /// <summary>
        /// 设置激活数据发生改变事件
        /// </summary>
        /// <param name="handler"></param>
        public void SetDataChangedHandler(Action<Data> handler)
        {
            OnActiveDataChangedHandler = handler;
        }

        /// <summary>
        /// 清理
        /// </summary>
        public void Clear()
        {
            CurrentPriority = int.MinValue;
            m_CurrentDatas.Clear();
        }

        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose()
        {
            Clear();
            OnActiveDataChangedHandler = null;
        }

        /// <summary>
        /// 尝试添加数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="isPrioritySingle">是否相同优先级下只能一个</param>
        public void TryAddData(Data data, bool isPrioritySingle = false)
        {
            if (data == null)
            {
                return;
            }

            if (!m_CurrentDatas.TryGetValue(data.Priority, out List<Data> cachedData))
            {
                cachedData                    = new List<Data>();
                m_CurrentDatas[data.Priority] = cachedData;
            }

            if (cachedData.Contains(data))
            {
                return;
            }

            if (isPrioritySingle)
            {
                cachedData.Clear();
            }

            cachedData.Add(data);

            if (this.CurrentPriority <= data.Priority)
            {
                this.CurrentPriority   = data.Priority;
                this.CurrentActiveData = data;
                OnHandleChangeData(data);
            }
        }

        private void OnHandleChangeData(Data data)
        {
            OnActiveDataChangedHandler?.Invoke(data);
        }

        /// <summary>
        /// 尝试移除数据
        /// </summary>
        /// <param name="dataKey"></param>
        /// <param name="priority"></param>
        public void TryRemoveData(Key dataKey, int priority)
        {
            if (dataKey == null)
            {
                return;
            }

            // 错误的优先级，直接跳过
            if (!m_CurrentDatas.TryGetValue(priority, out List<Data> cachedData))
            {
                return;
            }

            var findItem = cachedData.Find(temp => temp.DataKey.Equals(dataKey));
            if (findItem == null)
            {
                // 对应的优先级中找到不到对应的内容
                return;
            }

            // 移除掉
            cachedData.Remove(findItem);
            if (cachedData.Count == 0)
            {
                m_CurrentDatas.Remove(priority);
            }

            if (CurrentPriority == priority)
            {
                int nextPriority = int.MinValue;
                foreach (var key in m_CurrentDatas.Keys)
                {
                    if (key > nextPriority)
                    {
                        nextPriority = key;
                    }
                }

                if (m_CurrentDatas.TryGetValue(nextPriority, out List<Data> nextCachedData))
                {
                    this.CurrentPriority   = nextPriority;
                    this.CurrentActiveData = nextCachedData[^1];
                    OnHandleChangeData(this.CurrentActiveData);
                }
                else
                {
                    this.CurrentPriority   = nextPriority;
                    this.CurrentActiveData = null;
                    OnHandleChangeData(this.CurrentActiveData);
                }
            }
        }
    }
}
