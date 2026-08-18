// 【武侠迁移 2026-08-18】项目自有登录面板占位组件，后续可通过 YIUI AutoTool 替换视觉内容。
// 作用：提供登录流程稳定的组件类型和资源入口。
// 原因：不能直接复用 Demo 的 LoginPanel 类型，否则两个包会产生类型和地址冲突。
using System;
using UnityEngine;
using YIUIFramework;
using System.Collections.Generic;

namespace ET.Client
{
    public partial class WuXiaLoginPanelComponent : Entity
    {
    }
}
