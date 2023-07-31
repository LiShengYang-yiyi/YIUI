using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace YIUIFramework
{
    /// <summary>
    /// 简单的窗口动画
    /// </summary>
    public static class WindowFadeAnim
    {
        private static Vector3 m_AnimScale = new Vector3(0.8f, 0.8f, 0.8f);

        //淡入
        public static async UniTask In(UIBase uiBase, float time = 0.25f)
        {
            var obj = uiBase?.OwnerGameObject;
            if (obj == null) return;

            uiBase.SetActive(true);
            obj.transform.localScale = m_AnimScale;

            await obj.transform.DOScale(Vector3.one, time);
        }

        //淡出
        public static async UniTask Out(UIBase uiBase, float time = 0.25f)
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