using System;
using System.Collections;
using Unity.EditorCoroutines.Editor;
using UnityEngine;
using UnityEditor;

namespace YIUIFramework.Editor
{
    /// <summary>
    /// Unity提示框@l
    /// </summary>
    public static class UnityEditorTipsHelper
    {
        /// <summary>
        /// 显示自定义时长的编辑器通知
        /// </summary>
        public static void ShowNotification(string content, float showSeconds = 2f)
        {
            var mainWindow = EditorWindow.GetWindow(typeof(EditorWindow));
            mainWindow.ShowNotification(new GUIContent(content));
            EditorCoroutineUtility.StartCoroutine(RemoveAfterDelay(showSeconds, mainWindow), null);
        }

        static IEnumerator RemoveAfterDelay(float delay, EditorWindow window)
        {
            yield return new WaitForSeconds(delay);
            if (window != null)
            {
                window.RemoveNotification();
            }
        }
    }
}