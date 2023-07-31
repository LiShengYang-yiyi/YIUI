using System;

namespace YIUIFramework
{
    /// <summary>
    /// UI资源绑定信息
    /// </summary>
    public struct UIBindVo
    {
        //基类
        public Type CodeType; //Panel/View/Component

        //当前继承类
        public Type BaseType; //他继承的类 就是 当前 + Base

        //当前类
        public Type CreatorType;

        //包名/模块名
        public string PkgName;

        //资源名
        public string ResName;
    }
}