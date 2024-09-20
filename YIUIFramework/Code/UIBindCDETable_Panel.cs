using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using YIUIBind;
using UnityEngine;

namespace YIUIFramework
{
    //Panel的分块数据
    public sealed partial class UIBindCDETable
    {
        [OdinSerialize, ReadOnly, LabelText("源数据")]
        internal bool IsSplitData;

        //源数据 拆分前的源数据
#if UNITY_EDITOR
        private bool ShowPanelSplitData => IsSplitData && UICodeType == EUICodeType.Panel;
        
        [ShowIf(nameof(ShowPanelSplitData))]
#endif
        [OdinSerialize, ShowInInspector, BoxGroup("面板拆分数据", centerLabel: true)]
        internal UIPanelSplitData PanelSplitData = new UIPanelSplitData();

#if UNITY_EDITOR

        //拆分后的引用数据 
        [ShowInInspector, ReadOnly, OdinSerialize]
        [BoxGroup("面板拆分数据", centerLabel: true)]
        [HideIf(nameof(HidePanelSplitData))] //就是一个只读的 展示用数据 请不要使用此数据 或修改数据
        internal UIPanelSplitData PanelSplitEditorShowData;

        private bool HidePanelSplitData => IsSplitData || UICodeType != EUICodeType.Panel;
#endif
    }
}