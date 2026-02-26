namespace ET
{
    /// <summary>
    /// 特殊情况下,已知且允许可null时使用
    /// </summary>
    public static class EntityRefHelper
    {
        public static EntityRef<T> GetEntityRefSafety<T>(T entity) where T : Entity
        {
            if (entity == null)
            {
                return new EntityRef<T>();
            }

            EntityRef<T> entityRef = entity;
            return entityRef;
        }

        public static EntityWeakRef<T> GetEntityWeakRefSafety<T>(T entity) where T : Entity
        {
            if (entity == null)
            {
                return new EntityWeakRef<T>();
            }

            EntityWeakRef<T> entityWeakRef = entity;
            return entityWeakRef;
        }
    }
}