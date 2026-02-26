using System.Collections.Generic;
using YIUIFramework;

namespace ET.Client
{
    public partial class YIUIMgrComponent
    {
        /// <summary>
        /// 所有已经打开过的UI
        /// K = C#文件名
        /// 主要是作为缓存PanelInfo
        /// </summary>
        public readonly Dictionary<string, PanelInfo> m_PanelCfgMap = new();
    }
}
