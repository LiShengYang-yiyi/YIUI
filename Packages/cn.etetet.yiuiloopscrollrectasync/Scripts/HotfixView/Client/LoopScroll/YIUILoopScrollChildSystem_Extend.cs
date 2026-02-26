using System;
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
        //在开始时用startItem填充单元格，同时清除现有的单元格
        public static async ETTask RefillCells(this YIUILoopScrollChild self, int startItem = 0, float contentOffset = 0)
        {
            EntityRef<YIUILoopScrollChild> selfRef = self;
            var code = self.BanLayerOptionForever();
            self.SyncPoolCreateInterval(true);
            try
            {
                await self.m_Owner.RefillCells(startItem, contentOffset);
            }
            catch (Exception e)
            {
                Log.Error($"刷新错误 RefillCells : {e}");
            }
            finally
            {
                self = selfRef;
                self.SyncPoolCreateInterval(false);
                self.RecoverLayerOptionForever(code); 
            }
        }

        //在结束时重新填充endItem中的单元格，同时清除现有的单元格
        public static async ETTask RefillCellsFromEnd(this YIUILoopScrollChild self, int endItem = 0, bool alignStart = false)
        {
            EntityRef<YIUILoopScrollChild> selfRef = self;
            var code = self.BanLayerOptionForever();
            self.SyncPoolCreateInterval(true);
            try
            {
                await self.m_Owner.RefillCellsFromEnd(endItem, alignStart);
            }
            catch (Exception e)
            {
                Log.Error($"刷新错误 RefillCellsFromEnd : {e}");
            }
            finally
            {
                self = selfRef;
                self.SyncPoolCreateInterval(false);
                self.RecoverLayerOptionForever(code); 
            }
        }

        public static async ETTask RefreshCells(this YIUILoopScrollChild self)
        {
            EntityRef<YIUILoopScrollChild> selfRef = self;
            var code = self.BanLayerOptionForever();
            self.SyncPoolCreateInterval(true);
            try
            {
                await self.m_Owner.RefreshCells();
            }
            catch (Exception e)
            {
                Log.Error($"刷新错误 RefreshCells : {e}");
            }
            finally
            {
                self = selfRef;
                self.SyncPoolCreateInterval(false);
                self.RecoverLayerOptionForever(code); 
            }
        }

        private static void SyncPoolCreateInterval(this YIUILoopScrollChild self, bool open)
        {
            self.m_ItemPool.ChangeCreateInterval((open || self.m_Owner.u_ForeverInterval) ? self.m_Owner.u_CreateInterval : 0);
        }

        public static void ClearCells(this YIUILoopScrollChild self)
        {
            self.m_Owner.ClearCells();
        }

        public static int GetFirstItem(this YIUILoopScrollChild self, out float offset)
        {
            return self.m_Owner.GetFirstItem(out offset);
        }

        public static int GetLastItem(this YIUILoopScrollChild self, out float offset)
        {
            return self.m_Owner.GetLastItem(out offset);
        }

        private static int GetValidIndex(this YIUILoopScrollChild self, int index)
        {
            return Mathf.Clamp(index, 0, self.TotalCount - 1);
        }

        public static async ETTask ScrollToCell(this YIUILoopScrollChild self, int index, float speed)
        {
            if (self.TotalCount <= 0) return;
            EntityRef<YIUILoopScrollChild> selfRef = self;
            var code = self.BanLayerOptionForever();
            await self.m_Owner.ScrollToCell(self.GetValidIndex(index), speed);
            self = selfRef;
            self.RecoverLayerOptionForever(code);
        }

        public static async ETTask ScrollToCellWithinTime(this YIUILoopScrollChild self, int index, float time)
        {
            if (self.TotalCount <= 0) return;
            EntityRef<YIUILoopScrollChild> selfRef = self;
            var code = self.BanLayerOptionForever();
            await self.m_Owner.ScrollToCellWithinTime(self.GetValidIndex(index), time);
            self = selfRef;
            self.RecoverLayerOptionForever(code);
        }

        public static void StopMovement(this YIUILoopScrollChild self)
        {
            self.m_Owner.StopMovement();
        }

        private static long BanLayerOptionForever(this YIUILoopScrollChild self)
        {
            if (self.m_Owner.u_RefreshCanOption) return 0;
            var code = self.YIUIMgr().BanLayerOptionForever();
            self.m_BanLayerOptionForeverHashSet.Add(code);
            return code;
        }

        private static void RecoverLayerOptionForever(this YIUILoopScrollChild self, long code)
        {
            if (code == 0) return;
            self.m_BanLayerOptionForeverHashSet.Remove(code);
            self.YIUIMgr().RecoverLayerOptionForever(code);
        }
    }
}