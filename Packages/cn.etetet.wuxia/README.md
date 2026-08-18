# ET.WuXia

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

The expected outputs are:

```text
Assets/GameRes/YIUI/Login/Prefabs/LoginPanel.prefab
Scripts/ModelView/Client/YIUIGen/Login/LoginPanelComponentGen.cs
Scripts/ModelView/Client/YIUIComponent/Login/LoginPanelComponent.cs
Scripts/HotfixView/Client/YIUIGen/Login/LoginPanelComponentSystemGen.cs
Scripts/HotfixView/Client/YIUISystem/Login/LoginPanelComponentSystem.cs
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

The repository currently keeps `cn.etetet.yiuistatesync` active. During the
first phase, use its YIUI initialization and add only new WuXia panels here.
Do not add another `EntryEvent3_InitClient` yet, because two handlers would
initialize two UI roots.

After WuXia Login, Lobby, Main, and HUD are ready:

1. Move the required YIUI initialization and UI transition events into this package.
2. Add the WuXia YooAsset collector and remove the demo collector.
3. Remove `cn.etetet.yiuistatesync` from `Packages/manifest.json`.
4. Keep the legacy StateSync UGUI startup handlers disabled.
5. Regenerate assembly references and compile `ET.sln`.
6. Verify login, enter-map, movement, AOI, and map transfer end to end.

Do not copy the demo's `YIUIDemoWindow`, embedded `ET.sln`, `.github` files, or
test coroutine input. They are demo switching infrastructure, not game code.

## Package rules

- Package id `9001` is reserved by this project; keep all project package ids in
  the internal `9000-9999` range and document new allocations.
- Dependencies point from WuXia to vendor packages, never from vendor packages
  back to WuXia.
- Pin dependency versions. Upgrade them in a dedicated change with a full flow test.
- Add a package only when it has a real ownership or reuse boundary; do not make
  one Unity package per UI panel.
