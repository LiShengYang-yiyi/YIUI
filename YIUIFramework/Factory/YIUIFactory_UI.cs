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
        private static void SetParent(RectTransform self, RectTransform parent)
        {
            self.SetParent(parent, false);
            self.AutoReset();
        }

        internal static UIBase CreateCommon(string pkgName, string resName, GameObject obj)
        {
            var bingVo = UIBindHelper.GetBindVoByPath(pkgName, resName);
            if (bingVo == null) return null;
            var vo = bingVo.Value;
            return CreateByObjVo(vo, obj);
        }

        internal static UIBase CreatePanel(PanelInfo panelInfo)
        {
            return Create(panelInfo.PkgName, panelInfo.ResName);
        }

        private static T Create<T>() where T : UIBase
        {
            var data = UIBindHelper.GetBindVoByType<T>();
            if (data == null) return null;
            var vo = data.Value;

            return (T)Create(vo);
        }

        private static UIBase Create(string pkgName, string resName)
        {
            var bingVo = UIBindHelper.GetBindVoByPath(pkgName, resName);
            return bingVo == null ? null : Create(bingVo.Value);
        }

        private static UIBase Create(UIBindVo vo)
        {
            var obj = YIUILoadHelper.LoadAssetInstantiate(vo.PkgName, vo.ResName);
            if (obj == null)
            {
                Debug.LogError($"没有加载到这个资源 {vo.PkgName}/{vo.ResName}");
                return null;
            }

            return CreateByObjVo(vo, obj);
        }

        private static UIBase CreateByObjVo(UIBindVo vo, GameObject obj)
        {
            var cdeTable = obj.GetComponent<UIBindCDETable>();
            if (cdeTable == null)
            {
                Debug.LogError($"{obj.name} 没有 UIBindCDETable 组件 无法创建 请检查");
                return null;
            }

            var lastActive = cdeTable.gameObject.activeSelf;
            if (!lastActive)
            {
                //加载时必须保证处于激活状态 否则一些Mono相关未初始化导致不可预知的错误
                cdeTable.gameObject.SetActive(true);
            }

            cdeTable.CreateComponent();
            var uiBase = (UIBase)Activator.CreateInstance(vo.CreatorType);
            uiBase.InitUIBase(vo, obj);

            if (!lastActive)
            {
                //如果之前是隐藏的状态 则还原
                //此时已经初始化完毕 所以可能会收到被隐藏的消息 请自行酌情处理
                cdeTable.gameObject.SetActive(false);
            }

            return uiBase;
        }

        private static void CreateComponent(this UIBindCDETable cdeTable)
        {
            foreach (var childCde in cdeTable.AllChildCdeTable)
            {
                if (childCde == null)
                {
                    Debug.LogError($"{cdeTable.name} 存在null对象的childCde 检查是否因为删除或丢失或未重新生成");
                    continue;
                }

                var lastActive = childCde.gameObject.activeSelf;
                if (!lastActive)
                {
                    //加载时必须保证处于激活状态 否则一些Mono相关未初始化导致不可预知的错误
                    childCde.gameObject.SetActive(true);
                }

                var bingVo = UIBindHelper.GetBindVoByPath(childCde.PkgName, childCde.ResName);
                if (bingVo == null) continue;

                var childBase = (UIBase)Activator.CreateInstance(bingVo.Value.CreatorType);
                childCde.CreateComponent();
                childBase.InitUIBase(bingVo.Value, childCde.gameObject);
                cdeTable.AddUIBase(childCde.gameObject.name, childBase);

                if (!lastActive)
                {
                    //如果之前是隐藏的状态 则还原
                    //此时已经初始化完毕 所以可能会收到被隐藏的消息 请自行酌情处理
                    childCde.gameObject.SetActive(false);
                }
            }
        }
    }
}