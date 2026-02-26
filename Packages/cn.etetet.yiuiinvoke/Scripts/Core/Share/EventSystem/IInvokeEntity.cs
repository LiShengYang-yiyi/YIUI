using System;

namespace ET
{
    public abstract class AInvokeEntityHandler<A> : HandlerObject, IInvoke where A : struct
    {
        public Type Type
        {
            get
            {
                return typeof(A);
            }
        }

        public abstract void Handle(Entity entity, A args);
    }

    public abstract class AInvokeEntityHandler<A, T> : HandlerObject, IInvoke where A : struct
    {
        public Type Type
        {
            get
            {
                return typeof(A);
            }
        }

        public abstract T Handle(Entity entity, A args);
    }
}