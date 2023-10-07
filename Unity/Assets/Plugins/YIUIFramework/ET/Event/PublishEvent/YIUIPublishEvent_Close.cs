namespace ET.Client
{
    //UI消息  有UI被关闭之前 (有人调用了关闭XXpanel时)
    public struct YIUIEventPanelCloseBefore
    {
        public string UIPkgName;       //所在包名
        public string UIResName;       //资源名称
        public string UIComponentName; //组件名称
    }

    //UI消息  有UI被关闭之后 (已经完成了所有加载包括动画后) 被摧毁前
    public struct YIUIEventPanelCloseAfter
    {
        public string UIPkgName;       //所在包名
        public string UIResName;       //资源名称
        public string UIComponentName; //组件名称
    }
    
    //UI消息  被摧毁
    public struct YIUIEventPanelDestroy
    {
        public string UIPkgName;       //所在包名
        public string UIResName;       //资源名称
        public string UIComponentName; //组件名称
    }
}