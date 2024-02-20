using System.Collections;
using System.Diagnostics;

namespace ET.Server
{
    [EntitySystemOf(typeof(WatcherComponent))]
    [FriendOf(typeof(WatcherComponent))]
    public static partial class WatcherComponentSystem
    {
        [EntitySystem]
        public static void Awake(this WatcherComponent self)
        {
            string[] localIP = NetworkHelper.GetAddressIPs();
            var processConfigs = StartProcessConfigCategory.Instance.DataList;
            foreach (StartProcessConfig startProcessConfig in processConfigs)
            {
                if (!WatcherHelper.IsThisMachine(startProcessConfig.InnerIP, localIP))
                {
                    continue;
                }
                System.Diagnostics.Process process = WatcherHelper.StartProcess(startProcessConfig.Id);
                self.Processes.Add(startProcessConfig.Id, process);
            }
        }
    }
}