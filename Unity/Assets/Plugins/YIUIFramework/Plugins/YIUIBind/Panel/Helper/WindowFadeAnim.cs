using ET;
using DG.Tweening;
using ET.Client;
using UnityEngine;

namespace YIUIFramework
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
        private static Vector3 m_AnimScale = new Vector3(0.8f, 0.8f, 0.8f);

        //对dotween的异步扩展
        //临时方案 还不够完善
        //目前这个只是在UI动画上使用 其他地方请自行实现
        private static async ETTask GetAwaiter(this Tweener tweener)
        {
            var task = ETTask.Create();
            tweener.onKill     += () => { task.SetResult(); };
            tweener.onComplete += () => { task.SetResult(); };
            await task;
        }

        //淡入
        public static async ETTask In(YIUIComponent uiBase, float time = 0.25f)
        {
            var obj = uiBase?.OwnerGameObject;
            if (obj == null) return;

            uiBase.SetActive(true);

            obj.transform.localScale = m_AnimScale;

            await obj.transform.DOScale(Vector3.one, time);
        }

        //淡出
        public static async ETTask Out(YIUIComponent uiBase, float time = 0.25f)
        {
            var obj = uiBase?.OwnerGameObject;
            if (obj == null) return;

            obj.transform.localScale = Vector3.one;

            await obj.transform.DOScale(m_AnimScale, time);

            uiBase.SetActive(false);

            obj.transform.localScale = Vector3.one;
        }
    }
}