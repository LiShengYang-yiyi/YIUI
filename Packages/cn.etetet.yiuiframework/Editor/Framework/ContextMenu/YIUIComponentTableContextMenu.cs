using UnityEditor;
using UnityEngine;

namespace YIUIFramework.Editor
{
    public class YIUIComponentTableContextMenu
    {
        [MenuItem("CONTEXT/Component/添加到YIUI组件表", false, -int.MaxValue)]
        private static void AddYIUIComponentTable(MenuCommand command)
        {
            if (!UIOperationHelper.CheckUIOperation()) return;
            if (command.context is not Component menuComponent) return;
            var cdeTable = Selection.activeGameObject.GetComponentInParent<UIBindCDETable>();
            if (cdeTable == null) return;
            cdeTable.ComponentTable ??= cdeTable.gameObject.AddComponent<UIBindComponentTable>();
            cdeTable.ComponentTable.EditorAddComponent(menuComponent);
        }

        [MenuItem("CONTEXT/Component/添加到YIUI组件表", true)]
        private static bool AddYIUIComponentTableValidate(MenuCommand command)
        {
            if (Selection.activeGameObject == null) return false;
            var cdeTable = Selection.activeGameObject.GetComponentInParent<UIBindCDETable>();
            if (cdeTable == null) return false;
            return true;
        }
    }
}
