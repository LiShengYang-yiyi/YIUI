using YIUIFramework;

namespace ET.Client
{
    //一个UI的信息 在不同环境下表达的意境不同
    public struct YIUIPanelInfo
    {
        public string      UIPkgName;       //所在包名
        public string      UIResName;       //资源名称
        public string      UIComponentName; //组件名称
        public EPanelLayer PanelLayer;      //所在层级
    }
}