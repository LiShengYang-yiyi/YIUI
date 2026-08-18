using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace ET.WuXia.Editor
{
    // 【武侠迁移 2026-08-18】检查第三方包更新是否意外重新启用 Demo 或旧 UI 所有权。
    // 作用：在编辑器中尽早发现资源地址、启动事件和根节点引用冲突。
    // 原因：第三方包升级可能覆盖本地配置，导致 Login、Lobby、Main 被重复创建。
    public static class WuXiaYIUIMigrationValidator
    {
        // 【武侠包建设 2026-08-18】原 WuXiaRootGuid 常量随强制场景引用校验一并停用并保留记录。
        // private const string WuXiaRootGuid = "a6d5a2e738b64d649af948b1df924fed";
        private const string DemoRootGuid = "4200f826ed726d4478d4d19000af33c4";
        private const string OriginalLoginSourceGuid = "1c9381f9316dc9b4d8199232fc2c9190";
        private const string WuXiaLoginSourceGuid = "5d7d9dfe194f4f41ab910c467915abc1";
        private const string DemoGuard = "#if YIUI_STATESYNC_DEMO";

        private static readonly string[] ExpectedFiles =
        {
            "Packages/cn.etetet.wuxia/Assets/GameRes/YIUI/Common/Prefabs/YIUIRoot.prefab",
            "Packages/cn.etetet.wuxia/Assets/GameRes/YIUI/Login/Prefabs/WuXiaLoginPanel.prefab",
            "Packages/cn.etetet.wuxia/Assets/GameRes/YIUI/Lobby/Prefabs/WuXiaLobbyPanel.prefab",
            "Packages/cn.etetet.wuxia/Assets/GameRes/YIUI/Main/Prefabs/WuXiaMainPanel.prefab",
            "Packages/cn.etetet.wuxia/Scripts/HotfixView/Client/Entry/WuXiaEntryEvent3_InitClient.cs",
            "Packages/cn.etetet.wuxia/Settings/WuXiaYooAssetSetting.asset",
            "Packages/cn.etetet.wuxia/Settings/Templates/YIUIConstAsset.txt",
            "Packages/cn.etetet.wuxia/Settings/Templates/YIUIAtlasData.asset",
            "Assets/GameRes/YIUI/YIUISettings/YIUIConstAsset.txt",
            "Assets/GameRes/YIUI/YIUISettings/YIUIAtlasData.asset",
        };

        private static readonly string[] DemoEventFiles =
        {
            "Packages/cn.etetet.yiuistatesync/Scripts/HotfixView/Client/EntryEvent3_InitClient.cs",
            "Packages/cn.etetet.yiuistatesync/Scripts/HotfixView/Client/UI/UILogin/AppStartInitFinish_CreateLoginUI.cs",
            "Packages/cn.etetet.yiuistatesync/Scripts/HotfixView/Client/UI/UILogin/LoginFinish_RemoveLoginUI.cs",
            "Packages/cn.etetet.yiuistatesync/Scripts/HotfixView/Client/UI/UILobby/LoginFinish_CreateLobbyUI.cs",
            "Packages/cn.etetet.yiuistatesync/Scripts/HotfixView/Client/UI/UIHelp/SceneChangeFinishEvent_CreateUIHelp.cs",
        };

        private static readonly string[] WuXiaEventFiles =
        {
            "Packages/cn.etetet.wuxia/Scripts/HotfixView/Client/Entry/WuXiaEntryEvent3_InitClient.cs",
            "Packages/cn.etetet.wuxia/Scripts/HotfixView/Client/UI/UILogin/WuXiaAppStartInitFinish_CreateLoginUI.cs",
            "Packages/cn.etetet.wuxia/Scripts/HotfixView/Client/UI/UILogin/WuXiaLoginFinish_RemoveLoginUI.cs",
            "Packages/cn.etetet.wuxia/Scripts/HotfixView/Client/UI/UILobby/WuXiaLoginFinish_CreateLobbyUI.cs",
            "Packages/cn.etetet.wuxia/Scripts/HotfixView/Client/UI/UIMain/WuXiaSceneChangeFinish_CreateMainUI.cs",
        };

        private static readonly string[] LegacyStateSyncUiFiles =
        {
            "Packages/cn.etetet.statesync/Scripts/HotfixView/Client/EntryEvent3_InitClient.cs",
            "Packages/cn.etetet.statesync/Scripts/HotfixView/Client/UI/UILogin/AppStartInitFinish_CreateLoginUI.cs",
            "Packages/cn.etetet.statesync/Scripts/HotfixView/Client/UI/UILogin/LoginFinish_RemoveLoginUI.cs",
            "Packages/cn.etetet.statesync/Scripts/HotfixView/Client/UI/UILobby/LoginFinish_CreateLobbyUI.cs",
            "Packages/cn.etetet.statesync/Scripts/HotfixView/Client/UI/UIHelp/SceneChangeFinishEvent_CreateUIHelp.cs",
        };

        [InitializeOnLoadMethod]
        private static void ScheduleValidation()
        {
            EditorApplication.delayCall += ValidateOnLoad;
        }

        [MenuItem("ET/WuXia/Validate YIUI Migration")]
        private static void ValidateFromMenu()
        {
            List<string> errors = CollectErrors();
            if (errors.Count == 0)
            {
                EditorUtility.DisplayDialog("WuXia YIUI", "迁移校验通过。", "确定");
                return;
            }

            string message = string.Join(Environment.NewLine, errors);
            Debug.LogError($"WuXia YIUI 迁移校验失败：{Environment.NewLine}{message}");
            EditorUtility.DisplayDialog("WuXia YIUI", message, "确定");
        }

        private static void ValidateOnLoad()
        {
            List<string> errors = CollectErrors();
            if (errors.Count > 0)
            {
                Debug.LogError(
                    $"WuXia YIUI 迁移校验失败：{Environment.NewLine}" +
                    string.Join(Environment.NewLine, errors));
            }
        }

        internal static List<string> CollectErrors()
        {
            List<string> errors = new();
            foreach (string expectedFile in ExpectedFiles)
            {
                if (!File.Exists(AbsolutePath(expectedFile)))
                {
                    errors.Add($"缺少 WuXia 文件：{expectedFile}");
                }
            }

            string initScene = Read("Packages/cn.etetet.loader/Scenes/Init.unity");
            /*
             * 【武侠包建设 2026-08-18】停用“Loader Init 必须静态引用 WuXia YIUIRoot”的旧校验。
             * 作用：允许干净项目仅导入 cn.etetet.wuxia，不需要修改 Loader 场景即可启动。
             * 原因：YIUIMgrComponent.InitRoot 在场景找不到根节点时会按 Common/YIUIRoot 地址动态加载。
             * 原校验保留如下，便于理解既有项目为什么曾经修改 Init 场景：
             * if (!ContainsActiveYamlToken(initScene, WuXiaRootGuid))
             * {
             *     errors.Add("Init.unity 没有引用 WuXia YIUIRoot。");
             * }
             */

            if (ContainsActiveYamlToken(initScene, DemoRootGuid))
            {
                errors.Add("Init.unity 仍在活动引用 Demo YIUIRoot，会抢先于 WuXia 的按地址加载。");
            }

            string collector = Read("Packages/cn.etetet.statesync/Settings/AssetBundleCollectorSetting.asset");
            if (!IsCollectorEnabled(collector, "Packages/cn.etetet.wuxia/Assets/GameRes/YIUI"))
            {
                errors.Add("缺少已启用的 WuXia YIUI YooAsset 收集器。");
            }

            if (IsCollectorEnabled(collector, "Packages/cn.etetet.yiuistatesync/Assets/GameRes/YIUI"))
            {
                errors.Add("Demo YIUI YooAsset 收集器仍处于启用状态。");
            }

            if (!IsCollectorEnabled(collector, "Assets/GameRes/YIUI/YIUISettings"))
            {
                errors.Add("YIUISettings 收集器缺失或已停用。");
            }

            // 【武侠迁移 2026-08-18】校验原登录 Source Prefab 与 WuXia Source Prefab 的 GUID 所有权。
            // 作用：阻止 Unity 因 GUID 冲突自动改写原资源，确保原文件可稳定回滚。
            // 原因：WuXiaLoginPanelSource 最初复制了原 meta，导致两个 Prefab 使用同一 GUID。
            string originalLoginSourceMeta = Read(
                "Packages/cn.etetet.wuxia/Assets/GameRes/YIUI/Login/Source/LoginPanelSource.prefab.meta");
            string wuXiaLoginSourceMeta = Read(
                "Packages/cn.etetet.wuxia/Assets/GameRes/YIUI/Login/Source/WuXiaLoginPanelSource.prefab.meta");
            if (!originalLoginSourceMeta.Contains(OriginalLoginSourceGuid, StringComparison.Ordinal))
            {
                errors.Add("原 LoginPanelSource GUID 已发生变化。");
            }

            if (!wuXiaLoginSourceMeta.Contains(WuXiaLoginSourceGuid, StringComparison.Ordinal))
            {
                errors.Add("WuXiaLoginPanelSource GUID 缺失或不正确。");
            }

            foreach (string demoEventFile in DemoEventFiles)
            {
                if (!File.Exists(AbsolutePath(demoEventFile)))
                {
                    continue;
                }

                if (!Read(demoEventFile).TrimStart().StartsWith(DemoGuard, StringComparison.Ordinal))
                {
                    errors.Add($"Demo 事件类没有完整的条件编译保护：{demoEventFile}");
                }
            }

            foreach (string wuXiaEventFile in WuXiaEventFiles)
            {
                if (!Read(wuXiaEventFile).TrimStart().StartsWith("#if !YIUI_STATESYNC_DEMO", StringComparison.Ordinal))
                {
                    errors.Add($"WuXia 事件类没有完整的条件编译保护：{wuXiaEventFile}");
                }
            }

            foreach (string legacyFile in LegacyStateSyncUiFiles)
            {
                string legacyContent = Read(legacyFile).TrimStart();
                bool fullyBlockCommented =
                    legacyContent.StartsWith("/*", StringComparison.Ordinal) &&
                    legacyContent.TrimEnd().EndsWith("*/", StringComparison.Ordinal);
                if (!fullyBlockCommented &&
                    !legacyContent.StartsWith("#if WUXIA_LEGACY_STATESYNC_UI", StringComparison.Ordinal))
                {
                    errors.Add($"StateSync 旧 UGUI 处理器仍处于活动状态：{legacyFile}");
                }
            }

            return errors;
        }

        private static string Read(string relativePath)
        {
            string path = AbsolutePath(relativePath);
            return File.Exists(path) ? File.ReadAllText(path) : string.Empty;
        }

        private static bool ContainsActiveYamlToken(string text, string token)
        {
            foreach (string line in text.Split('\n'))
            {
                string trimmed = line.TrimStart();
                if (!trimmed.StartsWith("#", StringComparison.Ordinal) &&
                    trimmed.Contains(token, StringComparison.Ordinal))
                {
                    return true;
                }
            }

            return false;
        }

        // 【武侠迁移 2026-08-18】按 YooAsset 分组状态判断收集器是否真正启用。
        // 作用：允许完整保留 Demo 收集器，同时避免把 DisableGroup 中的回滚配置误判为活动配置。
        // 原因：Unity 序列化资源中的嵌套 YAML 注释会解析失败，必须改用原生 DisableGroup 停用。
        private static bool IsCollectorEnabled(string text, string collectorPath)
        {
            bool groupEnabled = false;
            foreach (string line in text.Split('\n'))
            {
                string trimmed = line.Trim();
                if (trimmed.StartsWith("- GroupName:", StringComparison.Ordinal))
                {
                    groupEnabled = false;
                    continue;
                }

                if (trimmed.StartsWith("ActiveRuleName:", StringComparison.Ordinal))
                {
                    groupEnabled = !trimmed.EndsWith("DisableGroup", StringComparison.Ordinal);
                    continue;
                }

                if (trimmed.StartsWith("- CollectPath:", StringComparison.Ordinal) &&
                    trimmed.EndsWith(collectorPath, StringComparison.Ordinal))
                {
                    return groupEnabled;
                }
            }

            return false;
        }

        private static string AbsolutePath(string relativePath)
        {
            string projectRoot = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
            return Path.Combine(projectRoot, relativePath.Replace('/', Path.DirectorySeparatorChar));
        }
    }
}
