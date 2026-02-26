#if UNITY_EDITOR
using ET;
using System.IO;
using UnityEditorInternal;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace YIUIFramework.Editor
{
    public static class YIUIScriptHelper
    {
        private const string SearchPath = "Packages";

        [StaticField]
        private static string mSearchKey = "";

        [StaticField]
        private static int mSearchIndex;

        public static void OpenScript(string fileName, string searchKey = "")
        {
            if (string.IsNullOrEmpty(fileName))
            {
                UnityEngine.Debug.Log($"无效的脚本名{fileName}");
                return;
            }

            var files = Directory.GetFiles(SearchPath, $"*.cs", SearchOption.AllDirectories);
            var fullName = string.Empty;
            for (int i = 0; i < files.Length; i++)
            {
                var fileInfo = new FileInfo(files[i]);
                if (fileInfo.Name.Equals(fileName + $".cs"))
                {
                    fullName = fileInfo.Directory?.FullName;
                    break;
                }
            }

            if (string.IsNullOrEmpty(fullName))
            {
                UnityEngine.Debug.Log($"找不到{fileName}脚本");
                return;
            }

            var scriptPath = $"{fullName}/{fileName}.cs";
            var lineNumber = 0;
            if (!string.IsNullOrEmpty(searchKey))
            {
                lineNumber = GetOpenFileLine(scriptPath, searchKey);
            }

            InternalEditorUtility.OpenFileAtLineExternal(scriptPath, lineNumber);
        }

        /// <summary>
        /// 搜索bind属性在文件的行数
        /// </summary>
        private static int GetOpenFileLine(string filePath, string keyWord)
        {
            if (string.IsNullOrEmpty(keyWord))
            {
                return 0;
            }

            if (!mSearchKey.Equals(keyWord))
            {
                mSearchKey = keyWord;
                mSearchIndex = 0;
            }

            var fileLines = File.ReadLines(filePath);

            var lines = new List<int>();
            var lineNumber = 0;
            foreach (var fileLine in fileLines)
            {
                lineNumber++;

                var pattern = @"\b" + Regex.Escape(keyWord) + @"\b";
                var matches = Regex.Matches(fileLine, pattern, RegexOptions.None);

                if (matches.Count > 0)
                {
                    lines.Add(lineNumber);
                }
            }

            if (lines.Count <= 0)
            {
                return 0;
            }

            if (mSearchIndex >= lines.Count)
            {
                mSearchIndex = 0;
            }

            return lines[mSearchIndex++];
        }
    }
}
#endif