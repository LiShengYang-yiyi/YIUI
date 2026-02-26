using System;
using UnityEngine;
using YIUIFramework;
using System.Collections.Generic;
using UnityEngine.UI;

namespace ET.Client
{
    [FriendOf(typeof(LoopScrollVerticalViewComponent))]
    [FriendOf(typeof(LoopScrollRectDemoItemComponent))]
    public static partial class LoopScrollVerticalViewComponentSystem
    {
        [EntitySystem]
        private static void YIUIInitialize(this LoopScrollVerticalViewComponent self)
        {
            self.m_Loop = self.AddChild<YIUILoopScrollChild, LoopScrollRect, Type, string>(self.u_ComLoopScrollVertical,
                typeof(LoopScrollRectDemoItemComponent), "u_EventSelect");
        }

        [EntitySystem]
        private static void Destroy(this LoopScrollVerticalViewComponent self)
        {
        }

        [EntitySystem]
        private static async ETTask<bool> YIUIOpen(this LoopScrollVerticalViewComponent self)
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
        private static void YIUILoopRenderer(this LoopScrollVerticalViewComponent self, LoopScrollRectDemoItemComponent item, int data, int index,
        bool select)
        {
            item.u_DataIndex.SetValue(index);
            item.u_DataSelect.SetValue(select);
        }

        [EntitySystem]
        private static void YIUILoopOnClick(this LoopScrollVerticalViewComponent self, LoopScrollRectDemoItemComponent item, int data, int index,
        bool select)
        {
            item.u_DataSelect.SetValue(select);
        }

        #region YIUIEvent开始

        #endregion YIUIEvent结束
    }
}