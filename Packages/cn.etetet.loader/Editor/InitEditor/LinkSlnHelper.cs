using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEditor;

namespace ET
{
    // 【框架适配 2026-08-18】保持 Loader 的 LinkSln 只负责通用 ET 方案链接。
    // 作用：让具体业务包在自身 Editor 初始化器中完成 Rider 方案增量适配。
    // 原因：基础 Loader 不应反向引用或识别 cn.etetet.wuxia 等项目业务包。
    public static class LinkSlnHelper
    {
        [MenuItem("ET/Loader/LinkSln")]
        public static void Run()
        {
            string etslnPath = Path.Combine(Directory.GetCurrentDirectory(), "ET.sln");
            if (File.Exists(etslnPath))
            {
                File.Delete(etslnPath);
            }
            
            List<string> slns = new List<string>();
            FileHelper.GetAllFiles(slns, "./Packages", "ET.sln");

            if (slns.Count == 0)
            {
                throw new Exception("not found ET.sln in et packages!");
            }
            
            Process process = ProcessHelper.PowerShell($"-c New-Item -ItemType HardLink -Target {slns[0]} ./ET.sln", waitExit: true);
            UnityEngine.Debug.Log(process.StandardOutput.ReadToEnd());
            
            
            // link xml
            string xmlFile = Path.Combine(Path.GetDirectoryName(slns[0]), "link.xml");
            
            if (File.Exists(xmlFile))
            {
                UnityEngine.Debug.LogWarning("not found link.xml !!!!");
                Process process2 = ProcessHelper.PowerShell($"-c New-Item -ItemType HardLink -Target {xmlFile} ./Assets/link.xml", waitExit: true);
                UnityEngine.Debug.Log(process2.StandardOutput.ReadToEnd());
            }
        }
    }
}
