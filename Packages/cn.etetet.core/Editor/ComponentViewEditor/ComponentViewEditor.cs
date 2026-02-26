#if ENABLE_VIEW
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace ET
{
    [CustomEditor(typeof(ComponentView))]
    public class ComponentViewEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var componentView = (ComponentView)target;
            var component     = componentView.Component;
            ComponentViewHelper.DrawAction(component);
        }
    }

    public static partial class ComponentViewHelper
    {
        private static readonly List<ITypeDrawer> typeDrawers = new();

        private static readonly Dictionary<Type, EntityDrawer> entityDrawerDict = new();

        private static Action<Entity> drawAction;

        static ComponentViewHelper()
        {
            var assemblies        = AppDomain.CurrentDomain.GetAssemblies();
            var systemGenericType = typeof(EntityDrawerSystem<>);

            foreach (Assembly assembly in assemblies)
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.IsDefined(typeof(TypeDrawerAttribute)))
                    {
                        ITypeDrawer iTypeDrawer = (ITypeDrawer)Activator.CreateInstance(type);
                        typeDrawers.Add(iTypeDrawer);
                    }

                    if (type.IsDefined(typeof(EntityDrawerAttribute)))
                    {
                        var baseType = type.BaseType;
                        if (baseType is not { IsGenericType: true })
                        {
                            continue;
                        }

                        if (baseType.GetGenericTypeDefinition() != systemGenericType)
                        {
                            continue;
                        }

                        var entityType = baseType.GetGenericArguments()[0];
                        var attribute  = type.GetCustomAttribute<EntityDrawerAttribute>();
                        var iDrawer    = (IEntityDrawer)Activator.CreateInstance(type);
                        var order      = attribute.Order;
                        var drawerData = new EntityDrawer
                        {
                            EntityType     = entityType,
                            Drawer         = iDrawer,
                            Order          = order,
                            SkipTypeDrawer = attribute.SkipTypeDrawer
                        };

                        if (entityDrawerDict.TryGetValue(entityType, out var drawer))
                        {
                            if (order > drawer.Order)
                            {
                                entityDrawerDict[entityType] = drawerData;
                            }
                        }
                        else
                        {
                            entityDrawerDict.Add(entityType, drawerData);
                        }
                    }
                }
            }

            drawAction = Draw;

            OverrideDraw();
        }

        public static void DrawAction(Entity entity)
        {
            try
            {
                drawAction?.Invoke(entity);
            }
            catch (Exception e)
            {
                Debug.Log($"组件视图绘制错误: {entity.GetType().FullName}, {e}");
            }
        }

        private static void Draw(Entity entity)
        {
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

        static partial void OverrideDraw();
    }
}
#endif