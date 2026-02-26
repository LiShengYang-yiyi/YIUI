#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEditor;
using UnityEngine;

namespace YIUIFramework
{
    public sealed partial class UIPanelSplitData
    {
        [OdinSerialize]
        [LabelText("生成通用界面枚举")]
        [ShowIf("ShowCreatePanelViewEnum")]
        internal bool CreatePanelViewEnum = true;

        internal bool ShowCreatePanelViewEnum()
        {
            if (!ShowAutoCheckBtn()) return false;
            return (AllCommonView.Count + AllCreateView.Count + AllPopupView.Count) >= 1;
        }

        private bool ShowAutoCheckBtn()
        {
            if (Panel == null)
            {
                return false;
            }

            if (!Panel.name.EndsWith(YIUIConstHelper.Const.UIPanelName))
            {
                return false;
            }

            if (UIOperationHelper.CheckUIOperationAll(Panel, false)) return false;
            return true;
        }

        private bool ShowCheckBtn()
        {
            if (Panel == null)
            {
                return false;
            }

            if (!Panel.name.EndsWith(YIUIConstHelper.Const.UISource))
            {
                return false;
            }

            return true;
        }

        [GUIColor(0, 1, 1)]
        [Button("检查拆分数据", 30)]
        [ShowIf("ShowCheckBtn")]
        private void AutoCheckSourceBtn()
        {
            AutoCheck();
        }

        [GUIColor(0f, 0.4f, 0.8f)]
        [Button("检查拆分数据", 30)]
        [ShowIf("ShowAutoCheckBtn")]
        private void AutoCheckPanelBtn()
        {
            if (AutoCheck())
            {
                ResetChild();
            }
        }

        internal bool AutoCheck()
        {
            if (Panel == null)
            {
                Debug.LogError($"没有找到 Panel");
                return false;
            }

            ReCheckViewParent();

            if (!CheckPanelName()) return false;

            FindChildParent();

            if (AllViewParent != null)
            {
                CheckViewParent(AllCommonView, AllViewParent);
                CheckViewParent(AllCreateView, AllViewParent);
            }
            else
            {
                AllCommonView.Clear();
                AllCreateView.Clear();
            }

            if (AllPopupViewParent != null)
            {
                CheckViewParent(AllPopupView, AllPopupViewParent);
            }
            else
            {
                AllPopupView.Clear();
            }

            if (AllViewParent != null)
            {
                CheckViewName(AllCommonView);
                CheckViewName(AllCreateView);
            }

            if (AllPopupViewParent != null)
            {
                CheckViewName(AllPopupView);
            }

            var hashList = new HashSet<RectTransform>();
            CheckRepetition(ref hashList, AllCommonView);
            CheckRepetition(ref hashList, AllCreateView);
            CheckRepetition(ref hashList, AllPopupView);

            return true;
        }

        private void ReCheckViewParent()
        {
            if (Panel == null) return;

            if (AllViewParent == null || AllViewParent?.name != YIUIConstHelper.Const.UIAllViewParentName)
            {
                AllViewParent = Panel.transform.FindChildByName(YIUIConstHelper.Const.UIAllViewParentName)?.GetComponent<RectTransform>();
            }

            if (AllViewParent != null)
            {
                if (!CheckViewParentIsPanel(AllViewParent))
                {
                    Debug.LogError($"{Panel.name}的{AllViewParent.name}不是Panel的子物体 请检查", Panel);
                    AllViewParent = null;
                }
            }

            if (AllPopupViewParent == null || AllPopupViewParent?.name != YIUIConstHelper.Const.UIAllPopupViewParentName)
            {
                AllPopupViewParent = Panel.transform.FindChildByName(YIUIConstHelper.Const.UIAllPopupViewParentName)?.GetComponent<RectTransform>();
            }

            if (AllPopupViewParent != null)
            {
                if (!CheckViewParentIsPanel(AllPopupViewParent))
                {
                    Debug.LogError($"{Panel.name}的{AllPopupViewParent.name}不是Panel的子物体 请检查", Panel);
                    AllPopupViewParent = null;
                }
            }
        }

        private bool CheckViewParentIsPanel(Transform parent)
        {
            if (parent == null || parent.parent == null) return false;
            if (parent.parent == Panel.transform) return true;
            return CheckViewParentIsPanel(parent.parent);
        }

