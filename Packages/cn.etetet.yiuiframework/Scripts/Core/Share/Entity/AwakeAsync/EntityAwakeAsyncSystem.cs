using System;

namespace ET
{
    public static class EntityAwakeAsyncSystem
    {
        public static async ETTask<T> AwakeAsync<T>(this T self) where T : Entity, IAwakeAsync
        {
            await EntityAwakeAsync(self);
            return self;
        }

        public static async ETTask<T> AwakeAsync<T, P1>(this T self, P1 p1) where T : Entity, IAwakeAsync<P1>
        {
            await EntityAwakeAsync(self, p1);
            return self;
        }

        public static async ETTask<T> AwakeAsync<T, P1, P2>(this T self, P1 p1, P2 p2) where T : Entity, IAwakeAsync<P1, P2>
        {
            await EntityAwakeAsync(self, p1, p2);
            return self;
        }

        public static async ETTask<T> AwakeAsync<T, P1, P2, P3>(this T self, P1 p1, P2 p2, P3 p3) where T : Entity, IAwakeAsync<P1, P2, P3>
        {
            await EntityAwakeAsync(self, p1, p2, p3);
            return self;
        }

        public static async ETTask<T> AwakeAsync<T, P1, P2, P3, P4>(this T self, P1 p1, P2 p2, P3 p3, P4 p4) where T : Entity, IAwakeAsync<P1, P2, P3, P4>
        {
            await EntityAwakeAsync(self, p1, p2, p3, p4);
            return self;
        }

        public static async ETTask<T> AwakeAsync<T, P1, P2, P3, P4, P5>(this T self, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5) where T : Entity, IAwakeAsync<P1, P2, P3, P4, P5>
        {
            await EntityAwakeAsync(self, p1, p2, p3, p4, p5);
            return self;
        }

        internal static async ETTask EntityAwakeAsync(Entity self)
        {
            if (self is not IAwakeAsync)
            {
                return;
            }

            var iAwakeSystems = EntitySystemSingleton.Instance.TypeSystems.GetSystems(self.GetType(), typeof(IAwakeAsyncSystem));
            if (iAwakeSystems == null)
            {
                return;
            }

            EntityRef<Entity> selfRef = self;
            foreach (IAwakeAsyncSystem aAwakeAsyncSystem in iAwakeSystems)
            {
                if (aAwakeAsyncSystem == null)
                {
                    continue;
                }

                try
                {
                    await aAwakeAsyncSystem.Run(selfRef);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }

        internal static async ETTask EntityAwakeAsync<P1>(Entity self, P1 p1)
        {
            if (self is not IAwakeAsync<P1>)
            {
                return;
            }

            var iAwakeAsyncSystems = EntitySystemSingleton.Instance.TypeSystems.GetSystems(self.GetType(), typeof(IAwakeAsyncSystem<P1>));
            if (iAwakeAsyncSystems == null)
            {
                return;
            }

            EntityRef<Entity> selfRef = self;
            foreach (IAwakeAsyncSystem<P1> aAwakeAsyncSystem in iAwakeAsyncSystems)
            {
                if (aAwakeAsyncSystem == null)
                {
                    continue;
                }

                try
                {
                    await aAwakeAsyncSystem.Run(selfRef, p1);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }

        internal static async ETTask EntityAwakeAsync<P1, P2>(Entity self, P1 p1, P2 p2)
        {
            if (self is not IAwakeAsync<P1, P2>)
            {
                return;
            }

            var iAwakeAsyncSystems = EntitySystemSingleton.Instance.TypeSystems.GetSystems(self.GetType(), typeof(IAwakeAsyncSystem<P1, P2>));
            if (iAwakeAsyncSystems == null)
            {
                return;
            }

            EntityRef<Entity> selfRef = self;
            foreach (IAwakeAsyncSystem<P1, P2> aAwakeAsyncSystem in iAwakeAsyncSystems)
            {
                if (aAwakeAsyncSystem == null)
                {
                    continue;
                }

                try
                {
                    await aAwakeAsyncSystem.Run(selfRef, p1, p2);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }

        internal static async ETTask EntityAwakeAsync<P1, P2, P3>(Entity self, P1 p1, P2 p2, P3 p3)
        {
            if (self is not IAwakeAsync<P1, P2, P3>)
            {
                return;
            }

            var iAwakeAsyncSystems = EntitySystemSingleton.Instance.TypeSystems.GetSystems(self.GetType(), typeof(IAwakeAsyncSystem<P1, P2, P3>));
            if (iAwakeAsyncSystems == null)
            {
                return;
            }

            EntityRef<Entity> selfRef = self;
            foreach (IAwakeAsyncSystem<P1, P2, P3> aAwakeAsyncSystem in iAwakeAsyncSystems)
            {
                if (aAwakeAsyncSystem == null)
                {
                    continue;
                }

                try
                {
                    await aAwakeAsyncSystem.Run(selfRef, p1, p2, p3);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }

        internal static async ETTask EntityAwakeAsync<P1, P2, P3, P4>(Entity self, P1 p1, P2 p2, P3 p3, P4 p4)
        {
            if (self is not IAwakeAsync<P1, P2, P3, P4>)
            {
                return;
            }

            var iAwakeAsyncSystems = EntitySystemSingleton.Instance.TypeSystems.GetSystems(self.GetType(), typeof(IAwakeAsyncSystem<P1, P2, P3, P4>));
            if (iAwakeAsyncSystems == null)
            {
                return;
            }

            EntityRef<Entity> selfRef = self;
            foreach (IAwakeAsyncSystem<P1, P2, P3, P4> aAwakeAsyncSystem in iAwakeAsyncSystems)
            {
                if (aAwakeAsyncSystem == null)
                {
                    continue;
                }

                try
                {
                    await aAwakeAsyncSystem.Run(selfRef, p1, p2, p3, p4);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }

        internal static async ETTask EntityAwakeAsync<P1, P2, P3, P4, P5>(Entity self, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5)
        {
            if (self is not IAwakeAsync<P1, P2, P3, P4, P5>)
            {
                return;
            }

            var iAwakeAsyncSystems = EntitySystemSingleton.Instance.TypeSystems.GetSystems(self.GetType(), typeof(IAwakeAsyncSystem<P1, P2, P3, P4, P5>));
            if (iAwakeAsyncSystems == null)
            {
                return;
            }

            EntityRef<Entity> selfRef = self;
            foreach (IAwakeAsyncSystem<P1, P2, P3, P4, P5> aAwakeAsyncSystem in iAwakeAsyncSystems)
            {
                if (aAwakeAsyncSystem == null)
                {
                    continue;
                }

                try
                {
                    await aAwakeAsyncSystem.Run(selfRef, p1, p2, p3, p4, p5);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }
    }
}