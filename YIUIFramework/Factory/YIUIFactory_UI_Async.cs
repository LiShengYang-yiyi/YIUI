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
        public static async UniTask<T> InstantiateAsync<T>(RectTransform parent = null) where T : UIBase
        {
            var data = UIBindHelper.GetBindVoByType<T>();
            if (data == null) return null;
            var vo = data.Value;

            return await InstantiateAsync<T>(vo, parent);
        }

        public static async UniTask<T> InstantiateAsync<T>(UIBindVo vo, RectTransform parent = null) where T : UIBase
        {
            var uiBase = await CreateAsync(vo);
            SetParent(uiBase.OwnerRectTransform, parent ? parent : PanelMgr.Inst.UICache);
            return (T)uiBase;
        }

        public static async UniTask<UIBase> InstantiateAsync(UIBindVo vo, RectTransform parent = null)
        {
            var uiBase = await CreateAsync(vo);
            SetParent(uiBase.OwnerRectTransform, parent ? parent : PanelMgr.Inst.UICache);
            return uiBase;
        }

        public static async UniTask<UIBase> InstantiateAsync(Type uiType, RectTransform parent = null)
        {
            var data = UIBindHelper.GetBindVoByType(uiType);
            if (data == null) return null;
            var vo = data.Value;

            return await InstantiateAsync(vo, parent);
        }

        public static async UniTask<UIBase> InstantiateAsync(string        pkgName, string resName,
                                                             RectTransform parent = null)
        {
            var data = UIBindHelper.GetBindVoByPath(pkgName, resName);
            if (data == null) return null;
            var vo = data.Value;

            return await InstantiateAsync(vo, parent);
        }

        internal static async UniTask<UIBase> CreatePanelAsync(PanelInfo panelInfo)
        {
            var bingVo = UIBindHelper.GetBindVoByPath(panelInfo.PkgName, panelInfo.ResName);
            if (bingVo == null) return null;
            var uiBase = await CreateAsync(bingVo.Value);
            return uiBase;
        }

        private static async UniTask<UIBase> CreateAsync(UIBindVo vo)
        {
            var obj = await YIUILoadHelper.LoadAssetAsyncInstantiate(vo.PkgName, vo.ResName);
            if (obj == null)
            {
                Debug.LogError($"没有加载到这个资源 {vo.PkgName}/{vo.ResName}");
                return null;
            }

            return CreateByObjVo(vo, obj);
        }
    }
}