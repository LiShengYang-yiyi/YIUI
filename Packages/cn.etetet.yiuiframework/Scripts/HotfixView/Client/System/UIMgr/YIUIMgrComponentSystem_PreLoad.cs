using System;
using UnityEngine;
using YIUIFramework;

namespace ET.Client
{
    [FriendOf(typeof(YIUIMgrComponent))]
    public static partial class YIUIMgrComponentSystem
    {
        /// <summary>
        /// 预加载UI Panel - 泛型版本
        /// </summary>
        /// <typeparam name="T">Panel组件类型</typeparam>
        /// <param name="self">UI管理器</param>
        /// <param name="parentEntity">父级Entity，可为空则使用默认</param>
        /// <param name="loadEntity">默认加载Entity，否则只是加载预制体, 区别就是一个会真的吧代码创建出来，一个只是预制体</param>
        /// <returns>是否预加载成功</returns>
        public static async ETTask<bool> PreLoadPanelAsync<T>(this YIUIMgrComponent self, Entity parentEntity = null, bool loadEntity = true) where T : Entity
        {
            EntityRef<YIUIMgrComponent> selfRef = self;

            var panelName = self.GetPanelName<T>();

            if (string.IsNullOrEmpty(panelName))
            {
                Debug.LogError($"<color=red> 预加载失败: 类型 {typeof(T).Name} 的面板名称为空 </color>");
                return false;
            }

            self = selfRef;
            return await self.PreLoadPanelAsync(panelName, parentEntity ?? self, loadEntity);
        }

        /// <summary>
        /// 预加载UI Panel - 字符串版本
        /// </summary>
        /// <param name="self">UI管理器</param>
        /// <param name="panelName">Panel名称</param>
        /// <param name="parentEntity">父级Entity，可为空则使用默认</param>
        /// <param name="loadEntity">默认加载Entity，否则只是加载预制体, 区别就是一个会真的吧代码创建出来，一个只是预制体</param>
        /// <returns>是否预加载成功</returns>
        public static async ETTask<bool> PreLoadPanelAsync(this YIUIMgrComponent self, string panelName, Entity parentEntity = null, bool loadEntity = true)
        {
            if (string.IsNullOrEmpty(panelName))
            {
                Debug.LogError($"<color=red> 预加载失败: 面板名称为空 </color>");
                return false;
            }

            EntityRef<YIUIMgrComponent> selfRef = self;
            EntityRef<Entity> parentEntityRef = EntityRefHelper.GetEntityRefSafety(parentEntity);

            #if YIUIMACRO_PANEL_OPENCLOSE
            Debug.Log($"<color=cyan> 正在预加载UI: {panelName} </color>");
            #endif

            var panelInfo = self.GetPanelInfo(panelName);
            if (panelInfo == null)
            {
                Debug.LogError($"<color=red> 预加载失败: 无法获取 {panelName} 的PanelInfo </color>");
                return false;
            }

            if (panelInfo.UIBase != null)
            {
                return true;
            }

            if (!loadEntity && panelInfo.PreLoadGameObject != null)
            {
                return true;
            }

            var coroutineLockCode = panelInfo.PanelLayer == EPanelLayer.Panel ? YIUIConstHelper.Const.UIProjectName.GetHashCode() : panelName.GetHashCode();

            using var coroutineLock = await self.Root().GetComponent<CoroutineLockComponent>().Wait(CoroutineLockType.YIUIPanel, coroutineLockCode);

            self = selfRef;
            panelInfo = self.GetPanelInfo(panelName);
            if (panelInfo == null)
            {
                Debug.LogError($"<color=red> 预加载失败: 无法获取 {panelName} 的PanelInfo </color>");
                return false;
            }

            if (panelInfo.UIBase != null)
            {
                return true;
            }

            try
            {
                #if YIUIMACRO_PANEL_OPENCLOSE
                var preLoadSW = System.Diagnostics.Stopwatch.StartNew();
                #endif

                if (loadEntity)
                {
                    self = selfRef;
                    parentEntity = parentEntityRef;
                    var uiCom = await YIUIFactory.CreatePanelAsync(self.Scene(), panelInfo, parentEntity);
                    if (uiCom == null)
                    {
                        Debug.LogError($"<color=red> 预加载失败: 面板 [{panelName}] 创建失败，包名={panelInfo.PkgName}，资源名={panelInfo.ResName} </color>");
                        return false;
                    }

                    var uiBase = uiCom.GetParent<YIUIChild>();
                    panelInfo.ResetUI(uiBase);
                    panelInfo.ResetEntity(uiCom);

                    await YIUIEventSystem.PreLoad(panelInfo.OwnerUIEntity);
                }
                else
                {
                    if (panelInfo.PreLoadGameObject == null)
                    {
                        var uiGameObject = await YIUIFactory.CreatePanelGameObjectAsync(self.Scene(), panelInfo);
                        panelInfo.ResetPreLoadGameObject(uiGameObject);
                    }
                }

                self = selfRef;
                self.AddPreLoadUI(panelInfo);

                if (panelInfo.UIPanel != null)
                {
                    panelInfo.UIPanel.StopCountDownDestroyPanel();
                }

                #if YIUIMACRO_PANEL_OPENCLOSE
                preLoadSW.Stop();
                Debug.Log($"<color=green> 预加载完成: {panelName}, 耗时: {preLoadSW.ElapsedMilliseconds} 毫秒 </color>");
                #endif

                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"<color=red> 预加载 {panelName} 时发生异常: {e.Message}\n{e.StackTrace} </color>");
                return false;
            }
        }

        /// <summary>
        /// 添加缓存UI
        /// </summary>
        internal static void AddPreLoadUI(this YIUIMgrComponent self, PanelInfo panelInfo)
        {
            RectTransform uiRect = null;
            if (panelInfo.UIBase != null)
            {
                var uiBase = panelInfo.UIBase;
                uiRect = uiBase.OwnerRectTransform;
            }

            if (panelInfo.PreLoadGameObject != null)
            {
                uiRect = panelInfo.PreLoadGameObject.GetComponent<RectTransform>();
            }

            if (uiRect == null)
            {
                Debug.LogError($"<color=red> 缓存UI失败: {panelInfo.ResName} </color>");
                return;
            }

            var layerRect = self.GetLayerRect(EPanelLayer.Cache);
            uiRect.SetParent(layerRect, false);
            uiRect.gameObject.SetActive(false);
        }
    }
}