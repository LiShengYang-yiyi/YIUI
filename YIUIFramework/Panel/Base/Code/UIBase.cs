using System;
using Sirenix.OdinInspector;
using YIUIBind;
using UnityEngine;

namespace YIUIFramework
{
    /// <summary>
    /// UI基类
    /// </summary>
    [HideLabel]
    [HideReferenceObjectPicker]
    public abstract class UIBase
    {
        #region 所有table表禁止public 不允许任何外界获取

        internal  UIBindCDETable m_CDETable;
        protected UIBindCDETable CDETable => m_CDETable;

        internal  UIBindComponentTable m_ComponentTable;
        protected UIBindComponentTable ComponentTable => m_ComponentTable;

        internal  UIBindDataTable m_DataTable;
        protected UIBindDataTable DataTable => m_DataTable;

        internal  UIBindEventTable m_EventTable;
        protected UIBindEventTable EventTable => m_EventTable;

        #endregion

        /// <summary>
        /// 当前UI的预设对象
        /// </summary>
        [LabelText("UI对象")]
        public GameObject OwnerGameObject;

        /// <summary>
        /// 当前UI的Tsf
        /// </summary>
        [HideInInspector]
        public RectTransform OwnerRectTransform;

        /// <summary>
        /// 初始化状态
        /// </summary>
        private bool m_UIBaseInit;

        public bool UIBaseInit => m_UIBaseInit;

        //用这个不用.单例而已
        protected PanelMgr m_PanelMgr;

        /// <summary>
        /// UI的资源包名
        /// </summary>
        public string UIPkgName => m_UIBindVo.PkgName;

        /// <summary>
        /// UI的资源名称
        /// </summary>
        public string UIResName => m_UIBindVo.ResName;

        /// <summary>
        /// 绑定信息
        /// </summary>
        private UIBindVo m_UIBindVo;

        internal UIBindVo UIBindVo => m_UIBindVo;

        /// <summary>
        /// 当前显示状态  显示/隐藏
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
        /// 初始化UIBase 由PanelMgr创建对象后调用
        /// 外部禁止
        /// </summary>
        internal bool InitUIBase(UIBindVo uiBindVo, GameObject ownerGameObject)
        {
            if (ownerGameObject == null)
            {
                Debug.LogError($"null对象无法初始化");
                return false;
            }

            OwnerGameObject    = ownerGameObject;
            OwnerRectTransform = ownerGameObject.GetComponent<RectTransform>();
            m_CDETable         = OwnerGameObject.GetComponent<UIBindCDETable>();
            if (CDETable == null)
            {
                Debug.LogError($"{OwnerGameObject.name} 没有UIBindCDETable组件 这是必须的");
                return false;
            }

            m_ComponentTable = CDETable.ComponentTable;
            m_DataTable      = CDETable.DataTable;
            m_EventTable     = CDETable.EventTable;

            m_UIBaseInit = true;
            m_UIBindVo   = uiBindVo;
            m_PanelMgr   = PanelMgr.Inst;
            CDETable.BindUIBase(this);
            UIBaseInitialize();
            return true;
        }

        #region 公共方法

        /// <summary>
        /// 设置显隐
        /// </summary>
        public void SetActive(bool value)
        {
            if (OwnerGameObject == null) return;
            OwnerGameObject.SetActive(value);
        }

        //其他的关于 RectTransform 相关的 不建议包一层
        //就直接 OwnerRectTransform. 使用Unity API 就可以了 没必要包一成
        //这么多方法 都有可能用到你都包一层嘛

        #endregion

        #region 生命周期

        //UIBase 生命周期顺序 2
        protected virtual void UIBind()
        {
        }

        //UIBase 生命周期顺序 3
        protected virtual void Initialize()
        {
        }

        private void UIBaseInitialize()
        {
            CDETable.UIBaseStart     = UIBaseStart;
            CDETable.UIBaseOnDestroy = UIBaseOnDestroy;
            try
            {
                SealedInitialize();
                UIBind();
                Initialize();
                if (ActiveSelf)
                    UIBaseOnEnable();
                else
                    UIBaseOnDisable();
                CDETable.UIBaseOnEnable  = UIBaseOnEnable;
                CDETable.UIBaseOnDisable = UIBaseOnDisable;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                throw;
            }
        }

        //UIBase 生命周期顺序 6
        protected virtual void Start()
        {
        }

        private void UIBaseStart()
        {
            SealedStart();
            Start();
        }

        //UIBase 生命周期顺序 4
        protected virtual void OnEnable()
        {
        }

        private void UIBaseOnEnable()
        {
            OnEnable();
        }

        //UIBase 生命周期顺序 4
        protected virtual void OnDisable()
        {
        }

        private void UIBaseOnDisable()
        {
            OnDisable();
        }

        //UIBase 生命周期顺序 7
        protected virtual void UnUIBind()
        {
        }

        //UIBase 生命周期顺序 8
        protected virtual void OnDestroy()
        {
        }

        private void UIBaseOnDestroy()
        {
            UnUIBind();
            OnDestroy();
            SealedOnDestroy();
            YIUIFactory.Destroy(this);
        }

        #region 密封虚方法由(下级继承后)重写后密封 其他人可以不用关心

        //这是给基类用的生命周期(BasePanel,BaseView) 为了防止有人重写时不调用基类 所以直接独立
        //没有什么穿插需求怎么办
        //基类会重写这个类且会密封你也调用不到
        //不要问为什么...
        //UIBase 生命周期顺序 1
        protected virtual void SealedInitialize()
        {
        }

        //UIBase 生命周期顺序 5
        protected virtual void SealedStart()
        {
        }

        //UIBase 生命周期顺序 9
        protected virtual void SealedOnDestroy()
        {
        }

        #endregion

        #endregion
    }
}