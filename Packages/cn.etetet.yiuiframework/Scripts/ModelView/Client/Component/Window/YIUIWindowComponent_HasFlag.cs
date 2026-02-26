using System;
using UnityEngine;
using YIUIFramework;

namespace ET.Client
{
    //快捷判断操作
    public partial class YIUIWindowComponent
    {
        /// <summary>
        /// 当打开的参数不一致时
        /// 是否可以使用基础Open
        /// 如果允许 别人调用open了一个不存在的Open参数时
        /// 也可以使用默认的open打开界面 则你可以改为true
        /// </summary>
        public bool WindowCanUseBaseOpen => WindowOption.HasFlag(EWindowOption.CanUseBaseOpen);

        /// <summary>
        /// 禁止使用ParamOpen
        /// </summary>
        public bool WindowBanParamOpen => WindowOption.HasFlag(EWindowOption.BanParamOpen);

        /// <summary>
        /// 我有其他IOpen时 允许用open
        /// 默认fasle 我有其他iopen接口时 是不允许使用open的
        /// </summary>
        public bool WindowHaveIOpenAllowOpen => WindowOption.HasFlag(EWindowOption.HaveIOpenAllowOpen);

        //先开
        public bool WindowFirstOpen => WindowOption.HasFlag(EWindowOption.FirstOpen);

        //后关
        public bool WindowLastClose => WindowOption.HasFlag(EWindowOption.LastClose);

        //禁止打开动画
        public bool WindowBanOpenTween => WindowOption.HasFlag(EWindowOption.BanOpenTween);

        //禁止关闭动画
        public bool WindowBanCloseTween => WindowOption.HasFlag(EWindowOption.BanCloseTween);

        //打开动画不可重复播放
        public bool WindowBanRepetitionOpenTween => WindowOption.HasFlag(EWindowOption.BanRepetitionOpenTween);

        //关闭动画不可重复播放
        public bool WindowBanRepetitionCloseTween => WindowOption.HasFlag(EWindowOption.BanRepetitionCloseTween);

        //不等待打开动画
        public bool WindowBanAwaitOpenTween => WindowOption.HasFlag(EWindowOption.BanAwaitOpenTween);

        //不等待关闭动画
        public bool WindowBanAwaitCloseTween => WindowOption.HasFlag(EWindowOption.BanAwaitCloseTween);

        //我关闭时跳过其他的打开动画
        public bool WindowSkipOtherOpenTween => WindowOption.HasFlag(EWindowOption.SkipOtherOpenTween);

        //我打开时跳过其他的关闭动画
        public bool WindowSkipOtherCloseTween => WindowOption.HasFlag(EWindowOption.SkipOtherCloseTween);

        //Home时 跳过我自己的打开动画
        public bool WindowSkipHomeOpenTween => WindowOption.HasFlag(EWindowOption.SkipHomeOpenTween);

        //播放动画时 可以操作  默认播放动画的时候是不能操作UI的 不然容易出问题
        public bool WindowAllowOptionByTween => WindowOption.HasFlag(EWindowOption.AllowOptionByTween);

        //Home时 被关闭不视作Back
        public bool WindowSkipHomeBack => WindowOption.HasFlag(EWindowOption.SkipHomeBack);

        //窗口关闭事件动画前触发
        public bool WindowCloseTweenBefore => WindowOption.HasFlag(EWindowOption.WindowCloseTweenBefore);

        //窗口已关闭也触发动画 99%的情况是不需要的
        public bool WindowCloseTriggerTween => WindowOption.HasFlag(EWindowOption.WindowCloseTriggerTween);
    }
}