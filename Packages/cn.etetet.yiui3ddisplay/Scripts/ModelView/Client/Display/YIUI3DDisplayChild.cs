using System;
using System.Collections.Generic;
using UnityEngine;
using YIUIFramework;

namespace ET.Client
{
    /// <summary>
    /// 3DDisplay的扩展组件
    /// 文档: https://lib9kmxvq7k.feishu.cn/wiki/FhGGwVZSyiCqHCkTVQYcKHQCnKf
    /// </summary>
    [ChildOf]
    public partial class YIUI3DDisplayChild : Entity, IAwake<UI3DDisplay>, IDestroy, IYIUIEnable, IYIUIDisable, ILateUpdate
    {
        public UI3DDisplay m_UI3DDisplay;

        public UI3DDisplay UI3DDisplay
        {
            get
            {
                if (m_UI3DDisplay == null)
                {
                    var objName = "";
                    if (this.Parent.Parent is YIUIChild parent)
                    {
                        objName = parent.OwnerGameObject.name;
                    }

                    Log.Error($"{objName} m_UI3DDisplay == null 请检查");
                    return null;
                }

                return m_UI3DDisplay;
            }
        }

        public Dictionary<string, GameObject> m_ObjPool = new();

        public Dictionary<GameObject, Dictionary<string, Camera>> m_CameraPool = new();
    }
}
