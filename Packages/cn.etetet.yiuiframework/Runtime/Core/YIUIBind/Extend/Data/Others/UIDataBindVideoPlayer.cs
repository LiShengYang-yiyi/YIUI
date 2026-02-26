using System;
using ET;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Video;
using UnityObject = UnityEngine.Object;

namespace YIUIFramework
{
    [RequireComponent(typeof(VideoPlayer))]
    [LabelText("VideoPlayer 视频播放器")]
    [AddComponentMenu("YIUIBind/Data/视频播放器 【VideoPlayer】 UIDataBindVideoPlayer")]
    public sealed class UIDataBindVideoPlayer : UIDataBindSelectBase
    {
        [SerializeField]
        [ReadOnly]
        [Required("必须有此组件")]
        [LabelText("视频播放器")]
        private VideoPlayer m_VideoPlayer;

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
            m_VideoPlayer ??= GetComponent<VideoPlayer>();
            if (!m_ChangeEnabled && !m_VideoPlayer.enabled)
            {
                Logger.LogError($"{name} 当前禁止修改Enabled 且当前处于隐藏状态 可能会出现问题 请检查");
            }
        }

        private void SetEnabled(bool set)
        {
            if (!m_ChangeEnabled) return;

            if (m_VideoPlayer == null) return;

            m_VideoPlayer.enabled = set;
        }

        protected override void OnValueChanged()
        {
            if (!UIOperationHelper.IsPlaying())
            {
                return;
            }

            if (this.m_VideoPlayer == null || gameObject == null) return;

            var dataValue = GetFirstValue<string>();

            if (string.IsNullOrEmpty(dataValue))
            {
                SetEnabled(false);
                return;
            }

            this.ChangeAudio(dataValue).NoContext();
        }

        private async ETTask ChangeAudio(string resName)
        {
            using var _ = await EventSystem.Instance?.YIUIInvokeEntityAsyncSafety<YIUIInvokeEntity_CoroutineLock, ETTask<Entity>>(YIUISingletonHelper.YIUIMgr, new YIUIInvokeEntity_CoroutineLock { Lock = GetHashCode() });

            if (m_LastResName == resName)
            {
                if (this.m_VideoPlayer != null && this.m_VideoPlayer.clip != null)
                {
                    SetEnabled(true);
                }
                else
                {
                    SetEnabled(false);
                }

                return;
            }

            var loadResult = await EventSystem.Instance?.YIUIInvokeEntityAsyncSafety<YIUIInvokeEntity_Load, ETTask<UnityObject>>(YIUISingletonHelper.YIUIMgr, new YIUIInvokeEntity_Load
            {
                LoadType = typeof(VideoClip),
                ResName  = resName
            });

            if (loadResult == null)
            {
                m_LastResName = "";
                SetEnabled(false);
                return;
            }

            this.ReleaseLastAudioClip();

            if (this == null || gameObject == null)
            {
                EventSystem.Instance?.YIUIInvokeEntitySyncSafety(YIUISingletonHelper.YIUIMgr, new YIUIInvokeEntity_Release { obj = loadResult });
                return;
            }

            if (m_VideoPlayer == null)
            {
                EventSystem.Instance?.YIUIInvokeEntitySyncSafety(YIUISingletonHelper.YIUIMgr, new YIUIInvokeEntity_Release { obj = loadResult });
                Logger.LogError($"{resName} 加载过程中 对象被摧毁了 m_VideoPlayer == null");
                return;
            }

            var videoClip = loadResult as VideoClip;
            this.m_LastVideoClip    = videoClip;
            this.m_VideoPlayer.clip = videoClip;

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

            this.ReleaseLastAudioClip();
        }

        private VideoClip m_LastVideoClip;

        private void ReleaseLastAudioClip()
        {
            if (this.m_LastVideoClip != null)
            {
                EventSystem.Instance?.YIUIInvokeEntitySyncSafety(YIUISingletonHelper.YIUIMgr, new YIUIInvokeEntity_Release { obj = this.m_LastVideoClip });
                this.m_LastVideoClip = null;
            }
        }
    }
}