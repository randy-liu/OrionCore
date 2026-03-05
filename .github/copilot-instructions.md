# Copilot Instructions

## Project Guidelines
- User wants the .NET upgrade target framework to be .NET 8.0 (not .NET 10.0) in this scenario.
- In .NET 8 upgrade planning, user wants explicit focus on rewriting `OrionCore.API.Tests/DbContextExtensions.cs` method `DbContextExtensions.ToSql` because EF Core internal API usage may break after upgrade.
- User wants the .NET 8 upgrade plan to prioritize cross-platform support (Windows and Linux) and questions any Windows-only target unless explicitly justified.