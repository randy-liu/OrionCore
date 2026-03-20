# Copilot Instructions

## Project Guidelines
- User wants the .NET upgrade target framework to be .NET 8.0 (not .NET 10.0) in this scenario.
- In .NET 8 upgrade planning, user wants explicit focus on rewriting `OrionCore.API.Tests/DbContextExtensions.cs` method `DbContextExtensions.ToSql` because EF Core internal API usage may break after upgrade.
- User wants the .NET 8 upgrade plan to prioritize cross-platform support (Windows and Linux) and questions any Windows-only target unless explicitly justified.
- User wants to optimize Captcha parameter validation (e.g., code/length checks) as a next step.
- User wants to record potential optimization items (e.g., DI wiring confirmation, Random/License settings) for future processing, without making immediate changes in the current step.

## Developer Guidelines for Copilot
- Local review tooling must explicitly require the upgraded `copilot` CLI and not support legacy `gh copilot` fallback.
- User prefers a hybrid code review flow: local staged review on Windows via `dotnet-cr`, automatic review on git push, and automatic review on PR review events, while maintaining the project in WSL Docker using the upgraded `copilot` CLI.

## Local Review Guidelines
- For local `dotnet-cr` reviews, always target staged changes (git add files) as the primary scope whenever manually running `dotnet-cr`.
- Exclude PR-trigger/workflow-operation warnings.
- Omit untracked-file warnings.
- Exclude temporary network-robustness warnings (e.g., curl fail-fast/timeout).

## Build & Test Instructions (CRITICAL)
This project runs in a cross-platform Docker environment. 
Do not attempt to change file permissions of the `obj` or `bin` folders to fix `MSB3374` or `Access denied` errors.

You MUST isolate outputs for ALL build and test commands by using the appropriate artifacts path based on the operating system:
- On Windows, use `$env:TEMP\orioncore-artifacts`
- On Linux, use `/tmp/orioncore-artifacts`

- To build MVC project: execute `bash OrionCore/build-mvc.sh`
- To test API project: execute `bash OrionCore/test-api.sh` 
(If you run any other `dotnet build` or `dotnet test` commands manually, you MUST append the appropriate `--artifacts-path` to the command).