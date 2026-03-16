# OrionCore

OrionCore 是一套基於 ASP.NET Core 的工具函式庫，提供兩個 NuGet 套件：

| 套件 | 版本 | 說明 |
|------|------|------|
| [OrionCore.Api](#orioncoreapi) | 1.1.2 | 通用工具、資料驗證、查詢建構、監控、擴充方法 |
| [OrionCore.Mvc](#orioncoremvc) | 1.0.10 | ASP.NET Core MVC / Razor Pages 擴充 |

---

## 需求環境

- .NET 8.0+

---

## 安裝

```bash
# 核心工具套件
dotnet add package OrionCore.Api

# MVC / Razor Pages 擴充套件（包含 OrionCore.Api）
dotnet add package OrionCore.Mvc
```

---

## 開發與測試

```bash
# 建置 OrionCore.Mvc（跨平台環境建議使用）
# 從專案根目錄的上一層（包含 OrionCore 資料夾）執行：
bash OrionCore/build-mvc.sh

# 執行測試
dotnet test OrionCore/OrionCore.API.Tests/OrionCore.API.Tests.csproj
dotnet test OrionCore/OrionCore.Mvc.Tests/OrionCore.Mvc.Tests.csproj
```

---

## OrionCore.Api

### OrionUtils — 通用工具

| 方法 | 說明 |
|------|------|
| `HasValue(object)` | 判斷值是否有效（支援 string、DateTime、Enum、IEnumerable、數值型態） |
| `TxSerializable()` / `TxReadCommitted()` … | 建立指定隔離層級的 `TransactionScope` |
| `EnumToDictionary<T>()` | 將 Enum 轉換為 `Dictionary<string, string>`（支援 `Display` 屬性） |
| `GetEnumValues<T>()` | 取得 Enum 所有值的陣列 |
| `EnumerateRange(int, int)` | 迭代整數範圍（升冪或降冪） |
| `EnumerateRange(DateTime, DateTime)` | 迭代日期範圍（逐日） |
| `GetSolarDay(DateTime?)` | 取得太陽日（`(year-1900)*1000 + dayOfYear`） |
| `ParseSolarDay(int?)` | 解析太陽日為 `DateTime` |
| `ParseCnDate(string)` | 解析民國日期字串為 `DateTime` |
| `Md5String(string)` / `Md5File(...)` | 計算 MD5 雜湊值 |
| `AllotPath(int)` | 產生分層路徑（`/000/000/123`） |
| `AllotHashPath(string, int)` | 產生 MD5 雜湊分層路徑 |
| `LockProcessId(string)` / `UnlockProcessId(string)` | 以 PID 檔進行單一執行個體鎖定 |

### Checker — 資料驗證

```csharp
Checker.Has(value, "欄位不可為空");
Checker.Min(age, 18, "年齡不可小於 {1}");
Checker.Max(price, 9999m, "金額超過上限 {1}");
Checker.Range(score, 0, 100, "分數需在 0～100 之間");
Checker.MaxLength(name, 50, "名稱不可超過 {1} 個字");
Checker.Pattern(email, @"^[^@]+@[^@]+$", "Email 格式錯誤");
Checker.In(status, allowedList, "狀態不在允許清單中");
Checker.StatusRule(fromStatus, toStatus, ruleDict, "不允許的狀態轉換：{0} → {1}");
```

所有驗證失敗時會拋出 `UserException`。

### WhereCommandBuilder — ADO.NET 查詢條件建構

```csharp
var cmd = connection.CreateCommand();
cmd.CommandText = "SELECT * FROM Orders WHERE 1=1";

var builder = new WhereCommandBuilder(cmd, whereParams);
builder.Bind("CustomerId")
       .Bind("OrderDate", "order_date")
       .Bind("Status");

// 支援運算子：Contains、StartsWith、Equals、In、Between、LessThan 等
```

### WhereQueryableBuilder — EF Core / LINQ 查詢條件建構

```csharp
var query = dbContext.Orders.AsQueryable();
query = new WhereQueryableBuilder<Order>(query, whereParams)
    .Bind(x => x.CustomerId)
    .Bind(x => x.Status)
    .Build();
```

### NotifiMonitor / Notifier — 效能監控

```csharp
// 透過 Notifier 自動攔截方法並記錄執行時間
var monitors = Notifier.GetMonitors(myService);
foreach (var m in monitors)
{
    Console.WriteLine($"{m.Method.Name}: 執行 {m.RunCount} 次，最長 {m.MaxSeconds:F2} 秒");
}
```

### VariableMonitor — 變數變化監控

```csharp
var monitor = new VariableMonitor<int>();
monitor.Changed += (oldVal, newVal) => Console.WriteLine($"{oldVal} → {newVal}");
monitor.Value = 42;
```

### ServiceContextGenerator — Autofac 依賴注入

```csharp
// 以 Autofac 產生 Service 的 Context 代理，自動記錄方法執行時間
builder.RegisterType<OrderService>()
       .As<IOrderService>()
       .EnableClassInterceptors();
```

### Orion.Api.Rendering — 渲染核心

跨平台渲染架構，支援 PDF（QuestPDF）與 PNG（ImageSharp），可在 Linux 與 Windows 上執行。

#### 核心介面與類別

| 類別 / 介面 | 說明 |
|------------|------|
| `IRenderGateway` / `RenderGateway` | 渲染入口，依格式分派至對應渲染器 |
| `IRenderer` | 單一格式渲染器介面 |
| `RenderFormat` | 輸出格式列舉（`Pdf`、`Png`） |
| `RenderRequest` | 渲染請求基底類別 |
| `PdfRenderRequest` | PDF 渲染請求（需傳入 QuestPDF `IDocument`） |
| `PngRenderRequest` | PNG 渲染請求（需傳入寬高與 RGBA32 像素資料） |
| `RenderResult` | 渲染輸出結果（含位元組內容、MIME 類型、副檔名） |
| `QuestPdfRenderer` | 以 QuestPDF 實作的 PDF 渲染器 |
| `ImageSharpPngRenderer` | 以 ImageSharp 實作的 PNG 渲染器 |

#### 使用範例

```csharp
// PDF 渲染
var request = new PdfRenderRequest(myQuestPdfDocument);
RenderResult result = await renderGateway.RenderPdfAsync(request);
// result.Content    → byte[]
// result.ContentType → "application/pdf"

// PNG 渲染
var pngRequest = new PngRenderRequest(width, height, rgbaPixels);
RenderResult pngResult = await renderGateway.RenderPngAsync(pngRequest);
// pngResult.ContentType → "image/png"

// 同步封裝（IRenderGateway 擴充方法）
Stream pdfStream = renderGateway.ToPdfStream(pdfRequest);
Stream pngStream = renderGateway.ToPngStream(pngRequest);
```

#### Autofac DI 註冊

```csharp
// ContainerBuilder 擴充（AutofacRenderingExtensions）
builder.RegisterOrionRenderingCore();
// 等同於：
// builder.RegisterType<QuestPdfRenderer>().As<IRenderer>().SingleInstance();
// builder.RegisterType<ImageSharpPngRenderer>().As<IRenderer>().SingleInstance();
// builder.RegisterType<RenderGateway>().As<IRenderGateway>().SingleInstance();
```

### PrintDocumentExtensions — 舊版列印支援（Windows 限定）

> ⚠️ `PrintDocument` 相關方法**僅支援 Windows**，非 Windows 平台呼叫會拋出 `PlatformNotSupportedException`。  
> 跨平台情境請改用 `Orion.Api.Rendering` 渲染核心。

```csharp
// Windows 限定：舊版路徑
Stream jpegStream = doc.ToJpegStream();   // [SupportedOSPlatform("windows")]
Stream pngStream  = doc.ToPngStream();    // [SupportedOSPlatform("windows")]

// 遷移至渲染核心（跨平台）
Stream pdfStream = renderGateway.ToPdfStream(pdfRequest);
Stream pngStream = renderGateway.ToPngStream(pngRequest);
```

### 常用擴充方法

#### StringExtensions
```csharp
"hello".HasText()                  // true
" ".NoText()                       // true
"hello".LimitLength(3)             // "hel"
"abc".Repeat(3, "-")               // "abc-abc-abc"
"Active".ToEnum<Status>()          // Status.Active
"1,2,3".ToIdsList<int>()           // [1, 2, 3]
model.TrimStringPropertys()        // 修剪所有 string 屬性的前後空白
```

#### DateTimeExtensions
```csharp
date.ToDateString()                // "yyyy/MM/dd"
date.ToDateTimeString()            // "yyyy/MM/dd HH:mm:ss"
date.StartOfMonth()
date.EndOfMonth()
```

#### EnumerableExtensions / ListExtensions
```csharp
list.JoinBy(", ")                  // 以分隔符號串接
list.ToList((item, index) => ...)  // 帶索引的投影
```

#### PaginationExtensions
```csharp
var pagination = query.ToPagination(pageParams);
// pagination.Items, pagination.TotalCount, pagination.PageCount
```

---

## OrionCore.Mvc

### Filters

| Filter | 用途 |
|--------|------|
| `ExceptionMessageActionFilter` | 統一捕捉 `UserException`，回傳 JSON 錯誤訊息 |
| `PageParamsActionFilter` / `PageParamsPageFilter` | 自動將分頁參數注入 Action |
| `SearchRememberActionFilter` | 記憶搜尋條件至 Session |
| `HandlerAuthorizeFilter` | 自訂授權邏輯 |
| `UseViewPageActionFilter` | 切換 Razor 視圖頁面 |

```csharp
// Program.cs
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ExceptionMessageActionFilter>();
    options.Filters.Add<PageParamsActionFilter>();
});
```

### Attributes

```csharp
[ActAuthorize(Roles = "Admin")]
public IActionResult Delete(int id) { ... }

[SearchRemember]
public IActionResult Index(SearchParams search) { ... }
```

### Extensions

#### ControllerExtensions
```csharp
this.SuccessJson(data);            // 回傳標準成功 JSON
this.ErrorJson("錯誤訊息");        // 回傳標準錯誤 JSON
```

#### CaptchaExtensions — 驗證碼（ImageSharp 跨平台實作）

驗證碼圖片已改用 **SixLabors.ImageSharp** 繪製，支援 Linux 與 Windows，不再依賴 `System.Drawing`。

```csharp
// Controller / PageModel：產生並儲存驗證碼，回傳 PNG 圖片
public IActionResult Captcha()
    => this.CaptchaResult(length: 5, colorName: "#1a3a5c");

// 驗證使用者輸入
bool valid = this.IsCaptchaValid(userInput);

// 底層方法：直接產生 PNG 串流（可自行控制輸出）
Stream stream = CaptchaExtensions.CreateCaptchaPng(code, fontColor: "DarkBlue");
```

#### RequestExtensions
```csharp
Request.IsAjax()                   // 判斷是否為 AJAX 請求
Request.GetClientIp()              // 取得用戶端 IP
```

#### ModelStateExtensions
```csharp
if (!ModelState.IsValid)
{
    return BadRequest(ModelState.GetErrors());
}
```

### RazorViewCaller — 程式碼中渲染 Razor 視圖

```csharp
string html = await razorViewCaller.RenderViewToStringAsync("~/Views/Email/Welcome.cshtml", model);
```

---

## 版本紀錄

| 版本 | 套件 | 異動 |
|------|------|------|
| 1.1.2 | OrionCore.Api | 升級至 .NET 8.0；新增 `Orion.Api.Rendering` 渲染核心（QuestPDF / ImageSharp）；`PrintDocumentExtensions` 標註 Windows 限定並提供 `IRenderGateway` 擴充；新增 Autofac DI 註冊擴充 `RegisterOrionRenderingCore()` |
| 1.0.10 | OrionCore.Mvc | 升級至 .NET 8.0；驗證碼改用 SixLabors.ImageSharp 跨平台實作，移除 System.Drawing 依賴 |

---

## 致謝與授權 
本專案基於 Jax Hu 所開發的 OrionCore.API 進行分叉 (fork) 與重構。

本專案沿用原專案的授權條款。

Copyright © 2020 Jax Hu

Copyright © 2026 Randy Liu 
