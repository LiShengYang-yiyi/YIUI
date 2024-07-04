//------------------------------------------------------------
// Author: 亦亦
// Mail: 379338943@qq.com
// Data: 2023年2月12日
//------------------------------------------------------------

using ET;
using ET.Client;
using UnityEngine;

namespace YIUIFramework
{
    public static partial class YIUIFactory
    {
        private static void SetParent(RectTransform self, Transform parent)
        {
            self.SetParent(parent, false);
            self.AutoReset();
        }

        internal static Entity CreateCommon(string pkgName, string resName, GameObject obj, Entity parentEntity)
        {
            var bingVo = YIUIBindHelper.GetBindVoByPath(pkgName, resName);
            if (bingVo == null) return null;
            var vo = bingVo.Value;
            return CreateByObjVo(vo, obj, parentEntity);
        }

        internal static Entity CreatePanel(PanelInfo panelInfo, Entity parentEntity)
        {
            return Create(panelInfo.PkgName, panelInfo.ResName, parentEntity);
        }

        private static T Create<T>(Entity parentEntity) where T : Entity
        {
            var data = YIUIBindHelper.GetBindVoByType<T>();
            if (data == null) return null;
            var vo = data.Value;

            return (T)Create(vo, parentEntity);
        }

        private static Entity Create(string pkgName, string resName, Entity parentEntity)
        {
            var bingVo = YIUIBindHelper.GetBindVoByPath(pkgName, resName);
            return bingVo == null ? null : Create(bingVo.Value, parentEntity);
        }

        private static Entity Create(YIUIBindVo vo, Entity parentEntity)
        {
            var obj = YIUILoadHelper.LoadAssetInstantiate(vo.PkgName, vo.ResName);
            if (obj == null)
            {
                Debug.LogError($"没有加载到这个资源 {vo.PkgName}/{vo.ResName}");
                return null;
            }

            return CreateByObjVo(vo, obj, parentEntity);
        }

        internal static Entity CreateByObjVo(YIUIBindVo vo, GameObject obj, Entity parentEntity)
        {
            var cdeTable = obj.GetComponent<UIBindCDETable>();
            if (cdeTable == null)
            {
                Debug.LogError($"{obj.name} 没有 UIBindCDETable 组件 无法创建 请检查");
                return null;
            }

            var lastActive = obj.activeSelf;
            if (!lastActive)
            {
                //加载时必须保证处于激活状态 否则一些Mono相关未初始化导致不可预知的错误
                obj.SetActive(true);
            }

            var uiBase    = parentEntity.AddChild<YIUIComponent, YIUIBindVo, GameObject>(vo, obj);
            var component = uiBase.AddComponent(vo.ComponentType);
            cdeTable.CreateComponent(component);
            uiBase.InitOwnerUIEntity(component); //这里就是要晚于其他内部组件这样才能访问时别人已经初始化好了

            if (!lastActive)
            {
                //如果之前是隐藏的状态 则还原
                //此时已经初始化完毕 所以可能会收到被隐藏的消息 请自行酌情处理
                obj.SetActive(false);
            }

            return component;
        }

        private static void CreateComponent(this UIBindCDETable cdeTable, Entity parentEntity)
        {
            foreach (var childCde in cdeTable.AllChildCdeTable)
            {
                if (childCde == null)
                {
                    Debug.LogError($"{cdeTable.name} 存在null对象的childCde 检查是否因为删除或丢失或未重新生成");
                    continue;
                }

                var childGameObject = childCde.gameObject;
                var lastActive      = childGameObject.activeSelf;
                if (!lastActive)
                {
                    //加载时必须保证处于激活状态 否则一些Mono相关未初始化导致不可预知的错误
                    childCde.gameObject.SetActive(true);
                }

                var data = YIUIBindHelper.GetBindVoByPath(childCde.PkgName, childCde.ResName);
                if (data == null) continue;
                var vo = data.Value;

                var uiBase    = parentEntity.AddChild<YIUIComponent, YIUIBindVo, GameObject>(vo, childGameObject);
                var component = uiBase.AddComponent(vo.ComponentType);
                cdeTable.AddUIOwner(childCde.gameObject.name, component);
                childCde.CreateComponent(component);
                uiBase.InitOwnerUIEntity(component); //这里就是要晚于其他内部组件这样才能访问时别人已经初始化好了

                if (!lastActive)
                {
                    //如果之前是隐藏的状态 则还原
                    //此时已经初始化完毕 所以可能会收到被隐藏的消息 请自行酌情处理
                    childGameObject.SetActive(false);
                }
            }
        }
    }
}