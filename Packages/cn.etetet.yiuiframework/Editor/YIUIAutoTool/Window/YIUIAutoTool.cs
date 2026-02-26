//------------------------------------------------------------
// Author: 亦亦
// Mail: 379338943@qq.com
// Data: 2023年2月12日
//------------------------------------------------------------

#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using Type = System.Type;

namespace YIUIFramework.Editor
{
    /// <summary>
    /// YIUI 自动化工具
    /// </summary>
    public class YIUIAutoTool : OdinMenuEditorWindow
    {
        [MenuItem("ET/YIUI 自动化工具")]
        public static void OpenWindow()
        {
            var window = GetWindow<YIUIAutoTool>("YIUI");
            if (window != null)
            {
                window.Show();
            }
        }

        /// <summary>
        /// 选中指定模块页签
        /// </summary>
        public void SelectModule(string moduleName)
        {
            if (m_OdinMenuTree == null) return;

            m_RestoringSelection = true;
            var result = SelectChainByPath(moduleName);
            m_RestoringSelection = false;

            if (result != null)
            {
                m_LastSelectMenuPrefs.Value = moduleName;
                m_LastSelectToolModulePathPrefs.Value = moduleName;
            }
        }

        //[MenuItem("ET/关闭 YIUI 自动化工具")]
        //错误时使用的 面板出现了错误 会导致如何都打不开 就需要先关闭
        public static void CloseWindow()
        {
            GetWindow<YIUIAutoTool>()?.Close();
        }

        //关闭后刷新资源
        public static void CloseWindowRefresh()
        {
            CloseWindow();
            AssetDatabase.SaveAssets();

            //AssetDatabase.Refresh();//下面的刷新更NB
            EditorApplication.ExecuteMenuItem("Assets/Refresh");
        }

        private FloatPrefs m_MenuWidthPrefs = new("YIUIAutoTool_MenuWidth", null, 300f);

        private float m_MenuWidth = 300f;

        public override float MenuWidth
        {
            get { return m_MenuWidth; }
            set { m_MenuWidth = value; }
        }

        private OdinMenuTree m_OdinMenuTree;
        private List<BaseTreeMenuItem> m_AllMenuItem = new List<BaseTreeMenuItem>();

        protected override OdinMenuTree BuildMenuTree()
        {
            m_OdinMenuTree = new OdinMenuTree();
            m_OdinMenuTree.Selection.SelectionChanged += OnSelectionChanged;

            m_AllMenuItem.Clear();

            m_AllMenuItem.Add(new TreeMenuItem<UIPublishModule>(this, m_OdinMenuTree, UIPublishModule.m_PublishName, EditorIcons.UnityFolderIcon));

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var allAutoMenus = new List<YIUIAutoMenuData>();

            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes();

                foreach (Type type in types)
                {
                    if (type.IsDefined(typeof(YIUIAutoMenuAttribute), false))
                    {
                        YIUIAutoMenuAttribute attribute = (YIUIAutoMenuAttribute)Attribute.GetCustomAttribute(type, typeof(YIUIAutoMenuAttribute));
                        allAutoMenus.Add(new YIUIAutoMenuData
                        {
                            Type = type,
                            MenuName = attribute.MenuName,
                            Order = attribute.Order
                        });
                    }
                }
            }

            allAutoMenus.Sort((a, b) => a.Order.CompareTo(b.Order));

            foreach (var attribute in allAutoMenus)
            {
                m_AllMenuItem.Add(NewTreeMenuItem(attribute.Type, attribute.MenuName));
            }

            m_OdinMenuTree.Add("全局设置", this, EditorIcons.SettingsCog);

            return m_OdinMenuTree;
        }

        private BaseTreeMenuItem NewTreeMenuItem(Type moduleType, string moduleName)
        {
            var treeMenuItemType = typeof(TreeMenuItem<>);

            var specificTreeMenuItemType = treeMenuItemType.MakeGenericType(moduleType);

            var constructor = specificTreeMenuItemType.GetConstructor(new Type[]
            {
                typeof(OdinMenuEditorWindow),
                typeof(OdinMenuTree),
                typeof(string)
            });

            object treeMenuItem = constructor.Invoke(new object[]
            {
                this,
                m_OdinMenuTree,
                moduleName
            });

            return (BaseTreeMenuItem)treeMenuItem;
        }

        private bool m_FirstInit = true;
        private StringPrefs m_LastSelectMenuPrefs = new StringPrefs("YIUIAutoTool_LastSelectMenu", null, "全局设置");
        private StringPrefs m_LastSelectToolModulePathPrefs = new StringPrefs("YIUIAutoTool_LastSelectToolModulePath", null, "");
        private readonly HashSet<OdinMenuItem> m_FirstSelect = new();
        private bool m_RestoringSelection;

