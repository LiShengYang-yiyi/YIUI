#if UNITY_EDITOR

using System;
using System.Collections;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace YIUIFramework.Editor
{
    [Serializable]
    internal class UIRedDotKeyEditorData
    {
        [HideLabel]
        [OnValueChanged("OnValueChangedKeyType")]
        [ValueDropdown("GetKeyTypesSelectList", DoubleClickToConfirm = true)]
        [OdinSerialize]
        public int KeyType;

        private IEnumerable GetKeyTypesSelectList()
        {
            var showValues = new ValueDropdownList<int>();
            var keys       = RedDotKeyHelper.GetKeys();

            foreach (var key in keys)
            {
                if (key == 0) continue;
                showValues.Add(RedDotKeyHelper.GetDisplayDesc(key), key);
            }

            return RedDotKeyHelper.SubDisplayValueDropdownList(showValues);
        }

        [HideLabel]
        [ReadOnly]
        [TableColumnWidth(200, resizable: false)]
        [OdinSerialize]
        public int Id;

        private UIRedDotConfigEditorData m_UIRedDotConfigEditorData;

        public UIRedDotKeyEditorData(UIRedDotConfigEditorData editorData)
        {
            m_UIRedDotConfigEditorData = editorData;
        }

        private void OnValueChangedKeyType()
        {
            Id = KeyType;
            m_UIRedDotConfigEditorData?.CheckParentList();
        }
    }
}
#endif