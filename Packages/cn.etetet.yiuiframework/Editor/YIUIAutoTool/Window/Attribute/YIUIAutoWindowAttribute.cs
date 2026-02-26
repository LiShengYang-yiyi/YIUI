using System;

namespace YIUIFramework.Editor
{
    public class YIUIAutoMenuAttribute : Attribute
    {
        public string MenuName;

        public int Order;

        public YIUIAutoMenuAttribute(string menuName, int order = 0)
        {
            MenuName = menuName;
            Order    = order;
        }
    }

    public class YIUIAutoMenuData
    {
        public Type Type;

        public string MenuName;

        public int Order;
    }
}