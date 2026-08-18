# Changelog

## 0.3.0 - 2026-08-18

- 新增包内 Rider 方案同步器，由 WuXia Init 幂等加入 `ET.WuXia.Editor`，不再要求 Loader 识别业务包。
- 同步前生成宿主私有 `ET.sln` 并在 `Library/WuXia/SolutionBackups` 保留原方案，避免修改 Demo 包硬链接源。
- 新增幂等的 `ET -> WuXia -> Init`，统一接入资源、YIUI 设置、程序集引用和 Demo 入口保护。
- 新增 WuXia 自有 YooAsset 配置与 YIUI 宿主设置模板，明确包内配置源所有权。
- WuXia 运行不再强制要求 Loader Init 静态引用 YIUIRoot，可通过 YooAsset 地址动态加载。
- 补齐 GitHub Packages 发布元数据和 `release-package.yml`。
- 新增完整的包安装、程序集边界、启动、登录、进图、移动、验收和发布文档。
- 将 `9001` 更正为开发期临时编号，正式发布前仍需由 ET Package 管理方分配。

## 0.2.0 - 2026-08-18

- 将 StateSync 客户端和 YIUI 初始化的运行所有权移交给 WuXia。
- 新增项目自有 Login、Lobby、Main 占位面板和运行流程。
- 将 Init 场景的 YIUIRoot 引用切换到 WuXia 自有预制体。
- 保留 Demo 资源收集器原文并注释停用，同时为事件增加回滚条件编译。
- 在包元数据和 Unity 锁文件中声明直接登录包依赖。
- 增加迁移校验器和维护文档。
- 修正 YooAsset 收集器停用方式，使用 `DisableGroup` 保留原 Demo 配置，避免 Unity YAML 解析失败。
- 恢复原 `LoginPanelSource` GUID，并为 `WuXiaLoginPanelSource` 分配独立 GUID，避免 Unity 自动改写原资源。

## 0.1.0 - 2026-08-18

- Create the ET9 four-assembly business package structure.
- Reserve package id 9001 for WuXia project constants.
- Add StateSync and YIUI package dependencies.
