using System;

namespace ET.Client
{
    public interface IYIUILoopOnClickCheck
    {
        /// <summary>
        /// 点击之前调用 点击事件检查 
        /// 调用SetOnClickCheck方法设置点击事件信息后生效
        /// </summary>
        bool OnClickCheck(Entity self, Entity item, object data, int index, bool select);
    }

    public interface IYIUILoopOnClickCheck<in T1, in T2, in T3> : ISystemType, IYIUILoopOnClickCheck
    {
    }

    [EntitySystem]
    public abstract class YIUILoopOnClickCheckSystem<T1, T2, T3, T4, T5> : SystemObject, IYIUILoopOnClickCheck<T1, T2, T3>
            where T1 : Entity, IYIUIBind, IYIUIInitialize
            where T2 : Entity, IYIUIBind, IYIUIInitialize
    {
        Type ISystemType.Type()
        {
            return typeof(T1);
        }

        Type ISystemType.SystemType()
        {
            return typeof(IYIUILoopOnClickCheck<T1, T2, T3>);
        }

        bool IYIUILoopOnClickCheck.OnClickCheck(Entity self, Entity item, object data, int index, bool select)
        {
            return YIUILoopOnClickCheck((T1)self, (T2)item, (T3)data, index, select);
        }

        protected abstract bool YIUILoopOnClickCheck(T1 self, T2 item, T3 data, int index, bool select);
    }
}