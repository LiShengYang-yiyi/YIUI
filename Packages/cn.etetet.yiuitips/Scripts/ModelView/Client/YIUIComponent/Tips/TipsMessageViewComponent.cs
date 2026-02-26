using System;
using YIUIFramework;

namespace ET.Client
{
    public partial class TipsMessageViewComponent : Entity, IYIUIOpen<ParamVo>, IYIUIOpenTween, IYIUICloseTween
    {
        public MessageTipsExtraData ExtraData;
    }

    //额外参数
    [EnableClass]
    public class MessageTipsExtraData
    {
        public string ConfirmName; //确定按钮换名字
        public string CancelName; //取消按钮换名字
        public bool ShowCancelButton; //显示取消按钮
        public bool ShowCloseButton; //显示关闭按钮
    }
}