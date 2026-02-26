//------------------------------------------------------------
// Author: 亦亦
// Mail: 379338943@qq.com
// Data: 2023年2月12日
//------------------------------------------------------------

using System;
using UnityEngine;
using YIUIFramework;

namespace ET.Client
{
    /// <summary>
    /// 事件扩展 可快速调用子类的事件
    /// 如自己是一个panel 内部有10个view的公共组件
    /// 当自己被打开时需要调用这10个view的open方法
    /// 不要傻傻的手写方法一个一个调用了
    /// </summary>
    public static partial class YIUIChildSystem
    {
        /*
         * 有人会问 我的不是一个view/panel 没有open事件怎么办
         * open 并非依赖某种UI类型 需要这个功能加上 IYIUIOpen 接口即可
         * 这里有open/close 也可以根据自己的需求扩展任意接口
         *
         * 为什么这个方法扩展在YIUIChild 没有在Panel / View / Window 中
         * 因为这是一个基类 否则每个都需要这个功能就需要都扩展
         * 不管你现在是什么类型 都可以调用到子类的方法
         */

        //调用子类的open事件
        //可等待全部子类执行完毕
        //其中有一个子类失败则返回失败
        public static async ETTask<bool> OpenAllChild(this YIUIChild self)
        {
            EntityRef<YIUIChild> selfRef = self;

            using var _ = await self.Root().GetComponent<CoroutineLockComponent>().Wait(CoroutineLockType.YIUIPanel, self.GetHashCode());

            bool result = true;

            using ListComponent<ETTask> listTask = ListComponent<ETTask>.Create();

            self = selfRef;

            foreach (var kv in self.OwnerUIEntity.Children)
            {
                var value = kv.Value;
                if (value is YIUIChild child)
                {
                    listTask.Add(Open(child.OwnerUIEntity));
                }
            }

            await ETTaskHelper.WaitAll(listTask);

            return result;

            async ETTask Open(Entity component)
            {
                var task = ETTask.Create(true);
                var childResult = await YIUIEventSystem.Open(component);
                if (!childResult)
                {
                    result = false;
                }

                task.SetResult();
            }
        }

        //如果不关心子类open结果可不等待 效率会稍稍高一点
        public static void OpenAllChildSync(this YIUIChild self)
        {
            foreach (var kv in self.OwnerUIEntity.Children)
            {
                var value = kv.Value;
                if (value is YIUIChild child)
                {
                    YIUIEventSystem.Open(child.OwnerUIEntity).NoContext();
                }
            }
        }

        public static async ETTask<bool> CloseAllChild(this YIUIChild self)
        {
            EntityRef<YIUIChild> selfRef = self;

            using var _ = await self.Root().GetComponent<CoroutineLockComponent>().Wait(CoroutineLockType.YIUIPanel, self.GetHashCode());

            bool result = true;

            using ListComponent<ETTask> listTask = ListComponent<ETTask>.Create();

            self = selfRef;

            foreach (var kv in self.OwnerUIEntity.Children)
            {
                var value = kv.Value;
                if (value is YIUIChild child)
                {
                    listTask.Add(Close(child.OwnerUIEntity));
                }
            }

            await ETTaskHelper.WaitAll(listTask);

            return result;

            async ETTask Close(Entity component)
            {
                var task = ETTask.Create(true);
                var childResult = await YIUIEventSystem.Close(component);
                if (!childResult)
                {
                    result = false;
                }

                task.SetResult();
            }
        }

        public static void CloseAllChildSync(this YIUIChild self)
        {
            foreach (var kv in self.OwnerUIEntity.Children)
            {
                var value = kv.Value;
                if (value is YIUIChild child)
                {
                    YIUIEventSystem.Close(child.OwnerUIEntity).NoContext();
                }
            }
        }
    }
}