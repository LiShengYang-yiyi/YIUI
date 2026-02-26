using System.Collections.Generic;

namespace ET.Client
{
    /// <summary>
    /// UI通用加载组件
    /// 全局唯一 挂在YIUI管理器下
    /// </summary>
    [ComponentOf(typeof(YIUIMgrComponent))]
    public partial class YIUILoadComponent : Entity, IAwake, IDestroy
    {
    }
}
