//------------------------------------------------------------
// Author: 亦亦
// Mail: 379338943@qq.com
// Data: 2023年2月12日
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using YIUIFramework;

namespace ET.Client
{
    /// <summary>
    /// YIUI绑定数据
    /// </summary>
    [ComponentOf(typeof(YIUIMgrComponent))]
    public class YIUIBindComponent : Entity, IAwake, IDestroy
    {
        /// <summary>
        /// 根据创建时的类获取
        /// type 限制为 ui的component
        /// </summary>
        public Dictionary<Type, YIUIBindVo> m_UITypeToPkgInfo;

        /// <summary>
        /// 根据 pkg + res 双字典获取
        /// </summary>
        public Dictionary<string, Dictionary<string, YIUIBindVo>> m_UIPathToPkgInfo;

        /// <summary>
        /// 因为使用yooasset 规定所有资源唯一
        /// 所以这里可以抛弃pkg+res 直接使用res 可以拿到对应的资源
        /// 如果你的不是唯一的 请删除这个方法不要使用
        /// </summary>
        public Dictionary<string, YIUIBindVo> m_UIToPkgInfo;

        //初始化记录
        public bool IsInit { get; set; }
    }
}