//------------------------------------------------------------
// Author: 亦亦
// Mail: 379338943@qq.com
// Data: 2023年2月12日
//------------------------------------------------------------

using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace YIUIFramework
{
    public static partial class YIUIFactory
    {
        public static T Instantiate<T>(RectTransform parent = null) where T : UIBase
        {
            var data = UIBindHelper.GetBindVoByType<T>();
            if (data == null) return null;
            var vo = data.Value;

            return Instantiate<T>(vo, parent);
        }

        public static T Instantiate<T>(UIBindVo vo, RectTransform parent = null) where T : UIBase
        {
            var instance = (T)Create(vo);
            if (instance == null) return null;

            SetParent(instance.OwnerRectTransform, parent ? parent : PanelMgr.Inst.UICache);

            return instance;
        }

        public static UIBase Instantiate(Type uiType, RectTransform parent = null)
        {
            var data = UIBindHelper.GetBindVoByType(uiType);
            if (data == null) return null;
            var vo = data.Value;

            return Instantiate(vo, parent);
        }

        public static UIBase Instantiate(UIBindVo vo, RectTransform parent = null)
        {
            var instance = Create(vo);
            if (instance == null) return null;

            SetParent(instance.OwnerRectTransform, parent ? parent : PanelMgr.Inst.UICache);

            return instance;
        }

        public static UIBase Instantiate(string pkgName, string resName, RectTransform parent = null)
        {
            var data = UIBindHelper.GetBindVoByPath(pkgName, resName);
            if (data == null) return null;
            var vo = data.Value;

            return Instantiate(vo, parent);
        }
    }
}