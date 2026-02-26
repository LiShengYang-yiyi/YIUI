#if !YIUIMACRO_SYNCLOAD_CLOSE
//------------------------------------------------------------
// Author: 亦亦
// Mail: 379338943@qq.com
// Data: 2023年2月12日
//------------------------------------------------------------

using System;
using UnityEngine;
using YIUIFramework;

namespace ET.Client
{
    public static partial class YIUIFactory
    {
        internal static Entity CreateCommon(string pkgName, string resName, GameObject obj, Entity parentEntity)
        {
            var bingVo = parentEntity.YIUIBind().GetBindVoByPath(pkgName, resName);
            if (bingVo == null) return null;
            var vo = bingVo.Value;
            return CreateByObjVo(vo, obj, parentEntity);
        }

        internal static Entity CreatePanel(Scene scene, PanelInfo panelInfo, Entity parentEntity)
        {
            return Create(scene, panelInfo.PkgName, panelInfo.ResName, parentEntity);
        }

        private static T Create<T>(Scene scene, Entity parentEntity) where T : Entity
        {
            var data = parentEntity.YIUIBind().GetBindVoByType<T>();
            if (data == null) return null;
            var vo = data.Value;

            return (T)Create(scene, vo, parentEntity);
        }

        private static Entity Create(Scene scene, string pkgName, string resName, Entity parentEntity)
        {
            var bingVo = parentEntity.YIUIBind().GetBindVoByPath(pkgName, resName);
            return bingVo == null ? null : Create(scene, bingVo.Value, parentEntity);
        }

        private static Entity Create(Scene scene, YIUIBindVo vo, Entity parentEntity)
        {
            var obj = scene.YIUILoad()?.LoadAssetInstantiate(vo.PkgName, vo.ResName);
            if (obj == null)
            {
                Debug.LogError($"没有加载到这个资源 {vo.PkgName}/{vo.ResName}");
                return null;
            }

            return CreateByObjVo(vo, obj, parentEntity);
        }

        public static T Instantiate<T>(Scene scene, Entity parentEntity, Transform parent = null) where T : Entity
        {
            var data = scene.YIUIBind().GetBindVoByType<T>();
            if (data == null) return null;
            var vo = data.Value;

            return Instantiate<T>(scene, vo, parentEntity, parent);
        }

        public static T Instantiate<T>(Scene scene, YIUIBindVo vo, Entity parentEntity, Transform parent = null) where T : Entity
        {
            var instance = (T)Create(scene, vo, parentEntity);
            if (instance == null) return null;

            SetParent(instance.GetParent<YIUIChild>().OwnerRectTransform, parent ? parent : scene.YIUIMgr().UICache);

            return instance;
        }

        public static Entity Instantiate(Scene scene, Type uiType, Entity parentEntity, Transform parent = null)
        {
            var data = scene.YIUIBind().GetBindVoByType(uiType);
            if (data == null) return null;
            var vo = data.Value;

            return Instantiate(scene, vo, parentEntity, parent);
        }

        public static Entity Instantiate(Scene scene, YIUIBindVo vo, Entity parentEntity, Transform parent = null)
        {
            var instance = Create(scene, vo, parentEntity);
            if (instance == null) return null;

            SetParent(instance.GetParent<YIUIChild>().OwnerRectTransform, parent ? parent : scene.YIUIMgr().UICache);

            return instance;
        }

        public static Entity Instantiate(Scene scene, string pkgName, string resName, Entity parentEntity, Transform parent = null)
        {
            var data = scene.YIUIBind().GetBindVoByPath(pkgName, resName);
            if (data == null) return null;
            var vo = data.Value;

            return Instantiate(scene, vo, parentEntity, parent);
        }

        public static Entity Instantiate(Scene scene, string resName, Entity parentEntity, Transform parent = null)
        {
            var data = scene.YIUIBind().GetBindVoByResName(resName);
            if (data == null) return null;
            var vo = data.Value;

            return Instantiate(scene, vo, parentEntity, parent);
        }
    }
}
#endif