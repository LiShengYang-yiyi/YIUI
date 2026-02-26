using System;

namespace ET.Client
{
    public interface IYIUILoopRenderer
    {
        /// <summary>
        /// 渲染数据项
        /// </summary>
        /// <param name="self">渲染器实体</param>
        /// <param name="item">显示对象</param>
        /// <param name="data">数据项</param>
        /// <param name="index">数据的索引</param>
        /// <param name="select">是否被选中</param>
        void Renderer(Entity self, Entity item, object data, int index, bool select);
    }

    public interface IYIUILoopRenderer<in T1, in T2, in T3> : ISystemType, IYIUILoopRenderer
    {
    }

    [EntitySystem]
    public abstract class YIUILoopRendererSystem<T1, T2, T3, T4, T5> : SystemObject, IYIUILoopRenderer<T1, T2, T3>
            where T1 : Entity, IYIUIBind, IYIUIInitialize
            where T2 : Entity, IYIUIBind, IYIUIInitialize
    {
        Type ISystemType.Type()
        {
            return typeof(T1);
        }

        Type ISystemType.SystemType()
        {
            return typeof(IYIUILoopRenderer<T1, T2, T3>);
        }

        void IYIUILoopRenderer.Renderer(Entity self, Entity item, object data, int index, bool select)
        {
            YIUILoopRenderer((T1)self, (T2)item, (T3)data, index, select);
        }

        protected abstract void YIUILoopRenderer(T1 self, T2 item, T3 data, int index, bool select);
    }
}