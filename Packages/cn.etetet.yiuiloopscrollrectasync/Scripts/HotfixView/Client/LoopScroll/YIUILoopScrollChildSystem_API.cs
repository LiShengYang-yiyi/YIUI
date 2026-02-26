using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ET.Client
{
    /// <summary>
    /// 无限循环列表 (异步)
    /// 文档: https://lib9kmxvq7k.feishu.cn/wiki/HPbwwkhsKi9aDik5VEXcqPhDnIh
    /// </summary>
    [FriendOf(typeof(YIUILoopScrollChild))]
    public static partial class YIUILoopScrollChildSystem
    {
        /// <summary>
        /// 当Data数据是泛型时 需要设置默认数据类型
        /// </summary>
        public static void SetDefaultDataType(this YIUILoopScrollChild self, Type type)
        {
            self.m_DefaultDataType = type;
        }

        //设置数据 然后刷新
        //不管是要修改数据长度 还是数据变更了 都用此方法刷新
        public static async ETTask SetDataRefresh(this YIUILoopScrollChild self, IList data)
        {
            self.Data = data;
            self.m_Owner.totalCount = data.Count;
            await self.RefillCells();
        }

        //刷新时默认选中某个索引数据
        //注意这里相当于+=操作 如果你会频繁调用这个方法
        //又想每次刷新选中不同的索引
        //那么你应该先自行调用一次 ClearSelect
        public static async ETTask SetDataRefresh(this YIUILoopScrollChild self, IList data, int index)
        {
            self.SetDefaultSelect(index);
            await self.SetDataRefresh(data);
        }

        //同上 请看注释 注意使用方式
        public static async ETTask SetDataRefresh(this YIUILoopScrollChild self, IList data, List<int> index)
        {
            self.SetDefaultSelect(index);
            await self.SetDataRefresh(data);
        }

        //刷新时默认选中某个索引数据 并滚动到这个位置(一瞬间 非动画滚动)
        public static async ETTask SetDataRefresh(this YIUILoopScrollChild self, IList data, int index, int scrollTo)
        {
            self.SetDefaultSelect(index);
            self.Data = data;
            self.m_Owner.totalCount = data.Count;
            await self.RefillCells(scrollTo);
        }

        //所有数据全部刷新 全部显示 不基于无限循环了
        //适用于数据量很少的情况 需要动态显示的
        public static async ETTask SetDataRefreshShowAll(this YIUILoopScrollChild self, IList data)
        {
            EntityRef<YIUILoopScrollChild> selfRef = self;
            self.Data = data;
            self.m_Owner.totalCount = data.Count;
            await self.RefillCells(0, 99999);
            self = selfRef;
            await self.ScrollToCellWithinTime(0, 0);
        }

        public static async ETTask SetDataRefreshShowAll(this YIUILoopScrollChild self, IList data, int index)
        {
            self.SetDefaultSelect(index);
            await self.SetDataRefreshShowAll(data);
        }

        public static async ETTask SetDataRefreshShowAll(this YIUILoopScrollChild self, IList data, List<int> index)
        {
            self.SetDefaultSelect(index);
            await self.SetDataRefreshShowAll(data);
        }

        //如果 < 0 则表示这个对象在对象池里
        public static int GetItemIndex(this YIUILoopScrollChild self, Entity item)
        {
            return self.GetItemIndex(item.GetParent<YIUIChild>().OwnerRectTransform);
        }

        //只能获取当前可见的对象
        public static Entity GetItemByIndex(this YIUILoopScrollChild self, int index, bool log = true)
        {
            if (index < self.ItemStart || index >= self.ItemEnd) return null;
            var childIndex = index - self.ItemStart;
            if (childIndex < 0 || childIndex >= self.Content.childCount)
            {
                if (log)
                {
                    Debug.LogError($"索引错误 请检查 index:{index} Start:{self.ItemStart} childIndex:{childIndex} childCount:{self.Content.childCount}");
                }

                return null;
            }

            var transform = self.Content.GetChild(childIndex);
            var item = self.GetItemRendererByDic(transform);
            return item;
        }

        //判断某个对象是否被选中
        public static bool IsSelect(this YIUILoopScrollChild self, Entity item)
        {
            return self.m_OnClickItemHashSet.Contains(self.GetItemIndex(item));
        }

        //就获取目前显示的这几个数据
        public static List<T> GetShowData<T>(this YIUILoopScrollChild self)
        {
            var listData = new List<T>();
            if (self.Data == null)
            {
                Log.Error($"数据为空 请先设置数据");
                return listData;
            }

            if (typeof(T) != self.m_DataType)
            {
                Log.Error($"数据类型不匹配 请检查 T:{typeof(T)} 实际数据类型:{self.m_DataType}");
                return listData;
            }

            var data = (IList<T>)self.Data;
            for (var i = self.ItemStart; i < self.ItemEnd; i++)
            {
                listData.Add(data[i]);
            }

            return listData;
        }

        //就获取目前显示的这几个
        public static List<T> GetShowItem<T>(this YIUILoopScrollChild self) where T : Entity
        {
            var listItem = new List<T>();
            if (self.Data == null)
            {
                Log.Error($"数据为空 请先设置数据");
                return listItem;
            }

            for (var i = self.ItemStart; i < self.ItemEnd; i++)
            {
                var item = self.GetItemByIndex(i);
                if (item is T target)
                {
                    listItem.Add(target);
                }
            }

            return listItem;
        }

        //原地刷新 重新触发一次可见的Item 时候数据变化 但是长度不变
        //又不想全刷新也不想改变当前滑动位置,选中状态等等, 纯只刷新状态用
        public static void ReRenderer(this YIUILoopScrollChild self)
        {
            if (self.Data == null)
            {
                Log.Error($"数据为空 请先设置数据");
                return;
            }

            for (var i = self.ItemStart; i < self.ItemEnd; i++)
            {
                self.UpdateRenderer(i);
            }
        }

        #region 点击相关 获取被选中目标..

        //reset=吧之前选择的都取消掉 讲道理应该都是true
        //false出问题自己查
        public static void ClearSelect(this YIUILoopScrollChild self, bool reset = true)
        {
            if (reset)
            {
                var selectCount = self.m_OnClickItemHashSet.Count;
                for (var i = 0; i < selectCount; i++)
                {
                    self.OnClickItemQueuePeek();
                }
            }

            self.m_OnClickItemQueue.Clear();
            self.m_OnClickItemHashSet.Clear();
        }

        //获取当前所有被选择的索引
        public static List<int> GetSelectIndex(this YIUILoopScrollChild self)
        {
            return self.m_OnClickItemQueue.ToList();
        }

        //只能得到当前可见的 不可见的拿不到
        public static List<Entity> GetSelectItem(this YIUILoopScrollChild self)
        {
            var selectList = new List<Entity>();
            foreach (var index in self.GetSelectIndex())
            {
                var item = self.GetItemByIndex(index);
                if (item != null)
                {
                    selectList.Add(item);
                }
            }

            return selectList;
        }

        //获取所有被选择的数据
        public static List<T> GetSelectData<T>(this YIUILoopScrollChild self)
        {
            var selectList = new List<T>();

            if (self.Data == null)
            {
                Log.Error($"数据为空 请先设置数据");
                return selectList;
            }

            if (typeof(T) != self.m_DataType)
            {
                Log.Error($"数据类型不匹配 请检查 T:{typeof(T)} 实际数据类型:{self.m_DataType}");
                return selectList;
            }

            var data = (IList<T>)self.Data;
            foreach (var index in self.GetSelectIndex())
            {
                selectList.Add(data[index]);
            }

            return selectList;
        }

        //移除某个选中的目标 然后刷新
        public static async ETTask RemoveSelectIndexRefresh(this YIUILoopScrollChild self, int index)
        {
            self.RemoveSelectIndex(index);
            await self.RefreshCells();
        }

        #endregion

        /// <summary>
        /// 设置创建间隔
        /// </summary>
        public static void ChangeCreateInterval(this YIUILoopScrollChild self, float interval)
        {
            self.m_Owner.u_CreateInterval = interval;
        }

        /// <summary>
        /// 垂直滚动
        /// </summary>
        public static void Vertical(this YIUILoopScrollChild self, bool value)
        {
            self.m_Owner.vertical = value;
        }

        /// <summary>
        /// 水平滚动
        /// </summary>
        public static void Horizontal(this YIUILoopScrollChild self, bool value)
        {
            self.m_Owner.horizontal = value;
        }
    }
}