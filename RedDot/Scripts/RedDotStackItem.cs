using System;
using YIUIBind;
using YIUIFramework;
using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace MizuYIUI.RedDot
{
    /// <summary>
    /// Author  YIUI
    /// Date    2024.6.2
    /// </summary>
    public sealed partial class RedDotStackItem : RedDotStackItemBase
    {
        public Action ShowStackAction;
    
        #region 生命周期
        
        
        
        #endregion
        
        
        #region Event开始
       
        protected override void OnEventShowStackAction()
        {
            ShowStackAction?.Invoke();
        }
        
        #endregion Event结束
    }
}