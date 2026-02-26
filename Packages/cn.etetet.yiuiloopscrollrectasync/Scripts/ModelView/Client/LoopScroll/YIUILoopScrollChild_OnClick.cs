//------------------------------------------------------------
// Author: 亦亦
// Mail: 379338943@qq.com
// Data: 2023年2月12日
//------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace ET.Client
{
    /// <summary>
    /// 无限循环列表 (异步)
    /// 文档: https://lib9kmxvq7k.feishu.cn/wiki/HPbwwkhsKi9aDik5VEXcqPhDnIh
    /// </summary>
    public partial class YIUILoopScrollChild
    {
        public bool m_OnClickInit; //是否已初始化
        public string m_ItemClickEventName; //ui中的点击UIEventP0
        public bool m_ItemClickCheck; //ui中的点击检查
        public Queue<int> m_OnClickItemQueue = new(); //当前所有已选择 遵循先进先出 有序
        public HashSet<int> m_OnClickItemHashSet = new(); //当前所有已选择 无序 为了更快查找
        public int m_MaxClickCount = 1; //可选最大数量 >=2 就是复选 最小1
        public bool m_RepetitionCancel = true; //重复选择 则取消选择
        public bool m_AutoCancelLast = true; //当选择操作最大数量过后 自动取消第一个选择的 否则选择无效
    }
}