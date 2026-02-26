using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace YIUIFramework
{
    public partial class YIUIConstAsset
    {
        [BoxGroup("GM", CenterLabel = true)]
        [LabelText("关闭GM功能")]
        public bool CloseGMCommand = false;

        [BoxGroup("GM", CenterLabel = true)]
        [LabelText("GM打开快捷键")]
        public KeyCode OpenGMViewKey = KeyCode.Escape;

        [BoxGroup("GM", CenterLabel = true)]
        [LabelText("默认打开第一个页签")]
        public bool OpenGMViewFirstType = false;
    }
}