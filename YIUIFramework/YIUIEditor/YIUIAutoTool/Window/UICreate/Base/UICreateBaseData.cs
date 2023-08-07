#if UNITY_EDITOR

namespace YIUIFramework.Editor
{
    public class UICreateBaseData
    {
        public bool   AutoRefresh;
        public bool   ShowTips;
        public string Namespace;     //命名空间
        public string PkgName;       //包名/模块名
        public string ResName;       //资源名 类名+Base
        public string BaseClass;     //继承什么类  BasePanel/BaseView
        public string Variables;     //变量
        public string UIBind;        //绑定方法里面的东西
        public string UIUnBind;      //解绑里面的东西
        public string VirtualMethod; //所有虚方法  Event里面的那些注册方法
        public string PanelViewEnum; //枚举生成
    }
}
#endif