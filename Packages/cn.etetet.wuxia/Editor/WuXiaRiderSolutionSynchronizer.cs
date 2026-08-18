using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace ET.WuXia.Editor
{
    // 【武侠包建设 2026-08-18】提供 WuXia 自有的 Rider 解决方案同步能力。
    // 作用：在不修改 Loader 和第三方 Demo 包的前提下，将 ET.WuXia.Editor 加入宿主 ET.sln。
    // 原因：ET 的 LinkSln 会把 Demo 方案硬链接到根目录，业务包必须先生成宿主私有副本再做增量适配。
    public static class WuXiaRiderSolutionSynchronizer
    {
        private const string SolutionFileName = "ET.sln";
        private const string ProjectFileName = "ET.WuXia.Editor.csproj";
        private const string ProjectEntryMarker = "\"ET.WuXia.Editor\", \"ET.WuXia.Editor.csproj\"";
        private const string AuditComment = "# 【武侠包接入 2026-08-18】由 cn.etetet.wuxia 初始化器加入 ET.WuXia.Editor。作用：在 Rider 中编辑和编译 WuXia 包内编辑器代码。原因：宿主 ET.sln 来源早于 WuXia 包，未包含该程序集。";

        [MenuItem("ET/WuXia/Sync Rider Solution")]
        public static void SyncFromMenu()
        {
            try
            {
                bool changed = Sync();
                string message = changed ? "Rider 解决方案同步完成。" : "Rider 解决方案已经包含 WuXia，无需修改。";
                Debug.Log(message);
                EditorUtility.DisplayDialog("WuXia Rider 方案", message, "确定");
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
                EditorUtility.DisplayDialog("WuXia Rider 方案", "同步失败，请根据 Console 错误处理后重试。", "确定");
            }
        }

        public static bool Sync()
        {
            string projectRoot = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
            string solutionPath = Path.Combine(projectRoot, SolutionFileName);

            if (!File.Exists(solutionPath))
            {
                throw new FileNotFoundException("没有找到根 ET.sln，请先执行 ET -> StateSync -> Init。", solutionPath);
            }

            // 【武侠包建设 2026-08-18】先调用 Unity 官方工程同步入口生成最新 csproj。
            // 作用：确保 ET.WuXia.Editor.csproj 已存在并包含包内全部 Editor 源码。
            // 原因：asmdef 由 Unity 转换为 csproj，WuXia 不应自行维护生成工程的源码清单。
            Unity.CodeEditor.CodeEditor.CurrentEditor.SyncAll();

            string projectPath = Path.Combine(projectRoot, ProjectFileName);
            if (!File.Exists(projectPath))
            {
                throw new FileNotFoundException("Unity 尚未生成 ET.WuXia.Editor.csproj，请等待资源刷新后重试。", projectPath);
            }

            string content = File.ReadAllText(solutionPath, Encoding.UTF8);
            if (content.IndexOf(ProjectEntryMarker, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return false;
            }

            CreatePrivateHostSolution(solutionPath, projectRoot);
            AddProjectWithDotNet(projectRoot);
            EnsureAuditComment(solutionPath);

            content = File.ReadAllText(solutionPath, Encoding.UTF8);
            if (content.IndexOf(ProjectEntryMarker, StringComparison.OrdinalIgnoreCase) < 0)
            {
                throw new InvalidOperationException("dotnet 已结束，但 ET.sln 中仍未找到 ET.WuXia.Editor。请检查本机 .NET SDK。");
            }

            Debug.Log("WuXia 已将 ET.WuXia.Editor 加入根 ET.sln；Loader 与第三方 Demo 包未被修改。");
            return true;
        }

        private static void CreatePrivateHostSolution(string solutionPath, string projectRoot)
        {
            string backupDirectory = Path.Combine(projectRoot, "Library", "WuXia", "SolutionBackups");
            Directory.CreateDirectory(backupDirectory);

            string timestamp = DateTime.Now.ToString("yyyyMMdd-HHmmss");
            string backupPath = Path.Combine(backupDirectory, $"ET.sln.before-wuxia-{timestamp}-{Guid.NewGuid():N}.bak");
            string temporaryPath = Path.Combine(backupDirectory, $"ET.sln.wuxia-{Guid.NewGuid():N}.tmp");
            File.Copy(solutionPath, temporaryPath, true);

            // 【武侠包建设 2026-08-18】替换根目录项前把原方案完整保存在 Library/WuXia/SolutionBackups。
            // 作用：解除根 ET.sln 与 Demo 包内 ET.sln 的潜在硬链接，只修改宿主副本且保留可恢复原件。
            // 原因：直接写入硬链接会反向改动 cn.etetet.statesync 或 cn.etetet.yiuistatesync 的包内文件。
            File.Move(solutionPath, backupPath);
            try
            {
                File.Move(temporaryPath, solutionPath);
            }
            catch
            {
                File.Move(backupPath, solutionPath);
                throw;
            }
        }

        private static void AddProjectWithDotNet(string projectRoot)
        {
            ProcessStartInfo startInfo = new()
            {
                FileName = "dotnet",
                Arguments = $"sln {SolutionFileName} add {ProjectFileName} --solution-folder Unity",
                WorkingDirectory = projectRoot,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            };

            using Process process = Process.Start(startInfo) ?? throw new InvalidOperationException("无法启动 dotnet，请检查 .NET SDK。");
            if (!process.WaitForExit(60000))
            {
                process.Kill();
                throw new TimeoutException("dotnet sln add 执行超过 60 秒，已终止本次同步。");
            }

            string standardOutput = process.StandardOutput.ReadToEnd();
            string standardError = process.StandardError.ReadToEnd();

            if (process.ExitCode != 0)
            {
                throw new InvalidOperationException($"dotnet sln add 执行失败：{standardError}{Environment.NewLine}{standardOutput}");
            }

            if (!string.IsNullOrWhiteSpace(standardOutput))
            {
                Debug.Log(standardOutput.Trim());
            }
        }

        private static void EnsureAuditComment(string solutionPath)
        {
            string content = File.ReadAllText(solutionPath, Encoding.UTF8);
            if (content.IndexOf(AuditComment, StringComparison.Ordinal) >= 0)
            {
                return;
            }

            int markerIndex = content.IndexOf(ProjectEntryMarker, StringComparison.OrdinalIgnoreCase);
            if (markerIndex < 0)
            {
                throw new InvalidDataException("ET.sln 缺少 ET.WuXia.Editor 项目声明，无法写入接入说明。");
            }

            int lineStart = content.LastIndexOf('\n', markerIndex);
            lineStart = lineStart < 0 ? 0 : lineStart + 1;
            string newLine = content.IndexOf("\r\n", StringComparison.Ordinal) >= 0 ? "\r\n" : "\n";
            content = content.Insert(lineStart, AuditComment + newLine);
            File.WriteAllText(solutionPath, content, new UTF8Encoding(false));
        }
    }
}
