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
        //相机的初始化位置
        public Vector3 m_ShowCameraDefPos;

        //摄像机控制器
        public UI3DDisplayCamera m_ShowCameraCtrl;

        //当前显示层级
        public int m_ShowLayer = 0;

        //显示的拖动旋转
        public float m_DragRotation;

        //显示渲染纹理
        public RenderTexture m_ShowTexture;

        //记录显示位置
        public Vector3 m_ShowPosition;

        //正交大小
        public float m_OrthographicSize;

        //每显示一次就会+1 用于位置偏移
        [StaticField]
        public static int g_DisPlayUIIndex = 0;

        //当前模型偏移位置
        public Vector3 m_ModelGlobalOffset = Vector3.zero;

        //所有已采集的阴影
        public List<Renderer> m_RenderList = new List<Renderer>();
    }
}
