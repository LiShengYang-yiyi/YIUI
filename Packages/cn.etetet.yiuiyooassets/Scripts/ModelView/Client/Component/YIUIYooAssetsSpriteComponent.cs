using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

namespace ET.Client
{
    /// <summary>
    /// 有图集时用图集加载,否则正常加载
    /// 如果你要访问图集干其他事情 请自行实现 因为不利于统一管理
    /// 这里只负责加载图片
    /// </summary>
    [ComponentOf(typeof(YIUILoadComponent))]
    public class YIUIYooAssetsSpriteComponent : Entity, IAwake
    {
        public EntityRef<YIUILoadComponent> m_YIUILoadRef;
        public YIUILoadComponent YIUILoad => m_YIUILoadRef;

        public readonly Dictionary<string, string> m_SpritePathMap = new();
        public readonly Dictionary<Sprite, SpriteAtlas> m_LoadedSprites = new();
    }
}