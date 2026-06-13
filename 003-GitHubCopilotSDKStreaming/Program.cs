using GitHub.Copilot;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

// GitHub Copilot SDK のクライアントを作成し、利用するモデルを指定してセッションを開始します。
CopilotClient client = new();
var session = await client.CreateSessionAsync(
    config: new()
    {
        Model = "claude-sonnet-4.6",
    });

// セッションがアイドル状態になるまで待つための通知用タスクです。
TaskCompletionSource<SessionIdleEvent> sessionIdle = new();

// セッションで発生する各種イベントを購読し、イベントの種類に応じて出力や完了通知を行います。
session.On<SessionEvent>(e =>
{
    Console.WriteLine(e.GetType());
    switch (e)
    {
        case SessionUsageInfoEvent sessionUsageInfoEvent:
            // トークン使用量などの情報は、読みやすい JSON 形式で表示します。
            Console.WriteLine(JsonSerializer.Serialize(sessionUsageInfoEvent.Data, new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
            }));
            break;
        case AssistantMessageEvent assistantMessageEvent:
            // アシスタントから返されたメッセージ本文を表示します。
            Console.WriteLine($"AssistantMessageEvent: {assistantMessageEvent.Data.Content}");
            break;
        case SessionIdleEvent idleEvent:
            // 応答処理が完了してセッションがアイドル状態になったことを待機側へ通知します。
            sessionIdle.TrySetResult(idleEvent);
            break;
    }
});

// ユーザー入力を送信し、すべての応答イベントが流れ終わるまで待機します。
var messageId = await session.SendAsync("やっほー！！");
await sessionIdle.Task;

// 実行結果
/*
GitHub.Copilot.SystemMessageEvent
GitHub.Copilot.SessionToolsUpdatedEvent
GitHub.Copilot.UserMessageEvent
GitHub.Copilot.SessionTitleChangedEvent
GitHub.Copilot.HookStartEvent
GitHub.Copilot.HookEndEvent
GitHub.Copilot.AssistantTurnStartEvent
GitHub.Copilot.SessionUsageInfoEvent
{
  "conversationTokens": 93,
  "currentTokens": 15032,
  "isInitial": true,
  "messagesLength": 2,
  "systemTokens": 7047,
  "tokenLimit": 200000,
  "toolDefinitionsTokens": 7892
}
GitHub.Copilot.AssistantUsageEvent
GitHub.Copilot.AssistantMessageEvent
AssistantMessageEvent: やっほー！！?? 何かお手伝いできることはありますか？
GitHub.Copilot.AssistantTurnEndEvent
GitHub.Copilot.HookStartEvent
GitHub.Copilot.HookEndEvent
GitHub.Copilot.SessionIdleEvent
*/

