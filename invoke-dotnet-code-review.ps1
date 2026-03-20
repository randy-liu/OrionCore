param(
    [string]$ProjectPath = ".",
    [string]$Note = ""
)

$ErrorActionPreference = "Stop"

$utf8NoBom = [System.Text.UTF8Encoding]::new($false)
[Console]::InputEncoding = $utf8NoBom
[Console]::OutputEncoding = $utf8NoBom
$OutputEncoding = $utf8NoBom

$projectRoot = (Resolve-Path $ProjectPath).Path
Set-Location $projectRoot
if (-not (Test-Path ".git")) { throw "Not a git repo: $projectRoot" }

function Find-Agent([string]$name) {
    $c = @(
        ".github/agents/$name",
        "agents/$name",
        $name
    )
    foreach ($p in $c) { if (Test-Path $p) { return (Resolve-Path $p).Path } }
    throw "Required file not found: $name"
}

$policyAgent = Find-Agent "dotnet8-pr-review.agent.md"
$runnerAgent = Find-Agent "dotnet8-local-prepush-review.agent.md"  # 保留顯示用途

Write-Host "Project      : $projectRoot"
Write-Host "Policy agent : $policyAgent"
Write-Host "Runner agent : $runnerAgent"

if (-not (Test-Path ".tmp")) { New-Item -ItemType Directory -Path ".tmp" | Out-Null }
$ts = Get-Date -Format "yyyyMMdd-HHmmss"
$outFile = ".tmp/dotnet8-local-review-$ts.md"

# 取 diff：dotnet-cr 本機手動審查固定以 staged 變更為主
$diff = git diff --staged | Out-String

$policyText = Get-Content $policyAgent -Raw

$instruction = @"
You are running local pre-push .NET code review.

All review output must be in Traditional Chinese (繁體中文, zh-TW).
Keep code blocks, file paths, class names, method names, and technical keywords in their original form.
Local review scope is DIFF-only.
Do not report PR trigger/workflow execution warnings in local mode.
Treat untracked-file omission as acceptable in local mode.
Ignore temporary network robustness warnings (e.g., curl fail-fast/timeout) unless explicitly requested.

Follow this policy exactly:
$policyText

Review the following git diff and produce the final answer in markdown using the policy required format.

Additional focus: $Note

DIFF:
$diff
"@

# 寫 prompt 方便除錯
$promptFile = ".tmp/dotnet8-local-review-prompt-$ts.txt"
Set-Content -Path $promptFile -Value $instruction -Encoding UTF8
Write-Host "Prompt file  : $promptFile"

# 使用新版 Copilot CLI，透過 @promptFile 避免超長命令列參數
$result = ""
$copilotCli = Get-Command copilot -ErrorAction SilentlyContinue
if ($null -ne $copilotCli) {
    Write-Host "Using: copilot -p @promptFile"
    $result = & copilot -p "@$promptFile" --silent | Out-String
} else {
    throw "No usable Copilot CLI found. Please install 'copilot' CLI."
}

if ([string]::IsNullOrWhiteSpace($result)) { throw "No output from copilot CLI." }

Set-Content -Path $outFile -Value $result -Encoding UTF8
Write-Host "✅ Review report: $outFile"