        private void FindChildParent()
        {
            if (AllViewParent != null)
            {
                for (int i = 0; i < AllViewParent.childCount; i++)
                {
                    var child = AllViewParent.GetChild(i).GetComponent<RectTransform>();
                    if (child.name.EndsWith(YIUIConstHelper.Const.UIViewParentName))
                    {
                        if (!AllCommonView.Contains(child) && !AllCreateView.Contains(child))
                        {
                            AllCreateView.Add(child);
                        }
                    }
                }
            }

            if (AllPopupViewParent != null)
            {
                for (int i = 0; i < AllPopupViewParent.childCount; i++)
                {
                    var child = AllPopupViewParent.GetChild(i).GetComponent<RectTransform>();
                    if (child.name.EndsWith(YIUIConstHelper.Const.UIViewParentName))
                    {
                        if (!AllPopupView.Contains(child))
                        {
                            AllPopupView.Add(child);
                        }
                    }
                }
            }
        }

        private void ResetChild()
        {
            if (AllViewParent != null)
            {
                CheckChild(AllCommonView, true);
                CheckChild(AllCreateView, false);
            }

            if (AllPopupViewParent != null)
            {
                CheckChild(AllPopupView, false);
            }
        }

        private bool CheckPanelName()
        {
            var qualifiedName = NameUtility.ToFirstUpper(Panel.name);
            if (Panel.name != qualifiedName)
            {
                Panel.name = qualifiedName;
            }

            var sourceName = Panel.name == YIUIConstHelper.Const.UIYIUIPanelSourceName;
            var panelName  = Panel.name == $"{YIUIConstHelper.Const.UIProjectName}{YIUIConstHelper.Const.UIPanelName}";
            if (sourceName || panelName)
            {
                Debug.LogError($"当前是默认名称 请手动修改名称 {(sourceName ? $"Xxx{YIUIConstHelper.Const.UIPanelSourceName}" : "")} {(panelName ? $"Xxx{YIUIConstHelper.Const.UIPanelName}" : "")}");
                return false;
            }

            var sourceEndsWith = Panel.name.EndsWith($"{YIUIConstHelper.Const.UIPanelSourceName}");
            var panelEndsWith  = Panel.name.EndsWith($"{YIUIConstHelper.Const.UIPanelName}");

            if (sourceEndsWith || panelEndsWith)
            {
                return true;
            }

            Debug.LogError($"{Panel.name} 命名必须以指定规则结尾: [{YIUIConstHelper.Const.UIPanelName} / {YIUIConstHelper.Const.UIPanelSourceName}] 请勿随意修改");
            return false;
        }

        private void CheckChild(List<RectTransform> list, bool needView)
        {
            for (var i = list.Count - 1; i >= 0; i--)
            {
                var current = list[i];
                if (current == null)
                {
                    list.RemoveAt(i);
                    continue;
                }

                var viewName = current.name.Replace(YIUIConstHelper.Const.UIParentName, "");

                var viewPath = GetOnlyPrefabAssetsPath(viewName, false);

                if (string.IsNullOrEmpty(viewPath))
                {
                    viewPath = ViewSaveAsPrefabAsset(current);
                }

                if (string.IsNullOrEmpty(viewPath))
                {
                    list.RemoveAt(i);
                    Debug.LogError($"{current.parent?.parent?.name}的{current.parent?.name}下的ViewParent 没有找到对应View预制 请手动生成",
                        current);
                    continue;
                }

                if (!needView)
                {
                    for (int j = 0; j < current.childCount; j++)
                    {
                        Object.DestroyImmediate(current.GetChild(0).gameObject);
                    }
                }
                else
                {
                    var reLink = false;

                    if (current.childCount == 1)
                    {
                        var child         = current.GetChild(0);
                        var checkInstance = PrefabUtility.IsPartOfPrefabInstance(child);
                        if (!checkInstance)
                        {
                            Debug.Log($"{current.parent?.parent?.name}的{current.parent?.name}下的ViewParent 子物体必须是指定预制体 将强制删除重新关联",
                                current);
                            reLink = true;
                        }
                        else
                        {
                            var prefabAssetPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(child);
                            var prefabName      = Path.GetFileNameWithoutExtension(prefabAssetPath);
                            if (prefabName != viewName)
                            {
                                Debug.Log($"{current.parent?.parent?.name}的{current.parent?.name}下的ViewParent 子物体必须是指定预制体 当前是预制但不是指定预制体 将强制删除重新关联",
                                    current);
                                reLink = true;
                            }
                        }
                    }
                    else
                    {
                        reLink = true;
                    }

                    if (reLink)
                    {
                        for (int j = 0; j < current.childCount; j++)
                        {
                            Object.DestroyImmediate(current.GetChild(0).gameObject);
                        }

                        var viewAsset = (GameObject)AssetDatabase.LoadAssetAtPath(viewPath, typeof(Object));
                        PrefabUtility.InstantiatePrefab(viewAsset, current);
                    }
                }
            }
        }

