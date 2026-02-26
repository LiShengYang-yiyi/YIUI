using System.Collections.Generic;
using UnityEngine;
using YIUIFramework;

namespace ET.Client
{
    /// <summary>
    /// 部类 界面拆分数据
    /// </summary>
    public partial class YIUIPanelComponent
    {
        public UIPanelSplitData m_PanelSplitData;

        public readonly Dictionary<string, EntityRef<Entity>> m_ExistView = new();

        public readonly Dictionary<string, RectTransform> m_ViewParent = new();

        public readonly List<EntityRef<YIUIWindowComponent>> m_LastCloseView = new();

        /// <summary>
        /// 当前已打开的UI View 不包含弹窗
        /// </summary>
        public EntityRef<Entity> u_CurrentOpenView;

        public Entity CurrentOpenView => u_CurrentOpenView;

        /// <summary>
        /// 外界可判断最后一次打开的view名字
        /// </summary>
        public string CurrentOpenViewName => CurrentOpenView?.GetParent<YIUIChild>().UIResName ?? "";

        /// <summary>
        /// 由于view是可以自己关闭自己的 所以当前的UI有可能会自己关闭自己 并不是用的通用打开其他被关闭
        /// 所以这里可以判断到他是否被关闭了
        /// </summary>
        public bool CurrentOpenViewActiveSelf => CurrentOpenView?.GetParent<YIUIChild>().ActiveSelf ?? false;
    }
}