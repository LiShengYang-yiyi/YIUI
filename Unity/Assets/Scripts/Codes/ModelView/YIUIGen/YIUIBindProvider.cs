﻿using YIUIFramework;

namespace YIUICodeGenerated
{
    /// <summary>
    /// 由YIUI工具自动创建 请勿修改
    /// 用法: YIUIBindHelper.InternalGameGetUIBindVoFunc = YIUICodeGenerated.YIUIBindProvider.Get;
    /// </summary>
    public static class YIUIBindProvider
    {
        public static YIUIBindVo[] Get()
        {
            var list          = new YIUIBindVo[14];

            {
                PkgName       = ET.Client.GMCommandItemComponent.PkgName,
                ResName       = ET.Client.GMCommandItemComponent.ResName,
                CodeType      = EUICodeType.Common,
                PanelLayer    = EPanelLayer.Any,
                ComponentType = typeof(ET.Client.GMCommandItemComponent),
            };

            {
                PkgName       = ET.Client.GMPanelComponent.PkgName,
                ResName       = ET.Client.GMPanelComponent.ResName,
                CodeType      = EUICodeType.Panel,
                PanelLayer    = EPanelLayer.Top,
                ComponentType = typeof(ET.Client.GMPanelComponent),
            };

            {
                PkgName       = ET.Client.GMParamItemComponent.PkgName,
                ResName       = ET.Client.GMParamItemComponent.ResName,
                CodeType      = EUICodeType.Common,
                PanelLayer    = EPanelLayer.Any,
                ComponentType = typeof(ET.Client.GMParamItemComponent),
            };

            {
                PkgName       = ET.Client.GMTypeItemComponent.PkgName,
                ResName       = ET.Client.GMTypeItemComponent.ResName,
                CodeType      = EUICodeType.Common,
                PanelLayer    = EPanelLayer.Any,
                ComponentType = typeof(ET.Client.GMTypeItemComponent),
            };

            {
                PkgName       = ET.Client.GMViewComponent.PkgName,
                ResName       = ET.Client.GMViewComponent.ResName,
                CodeType      = EUICodeType.View,
                PanelLayer    = EPanelLayer.Any,
                ComponentType = typeof(ET.Client.GMViewComponent),
            };

            {
                PkgName       = ET.Client.LobbyPanelComponent.PkgName,
                ResName       = ET.Client.LobbyPanelComponent.ResName,
                CodeType      = EUICodeType.Panel,
                PanelLayer    = EPanelLayer.Panel,
                ComponentType = typeof(ET.Client.LobbyPanelComponent),
            };

            {
                PkgName       = ET.Client.LoginPanelComponent.PkgName,
                ResName       = ET.Client.LoginPanelComponent.ResName,
                CodeType      = EUICodeType.Panel,
                PanelLayer    = EPanelLayer.Popup,
                ComponentType = typeof(ET.Client.LoginPanelComponent),
            };

            {
                PkgName       = ET.Client.MainPanelComponent.PkgName,
                ResName       = ET.Client.MainPanelComponent.ResName,
                CodeType      = EUICodeType.Panel,
                PanelLayer    = EPanelLayer.Panel,
                ComponentType = typeof(ET.Client.MainPanelComponent),
            };

            {
                PkgName       = ET.Client.RedDotDataItemComponent.PkgName,
                ResName       = ET.Client.RedDotDataItemComponent.ResName,
                CodeType      = EUICodeType.Common,
                PanelLayer    = EPanelLayer.Any,
                ComponentType = typeof(ET.Client.RedDotDataItemComponent),
            };

            {
                PkgName       = ET.Client.RedDotPanelComponent.PkgName,
                ResName       = ET.Client.RedDotPanelComponent.ResName,
                CodeType      = EUICodeType.Panel,
                PanelLayer    = EPanelLayer.Tips,
                ComponentType = typeof(ET.Client.RedDotPanelComponent),
            };

            {
                PkgName       = ET.Client.RedDotStackItemComponent.PkgName,
                ResName       = ET.Client.RedDotStackItemComponent.ResName,
                CodeType      = EUICodeType.Common,
                PanelLayer    = EPanelLayer.Any,
                ComponentType = typeof(ET.Client.RedDotStackItemComponent),
            };

            {
                PkgName       = ET.Client.MessageTipsViewComponent.PkgName,
                ResName       = ET.Client.MessageTipsViewComponent.ResName,
                CodeType      = EUICodeType.View,
                PanelLayer    = EPanelLayer.Any,
                ComponentType = typeof(ET.Client.MessageTipsViewComponent),
            };

            {
                PkgName       = ET.Client.TextTipsViewComponent.PkgName,
                ResName       = ET.Client.TextTipsViewComponent.ResName,
                CodeType      = EUICodeType.View,
                PanelLayer    = EPanelLayer.Any,
                ComponentType = typeof(ET.Client.TextTipsViewComponent),
            };

            {
                PkgName       = ET.Client.TipsPanelComponent.PkgName,
                ResName       = ET.Client.TipsPanelComponent.ResName,
                CodeType      = EUICodeType.Panel,
                PanelLayer    = EPanelLayer.Tips,
                ComponentType = typeof(ET.Client.TipsPanelComponent),
            };

            return list;
        }
    }
}