using DG.Tweening;
using UnityEngine;
using YIUIFramework;

namespace ET.Client
{
    /// <summary>
    /// 简单的窗口动画
    /// 只是一个案例 不需要可以删除
    /// 这里用到dotween动画 ettask 简单的扩展
    /// 目前已知BUG 同时有多个人调用动画 其他异步还没有完成时 被其他的删除了就会错误
    /// 导致其他异步全部中断
    /// </summary>
    public static class WindowFadeAnim
    {
        //对dotween的异步扩展
        //临时方案 还不够完善
        //目前这个只是在UI动画上使用 其他地方请自行实现
        private static async ETTask GetAwaiter(this Tweener tweener)
        {
            var task = ETTask.Create(true);
            tweener.onComplete += () =>
            {
                task.SetResult();
            };
            await task;
        }

        //淡入
        public static async ETTask In(GameObject gameObject, float time = 0.25f)
        {
            if (YIUIConstHelper.Const.BanDotweenAnim) return;

            if (gameObject == null) return;

            gameObject.SetActive(true);

            gameObject.transform.localScale = YIUIConstHelper.Const.DotweenAnimScale;

            await gameObject.transform.DOScale(Vector3.one, time);
        }

        //淡出
        public static async ETTask Out(GameObject gameObject, float time = 0.25f)
        {
            if (YIUIConstHelper.Const.BanDotweenAnim) return;

            if (gameObject == null) return;

            gameObject.transform.localScale = Vector3.one;

            await gameObject.transform.DOScale(YIUIConstHelper.Const.DotweenAnimScale, time);

            if (gameObject == null) return;

            gameObject.SetActive(false);

            // ReSharper disable once Unity.InefficientPropertyAccess
            gameObject.transform.localScale = Vector3.one;
        }
    }
}
