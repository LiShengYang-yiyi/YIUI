#if !ET10
using System;
using System.Diagnostics;

namespace ET
{
    [Conditional("ET10")]
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class IgnoreCircularDependencyAttribute : Attribute
    {
    }
}
#endif