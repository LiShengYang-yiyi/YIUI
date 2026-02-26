using YIUIFramework;

namespace ET.Client
{
    public static partial class YIUIMgrComponentSystem
    {
        /// <summary>
        /// 得到指定层级的最顶层的面板
        /// 可能没有
        /// </summary>
        public static PanelInfo GetTopPanel(this YIUIMgrComponent self,
                                            EPanelLayer layer = EPanelLayer.Any,
                                            EPanelOption ignoreOption = EPanelOption.None)
        {
            const int layerCount = (int)EPanelLayer.Count;

            for (var i = 0; i < layerCount; i++)
            {
                var currentLayer = (EPanelLayer)i;

                //如果是任意层级则 从上到下找
                //否则只会在目标层级上找
                if (layer != EPanelLayer.Any && currentLayer != layer)
                {
                    continue;
                }

                var list = self.GetLayerPanelInfoList(currentLayer);

                //倒序遍历 List 排在后面的就是最上面的
                for (int index = list.Count - 1; index >= 0; index--)
                {
                    var info = list[index];
                    if (info.UIPanel == null)
                    {
                        continue;
                    }

                    //有忽略操作 且满足条件 则这个界面无法获取到
                    if (ignoreOption != EPanelOption.None && (info.UIPanel.PanelOption & ignoreOption) != 0)
                    {
                        continue;
                    }

                    if (layer == EPanelLayer.Any || info.UIPanel.Layer == layer)
                    {
                        return info;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 关闭这个层级上的最前面的一个UI 异步
        /// </summary>
        public static async ETTask<bool> CloseLayerTopPanelAsync(this YIUIMgrComponent self,
                                                                 EPanelLayer layer,
                                                                 EPanelOption ignoreOption = EPanelOption.IgnoreClose,
                                                                 bool tween = true,
                                                                 bool ignoreElse = false,
                                                                 bool ignoreLock = false)
        {
            var topPanel = self.GetTopPanel(layer, ignoreOption);
            if (topPanel == null)
            {
                return false;
            }

            return await self.ClosePanelAsync(topPanel.Name, tween, ignoreElse, ignoreLock);
        }

        /// <summary>
        /// 关闭指定层级上的 最上层UI 同步
        /// </summary>
        public static void CloseLayerTopPanel(this YIUIMgrComponent self,
                                              EPanelLayer layer,
                                              EPanelOption ignoreOption = EPanelOption.IgnoreClose,
                                              bool tween = true,
                                              bool ignoreElse = false,
                                              bool ignoreLock = false)
        {
            self.CloseLayerTopPanelAsync(layer, ignoreOption, tween, ignoreElse, ignoreLock).NoContext();
        }

        /// <summary>
        /// 关闭Panel层级上的最上层UI 异步
        /// </summary>
        public static async ETTask<bool> CloseTopPanelAsync(this YIUIMgrComponent self,
                                                            EPanelLayer layer = EPanelLayer.Panel,
                                                            EPanelOption ignoreOption = EPanelOption.IgnoreClose,
                                                            bool tween = true,
                                                            bool ignoreElse = false,
                                                            bool ignoreLock = false)
        {
            return await self.CloseLayerTopPanelAsync(layer, ignoreOption, tween, ignoreElse, ignoreLock);
        }

        /// <summary>
        /// 关闭目标层级上的所有UI
        /// </summary>
        public static async ETTask CloseAll(this YIUIMgrComponent self,
                                            EPanelLayer layer = EPanelLayer.Any,
                                            EPanelOption ignoreOption = EPanelOption.IgnoreClose,
                                            bool tween = false,
                                            bool ignoreElse = true,
                                            bool ignoreLock = false)
        {
            EntityRef<YIUIMgrComponent> selfRef = self;

            while (true)
            {
                self = selfRef;
                if (self == null)
                {
                    return;
                }

                if (!await self.CloseLayerTopPanelAsync(layer, ignoreOption, tween, ignoreElse, ignoreLock))
                {
                    break;
                }
            }
        }
    }
}