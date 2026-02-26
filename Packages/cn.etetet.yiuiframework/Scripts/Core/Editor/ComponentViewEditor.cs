#if ENABLE_VIEW
using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using YIUIFramework.Editor;

namespace ET
{
    public static partial class ComponentViewHelper
    {
        static partial void OverrideDraw()
        {
            drawAction = YIUIDraw;
        }

        private static void DrawScripButton(Entity entity)
        {
            string componentName = entity.GetType().Name;

            // 绘制两个并排的按钮，一个查看View代码，一个查看System代码
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.Space(20, false);
            if (GUILayout.Button("查看Component代码"))
            {
                YIUIScriptHelper.OpenScript(componentName);
            }

            if (GUILayout.Button("查看System代码"))
            {
                YIUIScriptHelper.OpenScript($"{componentName}System");
            }

            EditorGUILayout.Space(20, false);
            EditorGUILayout.EndHorizontal();
        }

        private static void YIUIDraw(Entity entity)
        {
            DrawScripButton(entity);

            EditorGUILayout.BeginVertical();

            EditorGUILayout.LongField("InstanceId: ", entity.InstanceId);

            EditorGUILayout.LongField("Id: ", entity.Id);

            var entityType = entity.GetType();
            entityDrawerDict.TryGetValue(entityType, out var entityDrawer);
            var skipTypeDrawer = entityDrawer?.SkipTypeDrawer ?? false;

            if (!skipTypeDrawer)
            {
                var fields = entityType.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
                foreach (FieldInfo fieldInfo in fields)
                {
                    Type type = fieldInfo.FieldType;
                    if (type.IsDefined(typeof(HideInInspector), false))
                    {
                        continue;
                    }

                    if (fieldInfo.IsDefined(typeof(HideInInspector), false))
                    {
                        continue;
                    }

                    foreach (ITypeDrawer typeDrawer in typeDrawers)
                    {
                        if (!typeDrawer.HandlesType(type))
                        {
                            continue;
                        }

                        string fieldName = fieldInfo.Name;
                        if (fieldName.Length > 17 && fieldName.Contains("k__BackingField"))
                        {
                            fieldName = fieldName.Substring(1, fieldName.Length - 17);
                        }

                        object value = fieldInfo.GetValue(entity);

                        try
                        {
                            value = typeDrawer.DrawAndGetNewValue(type, fieldName, value, entity);
                        }
                        catch (Exception e)
                        {
                            Debug.LogError(e);
                        }

                        fieldInfo.SetValue(entity, value);
                        break;
                    }
                }
            }

            if (entityDrawer != null)
            {
                entityDrawer.Drawer.Drawer(entity);
            }

            EditorGUILayout.EndVertical();
        }
    }
}
#endif