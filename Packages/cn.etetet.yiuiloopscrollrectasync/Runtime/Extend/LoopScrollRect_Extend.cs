using ET;
using Sirenix.OdinInspector;

namespace UnityEngine.UI
{
    [EnableClass]
    public abstract partial class LoopScrollRect
    {
        [SerializeField]
        [LabelText("缓存父级对象")]
        public RectTransform u_CacheRect;

        [SerializeField]
        [LabelText("最大可点击数")]
        [MinValue(1)]
        public int u_MaxClickCount = 1;

        [SerializeField]
        [LabelText("自动取消上一个选择")]
        [MinValue(1)]
        public bool u_AutoCancelLast = true;

        [SerializeField]
        [LabelText("重复点击则取消")]
        public bool u_RepetitionCancel;

        [SerializeField]
        [LabelText("创建间隔")]
        public float u_CreateInterval = 0f;

        [SerializeField]
        [LabelText("滚动时使用创建间隔")]
        public bool u_ForeverInterval = false;

        [SerializeField]
        [LabelText("刷新时可操作")]
        public bool u_RefreshCanOption = false;

        [SerializeField]
        [LabelText("预加载")]
        public int u_PreLoadCount = 0;

        public int   u_StartLine              => StartLine;                                             //可见的第一行
        public int   u_CurrentLines           => CurrentLines;                                          //滚动中的当前行数
        public int   u_TotalLines             => TotalLines;                                            //总数
        public int   u_EndLine                => Mathf.Min(u_StartLine + u_CurrentLines, u_TotalLines); //可见的最后一行
        public int   u_ContentConstraintCount => contentConstraintCount;                                //限制 行/列 数
        public float u_ContentSpacing         => contentSpacing;                                        //间隔
        public int   u_ItemStart              => itemTypeStart;                                         //当前显示的第一个的Index 
        public int   u_ItemEnd                => itemTypeEnd;                                           //当前显示的最后一个index 被+1了注意
    }
}