namespace ET.Client
{
    public static class YIUIBindHelper
    {
        public static YIUIBindComponent YIUIBind(this Entity entity)
        {
            return entity.Root().GetComponent<YIUIMgrComponent>().GetComponent<YIUIBindComponent>();
        }

        public static YIUIBindComponent YIUIBind(this Scene scene)
        {
            return scene.Root().GetComponent<YIUIMgrComponent>().GetComponent<YIUIBindComponent>();
        }
    }
}