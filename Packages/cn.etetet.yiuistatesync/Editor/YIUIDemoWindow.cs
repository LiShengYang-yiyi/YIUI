//------------------------------------------------------------
// Author: 亦亦
// Mail: 379338943@qq.com
// Data: 2023年2月12日
//------------------------------------------------------------

using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using YooAsset.Editor;

namespace YIUIFramework.Editor
{
    public class YIUIDemoWindow : OdinEditorWindow
    {
        [Button("YIUI运行指南", 30, Icon = SdfIconType.Link45deg, IconAlignment = IconAlignment.LeftOfText)]
        [PropertyOrder(-99999)]
        public void OpenDocument()
        {
            Application.OpenURL("https://lib9kmxvq7k.feishu.cn/wiki/H7SmwXozNiliN3kahZFcqQxqnub");
        }

        [MenuItem("ET/YIUI Demo")]
        private static void OpenWindow()
        {
            var window = GetWindow<YIUIDemoWindow>("YIUIDemo");
            window.Show();
        }

        //[MenuItem("Tools/关闭 YIUI Demo")]
        //错误时使用的 面板出现了错误 会导致如何都打不开 就需要先关闭
        private static void CloseWindow()
        {
            GetWindow<YIUIDemoWindow>().Close();
        }

        //关闭后刷新资源
        public static void CloseWindowRefresh()
        {
            CloseWindow();
            AssetDatabase.SaveAssets();
            EditorApplication.ExecuteMenuItem("ET/Loader/ReGenerateProjectAssemblyReference");
            EditorApplication.ExecuteMenuItem("Assets/Refresh");
        }

        private const string YIUIPackageName = "yiuistatesync";
        private const string ETPackageName = "statesync";
        private const string ETLoaderPackageName = "loader";
        private const string UIProjectResPath = "Assets/GameRes/YIUI";

        public enum EDemoType
        {
            YIUI,
            ET
        }

        [Title("模式")]
        [HideLabel]
        [EnumToggleButtons]
        public EDemoType DemoType;

        private bool OpenYIUI => DemoType == EDemoType.YIUI;

        [BoxGroup(" ")]
        [Button("切换Demo", 50), GUIColor(0.4f, 0.8f, 1)]
        private void Switch()
        {
            var tips = "";
            if (SyncYooAssetSetting() && CopyET() && ChangeFile() && SwitchToScene())
            {
                tips = $"成功切换Demo >> {(OpenYIUI ? "YIUI" : "ET")} \n记得编译ET.sln工程!!!";
            }
            else
            {
                tips = $"切换Demo 失败 请检查";
            }

            EditorUtility.DisplayDialog("提示", tips, "确认");
            CloseWindowRefresh();
            ScriptsReferencesHelper.Run();
        }

        [HorizontalGroup("DemoChange")]
        [Button("同步YooAsset设置", 30)]
        private void DemoSyncYooAssetSetting()
        {
            if (SyncYooAssetSetting())
            {
                EditorUtility.DisplayDialog("提示", "同步YooAsset设置 成功", "确认");
                CloseWindowRefresh();
            }
        }

        private bool SyncYooAssetSetting()
        {
            var yooSetting = AssetBundleCollectorSettingData.Setting;
            if (yooSetting == null)
            {
                Debug.LogError($"没有找到yoo设置");
                return false;
            }

            var yiuiSetting = SettingLoader.LoadSettingData<YIUIYooAssetSetting>();
            if (yiuiSetting == null)
            {
                Debug.LogError($"没有找到YIUIYooAssetSetting");
                return false;
            }

            var yiuiDefaultPackage = yiuiSetting.GetPackage("DefaultPackage");
            if (yiuiDefaultPackage == null)
            {
                Debug.LogError($"yiuiSetting 没有找到默认包 DefaultPackage");
                return false;
            }

            var defaultPackage = yooSetting.GetPackage("DefaultPackage");
            if (defaultPackage == null)
            {
                Debug.LogError($"yooSetting 没有找到默认包 DefaultPackage");
                return false;
            }

            yooSetting.UniqueBundleName = true;
            defaultPackage.EnableAddressable = true; //YIUI Demo必须开启可寻址

            var yiuiGroupName = "YIUI";
            AssetBundleCollectorGroup yiuiGroup = default;
            foreach (var group in defaultPackage.Groups)
            {
                if (group.GroupName == yiuiGroupName)
                {
                    yiuiGroup = group;
                    break;
                }
            }

            if (yiuiGroup != null)
                defaultPackage.Groups.Remove(yiuiGroup);

            defaultPackage.Groups.AddRange(yiuiDefaultPackage.Groups);

            CreateUIProjectRes();
            EditorUtility.SetDirty(yooSetting);
            return true;
        }

