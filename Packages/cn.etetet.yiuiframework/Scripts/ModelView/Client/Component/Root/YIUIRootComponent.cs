//------------------------------------------------------------
// Author: 亦亦
// Mail: 379338943@qq.com
// Data: 2023年2月12日
//------------------------------------------------------------

namespace ET.Client
{
    /// <summary>
    /// 当前场景的UI管理器
    /// </summary>
    [ComponentOf]
    public partial class YIUIRootComponent : Entity, IAwake
    {
        public EntityRef<YIUIMgrComponent> m_YIUIMgrRef;
        public YIUIMgrComponent YIUIMgr => m_YIUIMgrRef;
    }
}