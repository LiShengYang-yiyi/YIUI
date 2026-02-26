namespace ET.Client
{
    [FriendOf(typeof(CountDownMgr))]
    [EntitySystemOf(typeof(CountDownMgr))]
    public static partial class CountDownMgrSystem
    {
        [EntitySystem]
        private static void Awake(this CountDownMgr self)
        {
        }

        [EntitySystem]
        private static void Destroy(this CountDownMgr self)
        {
        }

        [EntitySystem]
        private static void LateUpdate(this CountDownMgr self)
        {
            self.ManagerUpdate();
        }
    }
}