//------------------------------------------------------------
// Author: 亦亦
// Mail: 379338943@qq.com
// Data: 2023年2月12日
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;

namespace ET.Client
{
    /// <summary>
    /// 无限循环列表 (异步)
    /// 文档: https://lib9kmxvq7k.feishu.cn/wiki/HPbwwkhsKi9aDik5VEXcqPhDnIh
    /// </summary>
    public partial class YIUILoopScrollChild
    {
        public int TotalCount => m_Owner.totalCount; //总数
        public RectTransform Content => m_Owner.content;
        public RectTransform CacheRect => m_Owner.u_CacheRect;
        public int StartLine => m_Owner.u_StartLine; //可见的第一行
        public int CurrentLines => m_Owner.u_CurrentLines; //滚动中的当前行数
        public int TotalLines => m_Owner.u_TotalLines; //总数
        public int EndLine => Mathf.Min(StartLine + CurrentLines, TotalLines); //可见的最后一行
        public int ContentConstraintCount => m_Owner.u_ContentConstraintCount; //限制 行/列 数
        public float ContentSpacing => m_Owner.u_ContentSpacing; //间隔
        public int ItemStart => m_Owner.u_ItemStart; //当前显示的第一个的Index                
        public int ItemEnd => m_Owner.u_ItemEnd; //当前显示的最后一个index 被+1了注意            
        public int PreLoadCount => m_Owner.u_PreLoadCount; //初始化后自动预加载的个数
    }
}