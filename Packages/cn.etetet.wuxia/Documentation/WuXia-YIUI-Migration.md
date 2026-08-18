# WuXia YIUI 迁移与维护说明

## 当前结论

自 2026-08-18 起，StateSync 客户端的 UI 运行入口由
`cn.etetet.wuxia` 持有。原 `cn.etetet.yiuistatesync` 保留为固定版本的
回滚和参考包，不再拥有正常运行时事件，也不再参与项目流程 UI 的资源收集。

自 0.3.0 起，包内 `Settings/WuXiaYooAssetSetting.asset` 与
`Settings/Templates` 是安装配置源，`ET -> WuXia -> Init` 负责把它们幂等合并到
宿主项目。完整安装和运行说明见 `WuXia-Package-Guide.md`。

所有迁移修改使用以下注释格式：

```text
【武侠迁移 2026-08-18】：修改作用和原因
【武侠变更 2026-08-18】：WuXia 新增实现的职责
```

Unity YAML 注释可能在 Unity 重序列化时被移除，因此本文件和
`CHANGELOG.md` 是长期维护记录，不能只依赖场景文件里的注释。

## 运行所有权

| 职责 | 当前所有者 | 入口 |
|---|---|---|
| StateSync 客户端组件初始化 | WuXia | `WuXiaEntryEvent3_InitClient` |
| YIUI 初始化 | WuXia | `YIUIMgrComponent.Initialize()` |
| 启动后打开登录 | WuXia | `WuXiaAppStartInitFinish_CreateLoginUI` |
| 登录完成关闭登录 | WuXia | `WuXiaLoginFinish_RemoveLoginUI` |
| 登录完成打开大厅 | WuXia | `WuXiaLoginFinish_CreateLobbyUI` |
| 场景切换完成打开主界面 | WuXia | `WuXiaSceneChangeFinish_CreateMainUI` |
| 登录网络流程 | `cn.etetet.login` | `LoginHelper.Login` |
| 进入地图和等待切场景 | `cn.etetet.statesync` | `EnterMapHelper.EnterMapAsync` |

## 完整运行流程

```text
EntryEvent3
  -> WuXiaEntryEvent3_InitClient
  -> 添加 Global/ResourcesLoader/Player/CurrentScenes
  -> 初始化 YIUIMgrComponent
  -> 发布 AppStartInitFinish
  -> 打开 WuXiaLoginPanel

登录按钮
  -> WuXiaLoginPanelComponentSystem.OnEventLoginInvoke
  -> LoginHelper.Login
  -> 登录成功后发布 LoginFinish
  -> 关闭 WuXiaLoginPanel
  -> 打开 WuXiaLobbyPanel

进入地图按钮
  -> WuXiaLobbyPanelComponentSystem.OnEventEnterMapInvoke
  -> EnterMapHelper.EnterMapAsync
  -> 等待 Wait_SceneChangeFinish
  -> SceneChangeFinish
  -> 打开 WuXiaMainPanel
  -> 原 StateSync 输入、移动、AOI 和地图传送逻辑继续工作
```

## 资源说明

- `Common/Prefabs/YIUIRoot.prefab` 是从可运行 Demo 根节点迁移的项目副本。
- 当前仓库的 `Init.unity` 保留 WuXia YIUIRoot GUID
  `a6d5a2e738b64d649af948b1df924fed`，但这不是发布包的安装前提；干净项目中
  `YIUIMgrComponent.InitRoot` 会从 `Common/YIUIRoot` 地址动态加载。
- 运行资源地址分别为 `WuXiaLoginPanel`、`WuXiaLobbyPanel`、
  `WuXiaMainPanel`，避免与 Demo 地址冲突。
- 当前运行 Prefab 保留了 Demo 的最小可操作控件：账号、密码、登录按钮和
  进入地图按钮。Main 是后续 HUD 的占位实现。
- `Source/*Source.prefab` 是后续重新设计的空结构占位。未补齐控件和绑定前，
  不要直接发布覆盖当前可运行 Prefab。

