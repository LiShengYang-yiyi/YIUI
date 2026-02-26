using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using YIUIFramework;

namespace ET.Client
{
    /// <summary>
    /// 3DDisplay的扩展组件
    /// 文档: https://lib9kmxvq7k.feishu.cn/wiki/FhGGwVZSyiCqHCkTVQYcKHQCnKf
    /// </summary>
    public partial class YIUI3DDisplayChild
    {
        //可拖拽目标
        public GameObject m_DragTarge;

        public bool CanDrag
        {
            get => UI3DDisplay.m_CanDrag;
            set => UI3DDisplay.m_CanDrag = value;
        }

        public Vector2           m_OnClickDownPos = Vector2.zero;
        public RaycastHit        m_ClickRaycastHit;
        public RaycastHit        m_DragRaycastHit;
        public EntityRef<Entity> m_OnClickedEntity; //被点击的触发实体

        public Entity OnClickedEntity => m_OnClickedEntity;

        private Type m_IYIUI3DDisplayClickTypeSystem;

        public Type YIUI3DDisplayClickTypeSystem
        {
            get
            {
                return m_IYIUI3DDisplayClickTypeSystem ??= typeof(IYIUI3DDisplayClick<>).MakeGenericType(OnClickedEntity?.GetType());
            }
        }

        //需要手动开启是否需要点击事件
        private bool m_OnClick;

        public bool OnClick
        {
            get => m_OnClick;
            set
            {
                m_OnClick = value;
                if (UI3DDisplay)
                {
                    UI3DDisplay.OnClick = value;
                }
            }
        }
    }
}