#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace YIUIFramework.Editor
{
    [HideLabel]
    [Serializable]
    [HideReferenceObjectPicker]
    public abstract class MacroDataBase
    {
        public abstract void         Initialize();
        public abstract List<string> GetAll();
        public abstract List<string> GetSelect();
    }

    /// <summary>
    /// 宏数据基类
    /// </summary>
    [HideLabel]
    [HideReferenceObjectPicker]
    [Serializable]
    public class MacroDataBase<T> : MacroDataBase where T : struct
    {
        [LabelText("宏 枚举")]
        [ShowInInspector]
        protected T MacroEnumType;

        public override void Initialize()
        {
            var value = MacroHelper.InitEnumValue<T>(UnityMacroModule.BuildTargetGroup);
            Enum.TryParse(value.ToString(), out this.MacroEnumType);
        }

        /// <summary>
        /// 获取当前枚举所有宏
        /// </summary>
        /// <returns></returns>
        public override List<string> GetAll()
        {
            return MacroHelper.GetEnumAll<T>();
        }

        /// <summary>
        /// 获取选择宏
        /// </summary>
        /// <returns></returns>
        public override List<string> GetSelect()
        {
            return MacroHelper.GetEnumSelect(MacroEnumType);
        }
    }
}
#endif
