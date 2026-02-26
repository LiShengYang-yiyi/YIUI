using System;
using System.Collections.Generic;

namespace ET.Client
{
    /// <summary>
    /// UI通用等待
    /// </summary>
    [FriendOf(typeof(YIUIWaitComponent))]
    [EntitySystemOf(typeof(YIUIWaitComponent))]
    public static partial class YIUIWaitComponentSystem
    {
        [EntitySystem]
        private static void Awake(this YIUIWaitComponent self, long waitId)
        {
            self.Reset(waitId);
        }

        [EntitySystem]
        private static void Destroy(this YIUIWaitComponent self)
        {
            self.Notify(EHashWaitError.Destroy);
        }

        [EntitySystem]
        private static async ETTask YIUIWindowClose(this YIUIWaitComponent self, bool viewCloseResult)
        {
            if (viewCloseResult)
            {
                self.Notify(EHashWaitError.Success);
            }

            await ETTask.CompletedTask;
        }

        public static void Notify(this YIUIWaitComponent self, EHashWaitError error)
        {
            if (self.m_IsWaitCompleted) return;
            self.m_IsWaitCompleted = true;
            self.Root().YIUISceneRoot()?.GetComponent<HashWait>()?.Notify(self.m_WaitId, error);
            self.m_WaitId = 0;
        }

        public static void Reset(this YIUIWaitComponent self, long waitId = 0)
        {
            if (self.m_WaitId == waitId) return;

            if (self.m_WaitId != 0)
            {
                self.Notify(EHashWaitError.Reset);
            }

            if (waitId == 0)
            {
                Log.Error($"错误 waitId不能等于0");
                self.m_IsWaitCompleted = true;
                return;
            }

            self.m_WaitId          = waitId;
            self.m_IsWaitCompleted = false;
        }
    }
}
