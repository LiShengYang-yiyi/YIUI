using System;
using UnityEngine;
using YIUIFramework;

namespace ET.Client
{
    public static partial class YIUIViewComponentSystem
    {
        /// <summary>
        /// 使用基础Open 打开类
        /// </summary>
        public static async ETTask<bool> UseBaseOpen(this YIUIViewComponent self)
        {
            if (!self.UIWindow.WindowCanUseBaseOpen)
            {
                Debug.LogError($"当前传入的参数不支持 并未实现这个打开方式 且不允许使用基础Open打开 请检查");
                return false;
            }

            var success = false;

            try
            {
                if (self.OwnerUIEntity is IYIUIOpen _)
                {
                    success = await YIUIEventSystem.Open(self.OwnerUIEntity);
                }
                else
                {
                    success = true;
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"err={e.Message}{e.StackTrace}");
            }

            return success;
        }

        public static async ETTask<bool> Open(this YIUIViewComponent self)
        {
            self.UIBase.SetActive(true);

            var success = false;

            if (!self.UIWindow.WindowHaveIOpenAllowOpen && self.OwnerUIEntity is IYIUIOpenParam)
            {
                Debug.LogError($"当前Panel 有其他IOpen 接口 需要参数传入 不允许直接调用Open");
                return false;
            }

            EntityRef<YIUIViewComponent> selfRef = self;

            try
            {
                if (self.OwnerUIEntity is IYIUIOpen _)
                {
                    success = await YIUIEventSystem.Open(self.OwnerUIEntity);
                }
                else
                {
                    success = true;
                }
            }
            catch (Exception e)
            {
                self = selfRef;
                Debug.LogError($"ResName{self.UIBase.UIResName}, err={e.Message}{e.StackTrace}");
            }

            if (success)
            {
                self = selfRef;
                await self.UIWindow.InternalOnWindowOpenTween();
            }

            return success;
        }

        public static async ETTask<bool> Open(this YIUIViewComponent self, ParamVo param)
        {
            if (self.UIWindow.WindowBanParamOpen)
            {
                Debug.LogError($"当前禁止使用ParamOpen 请检查");
                return false;
            }

            self.UIBase.SetActive(true);

            var success = false;

            EntityRef<YIUIViewComponent> selfRef = self;
            if (self.OwnerUIEntity is IYIUIOpen<ParamVo> _)
            {
                try
                {
                    success = await YIUIEventSystem.Open(self.OwnerUIEntity, param);
                }
                catch (Exception e)
                {
                    self = selfRef;
                    Debug.LogError($"ResName{self.UIBase.UIResName}, err={e.Message}{e.StackTrace}");
                }
            }
            else
            {
                success = await self.UseBaseOpen();
            }

            if (success)
            {
                self = selfRef;
                await self.UIWindow.InternalOnWindowOpenTween();
            }

            return success;
        }

        public static async ETTask<bool> Open<P1>(this YIUIViewComponent self, P1 p1)
        {
            self.UIBase.SetActive(true);

            var success = false;

            EntityRef<YIUIViewComponent> selfRef = self;
            if (self.OwnerUIEntity is IYIUIOpen<P1> _)
            {
                try
                {
                    success = await YIUIEventSystem.Open(self.OwnerUIEntity, p1);
                }
                catch (Exception e)
                {
                    self = selfRef;
                    Debug.LogError($"ResName{self.UIBase.UIResName}, err={e.Message}{e.StackTrace}");
                }
            }
            else
            {
                success = await self.UseBaseOpen();
            }

            if (success)
            {
                self = selfRef;
                await self.UIWindow.InternalOnWindowOpenTween();
            }

            return success;
        }

        public static async ETTask<bool> Open<P1, P2>(this YIUIViewComponent self, P1 p1, P2 p2)
        {
            self.UIBase.SetActive(true);

            var success = false;

            EntityRef<YIUIViewComponent> selfRef = self;
            if (self.OwnerUIEntity is IYIUIOpen<P1, P2> _)
            {
                try
                {
                    success = await YIUIEventSystem.Open(self.OwnerUIEntity, p1, p2);
                }
                catch (Exception e)
                {
                    self = selfRef;
                    Debug.LogError($"ResName{self.UIBase.UIResName}, err={e.Message}{e.StackTrace}");
                }
            }
            else
            {
                success = await self.UseBaseOpen();
            }

            if (success)
            {
                self = selfRef;
                await self.UIWindow.InternalOnWindowOpenTween();
            }

            return success;
        }

        public static async ETTask<bool> Open<P1, P2, P3>(this YIUIViewComponent self, P1 p1, P2 p2, P3 p3)
        {
            self.UIBase.SetActive(true);

            var success = false;

            EntityRef<YIUIViewComponent> selfRef = self;
            if (self.OwnerUIEntity is IYIUIOpen<P1, P2, P3> _)
            {
                try
                {
                    success = await YIUIEventSystem.Open(self.OwnerUIEntity, p1, p2, p3);
                }
                catch (Exception e)
                {
                    self = selfRef;
                    Debug.LogError($"ResName{self.UIBase.UIResName}, err={e.Message}{e.StackTrace}");
                }
            }
            else
            {
                success = await self.UseBaseOpen();
            }

            if (success)
            {
                self = selfRef;
                await self.UIWindow.InternalOnWindowOpenTween();
            }

            return success;
        }

        public static async ETTask<bool> Open<P1, P2, P3, P4>(this YIUIViewComponent self, P1 p1, P2 p2, P3 p3, P4 p4)
        {
            self.UIBase.SetActive(true);

            var success = false;

            EntityRef<YIUIViewComponent> selfRef = self;
            if (self.OwnerUIEntity is IYIUIOpen<P1, P2, P3, P4> _)
            {
                try
                {
                    success = await YIUIEventSystem.Open(self.OwnerUIEntity, p1, p2, p3, p4);
                }
                catch (Exception e)
                {
                    self = selfRef;
                    Debug.LogError($"ResName{self.UIBase.UIResName}, err={e.Message}{e.StackTrace}");
                }
            }
            else
            {
                success = await self.UseBaseOpen();
            }

            if (success)
            {
                self = selfRef;
                await self.UIWindow.InternalOnWindowOpenTween();
            }

            return success;
        }

        public static async ETTask<bool> Open<P1, P2, P3, P4, P5>(this YIUIViewComponent self,
                                                                  P1 p1,
                                                                  P2 p2,
                                                                  P3 p3,
                                                                  P4 p4,
                                                                  P5 p5)
        {
            self.UIBase.SetActive(true);

            var success = false;

            EntityRef<YIUIViewComponent> selfRef = self;
            if (self.OwnerUIEntity is IYIUIOpen<P1, P2, P3, P4, P5> _)
            {
                try
                {
                    success = await YIUIEventSystem.Open(self.OwnerUIEntity, p1, p2, p3, p4, p5);
                }
                catch (Exception e)
                {
                    self = selfRef;
                    Debug.LogError($"ResName{self.UIBase.UIResName}, err={e.Message}{e.StackTrace}");
                }
            }
            else
            {
                success = await self.UseBaseOpen();
            }

            if (success)
            {
                self = selfRef;
                await self.UIWindow.InternalOnWindowOpenTween();
            }

            return success;
        }
    }
}