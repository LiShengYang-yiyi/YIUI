namespace ET.Client
{
    public static class YIUICountDownHelper
    {
        public static CountDownMgr YIUICountDown(this Entity entity)
        {
            return entity.YIUIMgr().GetComponent<CountDownMgr>();
        }

        public static CountDownMgr YIUICountDown(this Scene scene)
        {
            return scene.YIUIMgr().GetComponent<CountDownMgr>();
        }
    }
}