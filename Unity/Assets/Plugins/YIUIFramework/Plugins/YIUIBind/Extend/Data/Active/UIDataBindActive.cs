using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace YIUIFramework
{
    [LabelText("GameObject的显隐")]
    [AddComponentMenu("YIUIBind/Data/显隐 【Active】 UIDataBindActive")]
    public sealed class UIDataBindActive : UIDataBindBool
    {
        [SerializeField]
        [LabelText("过度类型")]
        private UITransitionModeEnum m_TransitionMode = UITransitionModeEnum.Instant;

        [SerializeField]
        [LabelText("过度时间")]
        [HideIf("m_TransitionMode", UITransitionModeEnum.Instant)]
        private float m_TransitionTime = 0.5f;

        private CanvasGroup m_CanvasGroup;

        protected override void OnValueChanged()
        {
            if (gameObject == null)
                return;

            var result = GetResult();

            if (m_TransitionMode == UITransitionModeEnum.Instant)
            {
                gameObject.SetActive(result);
            }
            else
            {
                if (m_CanvasGroup == null)
                {
                    m_CanvasGroup = gameObject.GetOrAddComponent<CanvasGroup>();
                }

                if (m_CanvasGroup != null)
                {
                    StopAllCoroutines();

                    switch (m_TransitionMode)
                    {
                        case UITransitionModeEnum.Fade:
                            if (result)
                            {
                                gameObject.SetActive(true);
                                StartCoroutine(TransitionFade(m_CanvasGroup, 1.0f, true));
                            }
                            else
                            {
                                StartCoroutine(TransitionFade(m_CanvasGroup, 0.0f, false));
                            }

                            break;
                        case UITransitionModeEnum.FadeIn:
                            if (result)
                            {
                                gameObject.SetActive(true);
                                m_CanvasGroup.alpha = 0;
                                StartCoroutine(TransitionFade(m_CanvasGroup, 1.0f, true));
                            }
                            else
                            {
                                gameObject.SetActive(false);
                            }

                            break;
                        case UITransitionModeEnum.FadeOut:
                            if (result)
                            {
                                m_CanvasGroup.alpha = 1f;
                                gameObject.SetActive(true);
                            }
                            else
                            {
                                m_CanvasGroup.alpha = 1f;
                                StartCoroutine(TransitionFade(m_CanvasGroup, 0.0f, false));
                            }

                            break;
                        case UITransitionModeEnum.Instant:
                        default:
                            gameObject.SetActive(result);
                            Debug.LogError($"不支持的功能 {m_TransitionMode}");
                            break;
                    }
                }
                else
                {
                    gameObject.SetActive(result);
                }
            }
        }

        private IEnumerator TransitionFade(CanvasGroup group, float alphaTarget, bool activeTarget)
        {
            var leftTime   = m_TransitionTime;
            var alphaStart = group.alpha;
            while (leftTime > 0.0f)
            {
                yield return null;
                leftTime -= Time.deltaTime;
                var alpha = Mathf.Lerp(alphaStart,
                                       alphaTarget,
                                       1.0f - (leftTime / m_TransitionTime));
                group.alpha = alpha;
            }

            group.gameObject.SetActive(activeTarget);
        }
    }
}
