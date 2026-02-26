using System;
using ET;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace YIUIFramework
{
    [RequireComponent(typeof(RawImage))]
    [LabelText("RawImage 图片Raw")]
    [AddComponentMenu("YIUIBind/Data/图片Raw 【RawImage】 UIDataBindRawImage")]
    public sealed class UIDataBindRawImage : UIDataBindSelectBase
    {
        [SerializeField]
        [ReadOnly]
        [Required("必须有此组件")]
        [LabelText("图片")]
        private RawImage m_RawImage;

        [SerializeField]
        [LabelText("自动调整图像大小")]
        private bool m_SetNativeSize = false;

        [SerializeField]
        [LabelText("可修改Enabled")]
        private bool m_ChangeEnabled = true;

        [NonSerialized]
        private string m_LastResName;

        protected override int Mask()
        {
            return 1 << (int)EUIBindDataType.String;
        }

        protected override int SelectMax()
        {
            return 1;
        }

        protected override void OnRefreshData()
        {
            base.OnRefreshData();
            m_RawImage ??= GetComponent<RawImage>();
            if (!m_ChangeEnabled && !m_RawImage.enabled)
            {
                Logger.LogError($"{name} 当前禁止修改Enabled 且当前处于隐藏状态 可能会出现问题 请检查");
            }
        }

        private void SetEnabled(bool set)
        {
            if (!m_ChangeEnabled) return;

            if (m_RawImage == null) return;

            m_RawImage.enabled = set;
        }

        protected override void OnValueChanged()
        {
            if (!UIOperationHelper.IsPlaying())
            {
                return;
            }

            if (m_RawImage == null || gameObject == null) return;

            var dataValue = GetFirstValue<string>();

            if (string.IsNullOrEmpty(dataValue))
            {
                SetEnabled(false);
                return;
            }

            ChangeTexture2D(dataValue).NoContext();
        }

        private async ETTask ChangeTexture2D(string resName)
        {
            using var _ = await EventSystem.Instance?.YIUIInvokeEntityAsyncSafety<YIUIInvokeEntity_CoroutineLock, ETTask<Entity>>(YIUISingletonHelper.YIUIMgr, new YIUIInvokeEntity_CoroutineLock { Lock = GetHashCode() });

            if (m_LastResName == resName)
            {
                if (m_RawImage != null && m_RawImage.texture != null)
                {
                    SetEnabled(true);
                }
                else
                {
                    SetEnabled(false);
                }

                return;
            }

            var texture2d = await EventSystem.Instance?.YIUIInvokeEntityAsyncSafety<YIUIInvokeEntity_LoadTexture2D, ETTask<Texture2D>>(YIUISingletonHelper.YIUIMgr, new YIUIInvokeEntity_LoadTexture2D { ResName = resName });

            if (texture2d == null)
            {
                m_LastResName = "";
                SetEnabled(false);
                return;
            }

            ReleaseLastTexture2D();

            if (this == null || gameObject == null)
            {
                EventSystem.Instance?.YIUIInvokeEntitySyncSafety(YIUISingletonHelper.YIUIMgr, new YIUIInvokeEntity_Release { obj = texture2d });
                return;
            }

            if (m_RawImage == null)
            {
                EventSystem.Instance?.YIUIInvokeEntitySyncSafety(YIUISingletonHelper.YIUIMgr, new YIUIInvokeEntity_Release { obj = texture2d });
                Logger.LogError($"{resName} 加载过程中 对象被摧毁了 m_Image == null");
                return;
            }

            m_LastTexture2D = texture2d;
            m_RawImage.texture = texture2d;
            if (m_SetNativeSize)
            {
                m_RawImage.SetNativeSize();
            }

            SetEnabled(true);
            m_LastResName = resName;
        }

        protected override void UnBindData()
        {
            base.UnBindData();
            if (!UIOperationHelper.IsPlaying())
            {
                return;
            }

            ReleaseLastTexture2D();
        }

        private Texture2D m_LastTexture2D;

        private void ReleaseLastTexture2D()
        {
            if (m_LastTexture2D != null)
            {
                EventSystem.Instance?.YIUIInvokeEntitySyncSafety(YIUISingletonHelper.YIUIMgr, new YIUIInvokeEntity_Release { obj = m_LastTexture2D });
                m_LastTexture2D = null;
            }
        }
    }
}