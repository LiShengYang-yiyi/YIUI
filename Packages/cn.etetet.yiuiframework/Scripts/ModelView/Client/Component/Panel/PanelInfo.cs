using YIUIFramework;

namespace ET.Client
{
    /// <summary>
    /// 面板信息
    /// </summary>
    [EnableClass]
    public class PanelInfo
    {
        //当前UI的 ui信息
        private EntityRef<YIUIChild> m_UIBase;

        public YIUIChild UIBase => m_UIBase;

        private EntityRef<YIUIWindowComponent> m_UIWindow;

        public YIUIWindowComponent UIWindow => m_UIWindow;

        private EntityRef<YIUIPanelComponent> m_UIPanel;

        public YIUIPanelComponent UIPanel => m_UIPanel;

        //当前UI的 ET组件
        private EntityRef<Entity> m_OwnerUIEntity;

        public Entity OwnerUIEntity => m_OwnerUIEntity;

        public bool ActiveSelf => UIBase?.ActiveSelf ?? false;

        /// <summary>
        /// UI资源绑定信息
        /// </summary>
        public YIUIBindVo BindVo { get; }

        /// <summary>
        /// 包名
        /// </summary>
        public string PkgName => this.BindVo.PkgName;

        /// <summary>
        /// 资源名称 因为每个包分开 这个资源名称是有可能重复的 虽然设计上不允许  
        /// </summary>
        public string ResName => this.BindVo.ResName;

        /// <summary>
        /// C#文件名 因为有可能存在Res名称与文件名不一致的问题
        /// </summary>
        public string Name => this.BindVo.ComponentType.Name;

        /// <summary>
        /// 所在层级 如果不是panel则无效
        /// </summary>
        public EPanelLayer PanelLayer => this.BindVo.PanelLayer;

        public PanelInfo(YIUIBindVo vo)
        {
            BindVo = vo;
        }

        public void ResetUI(YIUIChild uiBase)
        {
            if (uiBase is { IsDisposed: false })
            {
                m_UIBase = uiBase;
                var window = UIBase.GetComponent<YIUIWindowComponent>();
                m_UIWindow = window != null ? window : default;
                var panel = UIBase.GetComponent<YIUIPanelComponent>();
                m_UIPanel = panel != null ? panel : default;
            }
            else
            {
                m_UIBase = default;
                m_UIWindow = default;
                m_UIPanel = default;
            }
        }

        public void ResetEntity(Entity entity)
        {
            if (entity is { IsDisposed: false })
            {
                m_OwnerUIEntity = entity;
            }
            else
            {
                m_OwnerUIEntity = default;
            }
        }

        //预加载的预制体 如果预加载的是Entity 这里则无
        private UnityEngine.GameObject m_PreLoadGameObject;

        public UnityEngine.GameObject PreLoadGameObject => m_PreLoadGameObject;

        public void ResetPreLoadGameObject(UnityEngine.GameObject go)
        {
            m_PreLoadGameObject = go;
        }
    }
}