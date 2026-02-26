namespace YIUIFramework
{
    /// <summary>
    /// 回调方法
    /// </summary>
    /// <param name="residueTime">剩余时间</param>
    /// <param name="elapseTime">已过去时间</param>
    /// <param name="totalTime">总时间</param>
    public delegate void CountDownTimerCallback(double residueTime, double elapseTime, double totalTime);
}
