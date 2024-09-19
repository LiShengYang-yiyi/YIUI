﻿//------------------------------------------------------------
// Author: 亦亦
// Mail: 379338943@qq.com
// Data: 2023年2月12日
//------------------------------------------------------------

using Sirenix.OdinInspector;

namespace YIUIFramework
{
    /// <summary>
    /// UI静态助手
    /// </summary>
    public static class UIStaticHelper
    {
        [LabelText("YIUI根目录名称")]
        public const string UIProjectName = "MizuYIUI";

        [LabelText("YIUI项目命名空间")]
        public const string UINamespace = "MizuYIUI"; //所有生成文件的命名空间

        [LabelText("YIUI项目编辑器资源路径")]
        public const string UIProjectEditorPath = "Assets/Editor/" + UIProjectName; //编辑器才会用到的资源

        [LabelText("YIUI项目资源路径")]
        public const string UIProjectResPath = "Assets/Res/" + UIProjectName; //玩家的预设/图片等资源存放的地方

        [LabelText("YIUI项目脚本路径")]
        public const string UIGenerationPath = "Assets/Scripts/YIUIGeneration"; //自动生成的代码

        [LabelText("YIUI项目自定义脚本路径")]
        public const string UICodeScriptsPath = "Assets/Scripts/" + UIProjectName; //玩家可编写的核心代码部分

        [LabelText("YIUI框架所处位置路径")]
        public const string UIFrameworkPath = "Assets/Plugins/YIUI/YIUIFramework";

        [LabelText("YIUI项目代码模板路径")]
        public const string UITemplatePath = UIFrameworkPath + "/YIUIEditor/YIUIAutoTool/Template";

        public const string UIRootPrefabPath =
            UIFrameworkPath + "/YIUIEditor/UIRootPrefab/" + PanelMgr.UIRootName + ".prefab";

        public const string UIBaseName               = nameof(UIBase);
        public const string UIBasePanelName          = nameof(BasePanel);
        public const string UIBaseViewName           = nameof(BaseView);
        public const string UIBaseComponentName      = nameof(BaseComponent);
        public const string UIPanelName              = "Panel";
        public const string UIViewName               = "View";
        public const string UIParentName             = "Parent";
        public const string UIPrefabs                = "Prefabs";
        public const string UIPrefabsCN              = "预制";
        public const string UISprites                = "Sprites";
        public const string UISpritesCN              = "精灵";
        public const string UIAtlas                  = "Atlas";
        public const string UIAtlasCN                = "图集";
        public const string UISource                 = "Source";
        public const string UISourceCN               = "源文件";
        public const string UIAtlasIgnore            = "AtlasIgnore"; //图集忽略文件夹名称
        public const string UISpritesAtlas1          = "Atlas1";      //图集1 不需要华丽的取名 每个包内的自定义图集就按顺序就好 当然你也可以自定义其他
        public const string UIAllViewParentName      = "AllViewParent";
        public const string UIAllPopupViewParentName = "AllPopupViewParent";
        public const string UIYIUIPanelSourceName    = UIProjectName + UIPanelName + UISource;
        public const string UIPanelSourceName        = UIPanelName + UISource;
        public const string UIYIUIViewName           = UIProjectName + UIViewName;
        public const string UIViewParentName         = UIViewName + UIParentName;
        public const string UIYIUIViewParentName     = UIProjectName + UIViewName + UIParentName;
    }
}