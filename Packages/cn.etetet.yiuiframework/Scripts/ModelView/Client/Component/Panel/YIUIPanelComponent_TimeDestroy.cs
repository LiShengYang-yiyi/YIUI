using System;
using UnityEngine;

namespace ET.Client
{
    /// <summary>
    /// 倒计时摧毁
    /// </summary>
    public partial class YIUIPanelComponent
    {
        public float CachePanelTime = 10;

        public ETCancellationToken m_Token;
    }
}
