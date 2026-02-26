using UnityEngine;
using YIUIFramework;

namespace ET.Client
{
    /// <summary>
    /// 文档: https://lib9kmxvq7k.feishu.cn/wiki/NYADwMydliVmQ7kWXOuc0yxGn7p
    /// </summary>
    public partial class GMPanelComponent : Entity, IUpdate
    {
        public KeyCode    _OpenGMViewKey     = KeyCode.None;
        public FloatPrefs _GMBtn_Pos_X = new("GMPanelComponent_GMBtn_Pos_X");
        public FloatPrefs _GMBtn_Pos_Y = new("GMPanelComponent_GMBtn_Pos_Y");
        public Vector2    _Offset;
        public Vector2    _LimitSize;
    }
}