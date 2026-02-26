#if UNITY_EDITOR
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEngine;

namespace YIUIFramework.Editor
{
    [HideReferenceObjectPicker]
    public abstract class BaseYIUIToolModule : BaseCreateModule
    {
        [HideInInspector]
        public OdinMenuEditorWindow AutoTool { get; set; }

        [HideInInspector]
        public OdinMenuTree Tree { get; set; }

        [HideInInspector]
        public string ModuleName { get; set; }

        [HideInInspector]
        public object UserData { get; set; }

        public virtual void SelectionMenu()
        {
        }
    }
}
#endif