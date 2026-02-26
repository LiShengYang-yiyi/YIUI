using Sirenix.OdinInspector;

namespace YIUIFramework
{
    [LabelText("过度")]
    public enum UITransitionModeEnum
    {
        [LabelText("立即")]
        Instant,

        [LabelText("淡入淡出")]
        Fade,

        [LabelText("淡入")]
        FadeIn,

        [LabelText("淡出")]
        FadeOut,
    }
}
