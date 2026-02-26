using System;

namespace ET.Client
{
    public static partial class YIUIPanelComponentSystem
    {
        internal static void CacheTimeCountDownDestroyPanel(this YIUIPanelComponent self)
        {
            if (self.CachePanelTime <= 0)
            {
                self.RemoveUIReset();
                return;
            }

            self.StopCountDownDestroyPanel();
            self.m_Token = new ETCancellationToken();
            self.DoCountDownDestroyPanel().WithContext(self.m_Token);
        }

        internal static void StopCountDownDestroyPanel(this YIUIPanelComponent self)
        {
            if (self.m_Token == null) return;
            self.m_Token.Cancel();
            self.m_Token = null;
        }

        private static async ETTask DoCountDownDestroyPanel(this YIUIPanelComponent self)
        {
            EntityRef<YIUIPanelComponent> selfRef = self;

            try
            {
                var oldCancellationToken = await ETTaskHelper.GetContextAsync<ETCancellationToken>();

                self = selfRef;
                await ETTaskSafely.Await(selfRef.Entity?.Root()?.GetComponent<TimerComponent>()?.WaitAsync((long)(selfRef.Entity?.CachePanelTime * 1000 ?? 0)));

                if (oldCancellationToken != null && oldCancellationToken.IsCancel()) //取消倒计时
                {
                    return;
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
                return;
            }

            self = selfRef;
            self.m_Token = null;
            self.RemoveUIReset();
        }

        private static void RemoveUIReset(this YIUIPanelComponent self)
        {
            EventSystem.Instance?.YIUIInvokeEntitySync(self, new YIUIInvokeEntity_RemoveUIReset
            {
                PanelName = self.UIBindVo.ComponentType.Name
            });
        }
    }
}