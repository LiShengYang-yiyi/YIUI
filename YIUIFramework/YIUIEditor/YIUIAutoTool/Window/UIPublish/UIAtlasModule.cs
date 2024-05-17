﻿#if UNITY_EDITOR
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;

namespace YIUIFramework.Editor
{
    public enum EAtlasType
    {
        Master,
        Variant,
    }

    public enum EPlatformType
    {
        [LabelText("默认")] Default,
        [LabelText("电脑")] PC,
        [LabelText("安卓")] Android,
        [LabelText("苹果")] IPhone,
    }

    /// <summary> 全局设置 </summary>
    [HideReferenceObjectPicker]
    [HideLabel]
    public class UIAtlasModule : BaseCreateModule
    {
        [HideLabel]
        public UISpriteAtlasSettings SpriteAtlasSettings = new UISpriteAtlasSettings();

        [EnumToggleButtons, HideLabel, BoxGroup("平台设置", centerLabel: true)]
        public EPlatformType m_UIPublishPackageData = EPlatformType.Default;

        [HideInInspector]
        public Dictionary<string, UIPlatformSettings> AllUIPlatformSettings =
            new Dictionary<string, UIPlatformSettings>();

        [ShowIf(nameof(m_UIPublishPackageData), EPlatformType.Default)]
        [BoxGroup("平台设置", centerLabel: true)]
        public UIPlatformSettings Default = new UIPlatformSettings
        {
            PlatformType    = EPlatformType.Default,
            BuildTargetName = "DefaultTexturePlatform",
            Format          = TextureImporterFormat.Automatic,
        };

        [ShowIf(nameof(m_UIPublishPackageData), EPlatformType.PC)]
        [BoxGroup("平台设置", centerLabel: true)]
        public UIPlatformSettings PC = new UIPlatformSettings
        {
            PlatformType    = EPlatformType.PC,
            BuildTargetName = "Standalone",
            Format          = TextureImporterFormat.DXT5Crunched,
        };

        [ShowIf(nameof(m_UIPublishPackageData), EPlatformType.Android)]
        [BoxGroup("平台设置", centerLabel: true)]
        public UIPlatformSettings Android = new UIPlatformSettings
        {
            PlatformType    = EPlatformType.Android,
            BuildTargetName = "Android",
            Format          = TextureImporterFormat.ASTC_6x6,
        };

        [ShowIf(nameof(m_UIPublishPackageData), EPlatformType.IPhone)]
        [BoxGroup("平台设置", centerLabel: true)]
        public UIPlatformSettings iPhone = new UIPlatformSettings
        {
            PlatformType    = EPlatformType.IPhone,
            BuildTargetName = "iPhone",
            Format          = TextureImporterFormat.ASTC_4x4,
        };

        private GlobalSpriteAtlasSettings m_GlobalSpriteAtlasSettings = new GlobalSpriteAtlasSettings();

        public UIAtlasModule()
        {
            AllUIPlatformSettings.Add(Default.BuildTargetName, Default);
            AllUIPlatformSettings.Add(PC.BuildTargetName, PC);
            AllUIPlatformSettings.Add(Android.BuildTargetName, Android);
            AllUIPlatformSettings.Add(iPhone.BuildTargetName, iPhone);
            m_GlobalSpriteAtlasSettings.SpriteAtlasSettings = SpriteAtlasSettings;
            m_GlobalSpriteAtlasSettings.Default             = Default;
            m_GlobalSpriteAtlasSettings.PC                  = PC;
            m_GlobalSpriteAtlasSettings.Android             = Android;
            m_GlobalSpriteAtlasSettings.iPhone              = iPhone;
        }

        private const string GlobalSaveSpriteAtlasSettingsPath =
            UIStaticHelper.UIProjectEditorPath + "/GlobalSpriteAtlasSettings.txt";

        private class GlobalSpriteAtlasSettings
        {
            public UISpriteAtlasSettings SpriteAtlasSettings;
            public UIPlatformSettings    Default;
            public UIPlatformSettings    PC;
            public UIPlatformSettings    Android;
            public UIPlatformSettings    iPhone;
        }

