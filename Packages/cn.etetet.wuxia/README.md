# ET.WuXia

`cn.etetet.wuxia` 是 ET9 + StateSync + YIUI 的项目自有业务包。所有武侠业务代码、UI、资源、协议、配置源和安装工具都由本包管理；Login、StateSync、YIUI 作为单向包依赖使用。

首次安装按以下顺序执行：

1. `ET -> StateSync -> Init`
2. `ET -> WuXia -> Init`
3. 重新加载 Rider 中的根目录 `ET.sln`，确认 `Unity/ET.WuXia.Editor` 已出现
4. 编译根目录 `ET.sln`
5. `ET -> WuXia -> Validate YIUI Migration`
6. 从 `Packages/cn.etetet.loader/Scenes/Init.unity` 启动

完整的包结构、安装行为、启动/登录/进图/移动流程与发布要求见 `Documentation/WuXia-Package-Guide.md`。

`cn.etetet.wuxia` is the project-owned business package. Vendor packages under
`cn.etetet.statesync`, `cn.etetet.login`, and `cn.etetet.yiui*` are dependencies;
new game rules, UI, protocols, configuration, and project integration belong here.

## Assembly layout

| Directory | Assembly | Responsibility |
|---|---|---|
| `Scripts/Model/Share` | `ET.Model` | Shared entities, components, constants, and serialized data |
| `Scripts/Model/Client` | `ET.Model` | Client-only model declarations |
| `Scripts/Model/Server` | `ET.Model` | Server-only model declarations |
| `Scripts/Hotfix/Share` | `ET.Hotfix` | Shared runtime systems and pure game rules |
| `Scripts/Hotfix/Client` | `ET.Hotfix` | Client networking and non-Unity logic |
| `Scripts/Hotfix/Server` | `ET.Hotfix` | Server handlers and authoritative game logic |
| `Scripts/ModelView/Client` | `ET.ModelView` | Unity and YIUI component declarations |
| `Scripts/HotfixView/Client` | `ET.HotfixView` | YIUI systems, input, animation, and presentation |

Do not put UnityEngine or YIUI types in Model or Hotfix. Do not edit generated
files under `YIUIGen`; implement behavior in `YIUIComponent` and `YIUISystem`.

## YIUI module workflow

Create project UI modules with `ET -> YIUI AutoTool`:

1. Set the target ET package to `wuxia`.
2. Create a module such as `Login`, `Lobby`, `Main`, or `HUD`.
3. Keep prefabs under `Assets/GameRes/YIUI/<Module>/Prefabs` inside this package.
4. Publish/generate the module from the prefab asset, not a Hierarchy instance.
5. Commit both generated files and handwritten partial files.

在 Demo 包仍安装期间，项目面板必须保留 `WuXia` 前缀。生成结果应为：

```text
Assets/GameRes/YIUI/Login/Prefabs/WuXiaLoginPanel.prefab
Scripts/ModelView/Client/YIUIGen/Login/WuXiaLoginPanelComponentGen.cs
Scripts/ModelView/Client/YIUIComponent/Login/WuXiaLoginPanelComponent.cs
Scripts/HotfixView/Client/YIUIGen/Login/WuXiaLoginPanelComponentSystemGen.cs
Scripts/HotfixView/Client/YIUISystem/Login/WuXiaLoginPanelComponentSystem.cs
```

Before loading packaged UI at runtime, add this collector to the project's
YooAsset `DefaultPackage`:

```text
CollectPath: Packages/cn.etetet.wuxia/Assets/GameRes/YIUI
AddressRuleName: AddressByFileName
PackRuleName: PackDirectory
FilterRuleName: YIUIFilterRule
```

## Migration from YIUI.StateSync demo

自 2026-08-18 起，WuXia 负责客户端入口、YIUIRoot、Login、Lobby 和 Main 流程。
Demo 包仍作为固定版本的回滚和参考源保留；其中五个运行时事件通过
`YIUI_STATESYNC_DEMO` 条件编译保护，YooAsset 收集器保留原文但处于停用状态。

所有权、替换、校验和回滚说明见 `Documentation/WuXia-YIUI-Migration.md`。
第三方包更新后、发布构建前，请运行 `ET -> WuXia -> Validate YIUI Migration`。

Do not copy the demo's `YIUIDemoWindow`, embedded `ET.sln`, `.github` files, or
test coroutine input. They are demo switching infrastructure, not game code.

## Package rules

- Package id `9001` 仅为项目开发期临时编号；正式发布前必须向 ET Package 管理方申请编号并同步更新。
- Dependencies point from WuXia to vendor packages, never from vendor packages
  back to WuXia.
- Rider 方案同步由 `cn.etetet.wuxia/Editor` 自己负责；Loader 和第三方 Demo 包不得引用或识别 WuXia。
- Pin dependency versions. Upgrade them in a dedicated change with a full flow test.
- Add a package only when it has a real ownership or reuse boundary; do not make
  one Unity package per UI panel.
