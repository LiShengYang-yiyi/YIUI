#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace YIUIFramework.Editor
{
    [Serializable]
    [HideLabel]
    [HideReferenceObjectPicker]
    public class MacroCurrentData
    {
        [LabelText("当前已存在宏数量")]
        [ReadOnly]
        public int CurrentMacroCount;

        [LabelText("需要移除的宏")]
        [ValueDropdown("GetValues")]
        [InlineButton("Remove", "移除")]
        public string RemoveMacro;

        [LabelText("需要添加的宏")]
        [InlineButton("Add", "添加")]
        public string AddMacro;

        private List<string> AllMacro;

        //整个界面刷新 本来应该用通知的 这里偷懒直接回调了
        //不会有其他需求了
        private Action RefreshAction;

        public MacroCurrentData(Action refreshAction)
        {
            RefreshAction     = refreshAction;
            RemoveMacro       = null;
            AddMacro          = null;
            AllMacro          = MacroHelper.GetSymbols(UnityMacroModule.BuildTargetGroup);
            CurrentMacroCount = AllMacro.Count;
        }

        private IEnumerable<string> GetValues()
        {
            return AllMacro;
        }

        public void Add()
        {
            if (string.IsNullOrEmpty(AddMacro))
            {
                UnityTipsHelper.Show("请填写需要添加的宏名称");
                return;
            }

            MacroHelper.AddMacro(AddMacro, UnityMacroModule.BuildTargetGroup);
            RefreshAction?.Invoke();
        }

        public void Remove()
        {
            if (string.IsNullOrEmpty(RemoveMacro))
            {
                UnityTipsHelper.Show("请选择需要移除的宏");
                return;
            }

            MacroHelper.RemoveMacro(RemoveMacro, UnityMacroModule.BuildTargetGroup);
            RefreshAction?.Invoke();
        }
    }
}
#endif
