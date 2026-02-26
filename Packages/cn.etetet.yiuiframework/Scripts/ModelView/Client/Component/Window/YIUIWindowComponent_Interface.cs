using System;
using UnityEngine;
using YIUIFramework;

namespace ET.Client
{
    public partial class YIUIWindowComponent
    {
        private bool m_CloseTweenEnd;

        private bool m_CheckCloseTweenEnd;

        public bool CloseTweenEnd
        {
            get
            {
                if (m_CheckCloseTweenEnd)
                {
                    return m_CloseTweenEnd;
                }

                m_CloseTweenEnd      = OwnerUIEntity is IYIUICloseTweenEnd;
                m_CheckCloseTweenEnd = true;
                return m_CloseTweenEnd;
            }
        }

        private bool m_OpenTweenEnd;

        private bool m_CheckOpenTweenEnd;

        public bool OpenTweenEnd
        {
            get
            {
                if (m_CheckOpenTweenEnd)
                {
                    return m_OpenTweenEnd;
                }

                m_OpenTweenEnd      = OwnerUIEntity is IYIUIOpenTweenEnd;
                m_CheckOpenTweenEnd = true;
                return m_OpenTweenEnd;
            }
        }

        private bool m_OpenTween;

        private bool m_CheckOpenTween;

        public bool OpenTween
        {
            get
            {
                if (m_CheckOpenTween)
                {
                    return m_OpenTween;
                }

                m_OpenTween      = OwnerUIEntity is IYIUIOpenTween;
                m_CheckOpenTween = true;
                return m_OpenTween;
            }
        }

        private bool m_CloseTween;

        private bool m_CheckCloseTween;

        public bool CloseTween
        {
            get
            {
                if (m_CheckCloseTween)
                {
                    return m_CloseTween;
                }

                m_CloseTween      = OwnerUIEntity is IYIUICloseTween;
                m_CheckCloseTween = true;
                return m_CloseTween;
            }
        }
    }
}