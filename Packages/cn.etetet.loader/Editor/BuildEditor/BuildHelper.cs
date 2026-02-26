using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace ET
{
    public static class BuildHelper
    {
        private const string relativeDirPrefix = "./Release";

        public static string BuildFolder = "./Release/{0}/StreamingAssets/";

#if ENABLE_VIEW
        [MenuItem("ET/Loader/Remove ENABLE_VIEW", false, ETMenuItemPriority.ChangeDefine)]
        public static void RemoveEnableView()
        {
            DefineHelper.EnableDefineSymbols("ENABLE_VIEW", false);
        }
#else
        [MenuItem("ET/Loader/Add ENABLE_VIEW", false, ETMenuItemPriority.ChangeDefine)]
        public static void AddEnableView()
        {
            DefineHelper.EnableDefineSymbols("ENABLE_VIEW", true);
        }
#endif

        public static void Build(PlatformType type, BuildOptions buildOptions)
        {
            BuildTarget buildTarget = BuildTarget.StandaloneWindows;
            string programName = "ET";
            string exeName = programName;
            switch (type)
            {
                case PlatformType.Windows:
                    buildTarget = BuildTarget.StandaloneWindows64;
                    exeName += ".exe";
                    break;
                case PlatformType.Android:
                    buildTarget = BuildTarget.Android;
                    exeName += ".apk";
                    break;
                case PlatformType.IOS:
                    buildTarget = BuildTarget.iOS;
                    break;
                case PlatformType.MacOS:
                    buildTarget = BuildTarget.StandaloneOSX;
                    break;
                case PlatformType.Linux:
                    buildTarget = BuildTarget.StandaloneLinux64;
                    break;
                case PlatformType.WebGL:
                    buildTarget = BuildTarget.WebGL;
                    break;
            }

            AssetDatabase.Refresh();

            Debug.Log("start build");

            string[] levels = { "Packages/cn.etetet.loader/Scenes/Init.unity" };
            BuildReport report = BuildPipeline.BuildPlayer(levels, $"{relativeDirPrefix}/{exeName}", buildTarget, buildOptions);
            if (report.summary.result != BuildResult.Succeeded)
            {
                Debug.Log($"BuildResult:{report.summary.result}");
                return;
            }

            Debug.Log("finish build");
            EditorUtility.OpenWithDefaultApp(relativeDirPrefix);
        }
    }
}