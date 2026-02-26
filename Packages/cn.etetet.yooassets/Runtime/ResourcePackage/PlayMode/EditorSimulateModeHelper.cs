
namespace YooAsset
{
    public class EditorSimulateModeHelper
    {
        public static PackageInvokeBuildResult SimulateBuild(string packageName)
        {
            var buildParam = new PackageInvokeBuildParam(packageName);
            buildParam.BuildPipelineName = "EditorSimulateBuildPipeline";
            buildParam.InvokeAssmeblyName = "ET.YooAssets.Editor";
            buildParam.InvokeClassFullName = "YooAsset.Editor.AssetBundleSimulateBuilder";
            buildParam.InvokeMethodName = "SimulateBuild";
            return PackageInvokeBuilder.InvokeBuilder(buildParam);
        }
    }
}