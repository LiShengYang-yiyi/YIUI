namespace YIUIFramework
{
    /// <summary>
    /// 通用UI其他组件
    /// </summary>
    public partial class BaseComponent : UIBase
    {
        #region 密封生命周期根据需求扩展

        protected sealed override void SealedInitialize()
        {
        }

        protected sealed override void SealedStart()
        {
        }

        protected sealed override void SealedOnDestroy()
        {
        }

        #endregion
    }
}