using System;
using Sirenix.OdinInspector;

namespace YIUIFramework
{
    public partial class YIUIConstAsset
    {
        [BoxGroup("图集", CenterLabel = true)]
        [LabelText("生成图集资源名称")]
        [ReadOnly]
        public const string AtlasDataName = "YIUIAtlasData";

        [BoxGroup("图集", CenterLabel = true)]
        [LabelText("生成图集资源路径")]
        [ReadOnly]
        public const string AtlasDataPath = "Assets/GameRes/YIUI/YIUISettings/YIUIAtlasData.asset";
    }
}