        [HorizontalGroup("DemoChange")]
        [Button("场景切换", 30)]
        private void DemoSwitchToScene()
        {
            if (SwitchToScene())
            {
                EditorUtility.DisplayDialog("提示", "场景切换 成功", "确认");
                CloseWindowRefresh();
            }
        }

        private bool SwitchToScene()
        {
            var scenePath = $"Packages/cn.etetet.{(OpenYIUI ? YIUIPackageName : ETLoaderPackageName)}/Scenes/Init.unity";
            var path = $"{Application.dataPath}/../{scenePath}";
            if (!File.Exists(path))
            {
                Debug.LogError($"路径不存在场景: {path} ");
                return false;
            }

            EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
            return true;
        }

        [HorizontalGroup("DemoChange")]
        [Button("覆盖Init场景", 30)]
        private void DemoChangeCoverScene()
        {
            CallBackOk("覆盖ET.Loader 的Init场景 覆盖后就无法来回切换了请注意!!!", () =>
            {
                if (DemoCoverScene())
                {
                    EditorUtility.DisplayDialog("提示", "覆盖Init场景 成功", "确认");
                    CloseWindowRefresh();
                }
            });
        }

        private bool DemoCoverScene()
        {
            var sceneYIUIPath = $"Packages/cn.etetet.{YIUIPackageName}/Scenes/Init.unity";
            var sceneETPath = $"Packages/cn.etetet.{ETLoaderPackageName}/Scenes/Init.unity";

            try
            {
                if (!File.Exists(sceneYIUIPath))
                {
                    EditorUtility.DisplayDialog("提示", $"Init场景 源文件不存在。{sceneYIUIPath}", "确认");
                    return false;
                }

                File.Copy(sceneYIUIPath, sceneETPath, true);
            }
            catch (Exception ex)
            {
                Debug.LogError($"复制 Init场景 过程中失败 注意请关闭IDE编辑器使用 {ex}");
            }

            return true;
        }

        [HorizontalGroup("DemoChange")]
        [Button("拷贝ET工程", 30)]
        private void DemoCopyET()
        {
            if (CopyET())
            {
                EditorUtility.DisplayDialog("提示", "拷贝ET工程 成功", "确认");
                CloseWindowRefresh();
            }
        }

        private bool CopyET()
        {
            var et = "ET.sln";
            var packageName = OpenYIUI ? YIUIPackageName : ETPackageName;
            var sourceFilePath = $"{Application.dataPath}/../Packages/cn.etetet.{packageName}/{et}";
            var destinationFilePath = $"{Application.dataPath}/../{et}";

            try
            {
                if (!File.Exists(sourceFilePath))
                {
                    EditorUtility.DisplayDialog("提示", $"{et} 源文件不存在。{sourceFilePath}", "确认");
                    return false;
                }

                File.Copy(sourceFilePath, destinationFilePath, true);
            }
            catch (Exception ex)
            {
                Debug.LogError($"复制ET.sln过程中失败 注意请关闭IDE编辑器使用 {ex}");

                //return false;
            }

            return true;
        }

        [HorizontalGroup("DemoChange")]
        [Button("注释文件", 30)]
        private void DemoChangeFile()
        {
            if (ChangeFile())
            {
                EditorUtility.DisplayDialog("提示", "注释文件 成功", "确认");
                CloseWindowRefresh();
            }
        }

