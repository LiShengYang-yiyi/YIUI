#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;

namespace YIUIFramework.Editor
{
    public partial class YIUIAtlasModule
    {
        public static void RefreshAtlasData()
        {
            SpriteAtlasUtility.PackAllAtlases(EditorUserBuildSettings.activeBuildTarget, false);

            //不需要检查是否有重复的 因为 YOOASSET 开了可寻址必须是唯一的
            var atlasInfos = new List<YIUIAtlasInfo>();
            var absolutePaths = Directory.GetFiles(YIUIConstHelper.Const.UIProjectResPath, "*.spriteatlasv2", SearchOption.AllDirectories);
            foreach (var absolutePath in absolutePaths)
            {
                var relativePath = absolutePath.Replace(Application.dataPath, "Assets");
                var spriteAtlas = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(relativePath);

                if (spriteAtlas == null || spriteAtlas.spriteCount <= 0)
                {
                    continue;
                }

                var atlasName = spriteAtlas.name;

                var atlasInfo = new YIUIAtlasInfo
                {
                    AtlasName = atlasName,
                    SpriteNames = new string[spriteAtlas.spriteCount]
                };

                var sprites = new Sprite[spriteAtlas.spriteCount];
                var spriteCount = spriteAtlas.GetSprites(sprites);
                for (var i = 0; i < spriteCount; i++)
                {
                    atlasInfo.SpriteNames[i] = sprites[i].name.Replace("(Clone)", string.Empty);
                }

                atlasInfos.Add(atlasInfo);
            }

            var atlasSavePath = YIUIConstAsset.AtlasDataPath;
            var atlasAssetPath = atlasSavePath.Replace("Assets", Application.dataPath);
            if (File.Exists(atlasAssetPath))
            {
                File.Delete(atlasAssetPath);
            }

            var kConfigMapAsset = ScriptableObject.CreateInstance<YIUIAtlasData>();
            kConfigMapAsset.Infos = atlasInfos.ToArray();
            AssetDatabase.CreateAsset(kConfigMapAsset, atlasSavePath);
            EditorUtility.SetDirty(kConfigMapAsset);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
#endif