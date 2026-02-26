using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace YIUIFramework
{
    //Panel的分块数据
    public sealed partial class UIBindCDETable
    {
        [OdinSerialize]
        [LabelText("源数据")]
        [ReadOnly]
        [HideInInspector]
        internal bool IsSplitData;

        [ShowInInspector]
        [HideLabel]
        [BoxGroup("面板拆分数据", centerLabel: true)]
        [OdinSerialize]
        #if UNITY_EDITOR
        [ShowIf("ShowIfPanelSplit")]
        #endif
        internal UIPanelSplitData PanelSplitData = new();

        #if UNITY_EDITOR

        private bool ShowIfPanelSplit()
        {
            return PanelSplitData.AllViewParent != null || PanelSplitData.AllPopupViewParent != null;
        }
        #endif
    }
}