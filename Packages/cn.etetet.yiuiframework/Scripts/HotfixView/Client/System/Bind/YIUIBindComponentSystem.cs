using System;
using System.Collections.Generic;
using YIUIFramework;

namespace ET.Client
{
    [FriendOf(typeof(YIUIBindComponent))]
    [EntitySystemOf(typeof(YIUIBindComponent))]
    public static partial class YIUIBindComponentSystem
    {
        [EntitySystem]
        private static void Awake(this YIUIBindComponent self)
        {
        }

        [EntitySystem]
        private static void Destroy(this YIUIBindComponent self)
        {
            self.Reset();
        }

        /// <summary>
        /// 初始化获取到所有UI相关的绑定关系
        /// 由SG自动生成
        /// </summary>
        public static bool InitAllBind(this YIUIBindComponent self, YIUIBindVo[] binds)
        {
            if (self.IsInit)
            {
                Log.Error($"已经初始化过了 请检查");
                return false;
            }

            //关联UI工具中自动生成绑定代码 SG自动生成
            if (binds is not { Length: > 0 })
            {
                //如果才接入框架 第一个UI都没有生成是无法运行的 先生成一个UI吧
                Log.Error("没有找到绑定信息 或者 没有绑定信息 请检查 或 参考文档 https://lib9kmxvq7k.feishu.cn/wiki/AFj9w2gtriFXVZk8igEc7SMBnff");
                return false;
            }

            self.m_UITypeToPkgInfo = new(binds.Length);
            self.m_UIPathToPkgInfo = new(binds.Length);
            self.m_UIToPkgInfo = new(binds.Length);

            for (var i = 0; i < binds.Length; i++)
            {
                var vo = binds[i];
                self.m_UITypeToPkgInfo[vo.ComponentType] = vo;
                self.AddPkgInfoToPathDic(vo);
                self.m_UIToPkgInfo[vo.ResName] = vo;
            }

            self.IsInit = true;
            return true;
        }

        private static void AddPkgInfoToPathDic(this YIUIBindComponent self, YIUIBindVo vo)
        {
            var pkgName = vo.PkgName;
            var resName = vo.ResName;
            if (!self.m_UIPathToPkgInfo.ContainsKey(pkgName))
            {
                self.m_UIPathToPkgInfo.Add(pkgName, new Dictionary<string, YIUIBindVo>());
            }

            var dic = self.m_UIPathToPkgInfo[pkgName];

            if (!dic.TryAdd(resName, vo))
            {
                Log.Error($"重复资源 请检查 {pkgName} {resName}");
            }
        }

        /// <summary>
        /// 得到UI包信息
        /// </summary>
        public static YIUIBindVo? GetBindVoByType(this YIUIBindComponent self, Type uiType)
        {
            if (uiType == null)
            {
                Log.Error($"空 无法取到这个包信息 请检查");
                return null;
            }

            if (self.m_UITypeToPkgInfo.TryGetValue(uiType, out var vo))
            {
                return vo;
            }

            Log.Error($"未获取到这个UI包信息 请检查  {uiType.Name}");
            return null;
        }

        /// <summary>
        /// 得到UI包信息
        /// </summary>
        public static YIUIBindVo? GetBindVoByType<T>(this YIUIBindComponent self)
        {
            return self.GetBindVoByType(typeof(T));
        }

        /// <summary>
        /// 根据唯一ID获取
        /// 由pkg+res 拼接的唯一ID
        /// </summary>
        public static YIUIBindVo? GetBindVoByPath(this YIUIBindComponent self, string pkgName, string resName)
        {
            if (string.IsNullOrEmpty(pkgName) || string.IsNullOrEmpty(resName))
            {
                Log.Error($"空名称 无法取到这个包信息 请检查");
                return null;
            }

            if (!self.m_UIPathToPkgInfo.ContainsKey(pkgName))
            {
                Log.Error($"不存在这个包信息 请检查 {pkgName}");
                return null;
            }

            if (self.m_UIPathToPkgInfo[pkgName].TryGetValue(resName, out var vo))
            {
                return vo;
            }

            Log.Error($"未获取到这个包信息 请检查  {pkgName} {resName}");

            return null;
        }

        /// <summary>
        /// 根据resName获取
        /// 只有保证所有res唯一时才可使用
        /// </summary>
        public static YIUIBindVo? GetBindVoByResName(this YIUIBindComponent self, string resName)
        {
            if (string.IsNullOrEmpty(resName))
            {
                Log.Error($"空名称 无法取到这个包信息 请检查");
                return null;
            }

            if (self.m_UIToPkgInfo.TryGetValue(resName, out var vo))
            {
                return vo;
            }

            Log.Error($"未获取到这个包信息 请检查  {resName}");

            return null;
        }

        /// <summary>
        /// 重置 慎用
        /// </summary>
        private static void Reset(this YIUIBindComponent self)
        {
            if (self.m_UITypeToPkgInfo != null)
            {
                self.m_UITypeToPkgInfo.Clear();
                self.m_UITypeToPkgInfo = null;
            }

            if (self.m_UIPathToPkgInfo != null)
            {
                self.m_UIPathToPkgInfo.Clear();
                self.m_UIPathToPkgInfo = null;
            }

            if (self.m_UIToPkgInfo != null)
            {
                self.m_UIToPkgInfo.Clear();
                self.m_UIToPkgInfo = null;
            }

            self.IsInit = false;
        }
    }
}