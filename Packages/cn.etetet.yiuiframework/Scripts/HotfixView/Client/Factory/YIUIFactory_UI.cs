//------------------------------------------------------------
// Author: 亦亦
// Mail: 379338943@qq.com
// Data: 2023年2月12日
//------------------------------------------------------------

using UnityEngine;
using YIUIFramework;

namespace ET.Client
{
    public static partial class YIUIFactory
    {
        private static void SetParent(RectTransform self, Transform parent)
        {
            self.SetParent(parent, false);
            self.AutoReset();
        }

        [EnableAccessEntiyChild]
        internal static Entity CreateByObjVo(YIUIBindVo vo, GameObject obj, Entity parentEntity)
        {
            var cdeTable = obj.GetComponent<UIBindCDETable>();
            if (cdeTable == null)
            {
                Debug.LogError($"{obj.name} 没有 UIBindCDETable 组件 无法创建 请检查");
                return null;
            }

            // 显式初始化 CDE 表，解决同一帧内激活-关闭导致 Awake 不触发的问题
            cdeTable.InitializeCDE();

            if (parentEntity is { IScene: null })
            {
                Log.Error($"{parentEntity.GetType()} 不是场景实体 无法创建");
                return null;
            }

            var uiBase = parentEntity.AddChild<YIUIChild, YIUIBindVo, GameObject>(vo, obj);
            var component = uiBase.AddComponent(vo.ComponentType);
            cdeTable.CreateComponent(component);
            uiBase.InitOwnerUIEntity(component); //这里就是要晚于其他内部组件这样才能访问时别人已经初始化好了

            return component;
        }

        [EnableAccessEntiyChild]
        private static void CreateComponent(this UIBindCDETable cdeTable, Entity parentEntity)
        {
            foreach (var childCde in cdeTable.AllChildCdeTable)
            {
                if (childCde == null)
                {
                    Debug.LogError($"{cdeTable.name} 存在null对象的childCde 检查是否因为删除或丢失或未重新生成");
                    continue;
                }

                // 显式初始化子组件的 CDE 表
                childCde.InitializeCDE();

                var childGameObject = childCde.gameObject;
                var data = parentEntity.YIUIBind().GetBindVoByPath(childCde.PkgName, childCde.ResName);
                if (data == null) continue;
                var vo = data.Value;

                var uiBase = parentEntity.AddChild<YIUIChild, YIUIBindVo, GameObject>(vo, childGameObject);
                var component = uiBase.AddComponent(vo.ComponentType);
                cdeTable.AddUIOwner(childGameObject.name, component);
                childCde.CreateComponent(component);
                uiBase.InitOwnerUIEntity(component); //这里就是要晚于其他内部组件这样才能访问时别人已经初始化好了
            }
        }
    }
}