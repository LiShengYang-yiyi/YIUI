using UnityEngine;
using YIUIFramework;

namespace ET.Client
{
    [FriendOf(typeof(YIUIPanelComponent))]
    public static partial class YIUIMgrComponentSystem
    {
        /// <summary>
        /// 加入UI到对应层级 设置顺序 大小
        /// </summary>
        internal static void AddUI(this YIUIMgrComponent self, PanelInfo panelInfo)
        {
            var uiBase     = panelInfo.UIBase;
            var uiPanel    = panelInfo.UIPanel;
            var panelLayer = uiPanel.Layer;
            var priority   = uiPanel.Priority;
            var uiRect     = uiBase.OwnerRectTransform;

            var layerRect = self.GetLayerRect(panelLayer);
            if (layerRect == null)
            {
                panelLayer = EPanelLayer.Bottom;
                layerRect  = self.GetLayerRect(panelLayer);
                Debug.LogError($"没有找到这个UILayer {panelLayer}  强制修改为使用最低层 请检查");
            }

            var addLast = true; //放到最后 也就是最前面

            var infoList     = self.GetLayerPanelInfoList(panelLayer);
            var removeResult = infoList.Remove(panelInfo);
            if (removeResult)
            {
                uiRect.SetParent(self.UILayerRoot);
            }

            /*
             * 使用Unity的层级作为前后显示
             * 大的在前 小的在后
             * 所以根据优先级 从小到大排序
             * 当前优先级 >= 目标优先级时 插入
             */

            for (var i = infoList.Count - 1; i >= 0; i--)
            {
                var info         = infoList[i];
                var infoPriority = info.UIPanel?.Priority ?? 0;

                if (i == infoList.Count - 1 && priority >= infoPriority) break;

                if (priority >= infoPriority)
                {
                    infoList.Insert(i + 1, panelInfo);
                    uiRect.SetParent(layerRect);
                    uiRect.SetSiblingIndex(i + 1);
                    addLast = false;
                    break;
                }

                if (i <= 0)
                {
                    infoList.Insert(0, panelInfo);
                    uiRect.SetParent(layerRect);
                    uiRect.SetSiblingIndex(0);
                    addLast = false;
                    break;
                }
            }

            if (addLast)
            {
                infoList.Add(panelInfo);
                uiRect.SetParent(layerRect);
                uiRect.SetAsLastSibling();
            }

            uiRect.ResetToFullScreen();
            uiRect.ResetLocalPosAndRot();
            panelInfo.UIPanel.StopCountDownDestroyPanel();
        }

        /// <summary>
        /// 移除一个UI
        /// </summary>
        internal static void RemoveUI(this YIUIMgrComponent self, PanelInfo panelInfo)
        {
            if (panelInfo.UIBase == null)
            {
                Debug.LogError($"无法移除一个null panelInfo 数据 {panelInfo.ResName}");
                return;
            }

            EventSystem.Instance?.Publish(self.Root(),
                                          new YIUIEventPanelCloseAfter()
                                          {
                                              UIPkgName       = panelInfo.PkgName,
                                              UIResName       = panelInfo.ResName,
                                              UIComponentName = panelInfo.Name,
                                              PanelLayer      = panelInfo.PanelLayer
                                          });

            var uiBase       = panelInfo.UIBase;
            var uiPanel      = panelInfo.UIPanel;
            var foreverCache = uiPanel.PanelForeverCache;
            var timeCache    = uiPanel.PanelTimeCache;
            var panelLayer   = uiPanel.Layer;
            self.RemoveLayerPanelInfo(panelLayer, panelInfo);

            if (foreverCache || timeCache)
            {
                //缓存界面只是单纯的吧界面隐藏
                //再次被打开 如何重构界面需要自行设置
                var layerRect = self.GetLayerRect(EPanelLayer.Cache);
                var uiRect    = uiBase.OwnerRectTransform;
                uiRect.SetParent(layerRect, false);
                uiBase.SetActive(false);

                if (timeCache && !foreverCache)
                {
                    //根据配置时间X秒后自动摧毁
                    //如果X秒内又被打开则可复用
                    if (uiPanel.CachePanelTime > 0)
                    {
                        uiPanel.CacheTimeCountDownDestroyPanel();
                    }
                    else
                    {
                        Debug.LogWarning($"{panelInfo.Name} 缓存时间配置错误 {uiPanel.CachePanelTime} <= 0 请检查");
                        self.RemoveUIReset(panelInfo.Name);
                    }
                }
            }
            else
            {
                self.RemoveUIReset(panelInfo.Name);
            }
        }

        internal static void RemoveUIReset(this YIUIMgrComponent self, string panelName)
        {
            var panelInfo = self.GetPanelInfo(panelName);
            if (panelInfo == null)
            {
                Debug.LogError($"移除错误 没有找到PanelInfo {panelName}");
                return;
            }

            var uiObj = panelInfo.UIBase?.OwnerGameObject;
            if (uiObj == null)
            {
                return;
            }

            EventSystem.Instance?.Publish(self.Root(),
                                          new YIUIEventPanelDestroy()
                                          {
                                              UIPkgName       = panelInfo.PkgName,
                                              UIResName       = panelInfo.ResName,
                                              UIComponentName = panelInfo.Name,
                                              PanelLayer      = panelInfo.PanelLayer
                                          });

            UnityEngine.Object.Destroy(uiObj);
            panelInfo.ResetUI(null);
            panelInfo.ResetEntity(null);
        }

        /// <summary>
        /// 移除一个UI (被直接摧毁的)
        /// </summary>
        internal static void DestroyRemoveUI(this YIUIMgrComponent self, PanelInfo panelInfo)
        {
            EventSystem.Instance?.Publish(self.Root(),
                                          new YIUIEventPanelCloseAfter()
                                          {
                                              UIPkgName       = panelInfo.PkgName,
                                              UIResName       = panelInfo.ResName,
                                              UIComponentName = panelInfo.Name,
                                              PanelLayer      = panelInfo.PanelLayer
                                          });

            self.RemoveLayerPanelInfo(panelInfo.PanelLayer, panelInfo);

            EventSystem.Instance?.Publish(self.Root(),
                                          new YIUIEventPanelDestroy()
                                          {
                                              UIPkgName       = panelInfo.PkgName,
                                              UIResName       = panelInfo.ResName,
                                              UIComponentName = panelInfo.Name,
                                              PanelLayer      = panelInfo.PanelLayer
                                          });

            panelInfo.ResetUI(null);
            panelInfo.ResetEntity(null);
        }
    }
}
