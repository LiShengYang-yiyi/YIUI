using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace YIUIFramework
{
    [Serializable]
    public class YIUIAtlasInfo
    {
        [LabelText("图集")]
        [ReadOnly]
        public string AtlasName;

        [LabelText("图片")]
        [ReadOnly]
        public string[] SpriteNames;
    }

    public class YIUIAtlasData : ScriptableObject
    {
        [ReadOnly]
        public YIUIAtlasInfo[] Infos;
    }
}