namespace ET.Client
{
    public static class YIUILoadHelper
    {
        public static YIUILoadComponent YIUILoad(this Entity entity)
        {
            return entity.YIUIMgr().GetComponent<YIUILoadComponent>();
        }

        public static YIUILoadComponent YIUILoad(this Scene scene)
        {
            return scene.YIUIMgr().GetComponent<YIUILoadComponent>();
        }
    }
}