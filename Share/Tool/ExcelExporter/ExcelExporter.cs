using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace ET
{
    public static class ExcelExporter
    {
        /// <summary>
        /// 配置代码生成父目录
        /// </summary>
        const string GeneratedCodeBaseDir = "../Unity/Assets/Scripts/Model/Generate";

        /// <summary>
        /// 客户端配置代码生成目录
        /// </summary>
        const string ClientGeneratedCodeDir = $"{GeneratedCodeBaseDir}/Client/Config";

        /// <summary>
        /// 服务端配置代码生成目录
        /// (服务端因为机器人的存在必须包含客户端所有配置，所以单独的c字段没有意义，单独的c就表示cs，所以Server目录等价于ClientServer目录)
        /// </summary>
        const string ServerGeneratedCodeDir = $"{GeneratedCodeBaseDir}/Server/Config";

        /// <summary>
        /// 服务端启动配置代码生成目录
        /// </summary>
        const string ServerStartConfigGeneratedCodeDir = $"{GeneratedCodeBaseDir}/Server/Config/StartConfig";

        /// <summary>
        /// 服务端启动配置代码meta文件临时目录
        /// </summary>
        const string ServerStartConfigTempMetaDir = "../Temp/ServerStartConfigTempMeta";

        /// <summary>
        /// 双端配置代码生成目录
        /// </summary>
        const string ClientServerGeneratedCodeDir = $"{GeneratedCodeBaseDir}/ClientServer/Config";

        /// <summary>
        /// 客户端配置代码生成目录
        /// </summary>
        const string ClientGeneratedBytesDir = "../Config/Excel/c";

        /// <summary>
        /// 客户端Unity配置加载目录
        /// </summary>
        const string UnityClientBytesDir = "../Unity/Assets/Bundles/Config";

        /// <summary>
        /// 服务端配置二进制数据生成目录
        /// </summary>
        const string ServerGeneratedBytesDir = "../Config/Excel/s";

        /// <summary>
        /// 双端配置二进制数据生成目录
        /// </summary>
        const string ClientServerGeneratedBytesDir = "../Config/Excel/cs";

        /// <summary>
        /// 服务端配置Json数据生成目录
        /// </summary>
        const string ServerGeneratedJsonDir = "../Config/Json/s";

        /// <summary>
        /// 双端配置Json数据生成目录
        /// </summary>
        const string ClientServerGeneratedJsonDir = "../Config/Json/cs";

        public static void Export()
        {
            // 备份StartConfig的旧meta, 以免版本管理软件出现文件变化
            CopyDirectoryMetaFiles(ServerStartConfigGeneratedCodeDir, ServerStartConfigTempMetaDir);

            string shellFilePath = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? ".\\GenConfig.bat" : "./GenConfig.sh";
            Process configProcess = CreateProcess(shellFilePath, "../Tools/Luban/");

            try
            {
                configProcess.Start();
                configProcess.WaitForExit();

                // 覆盖新文件
                CopyClientBytesToUnity();
                CopyServerCodesToClientServerCodesInUnity();
                CopyServerDataToClientServerData();

                // 还原StartConfig的旧Meta, 并删除临时文件夹
                CopyDirectoryMetaFiles(ServerStartConfigTempMetaDir, ServerStartConfigGeneratedCodeDir);
                if (Directory.Exists(ServerStartConfigTempMetaDir))
                {
                    Directory.Delete(ServerStartConfigTempMetaDir, true);
                }

                // 清除无用Meta
                RemoveUnusedMetaFiles(UnityClientBytesDir);
                RemoveUnusedMetaFiles(ClientGeneratedCodeDir);
                RemoveUnusedMetaFiles(ServerGeneratedCodeDir);
                RemoveUnusedMetaFiles(ClientServerGeneratedCodeDir);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                configProcess.Close();
            }
        }

        /// <summary>
        /// 将客户端生成文件复制到Unity目录
        /// </summary>
        static void CopyClientBytesToUnity()
        {
            RemoveAllFilesExceptMeta(UnityClientBytesDir);
            FileHelper.CopyDirectory(ClientGeneratedBytesDir, UnityClientBytesDir);
        }

        /// <summary>
        /// 将服务端生成代码复制到双端生成代码目录
        /// </summary>
        static void CopyServerCodesToClientServerCodesInUnity()
        {
            if (!Directory.Exists(ServerGeneratedCodeDir))
            {
                return;
            }

            RemoveAllFilesExceptMeta(ClientServerGeneratedCodeDir);
            CopyDirectoryExceptMeta(ServerGeneratedCodeDir, ClientServerGeneratedCodeDir);
        }

        /// <summary>
        /// 从服务器目录复制生成数据文件到双端目录
        /// </summary>
        static void CopyServerDataToClientServerData()
        {
            if (Directory.Exists(ClientServerGeneratedBytesDir))
                Directory.Delete(ClientServerGeneratedBytesDir, true);

            if (Directory.Exists(ClientServerGeneratedJsonDir))
                Directory.Delete(ClientServerGeneratedJsonDir, true);

            FileHelper.CopyDirectory(ServerGeneratedBytesDir, ClientServerGeneratedBytesDir);
            FileHelper.CopyDirectory(ServerGeneratedJsonDir, ClientServerGeneratedJsonDir);
        }

        /// <summary>
        /// 复制所有meta文件到指定目录
        /// </summary>
        static void CopyDirectoryMetaFiles(string srcDir, string tgtDir)
        {
            DirectoryInfo source = new(srcDir);
            DirectoryInfo target = new(tgtDir);

            if (target.FullName.StartsWith(source.FullName, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new Exception("父目录不能拷贝到子目录！");
            }

            if (!source.Exists)
            {
                return;
            }

            if (!target.Exists)
            {
                target.Create();
            }

            FileInfo[] files = source.GetFiles();
            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].Name.EndsWith(".meta"))
                {
                    File.Copy(files[i].FullName, Path.Combine(target.FullName, files[i].Name), true);
                }
            }

            DirectoryInfo[] dirs = source.GetDirectories();
            for (int j = 0; j < dirs.Length; j++)
            {
                CopyDirectoryMetaFiles(dirs[j].FullName, Path.Combine(target.FullName, dirs[j].Name));
            }
        }

        /// <summary>
        /// 复制除meta文件以外的所有文件到另一个目录
        /// </summary>
        static void CopyDirectoryExceptMeta(string srcDir, string tgtDir)
        {
            DirectoryInfo source = new(srcDir);
            DirectoryInfo target = new(tgtDir);

            if (target.FullName.StartsWith(source.FullName, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new Exception("父目录不能拷贝到子目录！");
            }

            if (!source.Exists)
            {
                return;
            }

            if (!target.Exists)
            {
                target.Create();
            }

            FileInfo[] files = source.GetFiles();
            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].Name.EndsWith(".meta"))
                {
                    continue;
                }

                File.Copy(files[i].FullName, Path.Combine(target.FullName, files[i].Name), true);
            }

            DirectoryInfo[] dirs = source.GetDirectories();
            for (int j = 0; j < dirs.Length; j++)
            {
                CopyDirectoryExceptMeta(dirs[j].FullName, Path.Combine(target.FullName, dirs[j].Name));
            }
        }

        /// <summary>
        /// 删除meta以外的所有文件
        /// </summary>
        static void RemoveAllFilesExceptMeta(string directory)
        {
            if (!Directory.Exists(directory))
            {
                return;
            }

            DirectoryInfo targetDir = new(directory);
            FileInfo[] fileInfos = targetDir.GetFiles("*", SearchOption.AllDirectories);
            foreach (FileInfo info in fileInfos)
            {
                if (!info.Name.EndsWith(".meta"))
                {
                    File.Delete(info.FullName);
                }
            }
        }

        /// <summary>
        /// 删除多余的meta文件
        /// </summary>
        static void RemoveUnusedMetaFiles(string directory)
        {
            DirectoryInfo targetDir = new(directory);
            FileInfo[] fileInfos = targetDir.GetFiles("*.meta", SearchOption.AllDirectories);
            foreach (FileInfo info in fileInfos)
            {
                string pathWithoutMeta = info.FullName.Remove(info.FullName.LastIndexOf(".meta", StringComparison.Ordinal));
                if (!File.Exists(pathWithoutMeta) && !Directory.Exists(pathWithoutMeta))
                {
                    File.Delete(info.FullName);
                }
            }
        }

        static Process CreateProcess(string cmd, string workDirectory)
        {
            Process process = new();
            string app = "cmd.exe";
            string arguments = "/c";
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                app = "bash";
                arguments = "-c";
            }

            process.StartInfo = new ProcessStartInfo(app)
            {
                Arguments = $"{arguments} \"{cmd}\"",
                CreateNoWindow = false,
                UseShellExecute = true,
                WorkingDirectory = workDirectory
            };
            return process;
        }
    }
}