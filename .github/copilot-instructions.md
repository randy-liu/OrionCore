# Copilot Instructions

## Project Guidelines
- User wants the .NET upgrade target framework to be .NET 8.0 (not .NET 10.0) in this scenario.
- In .NET 8 upgrade planning, user wants explicit focus on rewriting `OrionCore.API.Tests/DbContextExtensions.cs` method `DbContextExtensions.ToSql` because EF Core internal API usage may break after upgrade.
- User wants the .NET 8 upgrade plan to prioritize cross-platform support (Windows and Linux) and questions any Windows-only target unless explicitly justified.


# Developer Guidelines for Copilot

## Build Instructions (CRITICAL)
This project runs in a cross-platform Docker environment (Windows NTFS mounted to Linux). To avoid `MSB3374` permission errors and timestamp issues, NEVER use the standard `dotnet build` command for the MVC project.

Whenever you need to build, compile, or verify `OrionCore.Mvc`, you MUST execute the following custom script:
`bash OrionCore/build-mvc.sh`

Do not attempt to change file permissions of the `obj` or `bin` folders. Always rely on the script above which uses `--artifacts-path` to isolate outputs.