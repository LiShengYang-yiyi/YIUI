using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace YIUIFramework.Editor
{
    /// <summary>
    /// 源数据拆分
    /// </summary>
    public static class UIPanelSourceSplit
    {
        public static void Do(UIBindCDETable source)
        {
            if (!source.IsSplitData)
            {
                UnityTipsHelper.ShowError($"只有源数据才可以拆分 请检查 {source.name}");
                return;
            }

            var pkgName = source.PkgName;
            if (pkgName == null)
            {
                UnityTipsHelper.ShowError($"未知错误 pkgName = null 请检查 {source.name}");
                return;
            }

            var path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(source);

            var loadSource = (GameObject)AssetDatabase.LoadAssetAtPath(path, typeof(Object));
            if (loadSource == null)
            {
                UnityTipsHelper.ShowError($"未知错误 没有加载到源数据 请检查 {source.name} {path}");
                return;
            }

            var oldSource    = UIMenuItemHelper.CopyGameObject(loadSource);
            var oldCdeTable  = oldSource.GetComponent<UIBindCDETable>();
            var oldSplitData = oldCdeTable.PanelSplitData;

            var newSource = UIMenuItemHelper.CopyGameObject(loadSource);
            var cdeTable  = newSource.GetComponent<UIBindCDETable>();
            newSource.name       = newSource.name.Replace(YIUIConstHelper.Const.UISource, "");
            cdeTable.IsSplitData = false;
            var splitData = cdeTable.PanelSplitData;

            string savePath = "";
            if (UIOperationHelper.CheckUIIsPackages(loadSource, false))
            {
                var etPkgName = UIOperationHelper.GetETPackagesName(loadSource, false);
                savePath = $"{string.Format(YIUIConstHelper.Const.UIProjectPackageResPath, etPkgName)}/{pkgName}/{YIUIConstHelper.Const.UIPrefabs}";
            }
            else
            {
                savePath = $"{YIUIConstHelper.Const.UIProjectResPath}/{pkgName}/{YIUIConstHelper.Const.UIPrefabs}";
            }

            AllViewSaveAsPrefabAsset(oldSplitData.AllCommonView, splitData.AllCommonView, savePath, true);
            AllViewSaveAsPrefabAsset(oldSplitData.AllCreateView, splitData.AllCreateView, savePath);
            AllViewSaveAsPrefabAsset(oldSplitData.AllPopupView, splitData.AllPopupView, savePath);

            //拆分后新的Panel
            var newPath = $"{savePath}/{newSource.name}.prefab";
            SaveAsPrefabAsset(newSource, newPath);
            Object.DestroyImmediate(newSource);

            var reserve = YIUIConstHelper.Const.SourceSplitReserve;
            if (reserve)
            {
                PrefabUtility.SaveAsPrefabAsset(oldSource, path);
            }
            else
            {
                AssetDatabase.DeleteAsset(path);
            }

            Object.DestroyImmediate(oldSource);
            UnityTipsHelper.Show($"源数据拆分完毕");
            AssetDatabase.Refresh();
        }

        private static void AllViewSaveAsPrefabAsset(List<RectTransform> oldList,
                                                     List<RectTransform> newList,
                                                     string              savePath,
                                                     bool                nest = false)
        {
            if (oldList.Count != newList.Count)
            {
                Debug.LogError($"未知错误 长度不一致 {savePath}");
                return;
            }

            for (var i = 0; i < newList.Count; i++)
            {
                var viewParent    = newList[i];
                var oldViewParent = oldList[i];
                SaveAsPrefabAssetViewParent(oldViewParent, viewParent, savePath, nest);
            }
        }

        private static bool SaveAsPrefabAssetViewParent(RectTransform oldViewParent,
                                                        RectTransform viewParent,
                                                        string        savePath,
                                                        bool          nest = false)
        {
            //View 查找
            var view = viewParent.FindChildByName(viewParent.name.Replace(YIUIConstHelper.Const.UIParentName, ""));
            if (view == null)
            {
                Debug.LogError($"{viewParent.name} 没找到View");
                return false;
            }

            var oldView = oldViewParent.FindChildByName(oldViewParent.name.Replace(YIUIConstHelper.Const.UIParentName, ""));
            if (oldView == null)
            {
                Debug.LogError($"{oldViewParent.name} 没找到oldView");
                return false;
            }

            //CDE 查找
            if (view.GetComponent<UIBindCDETable>() == null)
            {
                Debug.LogError($"{viewParent.name} 没找到 UIBindCDETable 组件 请检查");
                return false;
            }

            if (oldView.GetComponent<UIBindCDETable>() == null)
            {
                Debug.LogError($"{oldViewParent.name} Old没找到 UIBindCDETable 组件 请检查");
                return false;
            }

            var prefabPath = $"{savePath}/{view.name}.prefab";

            var viewPrefab = SaveAsPrefabAsset(view.gameObject, prefabPath);

            Object.DestroyImmediate(view.gameObject);
            Object.DestroyImmediate(oldView.gameObject);

            //old 是每个下面都有一个关联上
            var oldPrefabInstance = PrefabUtility.InstantiatePrefab(viewPrefab, oldViewParent) as GameObject;
            if (oldPrefabInstance == null)
            {
                Debug.LogError($"{oldViewParent.name} Old未知错误 得到一个null 对象");
                return false;
            }

            //新要根据情况保留的才关联
            if (nest)
            {
                var prefabInstance = PrefabUtility.InstantiatePrefab(viewPrefab, viewParent) as GameObject;
                if (prefabInstance == null)
                {
                    Debug.LogError($"{viewParent.name} 未知错误 得到一个null 对象");
                    return false;
                }
            }

            return true;
        }

        private static GameObject SaveAsPrefabAsset(GameObject obj, string path)
        {
            PrefabUtility.SaveAsPrefabAsset(obj, path);
            AssetDatabase.SaveAssets();
            EditorApplication.ExecuteMenuItem("Assets/Refresh");
            var selectPath = path.Replace("Assets/../", "");
            var prefab     = (GameObject)AssetDatabase.LoadAssetAtPath(selectPath, typeof(Object));
            if (prefab == null)
            {
                Debug.LogError($"生成完毕 {obj.name} 请手动检查所有");
            }
            else
            {
                var cde = prefab.GetComponent<UIBindCDETable>();
                if (cde == null)
                {
                    Debug.LogError($"{obj.name} cde == null");
                }
                else
                {
                    cde.AutoCheck();
                }

                return prefab;
            }

            return null;
        }
    }
}