using UnityEngine;

namespace YIUIFramework
{
    /// <summary>
    /// 面板信息
    /// </summary>
    public class PanelInfo
    {
        public BasePanel UIBasePanel { get; private set; }

        public bool ActiveSelf => UIBasePanel?.ActiveSelf ?? false;

        /// <summary>
        /// 包名
        /// </summary>
        public string PkgName { get; internal set; }

        /// <summary>
        /// 资源名称 因为每个包分开 这个资源名称是有可能重复的 虽然设计上不允许  
        /// </summary>
        public string ResName { get; internal set; }

        /// <summary>
        /// C#文件名 因为有可能存在Res名称与文件名不一致的问题
        /// </summary>
        public string Name { get; internal set; }

        internal void Reset(UIBase uiBase)
        {
            switch (uiBase)
            {
                case null:
                    UIBasePanel = null;
                    break;
                case BasePanel basePanel:
                    UIBasePanel = basePanel;
                    break;
                default:
                    Debug.LogError($"当前UI 不是Panel 请检查 {PkgName} {ResName}");
                    break;
            }
        }
    }
}