# cn.etetet.wuxia 包开发与运行指南

## 1. 定位与结论

`cn.etetet.wuxia` 是项目自有业务包，不是 `cn.etetet.yiuistatesync` 的子目录，也不要求业务代码写入 StateSync、Login 或 YIUI 包。

依赖关系只有一个方向：

```text
cn.etetet.wuxia
  -> cn.etetet.statesync
  -> cn.etetet.login
  -> cn.etetet.yiuiframework
  -> cn.etetet.yiuiyooassets
```

这些依赖等同于 NuGet/npm 的包引用。WuXia 可以调用依赖包公开的 `LoginHelper`、`EnterMapHelper`、协议、Entity 和 YIUI API；依赖包不能反向引用 WuXia。

Unity 的首场景仍是：

```text
Packages/cn.etetet.loader/Scenes/Init.unity
```

Loader 只负责启动宿主、加载四层程序集并创建 ET 入口。WuXia 在 `EntryEvent3` 接管 StateSync 客户端业务入口。Loader 场景不必静态引用 WuXia：YIUI 找不到场景内 `YIUIRoot` 时，会从 YooAsset 地址 `YIUIRoot` 动态实例化 WuXia 包内预制体。

## 2. 包目录与程序集边界

```text
cn.etetet.wuxia/
  Assets/                         WuXia 自有 Unity/YIUI 资源
  Documentation/                  安装、迁移、业务流程文档
  Editor/                         Init、校验和安装期适配工具
  Excel/                          WuXia 自有配置表源文件
  Proto/                          WuXia 自有协议定义
  Scripts/
    Model/{Client,Server,Share}    数据定义，不引用 Unity/YIUI
    Hotfix/{Client,Server,Share}   热更业务逻辑
    ModelView/Client               Unity/YIUI 表现层数据
    HotfixView/Client              Unity/YIUI 表现层系统
  Settings/                       包内配置源与安装模板
  package.json                    Unity/npm 依赖和发布元数据
  packagegit.json                 ET 包编号与四层程序集引用
  Ignore.ET.WuXia.asmdef          顶层默认隔离
```

`Scripts` 下的 `.asmref` 把代码合并到 ET 的 `ET.Model`、`ET.Hotfix`、`ET.ModelView`、`ET.HotfixView`。因此 Rider 中不会出现四个独立的 WuXia 运行时工程，这是 ET9 的正常包模型。`ET.WuXia.Editor` 是独立 Editor 程序集。

## 3. 从包安装

正式发布后，在目标项目 `Packages/manifest.json` 的 `dependencies` 中加入：

```json
"cn.etetet.wuxia": "0.3.0"
```

Unity/npm 会根据 `package.json` 自动解析 StateSync、Login 和 YIUI 依赖。安装完成后执行：

1. 先执行 `ET -> StateSync -> Init`，完成 Excel、Proto、Loader、程序集和 `INITED` 初始化。
2. 再执行 `ET -> WuXia -> Init`。
3. 等待 Unity 刷新，在 Rider 中重新加载根目录 `ET.sln`，确认 `Unity/ET.WuXia.Editor` 已出现。
4. 重新编译根目录 `ET.sln`。
5. 执行 `ET -> WuXia -> Validate YIUI Migration`。
6. 打开 `Packages/cn.etetet.loader/Scenes/Init.unity` 并运行。

当前仓库中 WuXia 是 Embedded/Custom 包，因此不需要再向本仓库的 `manifest.json` 写自身版本；发布到另一项目时才使用版本声明。

## 4. WuXia Init 做什么

`ET -> WuXia -> Init` 是幂等操作，可以在第三方包升级后再次执行。

### 4.1 YIUI 项目设置

YIUI 编辑器把项目配置路径固定为：

```text
Assets/GameRes/YIUI/YIUISettings
```

这是 YIUI 的宿主扩展点，不是 WuXia 业务源码目录。WuXia 的权威模板位于：

```text
Packages/cn.etetet.wuxia/Settings/Templates
```

初始化器在目标文件缺失时生成 `YIUIConstAsset.txt` 和 `YIUIAtlasData.asset`，并只把 `UIETCreatePackageName`、`UIETCreatePackagePath` 指向 `wuxia`，不会覆盖使用者已有的分辨率、安全区等参数。

### 4.2 Rider 解决方案同步

`ET -> WuXia -> Init` 会调用包内 `WuXiaRiderSolutionSynchronizer`：

