//------------------------------------------------------------
// Author: 亦亦
// Mail: 379338943@qq.com
// Data: 2023年2月12日
//------------------------------------------------------------

using System.Collections.Generic;
using YIUIFramework;
using UnityEngine;

namespace ET.Client
{
    /// <summary>
    /// 倒计时管理器 (更适合UI文本倒计时使用)
    /// 区别于Times 个人认为更适合UI上的时间倒计时
    ///                            Times                   CountDown
    /// 回调频率                     不可改                     可改                        (虽然中途改频率这个事情很少)
    /// 如果暂停                中间丢失的时间就没了      中途丢失的时间会快速倒计时             (万一有需求 中间的各种计算就丢掉了)
    /// 添加时可立即调用一次      否(还需要自己调一次)          可传参数控制                     (很多时候倒计时都需要第一时间刷新一次的)
    /// 一对多                        否                         是                         (因为用Callback做K 就没办法在同一个Callback下 被别人倒计时)
    /// 可提前结束                     否                         是                         (针对于 比如 匿名函数 等特殊情况)
    /// 回调参数            obj 但是麻烦 而且不可变           已过去时间/总时间                   (更适合于UI上的一些数字倒计时)
    /// 可循环                        否                          是                         (虽然0 都可以无限 但是万一要的是不是0的情况下循环呢 就得递归调自己吗)
    /// 多重载                        否                          是                         (满足各种需求)
    /// 匿名函数                      否                          是                          (匿名函数也可以被暂停 移除等操作)
    /// ......
    /// </summary>
    [ComponentOf(typeof(YIUIMgrComponent))]
    public partial class CountDownMgr : Entity, IAwake, ILateUpdate, IDestroy
    {
        /// <summary>
        /// 所有需要被倒计时的目标
        /// 这个可以一对多
        /// </summary>
        public Dictionary<long, CountDownData> m_AllCountDown = new Dictionary<long, CountDownData>();

        /// <summary>
        /// 临时存储
        /// 下一帧添加的倒计时
        /// </summary>
        public Dictionary<long, CountDownData> m_ToAddCountDown = new Dictionary<long, CountDownData>();

        /// <summary>
        /// 所有需要被倒计时的目标
        /// 这个只能一对一
        /// </summary>
        public Dictionary<CountDownTimerCallback, long> m_CallbackGuidDic = new();

        /// <summary>
        /// 临时存储
        /// 下一帧移除的倒计时
        /// </summary>
        public List<long> m_RemoveGuid = new List<long>();

        /// <summary>
        /// 可容纳的最大倒计时
        /// </summary>
        public long m_MaxCount = 1000;

        /// <summary>
        /// 当前已经存在的倒计时数量
        /// </summary>
        public long m_AtCount = 0;

        //统一所有取时间都用这个 且方便修改
        [StaticField]
        public static float GetTime
        {
            get
            {
                //这是一个倒计时时间不受暂停影响的
                return Time.realtimeSinceStartup;
            }
        }
    }
}