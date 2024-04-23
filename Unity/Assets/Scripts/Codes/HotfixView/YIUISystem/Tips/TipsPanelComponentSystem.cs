using System;
using YIUIFramework;
using UnityEngine;

namespace ET.Client
{
    /// <summary>
    /// 公共弹窗界面
    /// </summary>
    [FriendOf(typeof (TipsPanelComponent))]
    public static partial class TipsPanelComponentSystem
    {
        [ObjectSystem]
        public class TipsPanelComponentInitializeSystem: YIUIInitializeSystem<TipsPanelComponent>
        {
            protected override void YIUIInitialize(TipsPanelComponent self)
            {
            }
        }

        [ObjectSystem]
        public class TipsPanelComponentDestroySystem: DestroySystem<TipsPanelComponent>
        {
            protected override void Destroy(TipsPanelComponent self)
            {
            }
        }

        [ObjectSystem]
        public class TipsPanelComponentOpenSystem: YIUIOpenSystem<TipsPanelComponent, Type, Entity, ParamVo>
        {
            protected override async ETTask<bool> YIUIOpen(TipsPanelComponent self, Type viewType, Entity parent, ParamVo vo)
            {
                return await self.OpenTips(viewType, parent, vo);
            }
        }

        [ObjectSystem]
        public class TipsPanelComponentEventSystem: YIUIEventSystem<TipsPanelComponent, EventPutTipsView>
        {
            //消息 回收对象
            protected override async ETTask YIUIEvent(TipsPanelComponent self, EventPutTipsView message)
            {
                await self.PutTips(message.View, message.Tween);
                self.CheckRefCount();
            }
        }

        //对象池的实例化过程
        private static ETTask<Entity> OnCreateViewRenderer(this TipsPanelComponent self, Type uiType, Entity parent)
        {
            return YIUIFactory.InstantiateAsync(uiType, parent ?? YIUIMgrComponent.Inst.Root, self.UIBase.OwnerRectTransform);
        }

        //打开Tips对应的View
        private static async ETTask<bool> OpenTips(this TipsPanelComponent self, Type uiType, Entity parent, ParamVo vo)
        {
            if (!self._AllPool.ContainsKey(uiType))
            {
                async ETTask<Entity> Create()
                {
                    return await self.OnCreateViewRenderer(uiType, parent);
                }

                self._AllPool.Add(uiType, new ObjAsyncCache<Entity>(Create));
            }

            var pool = self._AllPool[uiType];
            self._RefCount += 1; //加载前引用计数+1 防止加载过程中有人关闭 出现问题
            var view = await pool.Get();
            if (view == null)
            {
                self._RefCount -= 1;
                return self._RefCount > 0;
            }

            if (view is not IYIUIOpen<ParamVo>)
            {
                Debug.LogError($"{uiType.Name} 必须实现 IYIUIOpen<ParamVo> 才可用Tips");
                self._RefCount -= 1;
                return self._RefCount > 0;
            }

            var uiComponent = view.GetParent<YIUIComponent>();
            if (uiComponent == null)
            {
                Debug.LogError($"{uiType.Name} 实例化的对象非 YIUIComponent");
                self._RefCount -= 1;
                return self._RefCount > 0;
            }

            var viewComponent = uiComponent.GetComponent<YIUIViewComponent>();
            if (viewComponent == null)
            {
                Debug.LogError($"{uiType.Name} 实例化的对象非 YIUIViewComponent");
                self._RefCount -= 1;
                return self._RefCount > 0;
            }

            uiComponent.OwnerRectTransform.SetAsLastSibling();

            uiComponent.SetParent(parent);

            var result = await viewComponent.Open(vo);
            if (!result)
                await self.PutTips(view, false);

            return self._RefCount > 0;
        }

        //回收
        private static async ETTask PutTips(this TipsPanelComponent self, Entity view, bool tween = true)
        {
            if (view == null)
            {
                Debug.LogError($"null对象 请检查");
                return;
            }

            var uiType = view.GetType();
            if (!self._AllPool.ContainsKey(uiType))
            {
                Debug.LogError($"没有这个对象池 请检查 {uiType}");
                return;
            }

            var uiComponent = view.GetParent<YIUIComponent>();
            if (uiComponent == null)
            {
                Debug.LogError($"{uiType.Name} 实例化的对象非 YIUIComponent");
                return;
            }

            var viewComponent = uiComponent.GetComponent<YIUIViewComponent>();
            if (viewComponent == null)
            {
                Debug.LogError($"{uiType.Name} 实例化的对象非 YIUIViewComponent");
                return;
            }

            await viewComponent.CloseAsync(tween);

            var pool = self._AllPool[uiType];
            pool.Put(view);
            self._RefCount -= 1;
        }

        //检查引用计数 如果<=0 就自动关闭UI
        private static void CheckRefCount(this TipsPanelComponent self)
        {
            if (self._RefCount > 0) return;

            self._RefCount = 0;
            self.UIPanel.Close();
        }

        #region YIUIEvent开始

        #endregion YIUIEvent结束
    }
}