#if !UNITY_EDITOR && UNITY_ANDROID
using System.Collections.Generic;
#endif
using System.IO;
using UnityEngine;

namespace YIUIFramework
{
    public static class StreamingAssets
    {
        #if !UNITY_EDITOR && UNITY_ANDROID
        private static AndroidJavaObject assetManager;
        private static Dictionary<string, HashSet<string>> folderLookup =
            new Dictionary<string, HashSet<string>>();

        static StreamingAssets()
        {
            var unityPlayerClass = new AndroidJavaClass(
                "com.unity3d.player.UnityPlayer");
            if (unityPlayerClass == null)
            {
                Debug.LogError(
                    "Can not find android Java class: " +
                    "com.unity3d.player.UnityPlayer");
                return;
            }

            var currentActivity = unityPlayerClass.GetStatic<AndroidJavaObject>(
                "currentActivity");
            if (currentActivity == null)
            {
                Debug.LogError("Can not get method: currentActivity");
                return;
            }

            assetManager = currentActivity.Call<AndroidJavaObject>("getAssets");
            if (assetManager == null)
            {
                Debug.LogError("Can not get assetManager.");
                return;
            }
        }
        #endif

        /// <summary>
        ///从流资源中指定文件名读取文件中的所有文本
        /// </summary>
        public static string ReadAllText(string filePath)
        {
            #if !UNITY_EDITOR && UNITY_ANDROID
            var inputStream = assetManager.Call<AndroidJavaObject>(
                "open", filePath);

            var length = inputStream.Call<int>("available");
            var jniBytes = AndroidJNI.NewByteArray(length);
            var clsPtr = inputStream.GetRawClass(); // AndroidJNI.FindClass("java.io.InputStream");
            var METHOD_read = AndroidJNIHelper.GetMethodID(clsPtr, "read", "([B)I");
            length = AndroidJNI.CallIntMethod(
                inputStream.GetRawObject(),
                METHOD_read,
                new[] { new jvalue() { l = jniBytes } });
            var bytes = AndroidJNI.FromByteArray(jniBytes);

            AndroidJNI.DeleteLocalRef(jniBytes);
            inputStream.Call("close");
            inputStream.Dispose();
            return System.Text.Encoding.ASCII.GetString(bytes, 0, length);
            #else
            var path = Path.Combine(Application.streamingAssetsPath, filePath);
            if (File.Exists(path))
            {
                return File.ReadAllText(path);
            }

            Debug.LogError(string.Format("<color=red>  没有读取到 :{0}</color>", path));
            return string.Empty;
            #endif
        }

        /// <summary>
        /// 检查文件夹中是否存在文件。
        /// </summary>
        public static bool Existed(string filePath)
        {
            #if !UNITY_EDITOR && UNITY_ANDROID
            var fileDir = Path.GetDirectoryName(filePath);
            var fileName = Path.GetFileName(filePath);
            var fileTable = GetFilesInDir(fileDir);
            return fileTable.Contains(fileName);
            #else
            var path = Path.Combine(Application.streamingAssetsPath, filePath);
            return File.Exists(path);
            #endif
        }

        #if !UNITY_EDITOR && UNITY_ANDROID
        private static HashSet<string> GetFilesInDir(string dir)
        {
            HashSet<string> fileTable;
            if (!folderLookup.TryGetValue(dir, out fileTable))
            {
                fileTable = new HashSet<string>();
                var files = assetManager.Call<string[]>("list", dir);
                foreach (var file in files)
                {
                    fileTable.Add(file);
                }

                folderLookup.Add(dir, fileTable);
            }

            return fileTable;
        }
        #endif
    }
}
