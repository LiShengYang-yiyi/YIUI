using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace YIUIFramework
{
    [AddComponentMenu("YIUIFramework/Widget/自适应大小 【ContentSizeFilterByRect】")]
    public class ContentSizeFilterByRect : UIBehaviour, ILayoutElement
    {
        [LabelText("自适应模式")]
        public enum FitMode
        {
            [LabelText("Width_宽")]
            Width,

            [LabelText("Height_高")]
            Height,

            [LabelText("Both_宽高")]
            Both
        }

        [SerializeField]
        protected FitMode m_Fit = FitMode.Both;

        public FitMode fit
        {
            get { return m_Fit; }
            set { m_Fit = value; }
        }

        [System.NonSerialized]
        private RectTransform m_Rect;

        private RectTransform rectTransform
        {
            get
            {
                if (m_Rect == null)
                {
                    m_Rect = GetComponent<RectTransform>();
                }
                return m_Rect;
            }
        }

        float ILayoutElement.minWidth
        {
            get { return ((ILayoutElement)this).preferredWidth; }
        }

        float ILayoutElement.preferredWidth
        {
            get
            {
                if (m_Fit == FitMode.Both || m_Fit == FitMode.Width)
                {
                    return rectTransform.rect.width;
                }
                else
                {
                    return -1;
                }
            }
        }

        float ILayoutElement.flexibleWidth
        {
            get { return -1; }
        }

        float ILayoutElement.minHeight
        {
            get { return ((ILayoutElement)this).preferredHeight; }
        }

        float ILayoutElement.preferredHeight
        {
            get
            {
                if (m_Fit == FitMode.Both || m_Fit == FitMode.Height)
                {
                    return rectTransform.rect.height;
                }
                else
                {
                    return -1;
                }
            }
        }

        float ILayoutElement.flexibleHeight
        {
            get { return -1; }
        }

        int ILayoutElement.layoutPriority
        {
            get { return 1; }
        }

        void ILayoutElement.CalculateLayoutInputHorizontal()
        {
        }

        void ILayoutElement.CalculateLayoutInputVertical()
        {
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            ForceRebuild();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            ForceRebuild();
        }

        public void ForceRebuild()
        {
            LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
        }
    }
}