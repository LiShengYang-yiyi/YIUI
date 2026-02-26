using System;
using System.Collections;
using System.Collections.Generic;

namespace YIUIFramework
{
    /// <summary>
    /// 优先级队列
    /// 是一个用于查找最大值的最大堆。
    /// </summary>
    public sealed class PriorityQueue<T> : IEnumerable<T>
    {
        private IComparer<T> comparer;
        private T[]          heap;
        private HashSet<T>   fastFinder = new HashSet<T>();

        public PriorityQueue()
                : this(null)
        {
        }

        public PriorityQueue(int capacity)
                : this(capacity, null)
        {
        }

        public PriorityQueue(IComparer<T> comparer)
                : this(16, comparer)
        {
        }

        public PriorityQueue(int capacity, IComparer<T> comparer)
        {
            this.comparer = (comparer == null) ? Comparer<T>.Default : comparer;
            this.heap     = new T[capacity];
        }

        /// <summary>
        /// 获取此队列中的计数。
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// 获取指定索引处的对象。
        /// </summary>
        public T this[int index]
        {
            get { return this.heap[index]; }
        }

        public bool Contains(T v)
        {
            return fastFinder.Contains(v);
        }

        /// <summary>
        /// 获取该项的枚举数。
        /// </summary>
        public IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(this.heap, this.Count);
        }

        /// <summary>
        /// 获取该项的枚举数。
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// 清理这个容器。
        /// </summary>
        public void Clear()
        {
            this.Count = 0;
            this.fastFinder.Clear();
        }

        /// <summary>
        /// 将新值压入此队列。
        /// </summary>
        public void Push(T v)
        {
            if (this.Count >= this.heap.Length)
            {
                Array.Resize(ref this.heap, this.Count * 2);
            }

            this.heap[this.Count] = v;
            this.SiftUp(this.Count++);
            fastFinder.Add(v);
        }

        /// <summary>
        /// 从这个队列中弹出最大值。
        /// </summary>
        public T Pop()
        {
            var v = this.Top();
            this.heap[0] = this.heap[--this.Count];
            if (this.Count > 0)
            {
                this.SiftDown(0);
            }

            fastFinder.Remove(v);
            return v;
        }

        /// <summary>
        /// 访问此队列中的最大值。
        /// </summary>
        public T Top()
        {
            if (this.Count > 0)
            {
                return this.heap[0];
            }

            throw new InvalidOperationException("The PriorityQueue is empty.");
        }

        private void SiftUp(int n)
        {
            var v = this.heap[n];
            for (var n2 = n / 2;
                 n > 0 && this.comparer.Compare(v, this.heap[n2]) > 0;
                 n = n2, n2 /= 2)
            {
                this.heap[n] = this.heap[n2];
            }

            this.heap[n] = v;
        }

        private void SiftDown(int n)
        {
            var v = this.heap[n];
            for (var n2 = n * 2; n2 < this.Count; n = n2, n2 *= 2)
            {
                if (n2 + 1 < this.Count &&
                    this.comparer.Compare(this.heap[n2 + 1], this.heap[n2]) > 0)
                {
                    ++n2;
                }

                if (this.comparer.Compare(v, this.heap[n2]) >= 0)
                {
                    break;
                }

                this.heap[n] = this.heap[n2];
            }

            this.heap[n] = v;
        }

        public struct Enumerator : IEnumerator<T>
        {
            private readonly T[] heap;
            private readonly int count;
            private          int index;

            internal Enumerator(T[] heap, int count)
            {
                this.heap  = heap;
                this.count = count;
                this.index = -1;
            }

            public T Current
            {
                get { return this.heap[this.index]; }
            }

            object IEnumerator.Current
            {
                get { return this.Current; }
            }

            public void Dispose()
            {
            }

            public void Reset()
            {
                this.index = -1;
            }

            public bool MoveNext()
            {
                return (this.index <= this.count) &&
                        (++this.index < this.count);
            }
        }
    }
}
