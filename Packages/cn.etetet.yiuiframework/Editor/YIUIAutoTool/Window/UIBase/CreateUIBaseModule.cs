#if UNITY_EDITOR
using System.IO;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace YIUIFramework.Editor
{
    /// <summary>
    /// UIBase 模块
    /// </summary>
    [HideReferenceObjectPicker]
    public class CreateUIBaseModule : BaseCreateModule
    {
        [LabelText("YIUI项目命名空间")]
        [ShowInInspector]
        [ReadOnly]
        private readonly string UINamespace = YIUIConstHelper.Const.UINamespace;

        [LabelText("YIUI项目资源路径")]
        [FolderPath]
        [ShowInInspector]
        [ReadOnly]
        private readonly string UIProjectResPath = YIUIConstHelper.Const.UIProjectResPath;

        [LabelText("YIUI项目脚本路径")]
        [FolderPath]
        [ShowInInspector]
        [ReadOnly]
        private readonly string UIGenerationPath = YIUIConstHelper.Const.UIETComponentGenPath;

        [LabelText("YIUI项目自定义脚本路径")]
        [FolderPath]
        [ShowInInspector]
        [ReadOnly]
        private readonly string UICodeScriptsPath = YIUIConstHelper.Const.UIETSystemGenPath;

        [HideLabel]
        [ShowInInspector]
        private CreateUIBaseEditorData UIBaseData = new CreateUIBaseEditorData();

        public override void Initialize()
        {
        }

        public override void OnDestroy()
        {
        }

        private const string m_CommonPkg = "Common";

        [Button("初始化项目")]
        private void CreateProject()
        {
            if (!UIOperationHelper.CheckUIOperation()) return;

            CopyYIUIPackage();
            EditorHelper.CreateExistsDirectory(UIProjectResPath);
            UICreateResModule.Create(m_CommonPkg); //默认初始化一个common模块
            CopyUIRoot();
            YIUIAutoTool.CloseWindowRefresh();
        }

        private void CopyUIRoot()
        {
            //如果有安装yiuistatesync包 就不需要这个root了
            if (Directory.Exists($"{Application.dataPath}/../Packages/cn.etetet.yiuistatesync"))
            {
                return;
            }

            var loadRoot = (GameObject)AssetDatabase.LoadAssetAtPath($"{YIUIConstHelper.Const.UIRootPrefabPath}", typeof(Object));
            if (loadRoot == null)
            {
                Debug.LogError($"没有找到原始UIRoot {YIUIConstHelper.Const.UIRootPrefabPath}");
                return;
            }

            var newGameObj = Object.Instantiate(loadRoot);
            var commonPath = $"{UIProjectResPath}/{m_CommonPkg}/{YIUIConstHelper.Const.UIPrefabs}/{YIUIConstHelper.Const.UIRootName}.prefab";
            PrefabUtility.SaveAsPrefabAsset(newGameObj, commonPath);
            Object.DestroyImmediate(newGameObj);
        }

        private void CopyYIUIPackage()
        {
            var PackagesPath      = $"{Application.dataPath}/../Packages";
            var sourceFolder      = $"{PackagesPath}/cn.etetet.yiuiframework/.Template/cn.etetet.yiui";
            var targetPackagePath = $"{PackagesPath}/cn.etetet.yiui";
            CopyFolder.Copy(sourceFolder, targetPackagePath);
        }
    }
}
#endif