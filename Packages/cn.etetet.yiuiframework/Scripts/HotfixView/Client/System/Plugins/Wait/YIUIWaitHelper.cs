namespace ET.Client
{
    public static class YIUIWaitHelper
    {
        public static void NotifyWait(this YIUIWindowComponent self, EHashWaitError error)
        {
            var waitComponent = self.GetComponent<YIUIWaitComponent>();
            if (waitComponent == null) return;
            waitComponent.Notify(error);
        }
    }
}