        public override void Initialize()
        {
            var data = OdinSerializationUtility.Load<GlobalSpriteAtlasSettings>(GlobalSaveSpriteAtlasSettingsPath);
            if (data == null) return;

            SpriteAtlasSettings = data.SpriteAtlasSettings;
            Default             = data.Default;
            PC                  = data.PC;
            Android             = data.Android;
            iPhone              = data.iPhone;
        }

        public override void OnDestroy()
        {
            OdinSerializationUtility.Save(m_GlobalSpriteAtlasSettings, GlobalSaveSpriteAtlasSettingsPath);
        }

        public void ResetTargetBuildSetting(SpriteAtlas spriteAtlas)
        {
            spriteAtlas.SetIncludeInBuild(SpriteAtlasSettings.IncludeInBuild);

            var packingSettings = new SpriteAtlasPackingSettings()
            {
                blockOffset        = SpriteAtlasSettings.BlockOffset,
                enableRotation     = SpriteAtlasSettings.EnableRotation,
                enableTightPacking = SpriteAtlasSettings.EnableTightPacking,
                padding            = SpriteAtlasSettings.Padding,
            };
            spriteAtlas.SetPackingSettings(packingSettings);

            var textureSettings = new SpriteAtlasTextureSettings()
            {
                readable        = SpriteAtlasSettings.Readable,
                generateMipMaps = SpriteAtlasSettings.GenerateMipMaps,
                sRGB            = SpriteAtlasSettings.sRGB,
                filterMode      = SpriteAtlasSettings.FilterMode,
            };
            spriteAtlas.SetTextureSettings(textureSettings);

            foreach (var targetSetting in AllUIPlatformSettings.Values)
            {
                var setting = spriteAtlas.GetPlatformSettings(targetSetting.BuildTargetName) ??
                    new TextureImporterPlatformSettings();
                setting.overridden          = targetSetting.Overridden;
                setting.maxTextureSize      = targetSetting.MaxTextureSize;
                setting.crunchedCompression = targetSetting.CrunchedCompression;
                setting.compressionQuality  = targetSetting.compressionQuality;
                setting.format              = targetSetting.Format;
                spriteAtlas.SetPlatformSettings(setting);
            }
        }
    }

    [HideReferenceObjectPicker, HideLabel]
    public class UISpriteAtlasSettings
    {
        [ReadOnly]
        public EAtlasType AtlasType = EAtlasType.Master;

        public bool IncludeInBuild = true;

        public int  BlockOffset        = 1;
        public bool EnableRotation     = false;
        public bool EnableTightPacking = false;

        [ValueDropdown(nameof(PaddingListKey))]
        public int Padding = 4;

        public bool                   GenerateMipMaps = false;
        public bool                   Readable        = false;
        public bool                   sRGB            = true;
        public UnityEngine.FilterMode FilterMode      = FilterMode.Bilinear;

        private static List<int> PaddingListKey = new List<int> { 2, 4, 8 };
    }

    [HideReferenceObjectPicker, HideLabel]
    public class UIPlatformSettings
    {
        [HideInInspector]
        public EPlatformType PlatformType;

        [HideInInspector]
        public string BuildTargetName;

        [HideIf(nameof(PlatformType), EPlatformType.Default)]
        public bool Overridden = true;

        [ValueDropdown(nameof(MaxTextureSizeListKey))]
        public int MaxTextureSize = 2048;

        private static List<int> MaxTextureSizeListKey = new List<int>
        {
            32, 64, 128, 256, 512, 1024, 2048, 4096, 8192
        };

        [ShowIf(nameof(PlatformType), EPlatformType.Default)]
        public bool CrunchedCompression = true;

        [Range(0, 100)]
        public int compressionQuality = 50; // 0=Fast 50=Normal 100=Best 对应安卓IOS时

        public TextureImporterFormat Format = TextureImporterFormat.Automatic;
    }
}
#endif