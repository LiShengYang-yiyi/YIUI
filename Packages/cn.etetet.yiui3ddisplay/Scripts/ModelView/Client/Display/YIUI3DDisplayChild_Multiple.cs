using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using YIUIFramework;

namespace ET.Client
{
    /// <summary>
    /// 3DDisplay的扩展组件
    /// 文档: https://lib9kmxvq7k.feishu.cn/wiki/FhGGwVZSyiCqHCkTVQYcKHQCnKf
    /// </summary>
    public partial class YIUI3DDisplayChild
    {
        //可动态设置改变多目标 但是尽量不要
        public bool MultipleTargetMode
        {
            get => UI3DDisplay.m_MultipleTargetMode;
            set => UI3DDisplay.m_MultipleTargetMode = value;
        }

        public List<GameObject> m_AllMultipleTarget;

        public Dictionary<GameObject, GameObject> m_MultipleCache;

        public bool m_InitMultipleData = false;
    }
}
