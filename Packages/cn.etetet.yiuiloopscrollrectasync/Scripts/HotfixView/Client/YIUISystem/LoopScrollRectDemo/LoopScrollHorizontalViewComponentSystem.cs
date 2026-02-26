using System;
using UnityEngine;
using YIUIFramework;
using System.Collections.Generic;
using UnityEngine.UI;

namespace ET.Client
{
    [FriendOf(typeof(LoopScrollHorizontalViewComponent))]
    [FriendOf(typeof(LoopScrollRectDemoItemComponent))]
    public static partial class LoopScrollHorizontalViewComponentSystem
    {
        [EntitySystem]
        private static void YIUIInitialize(this LoopScrollHorizontalViewComponent self)
        {
            self.m_Loop = self.AddChild<YIUILoopScrollChild, LoopScrollRect, Type, string>(self.u_ComLoopScrollHorizontal,
                typeof(LoopScrollRectDemoItemComponent), "u_EventSelect");
        }

        [EntitySystem]
        private static void Destroy(this LoopScrollHorizontalViewComponent self)
        {
        }

        [EntitySystem]
        private static async ETTask<bool> YIUIOpen(this LoopScrollHorizontalViewComponent self)
        {
            await ETTask.CompletedTask;

            List<int> list = new List<int>();
            for (int i = 0; i < 100; i++)
            {
                list.Add(i);
            }

            self.Loop.ClearSelect();
            self.Loop.SetDataRefresh(list, 0).NoContext();
            return true;
        }

        [EntitySystem]
        private static void YIUILoopRenderer(this LoopScrollHorizontalViewComponent self, LoopScrollRectDemoItemComponent item, int data, int index,
        bool select)
        {
            item.u_DataIndex.SetValue(index);
            item.u_DataSelect.SetValue(select);
        }

        [EntitySystem]
        private static void YIUILoopOnClick(this LoopScrollHorizontalViewComponent self, LoopScrollRectDemoItemComponent item, int data, int index,
        bool select)
        {
            item.u_DataSelect.SetValue(select);
        }

        #region YIUIEvent开始

        #endregion YIUIEvent结束
    }
}