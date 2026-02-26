using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace YIUIFramework
{
    [AddComponentMenu("YIUIFramework/红点/红点通用绑定 【RedDotBind】")]
    public class RedDotBind : MonoBehaviour
    {
        [SerializeField]
        [LabelText("红点枚举")]
        [EnableIf("@UIOperationHelper.CommonShowIf()")]
        #if UNITY_EDITOR
        [ValueDropdown("GetKeyTypesSelectList", DoubleClickToConfirm = true)]
        [OnValueChanged("OnKeyValueChanged")]
        #endif
        private int m_Key;

        #if UNITY_EDITOR
        private IEnumerable GetKeyTypesSelectList()
        {
            var showValues = new ValueDropdownList<int>();
            var keys       = RedDotKeyHelper.GetKeys();

            foreach (var key in keys)
            {
                showValues.Add(RedDotKeyHelper.GetDisplayDesc(key), key);
            }

            return RedDotKeyHelper.SubDisplayValueDropdownList(showValues);
        }

        [ShowInInspector]
        [NonSerialized]
        [Delayed]
        [OnValueChanged("OnInputKeyValue")]
        [HideInPlayMode]
        [LabelText("手动选择")]
        public int InputChangeKey; //当枚举多了 下拉菜单不好用的时候/或不想下拉菜单选择的时候可手动输入

        private void OnInputKeyValue()
        {
            if (!RedDotKeyHelper.ContainsKey(InputChangeKey))
            {
                Debug.LogError($"不存在这个Key 请检查 {InputChangeKey}");
                m_Key          = 0;
                InputChangeKey = 0;
                return;
            }

            m_Key = InputChangeKey;
        }

        private void OnKeyValueChanged()
        {
            InputChangeKey = m_Key;
        }

        #endif

        public int Key => m_Key;

        [ShowInInspector]
        [ReadOnly]
        [LabelText("显隐")]
        public bool Show { get; private set; }

        [ShowInInspector]
        [ReadOnly]
        [LabelText("数量")]
        public int Count { get; private set; }

        private void Awake()
        {
            if (m_Key <= 0)
            {
                Show  = false;
                Count = 0;
                Refresh();
                return;
            }

            OnDestroy();
            RedDotMgr.Inst?.AddChanged(m_Key, OnRedDotChangeHandler);
        }

        private void OnDestroy()
        {
            if (m_Key <= 0) return;
            if (YIUISingletonHelper.Disposing) return;
            RedDotMgr.Inst?.RemoveChanged(m_Key, OnRedDotChangeHandler);
        }

        private void OnRedDotChangeHandler(int count)
        {
            Show  = count >= 1;
            Count = count;
            Refresh();
        }

        private void Refresh()
        {
            gameObject.SetActive(Show);
            ChangeText();
        }

        protected virtual void ChangeText()
        {
        }

        //需要动态改变的,可以预制时,选无
        //然后调用这个方法动态的修改
        public void ChangeBind(int key, bool force = false)
        {
            if (key <= 0)
            {
                Debug.LogError($"修改为 {key} ,尝试修改的Key 错误 不可能 <= 0");
                return;
            }

            if (m_Key == key)
            {
                return;
            }

            if (m_Key > 0 && !force)
            {
                Debug.LogError($"已经有监听的红点 {m_Key} ,想修改为 {key} ,但是当前是非强制修改 所以禁止修改 请检查原因");
                return;
            }

            OnDestroy();
            m_Key = key;
            Awake();
        }
    }
}