using System;
using YIUIFramework;

namespace ET.Client
{
    /// <summary>
    /// 弹窗助手
    /// 可打开任意View
    /// T 必须是View
    /// View关闭发送回收消息 EventPutTipsView
    /// </summary>
    public static class TipsHelper
    {
        //在Tips界面打开任意一个View窗口
        //默认打开在根节点下 否则请使用 OpenToParent 方法 指定父级节点
        public static async ETTask Open<T>(params object[] paramMore) where T : Entity
        {
            var vo = ParamVo.Get(paramMore);
            await OpenToParent<T>(vo);
            ParamVo.Put(vo);
        }

        //扩展同步方法
        public static void OpenSync<T>(params object[] paramMore) where T : Entity
        {
            Open<T>(paramMore).Coroutine();
        }

        //在Tips界面打开任意一个View窗口
        public static async ETTask OpenToParent<T>(Entity parent, params object[] paramMore) where T : Entity
        {
            var vo = ParamVo.Get(paramMore);
            await OpenToParent<T>(vo, parent);
            ParamVo.Put(vo);
        }

        //扩展同步方法
        public static void OpenToParentSync<T>(Entity parent, params object[] paramMore) where T : Entity
        {
            OpenToParent<T>(parent, paramMore).Coroutine();
        }

        //使用paramvo参数打开
        public static async ETTask OpenToParent<T>(ParamVo vo, Entity parent = null) where T : Entity
        {
            using var coroutineLock = await CoroutineLockComponent.Instance.Wait(CoroutineLockType.YIUILoader, typeof (T).GetHashCode());

            await YIUIMgrComponent.Inst.Root.OpenPanelAsync<TipsPanelComponent, Type, Entity, ParamVo>(typeof (T), parent, vo);
        }

        //使用paramvo参数 同步打开 内部还是异步 为了解决vo被回收问题
        public static void OpenToParentSync<T>(ParamVo vo, Entity parent = null) where T : Entity
        {
            OpenToParent2NewVo<T>(vo, parent).Coroutine();
        }

        //在外部vo会被回收 所以不能使用同对象 所以这里会创建一个新的防止空对象
        private static async ETTask OpenToParent2NewVo<T>(ParamVo vo, Entity parent = null) where T : Entity
        {
            using var coroutineLock = await CoroutineLockComponent.Instance.Wait(CoroutineLockType.YIUILoader, typeof (T).GetHashCode());

            var newVo = ParamVo.Get(vo.Data);
            await YIUIMgrComponent.Inst.Root.OpenPanelAsync<TipsPanelComponent, Type, Entity, ParamVo>(typeof (T), parent, newVo);
            ParamVo.Put(newVo);
        }

        //关闭某个Tips通用方法
        public static async ETTask CloseTipsView(Entity view, bool tween = true)
        {
            await YIUIEventSystem.Event(new EventPutTipsView() { View = view, Tween = tween });
        }

        //同步方法 只是为了少写个Coroutine好看
        public static void CloseTipsViewSync(Entity view, bool tween = true)
        {
            CloseTipsView(view, tween).Coroutine();
        }
    }
}