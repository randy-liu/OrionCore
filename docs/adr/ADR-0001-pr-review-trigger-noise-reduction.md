# ADR-0001：PR Review 觸發降噪策略

- 狀態：Accepted
- 日期：2026-03-20
- 範圍：`.github/workflows/dotnet-pr-ai-review.yml`

## 背景

本專案採用混合式 code review 流程：

1. Windows 本機透過 `dotnet-cr` 對 staged 變更進行審查
2. `push` 後自動審查
3. `pull_request` 自動審查
4. `pull_request_review` 事件整合自動審查

若對每一次 `pull_request_review.submitted`（`commented`、`approved`、`changes_requested`）都執行 AI review，會造成重複執行與留言噪音。

## 決策

針對 `pull_request_review` 事件，僅在下列條件成立時執行 AI review：

- `github.event.review.state == 'changes_requested'`

同時，PR 留言採用 upsert（更新既有 bot 留言）方式，避免重複洗版。

## 影響

### 優點

- 降低 workflow 噪音與執行成本（compute/token）
- 訊號更集中：僅在 reviewer 明確要求修改時重跑
- PR 討論串更乾淨：維持單則 bot 留言更新

### 取捨

- 在 `approved` 或 `commented` 狀態下不會重跑
- 部分團隊可能偏好「所有 review 事件全覆蓋」

## 審查發現分級規則

當 workflow 明確採用此降噪策略時，`pull_request_review` 非全覆蓋應歸類為 **Info/Note**（非缺陷）。

僅在需求明確要求「所有 review 事件都必須重跑自動審查」時，才升級為真正 finding。

## 重新評估時機

若出現以下情況，應重新檢視本決策：

1. 團隊需求改為所有 review 事件都要重跑
2. 在 `approved/commented` 路徑出現實際漏檢風險
3. 成本或降噪限制放寬

## 相關檔案

- `.github/workflows/dotnet-pr-ai-review.yml`
- `.github/agents/dotnet8-pr-review.agent.md`
- `.github/copilot-instructions.md`