        private string ViewSaveAsPrefabAsset(RectTransform current)
        {
            var viewName = current.name.Replace(YIUIConstHelper.Const.UIParentName, "");
            var view     = CreateYIUIView(viewName);
            var source   = Panel.GetComponent<UIBindCDETable>();
            var pkgName  = source?.PkgName;
            if (string.IsNullOrEmpty(pkgName))
            {
                Debug.LogError($"{current.parent?.parent?.name}的{current.parent?.name}下的ViewParent: [{current.name}] Panel 不是CDE 且没有找到PakName",
                    current);
                return "";
            }

            var path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(source.gameObject);
            if (string.IsNullOrEmpty(path))
            {
                path = GetOnlyPrefabAssetsPath(source.gameObject.name);
            }

            var loadSource = (GameObject)AssetDatabase.LoadAssetAtPath(path, typeof(Object));
            if (loadSource == null)
            {
                Debug.LogError($"{current.parent?.parent?.name}的{current.parent?.name}下的ViewParent: [{current.name}] 没有找到资源 {path}",
                    current);
                return "";
            }

            var savePath = Path.GetDirectoryName(path);
            var newPath = $"{savePath}/{viewName}.prefab";
            PrefabUtility.SaveAsPrefabAsset(view, newPath);
            Object.DestroyImmediate(view);
            SelectAssetAtPath(newPath, false);
            return newPath.Replace("Assets/../", "");
        }

        private GameObject CreateYIUIView(string name)
        {
            var viewObject = new GameObject();
            viewObject.name = name;
            var viewRect = viewObject.GetOrAddComponent<RectTransform>();
            viewObject.GetOrAddComponent<CanvasRenderer>();
            var cdeTable = viewObject.GetOrAddComponent<UIBindCDETable>();
            cdeTable.UICodeType = EUICodeType.View;
            viewRect.ResetToFullScreen();
            viewObject.SetLayerRecursively(LayerMask.NameToLayer("UI"));
            return viewObject;
        }

        //命名检查
        private void CheckViewName(List<RectTransform> list)
        {
            for (var i = list.Count - 1; i >= 0; i--)
            {
                var current = list[i];
                if (current == null)
                {
                    list.RemoveAt(i);
                    continue;
                }

                var qualifiedName = NameUtility.ToFirstUpper(current.name);
                if (current.name != qualifiedName)
                {
                    current.name = qualifiedName;
                }

                if (current.name == YIUIConstHelper.Const.UIYIUIViewParentName)
                {
                    list.RemoveAt(i);
                    Debug.LogError($"{current.parent?.parent?.name}的{current.parent?.name}下的ViewParent 当前是默认名称 请手动修改名称 Xxx{YIUIConstHelper.Const.UIViewParentName}",
                        current);
                    continue;
                }

                if (!current.name.EndsWith(YIUIConstHelper.Const.UIViewParentName))
                {
                    list.RemoveAt(i);
                    Debug.LogError($"{current.parent?.parent?.name}的{current.parent?.name}下的ViewParent: [{current.name}] 命名必须以 {YIUIConstHelper.Const.UIViewParentName} 结尾 请勿随意修改",
                        current);
                    continue;
                }

                if (current.childCount >= 2)
                {
                    list.RemoveAt(i);
                    Debug.LogError($"{current.parent?.parent?.name}的{current.parent?.name}下的ViewParent: [{current.name}] 只能有<=1个子物体 你应该通过其他方式实现层级结构",
                        current);
                    continue;
                }

                if (current.transform.childCount == 1)
                {
                    var firstChild = current.transform.GetChild(0);
                    var viewCde    = firstChild.GetOrAddComponent<UIBindCDETable>();
                    viewCde.gameObject.name = current.name.Replace(YIUIConstHelper.Const.UIParentName, "");
                }
            }
        }

