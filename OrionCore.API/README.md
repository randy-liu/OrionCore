# OrionCore.Api

`OrionCore.Api` is the core utility package for OrionCore on .NET 8.

## Install

```bash
dotnet add package OrionCore.Api
```

## What it provides

- Validation helpers (`Checker`, `UserException`)
- Query builders for ADO.NET and EF Core (`WhereCommandBuilder`, `WhereQueryableBuilder`)
- Common extension methods (`String`, `DateTime`, `IQueryable`, pagination)
- Notification and monitoring utilities (`Notifier`, `VariableMonitor`)
- Rendering gateway (`PDF`/`PNG`) via QuestPDF and ImageSharp

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

