using System;
using UnityEngine;
using YIUIFramework;
using System.Collections.Generic;
using UnityEngine.UI;

namespace ET.Client
{
    [FriendOf(typeof(LoopScrollVerticalGroupViewComponent))]
    [FriendOf(typeof(LoopScrollRectDemoItemComponent))]
    public static partial class LoopScrollVerticalGroupViewComponentSystem
    {
        [EntitySystem]
        private static void YIUIInitialize(this LoopScrollVerticalGroupViewComponent self)
        {
            self.m_Loop = self.AddChild<YIUILoopScrollChild, LoopScrollRect, Type, string>(self.u_ComLoopScrollVerticalGroup,
                typeof(LoopScrollRectDemoItemComponent), "u_EventSelect");
        }

        [EntitySystem]
        private static void Destroy(this LoopScrollVerticalGroupViewComponent self)
        {
        }

        [EntitySystem]
        private static async ETTask<bool> YIUIOpen(this LoopScrollVerticalGroupViewComponent self)
        {
            await ETTask.CompletedTask;

            List<int> list = new List<int>();
            for (int i = 0; i < 500; i++)
            {
                list.Add(i);
            }

            self.Loop.ClearSelect();
            self.Loop.SetDataRefresh(list, 0).NoContext();
            return true;
        }

        [EntitySystem]
        private static void YIUILoopRenderer(this LoopScrollVerticalGroupViewComponent self, LoopScrollRectDemoItemComponent item, int data,
        int index,
        bool select)
        {
            item.u_DataIndex.SetValue(index);
            item.u_DataSelect.SetValue(select);
        }

        [EntitySystem]
        private static void YIUILoopOnClick(this LoopScrollVerticalGroupViewComponent self, LoopScrollRectDemoItemComponent item, int data, int index,
        bool select)
        {
            item.u_DataSelect.SetValue(select);
        }

        #region YIUIEvent开始

        #endregion YIUIEvent结束
    }
}