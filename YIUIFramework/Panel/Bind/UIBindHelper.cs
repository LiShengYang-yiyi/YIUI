//#define YIUIMACRO_SIMULATE_NONEEDITOR //模拟非编辑器状态  在编辑器使用 非编辑器加载模式 用于在编辑器下测试  

using System;
using System.Collections.Generic;
using UnityEngine;

namespace YIUIFramework
{
    /// <summary>
    /// UI关联帮助类
    /// </summary>
    public static class UIBindHelper
    {
        /// <summary>
        /// 根据创建时的类获取
        /// </summary>
        private static Dictionary<Type, UIBindVo> g_UITypeToPkgInfo = new Dictionary<Type, UIBindVo>();

        /// <summary>
        /// 根据 pkg + res 双字典获取
        /// </summary>
        private static Dictionary<string, Dictionary<string, UIBindVo>> g_UIPathToPkgInfo =
            new Dictionary<string, Dictionary<string, UIBindVo>>();

        /// <summary>
        /// 只有panel 的信息
        /// </summary>
        private static Dictionary<string, UIBindVo> g_UIPanelNameToPkgInfo = new Dictionary<string, UIBindVo>();

        //改为dll过后 提供给外部的方法
        //1 从UI工具中自动生成绑定代码
        //2 外部请直接调用此方法 UIBindHelper.InternalGameGetUIBindVoFunc = YIUICodeGenerated.UIBindProvider.Get;
        public static Func<UIBindVo[]> InternalGameGetUIBindVoFunc { internal get; set; }

        //初始化记录
        public static bool IsInit { get; private set; }

        public static Type BasePanelType = typeof(BasePanel);

        /// <summary>
        /// 初始化获取到所有UI相关的绑定关系
        /// Editor下是反射
        /// 其他 是序列化的文件 打包的时候一定要生成一次文件
        /// </summary>
        internal static bool InitAllBind()
        {
            if (IsInit)
            {
                Debug.LogError($"已经初始化过了 请检查");
                return false;
            }

            #if !UNITY_EDITOR || YIUIMACRO_SIMULATE_NONEEDITOR
            if (InternalGameGetUIBindVoFunc == null)
            {
                Debug.LogError($"使用非反射注册绑定 但是方法未实现 请检查");
                return false;
            }
            var binds = InternalGameGetUIBindVoFunc?.Invoke();
            #else
            var binds = new UIBindProvider().Get();
            #endif

            if (binds == null || binds.Length <= 0)
            {
                Debug.LogError("没有找到绑定信息 或者 没有绑定信息 请检查");
                return false;
            }

            g_UITypeToPkgInfo      = new Dictionary<Type, UIBindVo>(binds.Length);
            g_UIPathToPkgInfo      = new Dictionary<string, Dictionary<string, UIBindVo>>();
            g_UIPanelNameToPkgInfo = new Dictionary<string, UIBindVo>(binds.Length);

            for (var i = 0; i < binds.Length; i++)
            {
                var vo = binds[i];
                g_UITypeToPkgInfo[vo.CreatorType] = vo;
                AddPkgInfoToPathDic(vo);
                if (vo.CodeType == BasePanelType)
                    g_UIPanelNameToPkgInfo[vo.ResName] = vo;
            }

            IsInit = true;
            return true;
        }

        private static void AddPkgInfoToPathDic(UIBindVo vo)
        {
            var pkgName = vo.PkgName;
            var resName = vo.ResName;
            if (!g_UIPathToPkgInfo.ContainsKey(pkgName))
            {
                g_UIPathToPkgInfo.Add(pkgName, new Dictionary<string, UIBindVo>());
            }

            var dic = g_UIPathToPkgInfo[pkgName];

            if (dic.ContainsKey(resName))
            {
                Debug.LogError($"重复资源 请检查 {pkgName} {resName}");
                return;
            }

            dic.Add(resName, vo);
        }

        /// <summary>
        /// 得到UI包信息
        /// </summary>
        /// <param name="uiType"></param>
        /// <returns></returns>
        public static UIBindVo? GetBindVoByType(Type uiType)
        {
            if (uiType == null)
            {
                Debug.LogError($"空 无法取到这个包信息 请检查");
                return null;
            }

            if (g_UITypeToPkgInfo.TryGetValue(uiType, out var vo))
            {
                return vo;
            }

            Debug.LogError($"未获取到这个UI包信息 请检查  {uiType.Name}");
            return null;
        }

        /// <summary>
        /// 得到UI包信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static UIBindVo? GetBindVoByType<T>()
        {
            return GetBindVoByType(typeof(T));
        }

        /// <summary>
        /// 根据唯一ID获取
        /// 由pkg+res 拼接的唯一ID
        /// </summary>
        public static UIBindVo? GetBindVoByPath(string pkgName, string resName)
        {
            if (string.IsNullOrEmpty(pkgName) || string.IsNullOrEmpty(resName))
            {
                Debug.LogError($"空名称 无法取到这个包信息 请检查");
                return null;
            }

            if (!g_UIPathToPkgInfo.ContainsKey(pkgName))
            {
                Debug.LogError($"不存在这个包信息 请检查 {pkgName}");
                return null;
            }

            if (g_UIPathToPkgInfo[pkgName].TryGetValue(resName, out var vo))
            {
                return vo;
            }

            Debug.LogError($"未获取到这个包信息 请检查  {pkgName} {resName}");

            return null;
        }

        /// <summary>
        /// 根据panelName获取
        /// 只有是panel才会存在的信息
        /// 非Panel请使用其他
        /// </summary>
        internal static UIBindVo? GetBindVoByPanelName(string panelName)
        {
            if (string.IsNullOrEmpty(panelName))
            {
                Debug.LogError($"空名称 无法取到这个包信息 请检查");
                return null;
            }

            if (g_UIPanelNameToPkgInfo.TryGetValue(panelName, out var vo))
            {
                return vo;
            }

            Debug.LogError($"未获取到这个包信息 请检查  {panelName}");

            return null;
        }

        /// <summary>
        /// 重置 慎用
        /// </summary>
        internal static void Reset()
        {
            if (g_UITypeToPkgInfo != null)
            {
                g_UITypeToPkgInfo.Clear();
                g_UITypeToPkgInfo = null;
            }

            if (g_UIPathToPkgInfo != null)
            {
                g_UIPathToPkgInfo.Clear();
                g_UIPathToPkgInfo = null;
            }

            if (g_UIPanelNameToPkgInfo != null)
            {
                g_UIPanelNameToPkgInfo.Clear();
                g_UIPanelNameToPkgInfo = null;
            }

            IsInit = false;
        }
    }
}