        private bool ChangeFile()
        {
            if (!ReplaceEventSystem())
            {
                return false;
            }

            if (!ReplaceUIComponentSystem())
            {
                return false;
            }

            var sourceFilePath = $"{Application.dataPath}/../Packages/cn.etetet.{YIUIPackageName}/Scripts/HotfixView/Client";

            var csFilesInA = Directory.GetFiles(sourceFilePath, "*.cs", SearchOption.AllDirectories);

            foreach (var fileInA in csFilesInA)
            {
                var fileInB = fileInA.Replace("yiuistatesync", "statesync");
                if (!ChangeFile(fileInA, OpenYIUI))
                {
                    return false;
                }

                if (!ChangeFile(fileInB, !OpenYIUI))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool ChangeFile(string path, bool open)
        {
            if (File.Exists(path))
            {
                var fileName = Path.GetFileName(path);
                var fileContent = File.ReadAllText(path);

                bool startsWithSlashStar = fileContent.StartsWith("/*");
                bool endsWithStarSlash = fileContent.EndsWith("*/");

                string modifiedContent;

                if (open)
                {
                    modifiedContent = fileContent;

                    if (startsWithSlashStar)
                    {
                        modifiedContent = modifiedContent.Substring(2);
                    }

                    int endLength = endsWithStarSlash ? 2 : 0;
                    if (endLength > 0 && modifiedContent.Length > endLength)
                    {
                        modifiedContent = modifiedContent.Substring(0, modifiedContent.Length - endLength);
                    }
                }
                else
                {
                    if (!startsWithSlashStar)
                    {
                        modifiedContent = "/*" + fileContent;
                    }
                    else
                    {
                        modifiedContent = fileContent;
                    }

                    if (!endsWithStarSlash)
                    {
                        modifiedContent += "*/";
                    }
                }

                try
                {
                    File.WriteAllText(path, modifiedContent);

                    //Debug.Log($"文件 {fileName} 已修改。 {(open ? "打开" : "关闭")}");
                }
                catch (Exception e)
                {
                    Debug.LogError($"文件 {fileName} 写入失败 {e}。");
                    return false;
                }
            }

            return true;
        }

        private static bool ReplaceUIComponentSystem()
        {
            string filePath = $"{Application.dataPath}/../Packages/cn.etetet.ui/Scripts/HotfixView/Client/UIComponentSystem.cs";
            return ReplaceFile(filePath, "self.UIGlobalComponent =", "//self.UIGlobalComponent =");
        }

        private static bool ReplaceEventSystem()
        {
            string filePath = $"{Application.dataPath}/../Packages/cn.etetet.core/Scripts/Core/Share/World/EventSystem/EventSystem.cs";
            return ReplaceFile(filePath, "public class EventSystem", "public partial class EventSystem");
        }

        private static bool ReplaceFile(string filePath, string oldText, string newText, string checkString = "")
        {
            if (!File.Exists(filePath))
            {
                Debug.Log($"文件不存在：{filePath}");
                return true;
            }

            try
            {
                string content = File.ReadAllText(filePath);
                if (!content.Contains(string.IsNullOrEmpty(checkString) ? oldText : checkString))
                {
                    return true;
                }

                if (content.Contains(newText))
                {
                    return true;
                }

                content = Regex.Replace(content, oldText, newText);

                File.WriteAllText(filePath, content, Encoding.UTF8);

                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError($"替换失败：{filePath} {ex.Message}");
            }

            return false;
        }

        [BoxGroup("  ")]
        [Button("设置TMP中文字体", 40)]
        private void SetTMP()
        {
            var tmpSettings = Resources.Load<TMP_Settings>("TMP Settings");
            if (tmpSettings == null)
            {
                Debug.LogError($"没有找到TMP设置 请手动设置字体");
                return;
            }

            TMP_FontAsset newDefaultFontAsset = Resources.Load<TMP_FontAsset>("Fonts/SourceHanSansSC-VF SDF");
            if (newDefaultFontAsset == null)
            {
                Debug.LogError($"没有找到新的默认字体 请检查");
                return;
            }

            Type settingsType = typeof(TMP_Settings);
            FieldInfo defaultFontAssetField = settingsType.GetField("m_defaultFontAsset", BindingFlags.NonPublic | BindingFlags.Instance);

            if (defaultFontAssetField != null && newDefaultFontAsset != null)
            {
                defaultFontAssetField.SetValue(tmpSettings, newDefaultFontAsset);
            }

            EditorUtility.DisplayDialog("提示", "设置TMP字体完毕", "确认");
            CloseWindowRefresh();
        }

        [BoxGroup("   ")]
        [Button("打开YooAsset设置", 40)]
        private void SetYooAssetSetting()
        {
            Type type = typeof(AssetBundleCollectorSettingData);
            FieldInfo fieldInfo = type.GetField("_setting", BindingFlags.NonPublic | BindingFlags.Static);
            if (fieldInfo != null)
            {
                //根据Demo类型可以打开不同的YooAsset设置
                if (OpenYIUI)
                {
                    var tempSetting = ScriptableObject.CreateInstance<AssetBundleCollectorSetting>();
                    var yiuiSetting = SettingLoader.LoadSettingData<YIUIYooAssetSetting>();
                    if (yiuiSetting == null)
                    {
                        Debug.LogError($"没有找到YIUIYooAssetSetting");
                        return;
                    }

                    EditorUtility.SetDirty(yiuiSetting);
                    tempSetting.Packages = yiuiSetting.Packages;
                    fieldInfo.SetValue(null, tempSetting);
                }
                else
                {
                    fieldInfo.SetValue(null, null);
                }

                CloseWindowRefresh();
                EditorApplication.ExecuteMenuItem("YooAsset/AssetBundle Collector");
            }
            else
            {
                Debug.LogError($"没有找到AssetBundleCollectorSettingData._setting");
            }
        }

        private void CreateUIProjectRes()
        {
            var path = $"{Application.dataPath}/../{UIProjectResPath}";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public static void CallBackOk(string content, Action okCallBack, Action cancelCallBack = null)
        {
            #if UNITY_EDITOR
            var result = EditorUtility.DisplayDialog("提示", content, "确认");
            if (result) //确定
            {
                try
                {
                    okCallBack?.Invoke();
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                    throw;
                }
            }
            else
            {
                try
                {
                    cancelCallBack?.Invoke();
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                    throw;
                }
            }
            #endif
        }
    }
}