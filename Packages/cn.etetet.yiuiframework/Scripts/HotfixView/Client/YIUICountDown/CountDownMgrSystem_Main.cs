using UnityEngine;
using YIUIFramework;

namespace ET.Client
{
    public static partial class CountDownMgrSystem
    {
        //为了不受mono暂停影响 所以使用异步调用
        public static void ManagerUpdate(this CountDownMgr self)
        {
            var time = CountDownMgr.GetTime;

            //需要被添加的
            if (self.m_ToAddCountDown.Count >= 1)
            {
                foreach (var countDown in self.m_ToAddCountDown.Values)
                {
                    self.ToAddCountDownTimer(countDown);
                }

                self.m_ToAddCountDown.Clear();
            }

            //需要被移除的
            if (self.m_RemoveGuid.Count >= 1)
            {
                foreach (var guid in self.m_RemoveGuid)
                {
                    self.RemoveCountDownTimer(guid);
                }

                self.m_RemoveGuid.Clear();
            }

            //倒计时
            foreach (var data in self.m_AllCountDown.Values)
            {
                data.ElapseTime = time - data.StartTime;

                //已经用完时间
                if (data.TotalTime != 0 && data.ElapseTime >= data.TotalTime)
                {
                    data.ElapseTime = data.TotalTime;

                    self.Callback(data);

                    if (data.Forver)
                    {
                        self.Restart(data);
                    }
                    else
                    {
                        self.m_RemoveGuid.Add(data.Guid);
                    }
                }
                else if (time - data.LastCallBackTime >= data.Interval)
                {
                    //这样才可以确保如果卡了很久  该间隔回调的还是会回 不会遗漏
                    //但是如果一瞬间就到最后了 中途的还是会没有  因为这个倒计时类的需求就是这样的
                    data.LastCallBackTime += data.Interval;
                    self.Callback(data);
                }
            }
        }

        #region 私有

        /// <summary>
        /// 是否可添加倒计时   有同时倒计时上限限制
        /// </summary>
        /// <returns></returns>
        private static bool TryAdd(this CountDownMgr self)
        {
            return !(self.m_AtCount >= self.m_MaxCount);
        }

        /// <summary>
        /// 获取当前索引
        /// </summary>
        /// <returns></returns>
        private static long GetGuid(this CountDownMgr self)
        {
            return IdGenerater.Instance.GenerateId();
        }

        //从缓存列表添加到运行列表
        private static void ToAddCountDownTimer(this CountDownMgr self, CountDownMgr.CountDownData countDownData)
        {
            if (self.m_AllCountDown.ContainsKey(countDownData.Guid))
            {
                Log.Error($"<color=red> 添加的这个已经存在 :{countDownData.Guid}</color>");
                return;
            }

            self.m_AllCountDown.Add(countDownData.Guid, countDownData);
            self.m_AtCount++;
        }

        /// <summary>
        /// 添加一个到字典中
        /// </summary>
        private static long AddCountDownTimer(this CountDownMgr self, CountDownMgr.CountDownData countDownData)
        {
            long guid = self.GetGuid();
            countDownData.Guid = guid;
            self.m_ToAddCountDown.Add(guid, countDownData);
            return guid;
        }

        private static bool RemoveCountDownTimer(this CountDownMgr self, long guid)
        {
            if (guid == 0)
            {
                return true;
            }

            if (!self.m_AllCountDown.ContainsKey(guid))
            {
                return false;
            }

            var data = self.m_AllCountDown[guid];
            self.m_AllCountDown.Remove(guid);
            RefPool.Put(data);
            self.m_AtCount--;
            return true;
        }

        /// <summary>
        /// 移除一个回调
        /// 根据需求也可使用 Remove(ref long guid)
        /// </summary>
        public static bool Remove(this CountDownMgr self, long guid)
        {
            if (guid == 0)
            {
                return true;
            }

            //如果对方还在添加列表中 在移除列表中添加
            //在下一次update的时候 添加列表还是会先添加到all中
            //然后就会被移除 所以并不会被执行
            //最终所有移除的都是从all中移除的
            if (self.m_ToAddCountDown.ContainsKey(guid))
            {
                self.m_RemoveGuid.Add(guid);
                return true;
            }

            //如果正在执行中 则添加到移除中
            //下一次循环移除
            if (self.m_AllCountDown.ContainsKey(guid))
            {
                self.m_RemoveGuid.Add(guid);
                return true;
            }

            return false;
        }

        /// <summary>
        /// 让倒计时重新开始
        /// </summary>
        private static bool Restart(this CountDownMgr self, CountDownMgr.CountDownData data)
        {
            data.ElapseTime       = 0;
            data.LastCallBackTime = CountDownMgr.GetTime;
            data.StartTime        = data.LastCallBackTime;
            data.EndTime          = data.LastCallBackTime + data.TotalTime;
            return true;
        }

        private static void Callback(this CountDownMgr self, long guid)
        {
            var exist = self.m_AllCountDown.TryGetValue(guid, out CountDownMgr.CountDownData data);
            if (exist)
                self.Callback(data);
        }

        private static void Callback(this CountDownMgr self, CountDownMgr.CountDownData data)
        {
            data.TimerCallback?.Invoke(data.TotalTime - data.ElapseTime, data.ElapseTime, data.TotalTime);
        }

        private static void Callback(this CountDownMgr self, CountDownMgr.CountDownData data, double elapseTime)
        {
            data.TimerCallback?.Invoke(data.TotalTime - elapseTime, elapseTime, data.TotalTime);
        }

        #endregion
    }
}
