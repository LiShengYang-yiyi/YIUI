using System;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace YIUIFramework
{
    /// <summary>
    /// YIUI 常量配置
    /// 部类 可以根据需求自行扩展
    /// </summary>
    [Serializable]
    [HideReferenceObjectPicker]
    [HideLabel]
    public partial class YIUIConstAsset
    {
        #region 项目配置

        [ReadOnly]
        [BoxGroup("项目配置", CenterLabel = true)]
        [LabelText("YIUI根目录名称")]
        public string UIProjectName = "YIUI"; //不要修改

        [BoxGroup("项目配置", CenterLabel = true)]
        [LabelText("YIUI项目命名空间")]
        public string UINamespace = "ET.Client"; //所有生成文件的命名空间

        [BoxGroup("项目配置", CenterLabel = true)]
        [LabelText("YIUI项目编辑器资源")]
        public string UIProjectEditorPath = "Assets/Editor/YIUI"; //编辑器才会用到的资源

        [BoxGroup("项目配置", CenterLabel = true)]
        [LabelText("YIUI项目指定包编辑器资源")]
        public string UIProjectPackageEditorPath = "Assets/../Packages/cn.etetet.{0}/Assets/Editor/YIUI"; //指定包的编辑器才会用到的资源

        [BoxGroup("项目配置", CenterLabel = true)]
        [LabelText("YIUI项目资源")]
        public string UIProjectResPath = "Assets/GameRes/YIUI"; //玩家的预设/图片等资源存放的地方

        [BoxGroup("项目配置", CenterLabel = true)]
        [LabelText("YIUI项目指定包资源")]
        public string UIProjectPackageResPath = "Assets/../Packages/cn.etetet.{0}/Assets/GameRes/YIUI"; //指定包的资源存放的地方

        [BoxGroup("项目配置", CenterLabel = true)]
        [LabelText("YIUI项目生成包名")]
        public string UIETCreatePackageName = "yiui"; //对应指定代码生成到指定的包中

        [BoxGroup("项目配置", CenterLabel = true)]
        [LabelText("YIUI项目生成默认包")]
        public string UIETCreatePackagePath = "Assets/../Packages/cn.etetet.yiui";

        [BoxGroup("项目配置", CenterLabel = true)]
        [LabelText("YIUI项目脚本")]
        public string UIETComponentGenPath = "Assets/../Packages/cn.etetet.{0}/Scripts/ModelView/Client/YIUIGen"; //自动生成的代码会覆盖不可修改

        [BoxGroup("项目配置", CenterLabel = true)]
        [LabelText("YIUI项目ET组件")]
        public string UIETComponentPath = "Assets/../Packages/cn.etetet.{0}/Scripts/ModelView/Client/YIUIComponent"; //玩家可编写的核心代码部分 ET系统

        [BoxGroup("项目配置", CenterLabel = true)]
        [LabelText("YIUI项目自定义脚本")]
        public string UIETSystemGenPath = "Assets/../Packages/cn.etetet.{0}/Scripts/HotfixView/Client/YIUIGen"; //自动生成的代码会覆盖不可修改

        [BoxGroup("项目配置", CenterLabel = true)]
        [LabelText("YIUI项目ET系统")]
        public string UIETSystemPath = "Assets/../Packages/cn.etetet.{0}/Scripts/HotfixView/Client/YIUISystem"; //玩家可编写的核心代码部分 ET系统

        [ReadOnly]
        [BoxGroup("项目配置", CenterLabel = true)]
        [LabelText("YIUI框架所处位置")]
        public string UIFrameworkPath = "Assets/../Packages/cn.etetet.yiuiframework";

        [ReadOnly]
        [BoxGroup("项目配置", CenterLabel = true)]
        [LabelText("YIUI项目代码模板")]
        public string UITemplatePath = "Assets/../Packages/cn.etetet.yiuiframework/Editor/YIUIAutoTool/Template";

        [ReadOnly]
        [BoxGroup("项目配置", CenterLabel = true)]
        [LabelText("YIUI项目Root预制")]
        public string UIRootPrefabPath = "Packages/cn.etetet.yiuiframework/Editor/UIRootPrefab/YIUIRoot.prefab";

        [ReadOnly]
        [BoxGroup("项目配置", CenterLabel = true)]
        public string UIPackages = "Packages";

        [ReadOnly]
        [BoxGroup("项目配置", CenterLabel = true)]
        public string UIETPackagesFormat = "cn.etetet.";

        [ReadOnly]
        [BoxGroup("项目配置", CenterLabel = true)]
        public string UIRootName = "YIUIRoot";

        [ReadOnly]
        [BoxGroup("项目配置", CenterLabel = true)]
        public string UILayerRootName = "YIUILayerRoot";

        [ReadOnly]
        [BoxGroup("项目配置", CenterLabel = true)]
        public string UIRootPkgName = "Common";

        [ReadOnly]
        [BoxGroup("项目配置", CenterLabel = true)]
        public string UIPanelName = "Panel";

        [ReadOnly]
        [BoxGroup("项目配置", CenterLabel = true)]
        public string UICommonName = "Common";

        [ReadOnly]
        [BoxGroup("项目配置", CenterLabel = true)]
        public string UIViewName = "View";

        [ReadOnly]
        [BoxGroup("项目配置", CenterLabel = true)]
        public string UIParentName = "Parent";

        [ReadOnly]
        [BoxGroup("项目配置", CenterLabel = true)]
        public string UIPrefabs = "Prefabs";

        [ReadOnly]
        [BoxGroup("项目配置", CenterLabel = true)]
        public string UIPrefabsCN = "预制";

        [ReadOnly]
        [BoxGroup("项目配置", CenterLabel = true)]
        public string UISprites = "Sprites";

        [ReadOnly]
        [BoxGroup("项目配置", CenterLabel = true)]
        public string UISpritesCN = "精灵";

        [ReadOnly]
        [BoxGroup("项目配置", CenterLabel = true)]
        public string UIAtlas = "Atlas";

        [ReadOnly]
        [BoxGroup("项目配置", CenterLabel = true)]
        public string UIAtlasCN = "图集";

        [ReadOnly]
        [BoxGroup("项目配置", CenterLabel = true)]
        public string UISource = "Source";

        [ReadOnly]
        [BoxGroup("项目配置", CenterLabel = true)]
        public string UISourceCN = "源文件";

        [ReadOnly]
        [BoxGroup("项目配置", CenterLabel = true)]
        public string UIAtlasIgnore = "AtlasIgnore"; //图集忽略文件夹名称

        [ReadOnly]
        [BoxGroup("项目配置", CenterLabel = true)]
        public string UISpritesAtlas1 = "Atlas1"; //图集1 不需要华丽的取名 每个包内的自定义图集就按顺序就好 自动创建的默认名称

        [ReadOnly]
        [BoxGroup("项目配置", CenterLabel = true)]
        public string UIAllViewParentName = "AllViewParent";

        [ReadOnly]
        [BoxGroup("项目配置", CenterLabel = true)]
        public string UIAllPopupViewParentName = "AllPopupViewParent";

        [ReadOnly]
        [BoxGroup("项目配置", CenterLabel = true)]
        public string UIYIUIPanelSourceName = "YIUIPanelSource";

        [ReadOnly]
        [BoxGroup("项目配置", CenterLabel = true)]
        public string UIPanelSourceName = "PanelSource";

        [ReadOnly]
        [BoxGroup("项目配置", CenterLabel = true)]
        public string UIYIUIViewName = "YIUIView";

        [ReadOnly]
        [BoxGroup("项目配置", CenterLabel = true)]
        public string UIViewParentName = "ViewParent";

        [ReadOnly]
        [BoxGroup("项目配置", CenterLabel = true)]
        public string UIYIUIViewParentName = "YIUIViewParent";

        #endregion

        #region 基础设置

        [BoxGroup("基础设置", CenterLabel = true)]
        [LabelText("源文件拆分后是否保留")]
        public bool SourceSplitReserve = false;

        [BoxGroup("基础设置", CenterLabel = true)]
        [LabelText("使用老的CDEInspector显示模式")]
        public bool DisplayOldCDEInspector = false;

        #endregion

        #region Root

        [BoxGroup("Root", CenterLabel = true)]
        [LabelText("低品质")]
        public bool IsLowQuality = false; //低品质 将会影响动画等逻辑 也可以根据这个参数自定义一些区别

        [BoxGroup("Root", CenterLabel = true)]
        [LabelText("UI宽度")]
        public int DesignScreenWidth = 1920;

        [BoxGroup("Root", CenterLabel = true)]
        [LabelText("UI高度")]
        public int DesignScreenHeight = 1080;

        [BoxGroup("Root", CenterLabel = true)]
        [LabelText("UI宽度(float)")]
        public float DesignScreenWidth_F = 1920f;

        [BoxGroup("Root", CenterLabel = true)]
        [LabelText("UI高度(float)")]
        public float DesignScreenHeight_F = 1080f;

        [BoxGroup("Root", CenterLabel = true)]
        [LabelText("根节点偏移")]
        public int RootPosOffset = 1000;

        [BoxGroup("Root", CenterLabel = true)]
        [LabelText("每个层级的距离")]
        public int LayerDistance = 0;

        #endregion

        #region 安全区调试编辑器下有效

        //这个需要自行实现黑底背景 这里只是提供参数
        [BoxGroup("安全区", CenterLabel = true)]
        [LabelText("在刘海屏机子时，是否打开黑边")]
        public bool OpenBlackBorder = false;

        [BoxGroup("安全区", CenterLabel = true)]
        [LabelText("启用2倍安全 则左右2边都会裁剪")]
        public bool DoubleSafe = false;

        [BoxGroup("安全区", CenterLabel = true)]
        [LabelText("安全区X")]
        public int SafeAreaX = 0;

        [BoxGroup("安全区", CenterLabel = true)]
        [LabelText("安全区Y")]
        public int SafeAreaY = 0;

        #endregion

        #region 动画

        [BoxGroup("动画", CenterLabel = true)]
        [LabelText("Dotween默认动画缩放")]
        public Vector3 DotweenAnimScale = new(0.8f, 0.8f, 0.8f);

        [BoxGroup("动画", CenterLabel = true)]
        [LabelText("全局禁用默认的Dotween动画")]
        public bool BanDotweenAnim = false;

        #endregion

        #region 编辑器工具

        #if UNITY_EDITOR
        public const string YIUIGraphicSelectorEnableKey = "YIUIGraphicSelector_Enable";

        [BoxGroup("编辑器工具", CenterLabel = true)]
        [LabelText("图层快选工具")]
        [OnValueChanged("YIUIGraphicSelectorEnableChanged")]
        [OnInspectorInit("YIUIGraphicSelectorEnableInit")]
        [ShowInInspector]
        private bool m_YIUIGraphicSelectorEnable;

        public bool YIUIGraphicSelectorEnable => UnityEditor.EditorPrefs.GetBool(YIUIGraphicSelectorEnableKey, true);

        private void YIUIGraphicSelectorEnableInit()
        {
            m_YIUIGraphicSelectorEnable = UnityEditor.EditorPrefs.GetBool(YIUIGraphicSelectorEnableKey, true);
        }

        private void YIUIGraphicSelectorEnableChanged()
        {
            try
            {
                var editorAssembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.GetName().Name == "ET.YIUIFramework.Editor");

                if (editorAssembly != null)
                {
                    var graphicSelectorType = editorAssembly.GetType("YIUIFramework.Editor.YIUIGraphicSelector");
                    if (graphicSelectorType != null)
                    {
                        var enableProperty = graphicSelectorType.GetProperty("Enable", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);

                        if (enableProperty != null && enableProperty.CanWrite)
                        {
                            enableProperty.SetValue(null, m_YIUIGraphicSelectorEnable);
                        }
                        else
                        {
                            Debug.LogError($"没有找到 YIUIFramework.Editor.YIUIGraphicSelector  Enable 属性");
                        }
                    }
                    else
                    {
                        Debug.LogError($"没有找到 YIUIFramework.Editor.YIUIGraphicSelector");
                    }
                }
                else
                {
                    Debug.LogError($"没有找到程序集 ET.YIUIFramework.Editor");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"无法通过反射更新 YIUIGraphicSelector.Enable： {ex}");
            }
        }
        #endif

        #endregion

        #region AI

        [BoxGroup("AI", CenterLabel = true)]
        [LabelText("AI客户端名称")]
        public string YIUIDefaultOpenAIName = "droid";

        #endregion
    }
}