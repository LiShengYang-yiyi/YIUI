using System;
using YIUIFramework;

namespace ET.Client
{
    /// <summary>
    /// 可等待的弹窗 不知道泛型 只知道UI资源名称时使用
    /// 缺点这里没办法判断类型是否正确 是否有 IYIUIOpen〈ParamVo〉 接口
    /// </summary>
    public static partial class TipsHelper
    {
        public static async ETTask<EHashWaitError> OpenWait(Scene scene, string resName, params object[] paramMore)
        {
            var vo = ParamVo.Get(paramMore);
            var error = await OpenWaitToParent(scene, resName, vo);
            ParamVo.Put(vo);
            return error;
        }

        public static async ETTask<EHashWaitError> OpenWaitToParent(Scene scene, string resName, Entity parent, params object[] paramMore)
        {
            var vo = ParamVo.Get(paramMore);
            var error = await OpenWaitToParent(scene, resName, vo, parent);
            ParamVo.Put(vo);
            return error;
        }

        public static async ETTask<EHashWaitError> OpenWaitToParent(Scene scene, string resName, ParamVo vo, Entity parent = null)
        {
            var data = scene.YIUIBind().GetBindVoByResName(resName);
            if (data == null) return EHashWaitError.Error;
            var bindVo = data.Value;
            EntityRef<Entity> parentRef = EntityRefHelper.GetEntityRefSafety(parent);
            EntityRef<Scene> sceneRef = scene;
            using var _ = await scene.Root().GetComponent<CoroutineLockComponent>().Wait(CoroutineLockType.YIUIFramework, typeof(TipsHelper).GetHashCode());
            var guid = IdGenerater.Instance.GenerateId();
            scene = sceneRef;
            var yiuiMgrRoot = scene.YIUIRoot();
            var hashWait = yiuiMgrRoot.GetComponent<HashWait>().Wait(guid);
            await yiuiMgrRoot.OpenPanelAsync<TipsPanelComponent, Type, Entity, long, ParamVo>(bindVo.ComponentType, parentRef.Entity, guid, vo);
            return await hashWait;
        }
    }
}