//------------------------------------------------------------
// Author: 亦亦
// Mail: 379338943@qq.com
// Data: 2023年2月12日
//------------------------------------------------------------

using Sirenix.OdinInspector;
using UnityEngine;
using YIUIFramework;

namespace ET.Client
{
    /// <summary>
    /// UI主体
    /// </summary>
    [ChildOf]
    public partial class YIUIChild : Entity, IAwake<YIUIBindVo, GameObject>, IDestroy
    {
        /// <summary>
        /// UI的资源包名
        /// </summary>
        public string UIPkgName => m_UIBindVo.PkgName;

        /// <summary>
        /// UI的资源名称
        /// </summary>
        public string UIResName { get; set; }

        public UIBindCDETable       CDETable       { get; set; }
        public UIBindComponentTable ComponentTable { get; set; }
        public UIBindDataTable      DataTable      { get; set; }
        public UIBindEventTable     EventTable     { get; set; }

        /// <summary>
        /// 当前UI的预设对象
        /// </summary>
        [LabelText("UI对象")]
        public GameObject OwnerGameObject { get; set; }

        /// <summary>
        /// 当前UI的Tsf
        /// </summary>
        [HideInInspector]
        public RectTransform OwnerRectTransform { get; set; }

        /// <summary>
        /// 初始化状态
        /// </summary>
        public bool m_UIBaseInit;

        public bool UIBaseInit => m_UIBaseInit;

        public EntityRef<Entity> m_OwnerUIEntity;

        //被实例化后的UIEntity对象
        //我的XXComponent 不是当前的这个UIComponent
        public Entity OwnerUIEntity => m_OwnerUIEntity;

        /// <summary>
        /// UI名称 用于开关UI = componentName
        /// </summary>
        public string UIName => this.m_UIBindVo.ComponentType.Name;

        /// <summary>
        /// 绑定信息
        /// </summary>
        public YIUIBindVo m_UIBindVo;

        public YIUIBindVo UIBindVo => m_UIBindVo;

        /// <summary>
        /// 对象本身是否被激活
        /// 不要使用这个设置显影
        /// 应该使用控制器 或调用方法 SetActive();
        /// </summary>
        public bool ActiveSelf
        {
            get
            {
                if (OwnerGameObject == null) return false;
                return OwnerGameObject.activeSelf;
            }
        }

        /// <summary>
        /// 对象及其所有父对象是否都被激活
        /// </summary>
        public bool ActiveInHierarchy
        {
            get
            {
                if (OwnerGameObject == null) return false;
                return OwnerGameObject.activeInHierarchy;
            }
        }
        
    }
}
