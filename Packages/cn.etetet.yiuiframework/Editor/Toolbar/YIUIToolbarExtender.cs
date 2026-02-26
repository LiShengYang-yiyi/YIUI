using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace YIUIFramework.Editor
{
    [InitializeOnLoad]
    public static class YIUIToolbarExtender
    {
        static int m_toolCount;
        static GUIStyle m_commandStyle = null;

        private static readonly List<(int order, Action action)> m_LeftToolbarGUI = new List<(int, Action)>();
        private static readonly List<(int order, Action action)> m_RightToolbarGUI = new List<(int, Action)>();

        /// <summary>
        /// 添加左侧工具栏GUI
        /// </summary>
        /// <param name="action">GUI回调</param>
        /// <param name="order">排序优先级，数值越小越靠前 从右往左</param>
        public static void AddLeftToolbarGUI(Action action, int order = 0)
        {
            m_LeftToolbarGUI.Add((order, action));
            m_LeftToolbarGUI.Sort((a, b) => b.order.CompareTo(a.order));
        }

        /// <summary>
        /// 添加右侧工具栏GUI
        /// </summary>
        /// <param name="action">GUI回调</param>
        /// <param name="order">排序优先级，数值越小越靠前 从左往右</param>
        public static void AddRightToolbarGUI(Action action, int order = 0)
        {
            m_RightToolbarGUI.Add((order, action));
            m_RightToolbarGUI.Sort((a, b) => a.order.CompareTo(b.order));
        }

        /// <summary>
        /// 移除左侧工具栏GUI
        /// </summary>
        public static void RemoveLeftToolbarGUI(Action action)
        {
            m_LeftToolbarGUI.RemoveAll(item => item.action == action);
        }

        /// <summary>
        /// 移除右侧工具栏GUI
        /// </summary>
        public static void RemoveRightToolbarGUI(Action action)
        {
            m_RightToolbarGUI.RemoveAll(item => item.action == action);
        }

        static YIUIToolbarExtender()
        {
            Type toolbarType = typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.Toolbar");

            #if UNITY_2019_1_OR_NEWER
            string fieldName = "k_ToolCount";
            #else
			string fieldName = "s_ShownToolIcons";
            #endif

            FieldInfo toolIcons = toolbarType.GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);

            #if UNITY_2019_3_OR_NEWER
            m_toolCount = toolIcons != null ? ((int)toolIcons.GetValue(null)) : 8;
            #elif UNITY_2019_1_OR_NEWER
			m_toolCount = toolIcons != null ? ((int) toolIcons.GetValue(null)) : 7;
            #elif UNITY_2018_1_OR_NEWER
			m_toolCount = toolIcons != null ? ((Array) toolIcons.GetValue(null)).Length : 6;
            #else
			m_toolCount = toolIcons != null ? ((Array) toolIcons.GetValue(null)).Length : 5;
            #endif

            YIUIToolbarCallback.OnToolbarGUI = OnGUI;
            YIUIToolbarCallback.OnToolbarGUILeft = GUILeft;
            YIUIToolbarCallback.OnToolbarGUIRight = GUIRight;
        }

        #if UNITY_2019_3_OR_NEWER
        public const float space = 8;
        #else
		public const float space = 10;
        #endif
        public const float largeSpace = 20;
        public const float buttonWidth = 32;
        public const float dropdownWidth = 80;
        #if UNITY_2019_1_OR_NEWER
        public const float playPauseStopWidth = 140;
        #else
		public const float playPauseStopWidth = 100;
        #endif

        static void OnGUI()
        {
            if (m_commandStyle == null)
            {
                m_commandStyle = new GUIStyle("CommandLeft");
            }

            var screenWidth = EditorGUIUtility.currentViewWidth;

            float playButtonsPosition = Mathf.RoundToInt((screenWidth - playPauseStopWidth) / 2);

            Rect leftRect = new Rect(0, 0, screenWidth, Screen.height);
            leftRect.xMin += space;
            leftRect.xMin += buttonWidth * m_toolCount;
            #if UNITY_2019_3_OR_NEWER
            leftRect.xMin += space;
            #else
			leftRect.xMin += largeSpace; // Spacing between tools and pivot
            #endif
            leftRect.xMin += 64 * 2;
            leftRect.xMax = playButtonsPosition;

            Rect rightRect = new Rect(0, 0, screenWidth, Screen.height);
            rightRect.xMin = playButtonsPosition;
            rightRect.xMin += m_commandStyle.fixedWidth * 3;
            rightRect.xMax = screenWidth;
            rightRect.xMax -= space;
            rightRect.xMax -= dropdownWidth;
            rightRect.xMax -= space;
            rightRect.xMax -= dropdownWidth;
            #if UNITY_2019_3_OR_NEWER
            rightRect.xMax -= space;
            #else
			rightRect.xMax -= largeSpace; // Spacing between layers and account
            #endif
            rightRect.xMax -= dropdownWidth;
            rightRect.xMax -= space;
            rightRect.xMax -= buttonWidth;
            rightRect.xMax -= space;
            rightRect.xMax -= 78;

            leftRect.xMin += space;
            leftRect.xMax -= space;
            rightRect.xMin += space;
            rightRect.xMax -= space;

            #if UNITY_2019_3_OR_NEWER
            leftRect.y = 4;
            leftRect.height = 22;
            rightRect.y = 4;
            rightRect.height = 22;
            #else
			leftRect.y = 5;
			leftRect.height = 24;
			rightRect.y = 5;
			rightRect.height = 24;
            #endif

            if (leftRect.width > 0)
            {
                GUILayout.BeginArea(leftRect);
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();

                foreach (var item in m_LeftToolbarGUI)
                {
                    item.action();
                }

                GUILayout.EndHorizontal();
                GUILayout.EndArea();
            }

            if (rightRect.width > 0)
            {
                GUILayout.BeginArea(rightRect);
                GUILayout.BeginHorizontal();

                foreach (var item in m_RightToolbarGUI)
                {
                    item.action();
                }

                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                GUILayout.EndArea();
            }
        }

        public static void GUILeft()
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            foreach (var item in m_LeftToolbarGUI)
            {
                item.action();
            }

            GUILayout.EndHorizontal();
        }

        public static void GUIRight()
        {
            GUILayout.BeginHorizontal();

            foreach (var item in m_RightToolbarGUI)
            {
                item.action();
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
    }
}