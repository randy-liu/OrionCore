# OrionCore .NET 8 升級任務

## 概述

本文件追蹤 OrionCore 專案從 .NET Core 3.1 升級至 .NET 8.0 的執行作業。所有五個專案將在單一原子化作業中同步升級，隨後進行測試與驗證。

**進度**: 4/4 tasks complete (100%) ![100%](https://progress-bar.xyz/100)

---

## 任務

### [✓] TASK-001: 驗證前置準備環境 *(Completed: 2026-03-05 16:51)*
**參考**: Plan §Phase 0

- [✓] (1) 確認已安裝 .NET 8 SDK（執行 `dotnet --list-sdks` 驗證版本 ≥8.0）
- [✓] (2) .NET 8 SDK 已安裝 (**驗證**)
- [✓] (3) 檢查 global.json 檔案相容性（若存在於 repo root）
- [✓] (4) global.json 與 .NET 8 SDK 相容或不存在 (**驗證**)

---

### [✓] TASK-002: 原子化框架與套件升級及編譯修正 *(Completed: 2026-03-05 17:00)*
**參考**: Plan §Phase 1, Plan §各專案遷移計劃, Plan §套件升級參考, Plan §破壞性變更目錄

- [✓] (1) 更新所有 5 個專案的 TargetFramework 屬性（參考 Plan §各專案遷移計劃）：CodeGenerator→net8.0-windows, OrionCore.API→net8.0, OrionCore.Mvc→net8.0, OrionCore.API.Tests→net8.0, OrionCore.Mvc.Tests→net8.0
- [✓] (2) 所有專案 TargetFramework 已更新至目標版本 (**驗證**)
- [✓] (3) 更新所有 NuGet 套件參考（參考 Plan §套件升級參考）：EF Core 3.1.21→8.0.24, System.Drawing.Common 4.7.0→8.0.24, Newtonsoft.Json 13.0.1→13.0.4 及其他套件
- [✓] (4) 所有套件參考已更新至目標版本 (**驗證**)
- [✓] (5) 還原所有專案相依性（執行 `dotnet restore`）
- [✓] (6) 所有相依性成功還原 (**驗證**)
- [✓] (7) 建置整個解決方案並修正所有編譯錯誤（參考 Plan §破壞性變更目錄，重點：WPF API 相容性於 CodeGenerator、System.Drawing 跨平台議題、舊版組態 API、EF Core 查詢行為變更、ASP.NET Core hosting/middleware 差異）
- [✓] (8) 解決方案建置成功且 0 個錯誤 (**驗證**)

---

### [✓] TASK-003: 執行完整測試套件並驗證升級 *(Completed: 2026-03-05 17:06)*
**參考**: Plan §Phase 2, Plan §測試與驗證策略, Plan §各專案遷移計劃（OrionCore.API.Tests）

- [✓] (1) **優先重寫** OrionCore.API.Tests/DbContextExtensions.cs 的 DbContextExtensions.ToSql 方法（移除 EF Core 內部 API 反射依賴 `_relationalQueryContext`、`_relationalCommandCache`，改用 EF Core 8 支援機制如 interceptor 或命令記錄策略）
- [✓] (2) DbContextExtensions.ToSql 已重寫且不再依賴 EF 內部私有欄位 (**驗證**)
- [✓] (3) 執行 OrionCore.API.Tests 與 OrionCore.Mvc.Tests 測試專案
- [✓] (4) 修正所有測試失敗（參考 Plan §破壞性變更目錄，重點：EF Core 3.1→8.0 行為差異、API 合約變更）
- [✓] (5) 重新執行測試以驗證修正
- [✓] (6) 所有測試通過且 0 個失敗 (**驗證**)

---

### [✓] TASK-004: 最終提交 *(Completed: 2026-03-05 17:09)*
**參考**: Plan §原始碼版控策略

- [✓] (1) 提交所有變更，訊息："TASK-004: Complete .NET 8.0 upgrade for OrionCore (5 projects: CodeGenerator, OrionCore.API, OrionCore.Mvc, OrionCore.API.Tests, OrionCore.Mvc.Tests)"

---