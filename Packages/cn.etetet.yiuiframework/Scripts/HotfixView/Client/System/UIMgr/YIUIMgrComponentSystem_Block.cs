using UnityEngine;
using YIUIFramework;

namespace ET.Client
{
    public static partial class YIUIMgrComponentSystem
    {
        //永久屏蔽
        //适用于 不知道要屏蔽多久 但是能保证可以成对调用的
        //这个没有放到API类中
        //因为如果你不能保证请不要用
        //至少过程中要try起来finally 保证不会出错 否则请不要使用这个功能
        public static long BanLayerOptionForever(this YIUIMgrComponent self)
        {
            self.SetLayerBlockOption(false);
            var foreverBlockCode = IdGenerater.Instance.GenerateId();
            self.m_AllForeverBlockCode.Add(foreverBlockCode);
            return foreverBlockCode;
        }

        //恢复永久屏蔽
        public static void RecoverLayerOptionForever(this YIUIMgrComponent self, long code)
        {
            self.m_AllForeverBlockCode.Remove(code);

            if (self.m_AllForeverBlockCode.Count <= 0)
            {
                self.SetLayerBlockOption(true);
            }
        }

        /// <summary>
        /// 禁止层级操作
        /// 适合于知道想屏蔽多久 且可托管的操作
        /// </summary>
        /// <param name="time">需要禁止的时间</param>
        public static async ETTask BanLayerOptionCountDown(this YIUIMgrComponent self, long time)
        {
            EntityRef<YIUIMgrComponent> selfRef = self;
            var code = self.BanLayerOptionForever();
            await self.Root().GetComponent<TimerComponent>().WaitAsync(time);
            self = selfRef;
            self.RecoverLayerOptionForever(code);
        }

        /// <summary>
        /// 强制恢复层级到可操作状态
        /// 此方法会强制打断倒计时 
        /// 清除所有永久屏蔽
        /// 根据需求调用
        /// </summary>
        public static void RecoverLayerOption(this YIUIMgrComponent self)
        {
            self.RecoverLayerOptionAll();
        }

        //初始化添加屏蔽模块
        private static void InitAddUIBlock(this YIUIMgrComponent self)
        {
            self.m_LayerBlock = new GameObject("LayerBlock");
            var rect = self.m_LayerBlock.AddComponent<RectTransform>();
            self.m_LayerBlock.AddComponent<CanvasRenderer>();
            self.m_LayerBlock.AddComponent<UIBlock>();
            rect.SetParent(self.UILayerRoot);
            rect.SetAsLastSibling();
            rect.ResetToFullScreen();
            self.SetLayerBlockOption(true);
        }

        /// <summary>
        /// 设置UI是否可以操作
        /// 不能提供此API对外操作
        /// 因为有人设置过后就会忘记恢复
        /// 如果你确实需要你可以设置 禁止无限时间
        /// 之后调用恢复操作也可以做到
        /// </summary>
        /// <param name="value">true = 可以操作 = 屏蔽层会被隐藏</param>
        internal static void SetLayerBlockOption(this YIUIMgrComponent self, bool value)
        {
            self.m_LayerBlock.SetActive(!value);
        }

        /// <summary>
        /// 强制恢复层级到可操作状态
        /// 此方法会强制打断倒计时 根据需求调用
        /// </summary>
        internal static void RecoverLayerOptionAll(this YIUIMgrComponent self)
        {
            self.m_AllForeverBlockCode.Clear();
            self.SetLayerBlockOption(true);
        }
    }
}
