using System;
using System.Collections.Generic;
using System.Reflection;
using ET;
using UnityEngine;

namespace YIUIFramework
{
    /// <summary>
    /// 独立的 全局单例 静态管理类
    /// 所有继承了ISingleton的任意类型单例
    /// 主要目的为统计
    /// 不要轻易调用Dispose 一键释放所有单例
    /// </summary>
    public static class YIUISingletonHelper
    {
        private static EntityRef<Entity> m_YIUIMgrRef;
        internal static Entity YIUIMgr => m_YIUIMgrRef;

        private static readonly List<IYIUISingleton> g_Singles = new();

        public static bool Disposing { get; private set; } = true;

        public static int Count => g_Singles.Count;

        public static bool IsQuitting { get; private set; }

        static YIUISingletonHelper()
        {
            Application.quitting -= OnQuitting;
            Application.quitting += OnQuitting;
        }

        private static void OnQuitting()
        {
            //Debug.LogError("OnQuitting");
            IsQuitting = true;
        }

        //只能由一个地方 真的需要彻底清除时调用
        //游戏退出时不必调用 因为游戏都退了 所有都会被清空
        //需要调用的时机 如不退出游戏 但是要重置全部的情况下使用
        //一般情况是不需要使用的
        public static void DisposeAll()
        {
            if (IsQuitting)
            {
                //Debug.Log("正在退出游戏 不必清理");
                //千万别告诉我你退出的时候单例要干什么保存的逻辑
                return;
            }

            Disposing = true;

            //Debug.Log($"SingletonMgr.清除所有单例");
            var singles = g_Singles.ToArray();
            for (int i = 0; i < singles.Length; i++)
            {
                var inst = singles[i];

                if (inst == null || inst.Disposed) continue;

                inst.Dispose();
                singles[i] = null;
            }

            g_Singles.Clear();
            m_YIUIMgrRef = default;
        }

        //初始化
        public static async ETTask InitializeAll(Entity entity)
        {
            if (IsQuitting)
            {
                Debug.Log("正在退出游戏 禁止初始化");
                return;
            }

            //Debug.Log($"SingletonMgr.初始化所有单例");
            Disposing = false;
            m_YIUIMgrRef = entity;
            await RegisterAll();
        }

        internal static void Add(IYIUISingleton single)
        {
            //Debug.Log($"添加{single.GetType().Name}");
            g_Singles.Add(single);
        }

        internal static void Remove(IYIUISingleton single)
        {
            //Debug.Log($"移除{single.GetType().Name}");
            g_Singles.Remove(single);
        }

        private static async ETTask RegisterAll()
        {
            var allSingleton = AssemblyHelper.GetClassesWithAttribute<YIUISingletonAttribute>();

            allSingleton.Sort((x, y) =>
            {
                var xAttr = x.GetCustomAttribute<YIUISingletonAttribute>();
                var yAttr = y.GetCustomAttribute<YIUISingletonAttribute>();
                return xAttr.Order.CompareTo(yAttr.Order);
            });

            foreach (var singleton in allSingleton)
            {
                var instProperty = GetInstProperty(singleton);
                if (instProperty == null)
                {
                    Debug.LogError($"类型{singleton.Name}没有找到Inst属性");
                    continue;
                }

                object instValue = null;

                try
                {
                    instValue = instProperty.GetValue(null);
                }
                catch (Exception e)
                {
                    Debug.LogError($"类型{singleton.Name}的Inst属性获取失败 {e}");
                    continue;
                }

                if (instValue == null)
                {
                    Debug.LogError($"类型{singleton.Name}的Inst属性没有值");
                    continue;
                }

                await YIUIMgrCenter.Inst.Register((IYIUIManager)instValue);
            }
        }

        private static PropertyInfo GetInstProperty(Type type)
        {
            var instProperty = type.GetProperty("Inst", BindingFlags.Public | BindingFlags.Static | BindingFlags.GetProperty);

            if (instProperty != null)
            {
                return instProperty;
            }

            if (type.BaseType == null)
            {
                return null;
            }

            return GetInstProperty(type.BaseType);
        }
    }
}