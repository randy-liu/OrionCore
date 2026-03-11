#!/bin/bash
# 自動切換到腳本所在的目錄，確保不管從哪裡呼叫都不會迷路
cd "$(dirname "$0")"

# 專門給 Mvc 專案使用的建構捷徑，自動隔離暫存檔避開跨系統權限問題
dotnet build OrionCore.Mvc/OrionCore.Mvc.csproj -c Debug --nologo --artifacts-path /tmp/orioncore-artifacts