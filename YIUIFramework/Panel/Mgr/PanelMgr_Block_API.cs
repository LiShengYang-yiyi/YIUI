using UnityEngine;

namespace YIUIFramework
{
    public partial class PanelMgr
    {
        //当前层级屏蔽操作状态 true = 显示 = 无法操作 不要与可操作搞混
        public bool LayerBlockActiveSelf => m_LayerBlock.activeSelf;

        //当前UI是否可以操作 true = 可以操作
        public bool CanLayerBlockOption => !LayerBlockActiveSelf;

        //如果当前被屏蔽了操作 下一次可以操作的时间是什么时候基于 Time.unscaledTime;
        //=0 不表示可以操作  也有可能被永久屏蔽了
        //可单独判断是否永久屏蔽
        //也可以使用上面的方法 CanLayerBlockOption  也可以得到是否被屏蔽
        public float LastRecoverOptionTime => m_LastRecoverOptionTime;

        //如果当前被屏蔽了操作 可以拿到还有多久操作会恢复
        public float GetLastRecoverOptionResidueTime()
        {
            if (CanLayerBlockOption)
            {
                return 0;
            }

            return LastRecoverOptionTime - Time.unscaledTime;
        }

        /// <summary>
        /// 禁止层级操作
        /// </summary>
        /// <param name="time">需要禁止的时间</param>
        public void BanLayerOption(float time = 1f)
        {
            BanLayerOptionCountDown(time);
        }
        
        /// <summary>
        /// 强制恢复层级到可操作状态
        /// 此方法会强制打断倒计时 
        /// 清除所有永久屏蔽
        /// 根据需求调用
        /// </summary>
        public void RecoverLayerOption()
        {
            RecoverLayerOptionAll();
        }
    }
}