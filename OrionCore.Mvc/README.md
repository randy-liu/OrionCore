# OrionCore.Mvc

`OrionCore.Mvc` provides ASP.NET Core MVC / Razor Pages extensions for OrionCore on .NET 8.

## Install

```bash
dotnet add package OrionCore.Mvc
```

## What it provides

- MVC and Razor Page filters
- Authorization and request helper extensions
- Controller helper extensions for standard JSON responses
- Captcha helpers built on ImageSharp (cross-platform)
- Razor view rendering helper (`RazorViewCaller`)

## Quick example

```csharp
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ExceptionMessageActionFilter>();
    options.Filters.Add<PageParamsActionFilter>();
});
```

For full documentation and advanced samples, see the repository root `README.md`.
