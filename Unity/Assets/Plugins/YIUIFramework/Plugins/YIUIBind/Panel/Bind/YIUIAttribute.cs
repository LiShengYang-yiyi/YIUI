using System;
using ET;

namespace YIUIFramework
{
    /// <summary>
    /// 用于标记
    /// ET组件的关系 >> 关联 >> UIBase
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class YIUIAttribute: BaseAttribute
    {
        public EUICodeType YIUICodeType { get; }
        
        public YIUIAttribute(EUICodeType codeType)
        {
            YIUICodeType = codeType;
        }
    }
}