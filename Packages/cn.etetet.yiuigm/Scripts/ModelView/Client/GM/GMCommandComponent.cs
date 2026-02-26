using System.Collections.Generic;

namespace ET.Client
{
    /// <summary>
    /// 文档: https://lib9kmxvq7k.feishu.cn/wiki/NYADwMydliVmQ7kWXOuc0yxGn7p
    /// </summary>
    [ComponentOf(typeof (Scene))]
    public class GMCommandComponent: Entity, IAwake, IDestroy
    {
        public Dictionary<int, List<GMCommandInfo>> AllCommandInfo { get; set; }
    }

    //GM相关消息 关闭GMView
    public struct OnGMEventClose
    {
    }
}