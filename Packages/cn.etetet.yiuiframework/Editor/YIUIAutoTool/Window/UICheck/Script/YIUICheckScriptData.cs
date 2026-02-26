using System;
using System.IO;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace YIUIFramework.Editor
{
    /// <summary>
    /// UI资源绑定信息
    /// </summary>
    [HideReferenceObjectPicker]
    [HideLabel]
    public class YIUICheckScriptData
    {
        #region 信息

        [HideInInspector]
        public Type ComponentType;

        [TableColumnWidth(300, resizable: false)]
        [VerticalGroup("信息")]
        [LabelText("UI类型")]
        [LabelWidth(50)]
        [ReadOnly]
        public EUICodeType CodeType;

        [TableColumnWidth(300, resizable: false)]
        [VerticalGroup("信息")]
        [LabelText("层级")]
        [LabelWidth(50)]
        [ReadOnly]
        [ShowIf("ShowIfPanelLayer")]
        public EPanelLayer PanelLayer;

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

        private bool ShowIfPanelLayer()
        {
            return CodeType == EUICodeType.Panel;
        }

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

        private bool m_IsUpdateCheck;

        [VerticalGroup("操作")]
        [HideLabel]
        [TableColumnWidth(100, resizable: false)]
        [Button(50, Icon = SdfIconType.ArrowClockwise)]
        [ShowIf("ShowIfUpdatCheck")]
        public void UpdatCheck()
        {
            m_IsUpdateCheck = true;
            UpdatePath(ComponentType, ResName);
        }

        private bool ShowIfUpdatCheck()
        {
            return !m_IsUpdateCheck && !m_IsDelete;
        }

        private bool m_IsDelete;

        [VerticalGroup("操作")]
        [GUIColor(1, 0, 0)]
        [HideLabel]
        [TableColumnWidth(100, resizable: false)]
        [Button(50, Icon = SdfIconType.X)]
        [ShowIf("ShowIfIgonre")]
        private void DeleteScript()
        {
            DeleteScript(true, true);
        }

        public bool ShowIfDeleteScript()
        {
            return m_IsUpdateCheck && string.IsNullOrEmpty(PrefabPath) && !m_IsDelete;
        }

        public void DeleteScript(bool refresh, bool tips)
        {
            m_IsDelete = true;

            DeleteFile(Component);
            Component = "";
            DeleteFile(ComponentGen);
            ComponentGen = "";
            DeleteFile(System);
            System = "";
            DeleteFile(SystemGen);
            SystemGen = "";

            if (tips)
            {
                UnityTipsHelper.Show($"删除成功: {ComponentType.Name} 相关文件");
            }

            if (refresh)
            {
                YIUIAutoTool.CloseWindowRefresh();
            }
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

            var directoryPath = Path.GetDirectoryName(path);
            if (Directory.Exists(directoryPath))
            {
                var files          = Directory.GetFiles(directoryPath);
                var subdirectories = Directory.GetDirectories(directoryPath);

                if (files.Length == 0 && subdirectories.Length == 0)
                {
                    try
                    {
                        Directory.Delete(directoryPath, false);

                        var metaPath = $"{directoryPath}.meta";
                        if (File.Exists(metaPath))
                        {
                            File.Delete(metaPath);
                        }
                    }
                    catch (IOException e)
                    {
                        Debug.LogError($"无法删除文件夹: \n{directoryPath}\n{e}");
                    }
                    catch (UnauthorizedAccessException e)
                    {
                        Debug.LogError($"没有权限删除文件夹: \n{directoryPath}\n{e}");
                    }
                }
                else
                {
                    Debug.Log($"文件夹不为空 将保留文件夹: \n{directoryPath} ");
                }
            }
        }

        private void UpdatePath(Type componentType, string resName)
        {
            Component    = "";
            ComponentGen = "";
            System       = "";
            SystemGen    = "";
            PrefabPath   = "";

            var componentName = componentType.Name;
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

            foreach (string guid in AssetDatabase.FindAssets($"{resName} t:Prefab", null))
            {
                var path        = AssetDatabase.GUIDToAssetPath(guid);
                var fileNameAll = Path.GetFileName(path);
                if (string.IsNullOrEmpty(fileNameAll)) continue;

                var fileName = fileNameAll.Split('.')[0];
                if (fileName == resName)
                {
                    PrefabPath = path;
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
            return ShowIfDeleteScript() && !m_IsIgnore;
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