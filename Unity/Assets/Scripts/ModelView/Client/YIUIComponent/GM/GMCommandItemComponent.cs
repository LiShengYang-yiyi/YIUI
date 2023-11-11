using YIUIFramework;

namespace ET.Client
{
    public partial class GMCommandItemComponent: Entity
    {
        public GMCommandComponent                                CommandComponent;
        public GMCommandInfo                                     Info;
        public YIUILoopScroll<GMParamInfo, GMParamItemComponent> GMParamLoop;
    }
}