namespace YIUIFramework
{
    public partial class BaseView : BaseWindow, IYIUIView
    {
        public virtual EViewWindowType ViewWindowType => EViewWindowType.View;

        public virtual EViewStackOption StackOption => EViewStackOption.Visible;

        protected sealed override void SealedInitialize()
        {
        }

        protected sealed override void SealedStart()
        {
        }

        protected sealed override void SealedOnDestroy()
        {
        }
    }
}