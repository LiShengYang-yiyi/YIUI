using System;

namespace YIUIFramework
{
    /// <summary>
    /// UI资源绑定信息
    /// </summary>
    public struct YIUIBindVo
    {
        //当前UI类型
        public EUICodeType CodeType;
        
        //ET的类
        public Type ComponentType; 

        //包名/模块名
        public string PkgName;

        //资源名
        public string ResName;
    }
}