
## [2026-03-05 16:51] TASK-001: 驗證前置準備環境

Status: Complete

- **Verified**: .NET 8 SDK 已安裝且相容
- **Verified**: global.json 不存在，無相容性問題

Success - 前置準備環境驗證完成，可進行框架升級


## [2026-03-05 16:59] TASK-002: 原子化框架與套件升級及編譯修正

Status: Complete

- **Files Modified**: 
  - OrionCore.API\OrionCore.API.csproj
  - OrionCore.Mvc\OrionCore.Mvc.csproj
  - OrionCore.API.Tests\OrionCore.API.Tests.csproj
  - OrionCore.Mvc.Tests\OrionCore.Mvc.Tests.csproj
  - CodeGenerator\CodeGenerator.csproj

- **Code Changes**:
  - 所有專案 TargetFramework 更新完成：
    - OrionCore.API: netcoreapp3.1 → net8.0
    - OrionCore.Mvc: netcoreapp3.1 → net8.0
    - OrionCore.API.Tests: netcoreapp3.1 → net8.0
    - OrionCore.Mvc.Tests: netcoreapp3.1 → net8.0
    - CodeGenerator: netcoreapp3.1 → net8.0-windows (同時 SDK 從 Microsoft.NET.Sdk.WindowsDesktop → Microsoft.NET.Sdk)
  
  - 所有 NuGet 套件更新完成：
    - System.Drawing.Common: 4.7.0 → 8.0.24 (OrionCore.API, OrionCore.Mvc)
    - Newtonsoft.Json: 13.0.1 → 13.0.4 (OrionCore.API)
    - EF Core 套件群: 3.1.21 → 8.0.24 (OrionCore.API.Tests, CodeGenerator)

- **Verified**: dotnet restore 成功（4.0 秒）

- **Errors Found**: 預期中的編譯錯誤
  - OrionCore.API.Tests\DbContextExtensions.cs:38 - CS1061: RelationalCommandCache 不包含 GetRelationalCommand 方法
  - 此錯誤為計劃中明確標記的必修重寫項目（TASK-003）

Partial - 框架與套件升級完成，但存在預期中的 DbContextExtensions.ToSql 編譯錯誤，需在 TASK-003 處理


## [2026-03-05 17:06] TASK-003: 執行完整測試套件並驗證升級

Status: Complete

- **Files Modified**:
  - OrionCore.API.Tests\DbContextExtensions.cs (重寫 ToSql 方法)
  - OrionCore.API.Tests\WhereBuilderTests.cs (更新測試預期值)

- **Code Changes**:
  - DbContextExtensions.ToSql 完整重寫：
    - 移除反射相關 using (System.Reflection, EF 內部 namespaces)
    - 移除 getPrivate<T> 私有反射方法
    - 移除依賴 _relationalQueryContext 和 _relationalCommandCache 的實作
    - 移除 #pragma warning disable EF1001
    - 改用 EF Core 8 公開 API: query.ToQueryString()
    - 新增詳細 XML 文檔註解
  
  - WhereBuilderTests 測試更新（適應 EF Core 8 SQL 生成變更）：
    - In/NotIn 單一值：優化為 = / <> 
    - Between：優化為 >=
    - NOT (EXISTS：優化為 NOT EXISTS

- **Tests**: 
  - OrionCore.API.Tests: 231/231 通過 (100%)
  - OrionCore.Mvc.Tests: 20/20 通過 (100%)
  - 總計: 251/251 測試通過

- **Verified**: 
  - DbContextExtensions.ToSql 不再依賴 EF 內部私有欄位
  - 使用 EF Core 8 支援的公開 API
  - 所有測試在升級後 provider/runtime 組合下穩定通過

Success - 測試套件執行成功，DbContextExtensions.ToSql 重寫驗收通過


## [2026-03-05 17:09] TASK-004: 最終提交

Status: Complete

- **Commits**: 
  - 44ed023: "完成 .NET 8.0 升級（5 個專案，251 測試全通過）"
  - 9 files changed, 135 insertions(+), 72 deletions(-)

- **Files Modified**:
  - CodeGenerator/CodeGenerator.csproj
  - OrionCore.API/OrionCore.API.csproj
  - OrionCore.Mvc/OrionCore.Mvc.csproj
  - OrionCore.API.Tests/OrionCore.API.Tests.csproj
  - OrionCore.Mvc.Tests/OrionCore.Mvc.Tests.csproj
  - OrionCore.API.Tests/DbContextExtensions.cs
  - OrionCore.API.Tests/WhereBuilderTests.cs

- **Files Created**:
  - .github/upgrades/scenarios/new-dotnet-version_4435db/execution-log.md
  - .github/upgrades/scenarios/new-dotnet-version_4435db/tasks.md (updated)

Success - 所有升級變更已提交至 upgrade-to-NET8 分支