1. 通过 Unity 工程同步入口生成最新 `ET.WuXia.Editor.csproj`。
2. 将 Loader 链接的根 `ET.sln` 转为宿主私有副本，原内容备份到 `Library/WuXia/SolutionBackups`。
3. 使用标准 `dotnet sln add` 将 `ET.WuXia.Editor` 加入 `Unity` 解决方案分组。
4. 再次执行时检测已有项目并直接返回，不重复添加。

该流程只修改宿主根方案，不修改 `cn.etetet.loader`、`cn.etetet.statesync`、`cn.etetet.yiuistatesync` 中的源码或方案文件。单独需要刷新 Rider 时可以执行 `ET -> WuXia -> Sync Rider Solution`。

### 4.3 YooAsset 收集

包内源配置为：

```text
Packages/cn.etetet.wuxia/Settings/WuXiaYooAssetSetting.asset
```

初始化时合并到项目唯一的 `DefaultPackage`，生成启用的 `WuXia` 分组：

| 收集路径 | 过滤规则 | 用途 |
|---|---|---|
| `Packages/cn.etetet.wuxia/Assets/GameRes/YIUI` | `YIUIFilterRule` | Login、Lobby、Main、YIUIRoot 和图片 |
| `Assets/GameRes/YIUI/YIUISettings` | `CollectAll` | YIUIConstAsset、YIUIAtlasData |

若安装了 `cn.etetet.yiuistatesync`，其 UI 收集器会完整移动到 `DisableGroup`，不会删除，避免 `YIUIRoot` 等地址重复。

### 4.4 Demo 入口冲突

`cn.etetet.statesync` 是完整 Demo，会自带旧 UGUI 的 Entry、Login、Lobby、UIHelp 事件；`cn.etetet.yiuistatesync` 也会自带一套 YIUI 事件。它们与 WuXia 不能同时活动。

初始化器对存在的文件增加整文件条件编译保护：

```text
WUXIA_LEGACY_STATESYNC_UI   显式恢复 StateSync 旧 UGUI
YIUI_STATESYNC_DEMO         显式恢复 YIUI StateSync Demo，同时停用 WuXia 事件
```

原文件内容不会删除。每个新增保护都带中文的日期、作用和原因。正式发布项目通常不安装 `cn.etetet.yiuistatesync`，它只应作为参考或回滚包存在。

### 4.5 ET 四层引用

初始化器最后调用 ET 官方的 `ET/Loader/UpdateScriptsReferences`，把 `packagegit.json` 中的 `ET.YIUIFramework`、`Unity.TextMeshPro` 等引用汇总到宿主四层 asmdef。若项目版本把菜单显示为 `ET -> Refresh`，手动执行该菜单即可。

## 5. 运行时代码流程

### 5.1 启动

```text
Loader Init.unity
  -> Init MonoBehaviour
  -> CodeLoader 加载 Model/ModelView/Hotfix/HotfixView
  -> Entry 创建 Fiber
  -> StateSync Fiber 发布 EntryEvent3
  -> WuXiaEntryEvent3_InitClient
  -> 添加 GlobalComponent、ResourcesLoaderComponent、PlayerComponent、CurrentScenesComponent
  -> 添加并初始化 YIUIMgrComponent
  -> 场景查找 YIUIRoot；找不到则从 WuXia 的 Common/YIUIRoot 动态加载
  -> 发布 AppStartInitFinish
  -> 打开 WuXiaLoginPanel
```

入口代码：

```csharp
bool initialized = await root.AddComponent<YIUIMgrComponent>().Initialize();
if (!initialized)
{
    Log.Error("WuXia YIUI initialization failed");
    return;
}

await EventSystem.Instance.PublishAsync(root, new AppStartInitFinish());
```

### 5.2 登录

```text
WuXiaLoginPanel 登录按钮
  -> OnEventLoginInvoke
  -> 读取 GlobalConfig.Address、账号、密码
  -> LoginHelper.Login
  -> Router 获取 Realm/网关地址
  -> Realm 登录并取得 Gate Key
  -> Gate 登录并创建 ClientSender Session
  -> 发布 LoginFinish
  -> 关闭 WuXiaLoginPanel
  -> 打开 WuXiaLobbyPanel
```

当前代码直接复用 `cn.etetet.login` 的网络流程。正式账号系统应在 WuXia 的 Server/Hotfix 中增加认证、封禁、角色和重连规则，不要在 UI 中复制 Router/Realm/Gate 协议。

