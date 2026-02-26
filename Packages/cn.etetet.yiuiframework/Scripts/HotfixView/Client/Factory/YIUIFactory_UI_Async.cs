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
        public static async ETTask<T> InstantiateAsync<T>(Scene scene, Entity parentEntity, Transform parent = null) where T : Entity
        {
            var data = parentEntity.YIUIBind().GetBindVoByType<T>();
            if (data == null) return null;
            var vo = data.Value;

            return await InstantiateAsync<T>(scene, vo, parentEntity, parent);
        }

        public static async ETTask<T> InstantiateAsync<T>(Scene scene, YIUIBindVo vo, Entity parentEntity, Transform parent = null) where T : Entity
        {
            EntityRef<Scene> sceneRef = scene;
            var uiCom = await CreateAsync(scene, vo, parentEntity);
            scene = sceneRef;
            SetParent(uiCom.GetParent<YIUIChild>().OwnerRectTransform, parent ? parent : scene.YIUIMgr().UICache);
            return (T)uiCom;
        }

        public static async ETTask<Entity> InstantiateAsync(Scene scene, YIUIBindVo vo, Entity parentEntity, Transform parent = null)
        {
            EntityRef<Scene> sceneRef = scene;
            var uiCom = await CreateAsync(scene, vo, parentEntity);
            scene = sceneRef;
            SetParent(uiCom.GetParent<YIUIChild>().OwnerRectTransform, parent ? parent : scene.YIUIMgr().UICache);
            return uiCom;
        }

        public static async ETTask<Entity> InstantiateAsync(Scene scene, Type uiType, Entity parentEntity, Transform parent = null)
        {
            var data = parentEntity.YIUIBind().GetBindVoByType(uiType);
            if (data == null) return null;
            var vo = data.Value;

            return await InstantiateAsync(scene, vo, parentEntity, parent);
        }

        public static async ETTask<Entity> InstantiateAsync(Scene scene,
                                                            string pkgName,
                                                            string resName,
                                                            Entity parentEntity,
                                                            Transform parent = null)
        {
            var data = parentEntity.YIUIBind().GetBindVoByPath(pkgName, resName);
            if (data == null) return null;
            var vo = data.Value;

            return await InstantiateAsync(scene, vo, parentEntity, parent);
        }

        public static async ETTask<Entity> InstantiateAsync(Scene scene,
                                                            string resName,
                                                            Entity parentEntity,
                                                            Transform parent = null)
        {
            var data = parentEntity.YIUIBind().GetBindVoByResName(resName);
            if (data == null) return null;
            var vo = data.Value;

            return await InstantiateAsync(scene, vo, parentEntity, parent);
        }

        public static async ETTask<Entity> CreatePanelAsync(Scene scene, PanelInfo panelInfo, Entity parentEntity)
        {
            var bingVo = parentEntity.YIUIBind().GetBindVoByPath(panelInfo.PkgName, panelInfo.ResName);
            if (bingVo == null) return null;
            Entity panelEntity = null;
            if (panelInfo.PreLoadGameObject == null)
            {
                panelEntity = await CreateAsync(scene, bingVo.Value, parentEntity);
            }
            else
            {
                panelEntity = CreateByObjVo(bingVo.Value, panelInfo.PreLoadGameObject, parentEntity);
                panelInfo.ResetPreLoadGameObject(null);
            }

            return panelEntity;
        }

        public static async ETTask<Entity> CreateAsync(Scene scene, YIUIBindVo vo, Entity parentEntity)
        {
            EntityRef<Entity> parentEntityRef = parentEntity;
            var obj = await scene.YIUILoad()?.LoadAssetAsyncInstantiate(vo.PkgName, vo.ResName);
            if (obj == null)
            {
                Debug.LogError($"没有加载到这个资源 {vo.PkgName}/{vo.ResName}");
                return null;
            }

            return CreateByObjVo(vo, obj, parentEntityRef);
        }

        internal static async ETTask<GameObject> CreatePanelGameObjectAsync(Scene scene, PanelInfo panelInfo)
        {
            var bingVo = scene.YIUIBind().GetBindVoByPath(panelInfo.PkgName, panelInfo.ResName);
            if (bingVo == null) return null;
            return await CreateGameObjectAsync(scene, bingVo.Value);
        }

        internal static async ETTask<GameObject> CreateGameObjectAsync(Scene scene, YIUIBindVo vo)
        {
            var obj = await scene.YIUILoad()?.LoadAssetAsyncInstantiate(vo.PkgName, vo.ResName);
            if (obj == null)
            {
                Debug.LogError($"没有加载到这个资源 {vo.PkgName}/{vo.ResName}");
                return null;
            }

            return obj;
        }
    }
}