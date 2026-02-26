#if UNITY_EDITOR

using System.Collections.Generic;

namespace YIUIFramework.Editor
{
    internal class RedDotLinkData
    {
        internal int Key;

        internal bool ConfigSet;

        internal List<int> LinkKey = new List<int>();
    }
}

#endif