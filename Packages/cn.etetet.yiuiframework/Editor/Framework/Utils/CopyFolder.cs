using System.IO;
using UnityEditor;

namespace YIUIFramework.Editor
{
    public static class CopyFolder
    {
        public static void Copy(string sourceFolder, string targetFolder)
        {
            DirectoryInfo sourceDir = new(sourceFolder);
            DirectoryInfo targetDir = new(targetFolder);

            if (Directory.Exists(targetFolder))
            {
                UnityTipsHelper.CallBack($"{targetFolder} 已存在!!!\n \n警告将会执行全覆盖操作!!!", () =>
                {
                    Directory.Delete(targetFolder, true);
                    Start();
                });
            }
            else
            {
                Start();
            }

            return;

            void Start()
            {
                Directory.CreateDirectory(targetFolder);
                CopyAll(sourceDir, targetDir);
                UnityTipsHelper.Show("创建完毕");
                EditorApplication.ExecuteMenuItem("ET/Loader/ReGenerateProjectAssemblyReference");
                YIUIAutoTool.CloseWindowRefresh();
            }
        }

        private static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            if (!target.Exists)
            {
                target.Create();
            }

            foreach (FileInfo fi in source.GetFiles())
            {
                string targetFilePath = Path.Combine(target.FullName, fi.Name);
                fi.CopyTo(targetFilePath, true);
            }

            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir = target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }
    }
}