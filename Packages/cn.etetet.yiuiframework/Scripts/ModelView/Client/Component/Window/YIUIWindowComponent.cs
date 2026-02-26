//------------------------------------------------------------
// Author: 亦亦
// Mail: 379338943@qq.com
// Data: 2023年2月12日
//------------------------------------------------------------

using System;
using UnityEngine;
using YIUIFramework;

namespace ET.Client
{
    /// <summary>
    /// UI窗口组件
    /// </summary>
    [ComponentOf(typeof(YIUIChild))]
    public partial class YIUIWindowComponent : Entity, IAwake, IDestroy
    {
        public YIUIBindVo UIBindVo;

        private EntityRef<YIUIChild> _uiBase;
        private YIUIChild            _UIBase => _uiBase;

        public YIUIChild UIBase
        {
            get
            {
                if (_UIBase == null)
                {
                    var yiuiChild = this.GetParent<YIUIChild>();
                    if (yiuiChild is { IsDisposed: false })
                    {
                        _uiBase = yiuiChild;
                    }
                    else
                    {
                        _uiBase = default;
                    }
                }

                return _UIBase;
            }
        }

        private EntityRef<Entity> _ownerUIEntity;
        private Entity            _OwnerUIEntity => _ownerUIEntity;

        public Entity OwnerUIEntity
        {
            get
            {
                if (_OwnerUIEntity == null)
                {
                    var uiEntity = UIBase?.OwnerUIEntity;
                    if (uiEntity is { IsDisposed: false })
                    {
                        _ownerUIEntity = uiEntity;
                    }
                    else
                    {
                        _ownerUIEntity = default;
                    }
                }

                return _OwnerUIEntity;
            }
        }

        /// <summary>
        /// 窗口选项
        /// 实现各种配置
        /// 如非必要 所有属性判断可以直接去HasFlag分类中获取 不要直接判断这个
        /// </summary>
        public EWindowOption WindowOption = EWindowOption.None;

        public bool m_FirstOpenTween;
        public bool m_FirstCloseTween;
    }
}