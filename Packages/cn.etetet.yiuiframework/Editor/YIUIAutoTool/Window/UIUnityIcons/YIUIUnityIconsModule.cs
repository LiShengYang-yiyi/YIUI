using Sirenix.OdinInspector;

namespace YIUIFramework.Editor
{
    [YIUIAutoMenu("系统图标", int.MaxValue)]
    public class YIUIUnityIconsModule : BaseYIUIToolModule
    {
        [Button("系统图标", 30, Icon = SdfIconType.Grid3x3GapFill, IconAlignment = IconAlignment.LeftOfText)]
        [PropertyOrder(-99999)]
        public void OpenWindow()
        {
            UnityIconsWindow.ShowWindow();
        }

        public override void Initialize()
        {
            UnityIconsWindow.ShowWindow();
        }

        public override void OnDestroy()
        {
        }
    }
}