        //检查null / 父级
        private void CheckViewParent(List<RectTransform> list, RectTransform parent)
        {
            for (var i = list.Count - 1; i >= 0; i--)
            {
                if (list[i] == null)
                {
                    list.RemoveAt(i);
                    continue;
                }

                var current = list[i];

                var parentP = current.parent;
                if (parentP == null)
                {
                    list.RemoveAt(i);
                    continue;
                }

                if (parentP != parent)
                {
                    list.RemoveAt(i);

                    //因为只有2个父级 所以如果不是这个就会自动帮你移动到另外一个上面
                    //如果多了还是不要自动了
                    var currentParentName = parentP.name;
                    if (currentParentName == YIUIConstHelper.Const.UIAllViewParentName)
                    {
                        AllCreateView.Add(current);
                    }
                    else if (currentParentName == YIUIConstHelper.Const.UIAllPopupViewParentName)
                    {
                        AllPopupView.Add(current);
                    }
                }
            }
        }

        //检查重复
        private void CheckRepetition(ref HashSet<RectTransform> hashList, List<RectTransform> list)
        {
            for (var i = list.Count - 1; i >= 0; i--)
            {
                var current = list[i];
                if (!hashList.Add(current))
                {
                    list.RemoveAt(i);
                    Debug.LogError($"{current.parent?.parent?.name}的{current.parent?.name}下的ViewParent: [{current.name}] 重复存在 已移除 请检查",
                        current);
                }
            }
        }

        internal static GameObject SelectAssetAtPath(string assetPath, bool select = true)
        {
            AssetDatabase.SaveAssets();
            EditorApplication.ExecuteMenuItem("Assets/Refresh");
            var selectPath = assetPath.Replace("Assets/../", "");
            var assetObj   = (GameObject)AssetDatabase.LoadAssetAtPath(selectPath, typeof(Object));
            var cde        = assetObj.GetComponent<UIBindCDETable>();
            if (cde == null)
            {
                Debug.LogError($"{assetObj.name} 没有找到CDE 这是必须的 请检查");
            }
            else
            {
                cde.AutoCheck();
            }
            if (select)
            {
                EditorGUIUtility.PingObject(assetObj);
                Selection.activeObject = assetObj;
            }

            return assetObj;
        }

        internal static string GetOnlyPrefabAssetsPath(string assetName, bool tips = true)
        {
            var allResult = AssetDatabase.FindAssets($"{assetName} t:Prefab", null);
            if (allResult == null)
            {
                if (tips)
                    Debug.LogError($"未找到预制体 {assetName}");
                return null;
            }

            if (allResult.Length == 1)
            {
                return AssetDatabase.GUIDToAssetPath(allResult[0]);
            }

            var allPath = new List<string>();
            foreach (var guid in allResult)
            {
                var path     = AssetDatabase.GUIDToAssetPath(guid);
                var fileName = Path.GetFileNameWithoutExtension(path);
                if (fileName == assetName)
                {
                    allPath.Add(path);
                }
            }

            if (allPath.Count <= 0)
            {
                if (tips)
                    Debug.LogError($"未找到预制体 {assetName}");
                return null;
            }

            if (allPath.Count >= 2)
            {
                for (int i = 0; i < allPath.Count; i++)
                {
                    Debug.LogError($"[{i + 1}] 找到多个预制体 {assetName} 请检查 命名不能有重复!!!\n[{i + 1}] {allPath[i]}", LoadAssetAtPath(allPath[i]));
                }
            }

            return allPath[0];

            GameObject LoadAssetAtPath(string path)
            {
                var selectPath = path.Replace("Assets/../", "");
                var assetObj   = (GameObject)AssetDatabase.LoadAssetAtPath(selectPath, typeof(Object));
                return assetObj;
            }
        }
    }
}
#endif