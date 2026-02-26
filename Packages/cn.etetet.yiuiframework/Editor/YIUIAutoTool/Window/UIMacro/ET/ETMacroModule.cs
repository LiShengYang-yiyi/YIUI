#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace YIUIFramework.Editor
{
    /// <summary>
    ///  ET宏
    /// </summary>
    public class ETMacroModule : BaseYIUIToolModule
    {
        //TODO 功能TODO
        
        public override void Initialize()
        {
            var assembly = AssemblyHelper.GetAssembly("ET.YIUIFramework.Editor");

            var allMacroEnum = AssemblyHelper.GetClassesWithAttribute<YIUIEnumETMacroAttribute>(assembly);

            Type macroDataBaseType = typeof(MacroDataBase<>);

            foreach (var macroEnum in allMacroEnum)
            {
                Type specificType = macroDataBaseType.MakeGenericType(macroEnum);
                var  instance     = (MacroDataBase)Activator.CreateInstance(specificType);
                instance.Initialize();
            }
        }

        public override void OnDestroy()
        {
        }
    }
}
#endif