### 5.3 进入地图

```text
WuXiaLobbyPanel 进入地图按钮
  -> OnEventEnterMapInvoke
  -> EnterMapHelper.EnterMapAsync
  -> C2G_EnterMap
  -> GateMap 创建临时 Unit
  -> TransferHelper 转移到 Map1
  -> M2C_StartSceneChange
  -> 客户端加载 Map1
  -> M2C_CreateMyUnit
  -> SceneChangeFinish
  -> 打开 WuXiaMainPanel
```

Lobby 当前在请求开始时隐藏，完成后关闭。后续应补充按钮防重入、RPC 错误、场景等待超时和失败后恢复 Lobby。

### 5.4 地图移动

移动暂时复用 StateSync：

```text
右键点击 Map Layer 碰撞体
  -> Physics.Raycast
  -> C2M_PathfindingResult(target)
  -> Gate 通过 Location 转发到玩家所在 Map Fiber
  -> 服务端 Recast 寻路
  -> MoveComponent 服务端权威移动
  -> AOI 广播 M2C_PathfindingResult
  -> 客户端按相同路径表现插值
  -> 服务端广播 M2C_Stop
  -> 客户端用最终位置和旋转校正
```

地图地面必须有 Collider 且处于 `Map` Layer。正式业务必须在服务端增加目标点边界、可达性、最大距离、频率和速度校验，不能信任客户端坐标。

更完整的协议、Fiber、Actor Location、Transfer、AOI 和移动时序见项目 `Book/9.1登录进入地图移动完整流程.md`。

## 6. 替换占位 UI

1. 打开 `ET -> YIUI AutoTool`。
2. 确认目标 ET 包为 `wuxia`。
3. 编辑 WuXia 包内 `Source/WuXia*PanelSource.prefab`。
4. 从 Prefab 资源执行发布/生成，不要从 Hierarchy 临时实例生成。
5. 生成代码只写 `YIUIGen`；业务代码写 `YIUIComponent` 和 `YIUISystem`。
6. Login 保持 `u_ComAccount`、`u_ComPassword`、`u_EventLogin`，Lobby 保持 `u_EventEnterMap`，或者同步修改手写系统。
7. 重新初始化/刷新资源，编译并执行完整流程验收。

## 7. 验收与调试

静态验收：

1. `ET -> WuXia -> Validate YIUI Migration` 无错误。
2. `ET.WuXia.Editor`、`ET.ModelView`、`ET.HotfixView` 编译无错误。
3. YooAsset 模拟清单包含 `YIUIConstAsset`、`YIUIAtlasData`、`YIUIRoot`、三个 WuXia 面板。
4. 模拟清单不包含 Demo 的 Login/Lobby/Main/YIUIRoot 重复地址。

运行验收：

1. 启动只初始化一次 YIUI，只出现一个 WuXiaLoginPanel。
2. 登录请求只发送一次，成功后 Login 关闭、Lobby 打开。
3. 进入地图后 Lobby 关闭、WuXiaMainPanel 打开。
4. Map1 角色创建、右键移动、AOI 同步正常。
5. 按 `T` 在 Map1/Map2 传送后 Main 不重复堆叠。

## 8. 发布与版本

包根目录 `.github/workflows/release-package.yml` 使用 `npm pack --dry-run` 检查并发布到 GitHub Packages。发布前必须：

1. 向 ET Package 管理方申请正式 Package Id。
2. 同步修改 `packagegit.json` 和 `PackageType.WuXia`；当前 `9001` 只是项目开发期临时编号。
3. 确认 `package.json` 的 repository 指向实际仓库并提升版本。
4. 在干净 ET9 项目仅安装 WuXia，执行 StateSync Init、WuXia Init、编译和完整登录进图移动验收。
5. 确认发布包不依赖 `cn.etetet.yiuistatesync` Demo。

## 9. 所有权原则

- 新业务代码、协议、Excel、资源、UI、配置源和编辑器工具全部放在 `cn.etetet.wuxia`。
- 外部 `Assets/GameRes/YIUI/YIUISettings` 是 YIUI 强制的宿主生成目录，权威模板仍在 WuXia 包。
- 项目唯一的 YooAsset 最终配置属于宿主；WuXia 通过包内模板幂等合并，不携带一份会覆盖其他模块的全局配置。
- 第三方包源码不删除；不可共存的 Demo 入口只加条件编译保护。
- 修改第三方包时必须保留中文注释，写明日期、作用和原因。
