using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

namespace YIUIFramework
{
    [AddComponentMenu("YIUIFramework/红点/红点Text绑定 【RedDotTextBind】")]
    public class RedDotTextBind : RedDotBind
    {
        [PropertyOrder(int.MinValue)]
        [SerializeField]
        [LabelText("文本")]
        [EnableIf("@UIOperationHelper.CommonShowIf()")]
        private Text m_Text;

        protected override void ChangeText()
        {
            if (m_Text != null)
            {
                m_Text.text = Count.ToString();
            }
        }
    }
}