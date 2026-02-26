using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ET
{
    [DisallowMultipleComponent]
    [AddComponentMenu("")]
    public partial class YIUIGameObjectEntityRef : MonoBehaviour
    {
        public EntityRef<Entity> EntityRef;
        public Entity Entity => EntityRef;

        #if ENABLE_VIEW && UNITY_EDITOR

        [NonSerialized]
        private GameObject m_ViewGO;

        [PropertyOrder(int.MinValue)]
        [ReadOnly]
        [ShowInInspector]
        [HideInEditorMode]
        [LabelText("ViewGO")]
        public GameObject ViewGO
        {
            get
            {
                if (m_ViewGO == null)
                {
                    m_ViewGO = Entity?.ViewGO;
                }

                return m_ViewGO;
            }
        }
        #endif
    }
}