## 后续替换 UI

1. 在 YIUI AutoTool 中将目标包设置为 `wuxia`。
2. 编辑对应的 `WuXia*PanelSource.prefab`。
3. 保持运行资源名和组件名的 `WuXia` 前缀不变。
4. Login 至少保留 `u_ComAccount`、`u_ComPassword` 和
   `u_EventLogin`。
5. Lobby 至少保留 `u_EventEnterMap`。
6. 发布后检查生成代码只覆盖 `YIUIGen`，业务逻辑继续放在
   `YIUISystem`。
7. 运行迁移校验，再执行登录、进图和移动验证。

## 第三方包升级检查

升级 `cn.etetet.statesync` 或 `cn.etetet.yiuistatesync` 后必须检查：

1. Demo 的五个事件文件仍由 `YIUI_STATESYNC_DEMO` 保护。
2. StateSync 旧 UGUI 的 Entry、Login、Lobby、UIHelp 文件仍处于整文件注释状态。
3. Demo YIUI 收集器保留在任一 `DisableGroup` 分组中；WuXia 和
   `YIUISettings` 收集器必须处于启用的 `WuXia` 分组。
4. `Init.unity` 不再出现 Demo YIUIRoot GUID
   `4200f826ed726d4478d4d19000af33c4`。
5. 项目中只有一个活动的 `EntryEvent3` StateSync 客户端 UI 初始化处理器。

可通过菜单 `ET -> WuXia -> Validate YIUI Migration` 自动检查上述静态条件。

## 回滚

回滚必须在独立分支或明确的故障处理提交中完成，不能只打开 Demo 编译符号。

1. 定义 `YIUI_STATESYNC_DEMO`，使 Demo 五个事件处理器重新注册；WuXia 的
   事件属性使用相反条件，会自动停止注册。
2. 将 `YIUI_Demo_Disabled` 分组的 `ActiveRuleName` 改为 `EnableGroup`，并同时
   停用 WuXia 收集器所在分组，避免重复资源地址。
3. 如需完全恢复场景所有权，可将 `Init.unity` 的 YIUIRoot GUID 改回 Demo GUID；
   未静态引用根节点的项目则恢复 Demo 收集器，使其 `YIUIRoot` 地址重新生效。
4. 清理资源并重新编译，确认只能看到一套 Login/Lobby/Main。
5. 回滚结束后移除该编译符号，重新运行迁移校验。

## 验收清单

- Unity 和 `ET.sln` 无编译错误。
- 启动日志只出现一次 YIUI 初始化。
- 启动后只出现 `WuXiaLoginPanel`。
- 登录请求只发送一次，成功后只出现 `WuXiaLobbyPanel`。
- 点击进入地图后 Lobby 关闭，Main HUD 出现。
- Map1 中右键移动、位置同步和 AOI 创建/销毁正常。
- Map1/Map2 传送后 Main HUD 不重复堆叠。
- YooAsset 构建报告不包含 Demo Login/Lobby/Main。

## YooAsset 模拟清单故障

若日志出现 `YIUIAtlasData` 或 `YIUIConstAsset` 的 `location is invalid`：

1. 先确认 `AssetBundleCollectorSetting.asset` 能被 Unity 正常解析。
2. 确认 `Assets/GameRes/YIUI/YIUISettings` 使用 `CollectAll` 且处于启用分组。
3. 退出运行状态，等待 Unity 完成资源重新导入后再次运行；编辑器模拟模式会自动
   重新生成 `DefaultPackage_Simulate` 清单。
4. 在模拟清单中确认存在 `YIUIAtlasData`、`YIUIConstAsset` 和三个 WuXia 面板地址。

不要在 Unity 序列化的收集器列表内部使用嵌套 YAML 注释来停用条目。该写法可能
触发 Unity `Parser Failure`，应使用 `DisableGroup` 保存和停用原配置。
