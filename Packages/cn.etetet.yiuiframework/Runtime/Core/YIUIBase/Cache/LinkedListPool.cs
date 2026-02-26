using System.Collections.Generic;

namespace YIUIFramework
{
    public static class LinkedListPool<T>
    {
        private static readonly ObjectPool<LinkedList<T>> s_ListPool =
                new ObjectPool<LinkedList<T>>(null, l => l.Clear());

        public static LinkedList<T> Get()
        {
            return s_ListPool.Get();
        }

        public static void Release(LinkedList<T> toRelease)
        {
            s_ListPool.Release(toRelease);
        }
    }
}
