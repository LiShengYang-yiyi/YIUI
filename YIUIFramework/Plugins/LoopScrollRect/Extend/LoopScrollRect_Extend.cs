using Sirenix.OdinInspector;

namespace UnityEngine.UI
{
    public abstract partial class LoopScrollRect
    {
        [SerializeField]
        [LabelText("缓存父级对象")]
        internal RectTransform u_CacheRect;

        [SerializeField]
        [LabelText("最大可点击数")]
        [MinValue(1)]
        internal int u_MaxClickCount = 1;

        [SerializeField]
        [LabelText("重复点击则取消")]
        internal bool u_RepetitionCancel;

        internal int   u_StartLine              => StartLine;                                             //可见的第一行
        internal int   u_CurrentLines           => CurrentLines;                                          //滚动中的当前行数
        internal int   u_TotalLines             => TotalLines;                                            //总数
        internal int   u_EndLine                => Mathf.Min(u_StartLine + u_CurrentLines, u_TotalLines); //可见的最后一行
        internal int   u_ContentConstraintCount => contentConstraintCount;                                //限制 行/列 数
        internal float u_ContentSpacing         => contentSpacing;                                        //间隔
    }
}