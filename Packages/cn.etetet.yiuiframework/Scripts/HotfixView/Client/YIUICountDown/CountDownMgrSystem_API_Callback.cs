//------------------------------------------------------------
// Author: 亦亦
// Mail: 379338943@qq.com
// Data: 2023年2月12日
//------------------------------------------------------------

using UnityEngine;
using YIUIFramework;

namespace ET.Client
{
    public static partial class CountDownMgrSystem
    {
        private static bool TryAddCallback(this CountDownMgr self, CountDownTimerCallback timerCallback)
        {
            if (self.m_CallbackGuidDic.ContainsKey(timerCallback))
            {
                Debug.LogError($"当前回调已存在 不能重复添加 如果想重复添加请使用另外一个API 使用GUID");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 移除一个回调
        /// </summary>
        public static bool Remove(this CountDownMgr self, CountDownTimerCallback timerCallback)
        {
            if (self == null) return false;
            if (!self.m_CallbackGuidDic.ContainsKey(timerCallback))
            {
                return false;
            }

            self.Remove(self.m_CallbackGuidDic[timerCallback]);
            self.m_CallbackGuidDic.Remove(timerCallback);
            return true;
        }

        /// <summary>
        /// 添加一个回调
        /// </summary>
        /// <param name="timerCallback">回调方法</param>
        /// <param name="totalTime">总时间</param>
        /// <param name="interval">间隔</param>
        /// <param name="startCallback">是否立即回调一次</param>
        /// <returns>是否添加成功</returns>
        public static bool Add(this CountDownMgr      self,
                               CountDownTimerCallback timerCallback,
                               double                 totalTime,
                               double                 interval,
                               bool                   startCallback = false)
        {
            if (timerCallback == null)
            {
                Log.Error($"<color=red> 必须有callback </color>");
                return false;
            }

            if (!self.TryAddCallback(timerCallback))
            {
                return false;
            }

            long callbackGuid = 0;
            var  result       = self.Add(ref callbackGuid, totalTime, interval, timerCallback, startCallback);
            if (result)
            {
                self.m_CallbackGuidDic.Add(timerCallback, callbackGuid);
            }

            return result;
        }

        /// <summary>
        /// 添加一个只有一次的回调
        /// </summary>
        public static bool Add(this CountDownMgr      self,
                               CountDownTimerCallback timerCallback,
                               double                 totalTime,
                               bool                   startCallback = false)
        {
            if (timerCallback == null)
            {
                Log.Error($"<color=red> 必须有callback </color>");
                return false;
            }

            if (!self.TryAddCallback(timerCallback))
            {
                return false;
            }

            long callbackGuid = 0;
            var  result       = self.Add(ref callbackGuid, totalTime, timerCallback, startCallback);
            if (result)
            {
                self.m_CallbackGuidDic.Add(timerCallback, callbackGuid);
            }

            return result;
        }

        /// <summary>
        /// 有设置是否循环的
        /// </summary>
        public static bool Add(this CountDownMgr      self,
                               CountDownTimerCallback timerCallback,
                               double                 totalTime,
                               double                 interval,
                               bool                   forever,
                               bool                   startCallback)
        {
            if (timerCallback == null)
            {
                Log.Error($"<color=red> 必须有callback </color>");
                return false;
            }

            if (!self.TryAddCallback(timerCallback))
            {
                return false;
            }

            long callbackGuid = 0;
            var  result       = self.Add(ref callbackGuid, totalTime, interval, timerCallback, forever, startCallback);
            if (result)
            {
                self.m_CallbackGuidDic.Add(timerCallback, callbackGuid);
            }

            return result;
        }

        /// <summary>
        /// 判断这个倒计时是否存在
        /// </summary>
        public static bool ExistTimerCallback(this CountDownMgr self, CountDownTimerCallback timerCallback)
        {
            return self.m_CallbackGuidDic.ContainsKey(timerCallback);
        }

        /// <summary>
        /// 获取一个倒计时的GUID 如果存在则有
        /// </summary>
        public static long GetTimerCallbackGuid(this CountDownMgr self, CountDownTimerCallback timerCallback)
        {
            if (!self.m_CallbackGuidDic.TryGetValue(timerCallback, out long guid))
                return 0;

            return guid;
        }

        /// <summary>
        /// 重新设置这个倒计时的已过去时间
        /// </summary>
        public static bool SetElapseTime(this CountDownMgr self, CountDownTimerCallback timerCallback, double elapseTime)
        {
            if (!self.m_CallbackGuidDic.TryGetValue(timerCallback, out long guid))
                return false;

            return self.SetElapseTime(guid, elapseTime);
        }

        /// <summary>
        /// 获取一个倒计时剩余的倒计时时间
        /// </summary>
        public static double GetRemainTime(this CountDownMgr self, CountDownTimerCallback timerCallback)
        {
            if (!self.m_CallbackGuidDic.TryGetValue(timerCallback, out long guid))
                return 0;

            return self.GetRemainTime(guid);
        }

        /// <summary>
        /// 强制执行 一个倒计时到最后时间
        /// </summary>
        public static bool ForceToEndTime(this CountDownMgr self, CountDownTimerCallback timerCallback)
        {
            if (!self.m_CallbackGuidDic.TryGetValue(timerCallback, out long guid))
                return false;

            return self.ForceToEndTime(guid);
        }

        /// <summary>
        /// 让倒计时重新开始
        /// </summary>
        public static bool Restart(this CountDownMgr self, CountDownTimerCallback timerCallback)
        {
            if (!self.m_CallbackGuidDic.TryGetValue(timerCallback, out long guid))
                return false;

            return self.Restart(guid);
        }
    }
}
