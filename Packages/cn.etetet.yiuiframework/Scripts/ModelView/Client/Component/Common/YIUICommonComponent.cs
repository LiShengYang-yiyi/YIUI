//------------------------------------------------------------
// Author: 亦亦
// Mail: 379338943@qq.com
// Data: 2023年2月12日
//------------------------------------------------------------

namespace ET.Client
{
    /// <summary>
    /// UI通用组件
    /// </summary>
    [ComponentOf(typeof(YIUIChild))]
    public partial class YIUICommonComponent : Entity, IAwake, IYIUIInitialize, IDestroy
    {
        private EntityRef<YIUIChild> _uiBase;
        private YIUIChild            _UIBase => _uiBase;

        public YIUIChild UIBase
        {
            get
            {
                if (_UIBase == null)
                {
                    var yiuiChild = this.GetParent<YIUIChild>();
                    if (yiuiChild is { IsDisposed: false })
                    {
                        _uiBase = yiuiChild;
                    }
                }

                return _UIBase;
            }
        }
    }
}
