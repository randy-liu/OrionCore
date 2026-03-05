# `.NET 8` 升級計劃（`OrionCore`）

## 目錄
- [執行摘要](#執行摘要)
- [遷移策略](#遷移策略)
- [詳細相依性分析](#詳細相依性分析)
- [各專案遷移計劃](#各專案遷移計劃)
- [套件升級參考](#套件升級參考)
- [破壞性變更目錄](#破壞性變更目錄)
- [測試與驗證策略](#測試與驗證策略)
- [風險管理](#風險管理)
- [複雜度與工作量評估](#複雜度與工作量評估)
- [原始碼版控策略](#原始碼版控策略)
- [完成標準](#完成標準)

## 執行摘要
### 已選策略
**All-At-Once（一次性升級）策略**：所有專案在同一個原子化升級作業中同步調整。

### 平台目標（你提出的重點）
- 主要執行平台目標：**Windows + Linux（跨平台）**。
- `OrionCore.API`、`OrionCore.Mvc`、`OrionCore.API.Tests`、`OrionCore.Mvc.Tests` 目標為 `net8.0`（可跨平台）。
- `CodeGenerator` 因為是 `WPF` 專案，需使用 `net8.0-windows`（Windows-only）。

### 選擇理由
- 共 `5` 個專案，相依關係清楚，未發現循環相依。
- 目標框架一致為 `.NET 8.0`（`net8.0`；`CodeGenerator` 為 `net8.0-windows`）。
- assessment 已清楚指出升級範圍：框架、NuGet 套件、API 相容性。
- 符合本情境要求的統一升級模式。

### 探勘指標
- 專案數：`5`
- 總議題數：`206`（Mandatory `110`、Potential `94`、Optional `2`）
- 受影響檔案：`14`
- 程式碼行數（LOC）：`22,009`
- 主要風險特徵：`GDI+ / System.Drawing`（`61`）、`WPF`（`26`）、`Legacy Configuration`（`19`）

### 一致性確認（你提出的 WPF 疑問）
- `OrionCore.API` 專案類型是 `ClassLibrary`，**不屬於 WPF 專案**。
- 目前 assessment 內的 WPF 風險來源集中在 `CodeGenerator\CodeGenerator.csproj`。
- 因此本計劃中的 WPF 升級工作只應套用在 `CodeGenerator`，不應套用到 `OrionCore.API`。

### 矛盾點通知
- 若要求「**所有**專案都同時支援 Windows/Linux」，目前與 `CodeGenerator` 的 `WPF` 技術選型衝突。
- 原因：`WPF` 在 `.NET 8` 仍是 Windows 專屬。
- 可行解法：
  1. **維持現況（建議）**：核心服務/API 保持跨平台，`CodeGenerator` 維持 Windows-only。
  2. **完全跨平台改造**：將 `CodeGenerator` 從 `WPF` 遷移到可跨平台 UI/工具方案（需另開獨立遷移計畫）。

### 重點說明
本次計劃使用「一次性升級」，避免中間版本狀態導致套件與框架混搭不一致。

## 遷移策略
### 整體作法
- 以**單一協調批次**完成所有專案框架與套件更新。
- 依賴順序僅用於除錯與問題收斂，不採專案分批上線。
- 全程在同一升級分支進行。

### 實施階段
#### Phase 0：前置準備
- 確認工作分支：`upgrade-to-NET8`
- 確認 `.NET 8 SDK` 與 `global.json`（若存在）相容

#### Phase 1：原子化升級
- 更新所有專案與 MSBuild 匯入檔中的目標框架
- 依 assessment 建議更新所有 NuGet 套件版本
- 還原相依、建置、修正升級造成的編譯問題
- 重新建置直到達到乾淨建置標準

#### Phase 2：測試驗證
- 執行所有測試專案
- 修正因框架／套件／API 變動造成的測試回歸

## 詳細相依性分析
### 相依圖摘要
- `OrionCore.API`（核心/葉節點）
- `OrionCore.Mvc` 相依 `OrionCore.API`
- `OrionCore.API.Tests` 相依 `OrionCore.API`
- `OrionCore.Mvc.Tests` 相依 `OrionCore.Mvc` 與 `OrionCore.API`
- `CodeGenerator` 為獨立專案（`WPF`）

### 升級分組（同一批次內協調）
- **核心專案**：`OrionCore.API`、`OrionCore.Mvc`、`CodeGenerator`
- **測試專案**：`OrionCore.API.Tests`、`OrionCore.Mvc.Tests`

### 關鍵路徑
`OrionCore.API` → `OrionCore.Mvc` → `OrionCore.Mvc.Tests`

### 循環相依
- assessment 未顯示循環相依。

## 各專案遷移計劃
### 專案：`CodeGenerator\CodeGenerator.csproj`
**現況**：`netcoreapp3.1`、WPF、EF Core 3.1 套件、API 破壞性議題較多

**目標**：`net8.0-windows`

**平台說明**：此專案為 `WPF`，在 `.NET 8` 下屬 Windows-only，無法直接達成 Linux 執行。

**遷移步驟**：
1. 更新 `TargetFramework` 為 `net8.0-windows`。
2. 保留並確認桌面設定（`UseWPF`／Windows Desktop 需求）。
3. EF Core 相關套件升級至 `8.0.24`。
4. 修正 WPF 與舊版組態 API 相容性問題。
5. 驗證 Windows 環境建置與啟動行為。

### 專案：`OrionCore.API\OrionCore.API.csproj`
**現況**：`netcoreapp3.1`，`ClassLibrary`（非 WPF），含 `System.Drawing.Common 4.7.0`

**目標**：`net8.0`

**遷移步驟**：
1. 更新 `TargetFramework` 為 `net8.0`。
2. 更新 `System.Drawing.Common`：`4.7.0 -> 8.0.24`。
3. 更新 `Newtonsoft.Json`：`13.0.1 -> 13.0.4`。
4. 處理 `System.Drawing` 的跨平台風險。
5. 處理 Legacy Cryptography 相容性警示。

### 專案：`OrionCore.Mvc\OrionCore.Mvc.csproj`
**現況**：`netcoreapp3.1`，相依 `OrionCore.API`，使用 `System.Drawing.Common`

**目標**：`net8.0`

**遷移步驟**：
1. 更新 `TargetFramework` 為 `net8.0`。
2. 更新 `System.Drawing.Common`：`4.7.0 -> 8.0.24`。
3. 對齊 API 專案合約變更造成的相依影響。
4. 驗證 ASP.NET Core 設定與 middleware 相容性。

### 專案：`OrionCore.API.Tests\OrionCore.API.Tests.csproj`
**現況**：`netcoreapp3.1`，測試與 EF 套件仍為 3.1 世代

**目標**：`net8.0`

**遷移步驟**：
1. 更新 `TargetFramework` 為 `net8.0`。
2. EF 相關測試套件升級至 `8.0.24`。
3. **優先重寫** `OrionCore.API.Tests/DbContextExtensions.cs` 的 `DbContextExtensions.ToSql`（目前透過反射讀取 EF Core 私有欄位 `_relationalQueryContext`、`_relationalCommandCache`）。
4. 將內部反射式 SQL 取得方式改為 EF Core 8 可支援機制（例如 provider 可用的 SQL 擷取、interceptor、命令記錄策略）。
5. 修正其餘受 API／EF 行為改變影響的測試程式。

**關鍵升級焦點：`DbContextExtensions.ToSql`**
- 目前做法依賴 EF 內部 API（`EF1001`）與私有欄位名稱，升級後高機率失效。
- 本項視為 `.NET 8 + EF Core 8` 測試穩定性的**必修阻斷項**。
- 需達成以下驗收條件：
  - 不使用 EF 內部私有反射仍可取得 SQL。
  - 在升級後 provider/runtime 組合可穩定通過。
  - 不再依賴 `_relationalQueryContext` / `_relationalCommandCache`。

### 專案：`OrionCore.Mvc.Tests\OrionCore.Mvc.Tests.csproj`
**現況**：`netcoreapp3.1`

**目標**：`net8.0`

**遷移步驟**：
1. 更新 `TargetFramework` 為 `net8.0`。
2. 依主專案框架與執行行為變更調整測試。

### 補充說明
你關注的 `System.Drawing.Common` 與 `DbContextExtensions.ToSql` 已納入本計劃的高優先級重點。

## 套件升級參考
### 共用套件升級（多專案）
| 套件 | 目前版本 | 目標版本 | 影響專案數 | 升級原因 |
|---|---:|---:|---:|---|
| `Microsoft.EntityFrameworkCore` | 3.1.21 | 8.0.24 | 2 | assessment 建議升級 |
| `Microsoft.EntityFrameworkCore.Relational` | 3.1.21 | 8.0.24 | 2 | assessment 建議升級 |
| `System.Drawing.Common` | 4.7.0 | 8.0.24 | 2 | `.NET 8` 對齊 + GDI 風險修正 |

### 測試/資料層套件升級
| 套件 | 目前版本 | 目標版本 | 影響專案數 | 升級原因 |
|---|---:|---:|---:|---|
| `Microsoft.EntityFrameworkCore.Design` | 3.1.21 | 8.0.24 | 1 | assessment 建議升級 |
| `Microsoft.EntityFrameworkCore.Sqlite` | 3.1.21 | 8.0.24 | 1 | assessment 建議升級 |
| `Microsoft.EntityFrameworkCore.SqlServer` | 3.1.21 | 8.0.24 | 1 | assessment 建議升級 |
| `Microsoft.EntityFrameworkCore.Tools` | 3.1.21 | 8.0.24 | 1 | assessment 建議升級 |
| `Newtonsoft.Json` | 13.0.1 | 13.0.4 | 1 | assessment 建議升級 |

## 破壞性變更目錄
### 需重點處理的框架/API 類型
- `System.Drawing` 在非 Windows 環境的支援限制與行為差異。
- `CodeGenerator` 在 `net8.0-windows` 下的 WPF API 相容性（僅此專案）。
- 舊版組態系統（`System.Configuration`）的遷移或橋接。
- EF Core `3.1 -> 8.0` 的查詢/模型/執行行為差異。
- ASP.NET Core `3.1 -> 8.0` 的 hosting、設定、middleware 差異。
- `OrionCore.API.Tests/DbContextExtensions.ToSql` 依賴 EF 內部 API/私有欄位（高風險破壞點）。

### `System.Drawing` 專項說明
- 升級到 `8.0.24` 是必要條件，但**不足以**保證跨平台伺服器情境穩定。
- 若需跨平台一致行為，應規劃替代方案（`ImageSharp`／`SkiaSharp`）。

## 測試與驗證策略
### Phase 1 驗證（完成原子化升級）
- Solution `restore` 成功。
- Solution `build` 成功且 0 errors。
- 無套件解析衝突。

### Phase 2 驗證（測試）
- 執行：
  - `OrionCore.API.Tests`
  - `OrionCore.Mvc.Tests`
- 驗證並修復框架/套件升級造成的回歸。

### `ToSql` 專項驗證
- 使用支援機制可穩定取得 SQL。
- 不使用 EF 內部私有欄位反射。
- 在升級後 provider/runtime 組合下結果一致。

### 跨平台專項驗證
- 盤點 API/MVC 執行路徑中的 `System.Drawing` 使用點。
- 至少在 Windows/Linux 進行行為驗證。

## 風險管理
| 專案/區域 | 風險等級 | 風險描述 | 緩解策略 |
|---|---|---|---|
| `CodeGenerator` | 高 | WPF + 大量 binary incompatible API | 維持 Windows 專用目標，先收斂 UI/API 編譯問題 |
| `OrionCore.API` | 高 | `System.Drawing` + crypto/API 變動 | 升級套件並評估繪圖堆疊替代 |
| `OrionCore.Mvc` | 中 | 相依 API 且使用 `System.Drawing` | 對齊 API 修正並驗證執行行為 |
| 測試專案 | 高 | EF 升級 + `DbContextExtensions.ToSql` 內部 API 依賴 | 優先重寫 `ToSql`，再做最終測試穩定化 |

### 安全性處理
- assessment 顯示有 `NuGet.0004` 安全性弱點議題。
- 本計劃假設在同一升級波次一併處理。

### 應變方案
- 若 `System.Drawing` 在跨平台仍有阻斷，先以抽象層隔離，再對非 Windows 路徑導入替代實作。

## 複雜度與工作量評估
| 專案 | 複雜度 | 主要因素 |
|---|---|---|
| `CodeGenerator` | 高 | WPF + 多個 API 破壞點 |
| `OrionCore.API` | 高 | `System.Drawing` 與 API 相容性調整 |
| `OrionCore.Mvc` | 中 | 相依 API 且有繪圖路徑 |
| `OrionCore.API.Tests` | 中 | EF/Test 對齊 + `ToSql` 重寫 |
| `OrionCore.Mvc.Tests` | 低 | 消費端測試調整 |

### 整體評估
- 方案複雜度：**中高**
- 執行模式：**All-At-Once**（一次性升級）

## 原始碼版控策略
- 工作分支：`upgrade-to-NET8`
- 建議提交模型：升級穩定後採用**單一主提交**。
- 若有需要，可先有一個前置提交（SDK/`global.json`）。
- 合併策略：通過建置、測試與安全檢查後，以 PR 合併到 `master`。

## 完成標準
### 技術標準
- 所有專案完成目標框架更新（`net8.0`；`CodeGenerator`=`net8.0-windows`）。
- assessment 建議升級之套件全部完成。
- Solution 建置 0 errors。
- 測試專案執行成功。
- 無未解決相依衝突。

### 品質標準
- `System.Drawing` 跨平台風險有明確緩解（Windows 邊界清楚或替代路徑明確）。
- assessment 標記的關鍵 API 破壞點已處理。
- `DbContextExtensions.ToSql` 已完成可支援的重寫與驗收。

### 流程標準
- 依 All-at-Once 策略完成單一協調升級作業。
- 依版控流程完成審查與合併。

### 跨平台驗收標準（新增）
- `OrionCore.API` 與 `OrionCore.Mvc` 在 Windows/Linux 皆可建置與執行。
- 測試專案在 Windows/Linux 至少完成一次成功執行（或明確註記平台限制）。
- `CodeGenerator` 明確標記為 Windows-only；若要 Linux 支援，需進入下一階段 UI 技術遷移。
