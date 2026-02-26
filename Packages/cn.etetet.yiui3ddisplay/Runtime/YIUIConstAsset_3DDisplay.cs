using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace YIUIFramework
{
    public partial class YIUIConstAsset
    {
        [BoxGroup("3DDisplay", CenterLabel = true)]
        [LabelText("3D显示层名称")]
        public string YIUI3DLayer = "YIUI3DLayer";
    }
}