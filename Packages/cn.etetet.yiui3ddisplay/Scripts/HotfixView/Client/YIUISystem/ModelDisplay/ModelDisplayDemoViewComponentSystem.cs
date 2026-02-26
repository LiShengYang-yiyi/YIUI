using System;
using UnityEngine;
using YIUIFramework;

namespace ET.Client
{
    [FriendOf(typeof(ModelDisplayDemoViewComponent))]
    public static partial class ModelDisplayDemoViewComponentSystem
    {
        [EntitySystem]
        private static void YIUIInitialize(this ModelDisplayDemoViewComponent self)
        {
            self.m_Display = self.AddChild<YIUI3DDisplayChild, UI3DDisplay>(self.u_ComDisplay);
        }

        [EntitySystem]
        private static void Destroy(this ModelDisplayDemoViewComponent self)
        {
        }

        [EntitySystem]
        private static async ETTask<bool> YIUIOpen(this ModelDisplayDemoViewComponent self)
        {
            await ETTask.CompletedTask;
            return true;
        }

        [EntitySystem]
        private static async ETTask<bool> YIUIOpen(this ModelDisplayDemoViewComponent self, ParamVo vo)
        {
            await self.Show();
            return true;
        }

        private static async ETTask Show(this ModelDisplayDemoViewComponent self)
        {
            EntityRef<ModelDisplayDemoViewComponent> selfRef = self;
            await self.Display.ShowAsync("DisplayDemoModel", "CustomCamera");
            self = selfRef;
            self.Display.ResetOnClick(true); //如果要点击模型显示详细信息， 必须打开点击事件
        }

        //案例: 点击事件的通用System注册 如果没有实现就会报错
        [EntitySystem]
        private static void YIUI3DDisplayClick(this ModelDisplayDemoViewComponent self, UI3DDisplay display, GameObject target, GameObject root)
        {
            Log.Info($"点击模型 目标: {target.name}  根节点:{root.name}");
        }

        #region YIUIEvent开始

        #endregion YIUIEvent结束
    }
}