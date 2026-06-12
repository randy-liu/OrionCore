# OrionCore.Api

`OrionCore.Api` is the core utility package for OrionCore on .NET 8.

## Install

```bash
dotnet add package OrionCore.Api
```

## What it provides

- **Validation & Exceptions**: Robust validation (`Checker`, `UserException`) and error catching utilities (`ExceptionCatcher`) for clean application workflows.
- **Dynamic Query Builders**: Advanced query builders for ADO.NET (`WhereCommandBuilder`) and EF Core (`WhereQueryableBuilder`) to seamlessly build dynamic SQL and LINQ filter conditions.
- **Rich Extension Methods**: Comprehensive extension files (25+) covering:
  - Base Types: `String`, `DateTime`, `Object`, `Enumerable`, `List`
  - Database & Data: `DbConnection`, `DbCommand`, `DataTable`, `DataRow`, `IQueryable`, pagination
  - Dependency Injection: `Autofac` registration shortcuts
  - Infrastructure: `Socket` helpers, `PrintDocument` utilities
- **Notification & Monitoring**: Interactive notification (`Notifier`) and property-change triggers (`VariableMonitor`, `NotifiAttribute`).
- **Security & Cryptography**: Customizable hashing interfaces (`IPasswordHandle`, `PasswordSHA256Handle`).
- **Logging Gateway**: Flexible logger abstraction (`IOrionLogger`) supporting NLog and Windows Event Log implementations.
- **Rendering Gateway**: Document rendering engine (`PDF`/`PNG`) built on QuestPDF and ImageSharp.

## Quick example

```csharp
Checker.Has(orderNo, "Order number is required");

var query = dbContext.Orders.AsQueryable();
query = new WhereQueryableBuilder<Order>(query, whereParams)
    .Bind(x => x.CustomerId)
    .Bind(x => x.Status)
    .Build();
```

For full documentation and advanced samples, see the repository root `README.md`.

## Local Packaging

To build and pack the NuGet package locally to a custom directory, you can override the target directory using the `NugetPublishDir` property:

```powershell
dotnet pack OrionCore.API.csproj -c Release /p:NugetPublishDir=D:\Source\NugetPackages
```