// メモ: イベント一覧
//[JsonDerivedType(typeof(AbortEvent), "abort")]
//[JsonDerivedType(typeof(AssistantIntentEvent), "assistant.intent")]
//[JsonDerivedType(typeof(AssistantMessageEvent), "assistant.message")]
//[JsonDerivedType(typeof(AssistantMessageDeltaEvent), "assistant.message_delta")]
//[JsonDerivedType(typeof(AssistantMessageStartEvent), "assistant.message_start")]
//[JsonDerivedType(typeof(AssistantReasoningEvent), "assistant.reasoning")]
//[JsonDerivedType(typeof(AssistantReasoningDeltaEvent), "assistant.reasoning_delta")]
//[JsonDerivedType(typeof(AssistantStreamingDeltaEvent), "assistant.streaming_delta")]
//[JsonDerivedType(typeof(AssistantTurnEndEvent), "assistant.turn_end")]
//[JsonDerivedType(typeof(AssistantTurnStartEvent), "assistant.turn_start")]
//[JsonDerivedType(typeof(AssistantUsageEvent), "assistant.usage")]
//[JsonDerivedType(typeof(AutoModeSwitchCompletedEvent), "auto_mode_switch.completed")]
//[JsonDerivedType(typeof(AutoModeSwitchRequestedEvent), "auto_mode_switch.requested")]
//[JsonDerivedType(typeof(CapabilitiesChangedEvent), "capabilities.changed")]
//[JsonDerivedType(typeof(CommandCompletedEvent), "command.completed")]
//[JsonDerivedType(typeof(CommandExecuteEvent), "command.execute")]
//[JsonDerivedType(typeof(CommandQueuedEvent), "command.queued")]
//[JsonDerivedType(typeof(CommandsChangedEvent), "commands.changed")]
//[JsonDerivedType(typeof(ElicitationCompletedEvent), "elicitation.completed")]
//[JsonDerivedType(typeof(ElicitationRequestedEvent), "elicitation.requested")]
//[JsonDerivedType(typeof(ExitPlanModeCompletedEvent), "exit_plan_mode.completed")]
//[JsonDerivedType(typeof(ExitPlanModeRequestedEvent), "exit_plan_mode.requested")]
//[JsonDerivedType(typeof(ExternalToolCompletedEvent), "external_tool.completed")]
//[JsonDerivedType(typeof(ExternalToolRequestedEvent), "external_tool.requested")]
//[JsonDerivedType(typeof(HookEndEvent), "hook.end")]
//[JsonDerivedType(typeof(HookProgressEvent), "hook.progress")]
//[JsonDerivedType(typeof(HookStartEvent), "hook.start")]
//[JsonDerivedType(typeof(McpAppToolCallCompleteEvent), "mcp_app.tool_call_complete")]
//[JsonDerivedType(typeof(McpOauthCompletedEvent), "mcp.oauth_completed")]
//[JsonDerivedType(typeof(McpOauthRequiredEvent), "mcp.oauth_required")]
//[JsonDerivedType(typeof(ModelCallFailureEvent), "model.call_failure")]
//[JsonDerivedType(typeof(PendingMessagesModifiedEvent), "pending_messages.modified")]
//[JsonDerivedType(typeof(PermissionCompletedEvent), "permission.completed")]
//[JsonDerivedType(typeof(PermissionRequestedEvent), "permission.requested")]
//[JsonDerivedType(typeof(SamplingCompletedEvent), "sampling.completed")]
//[JsonDerivedType(typeof(SamplingRequestedEvent), "sampling.requested")]
//[JsonDerivedType(typeof(SessionAutopilotObjectiveChangedEvent), "session.autopilot_objective_changed")]
//[JsonDerivedType(typeof(SessionBackgroundTasksChangedEvent), "session.background_tasks_changed")]
//[JsonDerivedType(typeof(SessionCanvasClosedEvent), "session.canvas.closed")]
//[JsonDerivedType(typeof(SessionCanvasOpenedEvent), "session.canvas.opened")]
//[JsonDerivedType(typeof(SessionCanvasRegistryChangedEvent), "session.canvas.registry_changed")]
//[JsonDerivedType(typeof(SessionCompactionCompleteEvent), "session.compaction_complete")]
//[JsonDerivedType(typeof(SessionCompactionStartEvent), "session.compaction_start")]
//[JsonDerivedType(typeof(SessionContextChangedEvent), "session.context_changed")]
//[JsonDerivedType(typeof(SessionCustomAgentsUpdatedEvent), "session.custom_agents_updated")]
//[JsonDerivedType(typeof(SessionCustomNotificationEvent), "session.custom_notification")]
//[JsonDerivedType(typeof(SessionErrorEvent), "session.error")]
//[JsonDerivedType(typeof(SessionExtensionsLoadedEvent), "session.extensions_loaded")]
//[JsonDerivedType(typeof(SessionExtensionsAttachmentsPushedEvent), "session.extensions.attachments_pushed")]
//[JsonDerivedType(typeof(SessionHandoffEvent), "session.handoff")]
//[JsonDerivedType(typeof(SessionIdleEvent), "session.idle")]
//[JsonDerivedType(typeof(SessionInfoEvent), "session.info")]
//[JsonDerivedType(typeof(SessionMcpServerStatusChangedEvent), "session.mcp_server_status_changed")]
//[JsonDerivedType(typeof(SessionMcpServersLoadedEvent), "session.mcp_servers_loaded")]
//[JsonDerivedType(typeof(SessionModeChangedEvent), "session.mode_changed")]
//[JsonDerivedType(typeof(SessionModelChangeEvent), "session.model_change")]
//[JsonDerivedType(typeof(SessionPermissionsChangedEvent), "session.permissions_changed")]
//[JsonDerivedType(typeof(SessionPlanChangedEvent), "session.plan_changed")]
//[JsonDerivedType(typeof(SessionRemoteSteerableChangedEvent), "session.remote_steerable_changed")]
//[JsonDerivedType(typeof(SessionResumeEvent), "session.resume")]
//[JsonDerivedType(typeof(SessionScheduleCancelledEvent), "session.schedule_cancelled")]
//[JsonDerivedType(typeof(SessionScheduleCreatedEvent), "session.schedule_created")]
//[JsonDerivedType(typeof(SessionShutdownEvent), "session.shutdown")]
//[JsonDerivedType(typeof(SessionSkillsLoadedEvent), "session.skills_loaded")]
//[JsonDerivedType(typeof(SessionSnapshotRewindEvent), "session.snapshot_rewind")]
//[JsonDerivedType(typeof(SessionStartEvent), "session.start")]
//[JsonDerivedType(typeof(SessionTaskCompleteEvent), "session.task_complete")]
//[JsonDerivedType(typeof(SessionTitleChangedEvent), "session.title_changed")]
//[JsonDerivedType(typeof(SessionToolsUpdatedEvent), "session.tools_updated")]
//[JsonDerivedType(typeof(SessionTruncationEvent), "session.truncation")]
//[JsonDerivedType(typeof(SessionUsageInfoEvent), "session.usage_info")]
//[JsonDerivedType(typeof(SessionWarningEvent), "session.warning")]
//[JsonDerivedType(typeof(SessionWorkspaceFileChangedEvent), "session.workspace_file_changed")]
//[JsonDerivedType(typeof(SkillInvokedEvent), "skill.invoked")]
//[JsonDerivedType(typeof(SubagentCompletedEvent), "subagent.completed")]
//[JsonDerivedType(typeof(SubagentDeselectedEvent), "subagent.deselected")]
//[JsonDerivedType(typeof(SubagentFailedEvent), "subagent.failed")]
//[JsonDerivedType(typeof(SubagentSelectedEvent), "subagent.selected")]
//[JsonDerivedType(typeof(SubagentStartedEvent), "subagent.started")]
//[JsonDerivedType(typeof(SystemMessageEvent), "system.message")]
//[JsonDerivedType(typeof(SystemNotificationEvent), "system.notification")]
//[JsonDerivedType(typeof(ToolExecutionCompleteEvent), "tool.execution_complete")]
//[JsonDerivedType(typeof(ToolExecutionPartialResultEvent), "tool.execution_partial_result")]
//[JsonDerivedType(typeof(ToolExecutionProgressEvent), "tool.execution_progress")]
//[JsonDerivedType(typeof(ToolExecutionStartEvent), "tool.execution_start")]
//[JsonDerivedType(typeof(ToolUserRequestedEvent), "tool.user_requested")]
//[JsonDerivedType(typeof(UserInputCompletedEvent), "user_input.completed")]
//[JsonDerivedType(typeof(UserInputRequestedEvent), "user_input.requested")]
//[JsonDerivedType(typeof(UserMessageEvent), "user.message")]
