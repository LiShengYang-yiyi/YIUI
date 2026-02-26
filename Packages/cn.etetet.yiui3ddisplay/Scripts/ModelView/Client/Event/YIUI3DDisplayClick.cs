using System;
using UnityEngine;
using YIUIFramework;

namespace ET.Client
{
    public interface IYIUI3DDisplayClick
    {
        void OnClick(Entity self, UI3DDisplay display, GameObject target, GameObject root);
    }

    public interface IYIUI3DDisplayClick<in T1> : ISystemType, IYIUI3DDisplayClick
    {
    }

    [EntitySystem]
    public abstract class YIUI3DDisplayClickSystem<T1, T2, T3, T4> : SystemObject, IYIUI3DDisplayClick<T1>
            where T1 : Entity, IYIUIBind, IYIUIInitialize
    {
        Type ISystemType.Type()
        {
            return typeof(T1);
        }

        Type ISystemType.SystemType()
        {
            return typeof(IYIUI3DDisplayClick<T1>);
        }

        void IYIUI3DDisplayClick.OnClick(Entity self, UI3DDisplay display, GameObject target, GameObject root)
        {
            YIUI3DDisplayClick((T1)self, display, target, root);
        }

        protected abstract void YIUI3DDisplayClick(T1 self, UI3DDisplay display, GameObject target, GameObject root);
    }
}