        private void OnSelectionChanged(SelectionChangedType obj)
        {
            if (m_RestoringSelection)
            {
                return;
            }

            if (obj != SelectionChangedType.ItemAdded)
            {
                return;
            }

            if (m_FirstInit)
            {
                m_FirstInit = false;

                if (!string.IsNullOrEmpty(m_LastSelectToolModulePathPrefs.Value))
                {
                    m_RestoringSelection = true;
                    var restored = SelectChainByPath(m_LastSelectToolModulePathPrefs.Value);
                    m_RestoringSelection = false;
                    if (restored != null)
                    {
                        return;
                    }
                }

                foreach (var menu in m_OdinMenuTree.MenuItems)
                {
                    if (menu.Name != m_LastSelectMenuPrefs.Value) continue;
                    menu.Select();
                    menu.Toggled = true;
                    break;
                }

                return;
            }

            if (m_OdinMenuTree.Selection.Count > 1)
            {
                Debug.LogError($"不可能同时选多个: {m_OdinMenuTree.Selection.Count}");
                return;
            }

            var selectedMenuItem = m_OdinMenuTree.Selection[0];

            if (selectedMenuItem.Value is BaseTreeMenuItem menuItem)
            {
                menuItem.SelectionMenu();
                m_LastSelectToolModulePathPrefs.Value = menuItem.ModuleName;
            }
            else if (selectedMenuItem.Value is BaseYIUIToolModule module)
            {
                module.SelectionMenu();
                m_LastSelectToolModulePathPrefs.Value = module.ModuleName;
            }

            if (m_FirstSelect.Add(selectedMenuItem))
            {
                selectedMenuItem.Toggled = true;
            }
            else
            {
                selectedMenuItem.Toggled = !selectedMenuItem.Toggled;
            }

            foreach (var menu in m_OdinMenuTree.MenuItems)
            {
                if (!menu.IsSelected) continue;
                m_LastSelectMenuPrefs.Value = menu.Name;
                break;
            }

            var root = selectedMenuItem;
            while (root.Parent != null)
            {
                root = root.Parent;
            }

            m_LastSelectMenuPrefs.Value = root.Name;
        }

        private OdinMenuItem FindMenuItemByModulePath(string modulePath)
        {
            foreach (var root in m_OdinMenuTree.MenuItems)
            {
                var result = FindRecursive(root, modulePath);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        private OdinMenuItem FindRecursive(OdinMenuItem item, string modulePath)
        {
            if (item.Value is BaseTreeMenuItem bt && bt.ModuleName == modulePath) return item;
            if (item.Value is BaseYIUIToolModule bm && bm.ModuleName == modulePath) return item;
            foreach (var child in item.ChildMenuItems)
            {
                var res = FindRecursive(child, modulePath);
                if (res != null) return res;
            }

            return null;
        }

        private void ExpandParents(OdinMenuItem item)
        {
            var cur = item.Parent;
            while (cur != null)
            {
                cur.Toggled = true;
                cur = cur.Parent;
            }

            item.Toggled = true;
        }

        private OdinMenuItem SelectChainByPath(string modulePath)
        {
            if (string.IsNullOrEmpty(modulePath))
            {
                return null;
            }

            var segments = modulePath.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            if (segments.Length == 0)
            {
                return null;
            }

            OdinMenuItem current = null;

            foreach (var root in m_OdinMenuTree.MenuItems)
            {
                if (root.Name == segments[0])
                {
                    current = root;
                    break;
                }
            }

            if (current == null)
            {
                return null;
            }

            SelectAndInit(current);
            ExpandParents(current);

            for (int i = 1; i < segments.Length; i++)
            {
                var nextName = segments[i];
                OdinMenuItem next = null;
                foreach (var child in current.ChildMenuItems)
                {
                    if (child.Name == nextName)
                    {
                        next = child;
                        break;
                    }
                }

                if (next == null)
                {
                    SelectAndInit(current);
                    foreach (var child in current.ChildMenuItems)
                    {
                        if (child.Name == nextName)
                        {
                            next = child;
                            break;
                        }
                    }
                }

                if (next == null)
                {
                    return null;
                }

                current = next;
                SelectAndInit(current);
                ExpandParents(current);
            }

            return current;
        }

        private void SelectAndInit(OdinMenuItem item)
        {
            item.Select();
            item.Toggled = true;
            if (item.Value is BaseTreeMenuItem bt)
            {
                bt.SelectionMenu();
            }
            else if (item.Value is BaseYIUIToolModule bm)
            {
                bm.SelectionMenu();
            }
        }

        public static StringPrefs UserNamePrefs = new StringPrefs("YIUIAutoTool_UserName", null, "YIUI");

        [LabelText("用户名")]
        [Required("请填写用户名")]
        [ShowInInspector]
        private static string m_Author;

        public static string Author
        {
            get
            {
                if (string.IsNullOrEmpty(m_Author))
                {
                    m_Author = UserNamePrefs.Value;
                }

                return m_Author;
            }
        }

        [HideLabel]
        [HideReferenceObjectPicker]
        [ShowInInspector]
        private BaseCreateModule m_UIBaseModule;

        [BoxGroup("全局图集设置", centerLabel: true)]
        [ShowInInspector]
        internal UIAtlasModule AtlasModule;

        [BoxGroup("其他设置", centerLabel: true)]
        [ShowInInspector]
        internal UIOtherModule OtherModule;

        protected override void Initialize()
        {
            base.Initialize();
            m_UIBaseModule = new CreateUIBaseModule();
            AtlasModule = new();
            OtherModule = new();
            m_Author = UserNamePrefs.Value;
            m_UIBaseModule?.Initialize();
            AtlasModule?.Initialize();
            OtherModule?.Initialize();
            m_MenuWidth = MathF.Max(m_MenuWidthPrefs.Value, 200f);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            UserNamePrefs.Value = Author;
            m_UIBaseModule?.OnDestroy();
            AtlasModule?.OnDestroy();
            OtherModule?.OnDestroy();
            m_MenuWidthPrefs.Value = MathF.Min(MathF.Max(m_MenuWidth, 200f), position.width - 100f);

            foreach (var menuItem in m_AllMenuItem)
            {
                menuItem.OnDestroy();
            }
        }
    }
}
#endif