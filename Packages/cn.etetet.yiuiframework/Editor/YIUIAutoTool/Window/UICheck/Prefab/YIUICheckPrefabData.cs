using System;
using System.IO;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace YIUIFramework.Editor
{
    [HideLabel]
    [HideReferenceObjectPicker]
    public class YIUICheckPrefabData
    {
        #region 信息

        [TableColumnWidth(300, resizable: false)]
        [VerticalGroup("信息")]
        [LabelText("模块名")]
        [LabelWidth(50)]
        [ReadOnly]
        public string PkgName;

        [TableColumnWidth(300, resizable: false)]
        [VerticalGroup("信息")]
        [LabelText("资源名")]
        [LabelWidth(50)]
        [ReadOnly]
        public string ResName;

        #endregion

        #region 路径

        [TableColumnWidth(400)]
        [VerticalGroup("路径")]
        [ShowInInspector]
        [LabelText("C")]
        [LabelWidth(50)]
        [ReadOnly]
        [Sirenix.OdinInspector.FilePath]
        public string Component;

        [TableColumnWidth(400)]
        [VerticalGroup("路径")]
        [ShowInInspector]
        [LabelText("CGen")]
        [LabelWidth(50)]
        [ReadOnly]
        [Sirenix.OdinInspector.FilePath]
        public string ComponentGen;

        [TableColumnWidth(400)]
        [VerticalGroup("路径")]
        [ShowInInspector]
        [LabelText("S")]
        [LabelWidth(50)]
        [ReadOnly]
        [Sirenix.OdinInspector.FilePath]
        public string System;

        [TableColumnWidth(400)]
        [VerticalGroup("路径")]
        [ShowInInspector]
        [LabelText("SGen")]
        [LabelWidth(50)]
        [ReadOnly]
        [Sirenix.OdinInspector.FilePath]
        public string SystemGen;

        [TableColumnWidth(400)]
        [VerticalGroup("路径")]
        [ShowInInspector]
        [LabelText("预制")]
        [LabelWidth(50)]
        [ReadOnly]
        [Sirenix.OdinInspector.FilePath]
        public string PrefabPath;

        #endregion

        public YIUICheckPrefabData(string prefabPath, string pkgName, string resName)
        {
            PrefabPath = prefabPath;
            PkgName    = pkgName;
            ResName    = resName;
            UpdateData(ResName);
            LoadGameObject();
        }

        [VerticalGroup("操作")]
        [HideLabel]
        [TableColumnWidth(100, resizable: false)]
        [Button(30, Icon = SdfIconType.HandIndex)]
        [ShowIf("ShowIfSelectPrefab")]
        private void SelectPrefab()
        {
            if (m_GameObject == null) return;
            EditorGUIUtility.PingObject(m_GameObject);
            Selection.activeObject = m_GameObject;
        }

        private bool ShowIfSelectPrefab()
        {
            return m_GameObject != null;
        }

        private bool m_IsDelete;

        [VerticalGroup("操作")]
        [GUIColor(1, 0, 0)]
        [HideLabel]
        [TableColumnWidth(100, resizable: false)]
        [Button(50, Icon = SdfIconType.X)]
        [ShowIf("ShowIfIgonre")]
        private void Delete()
        {
            Delete(true, true);
        }

        public bool ShowIfDelete()
        {
            if (m_IsDelete)
            {
                return false;
            }

            if (string.IsNullOrEmpty(PrefabPath))
            {
                return false;
            }

            if (m_GameObject == null)
            {
                return false;
            }

            if (m_CDETable == null)
            {
                return true;
            }

            if (string.IsNullOrEmpty(Component))
            {
                return true;
            }

            return false;
        }

        public void Delete(bool refresh, bool tips)
        {
            m_IsDelete = true;

            DeleteFile(PrefabPath);

            if (tips)
            {
                UnityTipsHelper.Show($"删除成功: {PrefabPath}");
            }

            if (refresh)
            {
                YIUIAutoTool.CloseWindowRefresh();
            }

            PrefabPath = "";
        }

        private void DeleteFile(string path)
        {
            if (!File.Exists(path)) return;
            try
            {
                File.Delete(path);

                var metaPath = $"{path}.meta";
                if (File.Exists(metaPath))
                {
                    File.Delete(metaPath);
                }
            }
            catch (IOException e)
            {
                Debug.LogError($"无法删除文件: \n{path}\n{e}");
            }
            catch (UnauthorizedAccessException e)
            {
                Debug.LogError($"没有权限删除文件: \n{path}\n{e}");
            }
        }

        private void UpdateData(string resName)
        {
            Component    = "";
            ComponentGen = "";
            System       = "";
            SystemGen    = "";
            var componentName = $"{resName}Component";
            foreach (string guid in AssetDatabase.FindAssets($"{componentName} t:Script", null))
            {
                var path        = AssetDatabase.GUIDToAssetPath(guid);
                var fileNameAll = Path.GetFileName(path);
                if (string.IsNullOrEmpty(fileNameAll)) continue;

                var fileName = fileNameAll.Split('.')[0];

                if (fileName.EndsWith("SystemGen"))
                {
                    SystemGen = path;
                }
                else if (fileName.EndsWith("System"))
                {
                    System = path;
                }
                else if (fileName.EndsWith("Gen"))
                {
                    ComponentGen = path;
                }
                else
                {
                    Component = path;
                }
            }
        }

        private GameObject     m_GameObject;
        private UIBindCDETable m_CDETable;
        public  bool           IsCDETable => m_CDETable != null;

        private void LoadGameObject()
        {
            m_CDETable   = null;
            m_GameObject = AssetDatabase.LoadAssetAtPath<GameObject>(PrefabPath);
            if (m_GameObject != null)
            {
                m_CDETable = m_GameObject.GetComponent<UIBindCDETable>();
                if (m_CDETable != null)
                {
                    if (PkgName != m_CDETable.PkgName)
                    {
                        Debug.LogError($"{PrefabPath} PkgName不匹配 {PkgName} != {m_CDETable.PkgName}");
                        PkgName = m_CDETable.PkgName;
                    }

                    if (ResName != m_CDETable.ResName)
                    {
                        Debug.LogError($"{PrefabPath} ResName不匹配 {ResName} != {m_CDETable.ResName}");
                        ResName = m_CDETable.ResName;
                    }
                }
            }
        }

        private bool m_IsIgnore;

        [VerticalGroup("操作")]
        [HideLabel]
        [TableColumnWidth(100, resizable: false)]
        [Button(50, Icon = SdfIconType.DashCircle)]
        [ShowIf("ShowIfIgonre")]
        private void Igonre()
        {
            m_IsIgnore = true;
        }

        public bool ShowIfIgonre()
        {
            return ShowIfDelete() && !m_IsIgnore;
        }

        [VerticalGroup("操作")]
        [HideLabel]
        [TableColumnWidth(100, resizable: false)]
        [Button(50, Icon = SdfIconType.DashCircleDotted)]
        [ShowIf("ShowIfReIgonre")]
        private void ReIgonre()
        {
            m_IsIgnore = false;
        }

        public bool ShowIfReIgonre()
        {
            return m_IsIgnore;
        }
    }
}