#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace YIUIFramework
{
    internal class UIBindProvider : ICodeGenerator<UIBindVo>
    {
        //业务代码相关程序集的名字
        //默认有Unity默认程序集 可以根据需求修改
        internal static string[] LogicAssemblyNames = { "Assembly-CSharp" };

        private static Type[] GetLogicTypes()
        {
            return AppDomain.CurrentDomain.GetTypesByAssemblyName(LogicAssemblyNames);
        }

        private Type m_BasePanelType     = typeof(BasePanel);
        private Type m_BaseViewType      = typeof(BaseView);
        private Type m_BaseComponentType = typeof(BaseComponent);

        public UIBindVo[] Get()
        {
            var gameTypes = GetLogicTypes();
            if (gameTypes.Length < 1)
            {
                return Array.Empty<UIBindVo>();
            }

            var panelTypes     = new List<Type>(); //继承panel的
            var viewTypes      = new List<Type>(); //继承View的
            var componentTypes = new List<Type>(); //继承Component的
            var binds          = new List<UIBindVo>();

            foreach (var gameType in gameTypes)
            {
                if (m_BasePanelType.IsAssignableFrom(gameType))
                {
                    panelTypes.Add(gameType);
                }
                else if (m_BaseViewType.IsAssignableFrom(gameType))
                {
                    viewTypes.Add(gameType);
                }
                else if (m_BaseComponentType.IsAssignableFrom(gameType))
                {
                    componentTypes.Add(gameType);
                }
            }

            //panel绑定
            foreach (var panelType in panelTypes)
            {
                if (panelType.BaseType == null)
                {
                    continue;
                }

                if (GetBindVo(panelType.BaseType, panelType, m_BasePanelType, out var bindVo))
                {
                    binds.Add(bindVo);
                }
            }

            //view绑定
            foreach (var viewType in viewTypes)
            {
                if (viewType.BaseType == null)
                {
                    continue;
                }

                if (GetBindVo(viewType.BaseType, viewType, m_BaseViewType, out var bindVo))
                {
                    binds.Add(bindVo);
                }
            }

            //component绑定
            foreach (var componentType in componentTypes)
            {
                if (componentType.BaseType == null)
                {
                    continue;
                }

                if (GetBindVo(componentType.BaseType, componentType, m_BaseComponentType, out var bindVo))
                {
                    binds.Add(bindVo);
                }
            }

            return binds.ToArray();
        }

        private static bool GetBindVo(Type uiBaseType, Type creatorType, Type groupType, out UIBindVo bindVo)
        {
            bindVo = new UIBindVo();
            if (uiBaseType == null ||
                !uiBaseType.GetFieldValue("PkgName", out bindVo.PkgName) ||
                !uiBaseType.GetFieldValue("ResName", out bindVo.ResName))
            {
                return false;
            }

            bindVo.CodeType    = uiBaseType.BaseType;
            bindVo.BaseType    = uiBaseType;
            bindVo.CreatorType = creatorType;
            return true;
        }

        public void WriteCode(UIBindVo info, StringBuilder sb)
        {
            sb.Append("            {\r\n");
            sb.AppendFormat("                PkgName     = {0}.PkgName,\r\n", info.BaseType.FullName);
            sb.AppendFormat("                ResName     = {0}.ResName,\r\n", info.BaseType.FullName);
            sb.AppendFormat("                CodeType    = {0},\r\n", GetCodeTypeName(info.CodeType));
            sb.AppendFormat("                BaseType    = typeof({0}),\r\n", info.BaseType.FullName);
            sb.AppendFormat("                CreatorType = typeof({0}),\r\n", info.CreatorType.FullName);
            sb.Append("            };\r\n");
        }

        private string GetCodeTypeName(Type uiBaseType)
        {
            if (uiBaseType == m_BasePanelType)
            {
                return UIStaticHelper.UIBasePanelName;
            }
            else if (uiBaseType == m_BaseViewType)
            {
                return UIStaticHelper.UIBaseViewName;
            }
            else if (uiBaseType == m_BaseComponentType)
            {
                return UIStaticHelper.UIBaseComponentName;
            }
            else
            {
                Debug.LogError($"当前类型错误 是否新增了类型 {uiBaseType}");
                return UIStaticHelper.UIBaseName;
            }
        }

        public void NewCode(UIBindVo info, StringBuilder sb)
        {
        }
    }
}
#endif