#if YIUI
using System.Diagnostics;
using UnityEditor;
using UnityEngine;

namespace YIUIFramework.Editor
{
    [InitializeOnLoad]
    public static class YIUICLIToolBar
    {
        static YIUICLIToolBar()
        {
            YIUIToolbarExtender.AddRightToolbarGUI(OnCLIToolbarGUI, 1000);
        }

        const string iconPath = "Packages\\cn.etetet.yiuiframework\\Editor\\Toolbar\\Icon\\icon_droid.png";
        private static Texture _cachedIcon;

        private static void OnCLIToolbarGUI()
        {
            _cachedIcon ??= AssetDatabase.LoadAssetAtPath<Texture>(iconPath);

            GUILayout.Space(5);
            GUIContent iconContent = new(string.Empty, _cachedIcon);
            iconContent.tooltip = "Droid";
            if (GUILayout.Button(iconContent))
            {
                var projectPath = System.IO.Path.GetDirectoryName(Application.dataPath);
                StartPowerShellProcess("droid", projectPath);
            }

            GUILayout.Space(5);
            iconContent.tooltip = "Claude";
            if (GUILayout.Button(iconContent))
            {
                var projectPath = System.IO.Path.GetDirectoryName(Application.dataPath);
                StartPowerShellProcess("claude", projectPath);
            }

            GUILayout.Space(5);
        }

        private static void StartPowerShellProcess(string command, string workingDir)
        {
            try
            {
                Process process = new();
                var start = new ProcessStartInfo
                {
                    FileName = "wt.exe",
                    Arguments = $"-d \"{workingDir}\" powershell -NoExit -Command \"{command}\"",
                    CreateNoWindow = false,
                    ErrorDialog = true,
                    UseShellExecute = true,
                    WorkingDirectory = workingDir
                };

                process.StartInfo = start;
                process.Start();
            }
            catch (System.Exception ex)
            {
                EditorUtility.DisplayDialog("执行失败",
                    $"无法执行命令: {command}\n\n错误信息: {ex.Message}\n\n请检查:\n1. 命令是否正确\n2. Windows Terminal是否已安装\n3. wt.exe是否在系统PATH环境变量中",
                    "确定");
            }
        }
    }
}
#endif