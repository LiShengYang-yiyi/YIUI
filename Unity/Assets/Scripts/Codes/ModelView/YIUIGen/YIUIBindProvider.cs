using YIUIFramework;

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
            list[0] = new YIUIBindVo
            {
                PkgName       = ET.Client.GMCommandItemComponent.PkgName,
                ResName       = ET.Client.GMCommandItemComponent.ResName,
                CodeType      = EUICodeType.Common,
                PanelLayer    = EPanelLayer.Any,
                ComponentType = typeof(ET.Client.GMCommandItemComponent),
            };
            list[1] = new YIUIBindVo
            {
                PkgName       = ET.Client.GMPanelComponent.PkgName,
                ResName       = ET.Client.GMPanelComponent.ResName,
                CodeType      = EUICodeType.Panel,
                PanelLayer    = EPanelLayer.Top,
                ComponentType = typeof(ET.Client.GMPanelComponent),
            };
            list[2] = new YIUIBindVo
            {
                PkgName       = ET.Client.GMParamItemComponent.PkgName,
                ResName       = ET.Client.GMParamItemComponent.ResName,
                CodeType      = EUICodeType.Common,
                PanelLayer    = EPanelLayer.Any,
                ComponentType = typeof(ET.Client.GMParamItemComponent),
            };
            list[3] = new YIUIBindVo
            {
                PkgName       = ET.Client.GMTypeItemComponent.PkgName,
                ResName       = ET.Client.GMTypeItemComponent.ResName,
                CodeType      = EUICodeType.Common,
                PanelLayer    = EPanelLayer.Any,
                ComponentType = typeof(ET.Client.GMTypeItemComponent),
            };
            list[4] = new YIUIBindVo
            {
                PkgName       = ET.Client.GMViewComponent.PkgName,
                ResName       = ET.Client.GMViewComponent.ResName,
                CodeType      = EUICodeType.View,
                PanelLayer    = EPanelLayer.Any,
                ComponentType = typeof(ET.Client.GMViewComponent),
            };
            list[5] = new YIUIBindVo
            {
                PkgName       = ET.Client.LobbyPanelComponent.PkgName,
                ResName       = ET.Client.LobbyPanelComponent.ResName,
                CodeType      = EUICodeType.Panel,
                PanelLayer    = EPanelLayer.Panel,
                ComponentType = typeof(ET.Client.LobbyPanelComponent),
            };
            list[6] = new YIUIBindVo
            {
                PkgName       = ET.Client.LoginPanelComponent.PkgName,
                ResName       = ET.Client.LoginPanelComponent.ResName,
                CodeType      = EUICodeType.Panel,
                PanelLayer    = EPanelLayer.Popup,
                ComponentType = typeof(ET.Client.LoginPanelComponent),
            };
            list[7] = new YIUIBindVo
            {
                PkgName       = ET.Client.MainPanelComponent.PkgName,
                ResName       = ET.Client.MainPanelComponent.ResName,
                CodeType      = EUICodeType.Panel,
                PanelLayer    = EPanelLayer.Panel,
                ComponentType = typeof(ET.Client.MainPanelComponent),
            };
            list[8] = new YIUIBindVo
            {
                PkgName       = ET.Client.RedDotDataItemComponent.PkgName,
                ResName       = ET.Client.RedDotDataItemComponent.ResName,
                CodeType      = EUICodeType.Common,
                PanelLayer    = EPanelLayer.Any,
                ComponentType = typeof(ET.Client.RedDotDataItemComponent),
            };
            list[9] = new YIUIBindVo
            {
                PkgName       = ET.Client.RedDotPanelComponent.PkgName,
                ResName       = ET.Client.RedDotPanelComponent.ResName,
                CodeType      = EUICodeType.Panel,
                PanelLayer    = EPanelLayer.Tips,
                ComponentType = typeof(ET.Client.RedDotPanelComponent),
            };
            list[10] = new YIUIBindVo
            {
                PkgName       = ET.Client.RedDotStackItemComponent.PkgName,
                ResName       = ET.Client.RedDotStackItemComponent.ResName,
                CodeType      = EUICodeType.Common,
                PanelLayer    = EPanelLayer.Any,
                ComponentType = typeof(ET.Client.RedDotStackItemComponent),
            };
            list[11] = new YIUIBindVo
            {
                PkgName       = ET.Client.MessageTipsViewComponent.PkgName,
                ResName       = ET.Client.MessageTipsViewComponent.ResName,
                CodeType      = EUICodeType.View,
                PanelLayer    = EPanelLayer.Any,
                ComponentType = typeof(ET.Client.MessageTipsViewComponent),
            };
            list[12] = new YIUIBindVo
            {
                PkgName       = ET.Client.TextTipsViewComponent.PkgName,
                ResName       = ET.Client.TextTipsViewComponent.ResName,
                CodeType      = EUICodeType.View,
                PanelLayer    = EPanelLayer.Any,
                ComponentType = typeof(ET.Client.TextTipsViewComponent),
            };
            list[13] = new YIUIBindVo
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