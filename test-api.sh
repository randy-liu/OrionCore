#!/bin/bash
cd "$(dirname "$0")"
dotnet test OrionCore.API.Tests/OrionCore.API.Tests.csproj --nologo --artifacts-path /tmp/orioncore-artifacts