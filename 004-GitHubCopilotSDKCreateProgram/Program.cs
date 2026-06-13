using GitHub.Copilot;
using GitHub.Copilot.Rpc;

CopilotClient client = new();

const string WorkingDirectory = @"d:\temp\copilot-sdk-lab";
if (Directory.Exists(WorkingDirectory))
{
    Console.WriteLine($"Deleting existing directory: {WorkingDirectory}");
    Directory.Delete(WorkingDirectory, recursive: true);
}
Directory.CreateDirectory(WorkingDirectory);

var session = await client.CreateSessionAsync(
    new()
    {
        WorkingDirectory = WorkingDirectory,
        Model = "claude-sonnet-4.6",
        OnPermissionRequest = async (request, invocation) =>
        {
            bool isApproved = false;
            switch (request)
            {
                case PermissionRequestShell permissionRequestShell:
                    foreach (var command in permissionRequestShell.Commands)
                    {
                        Console.WriteLine($"Permission requested for shell command: {command.Identifier}, readOnly: {command.ReadOnly}");
                    }
                    isApproved = true;
                    break;
                case PermissionRequestWrite permissionRequestWrite:
                    Console.WriteLine($"Permission requested for write access to file: {permissionRequestWrite.FileName}, Diff: {permissionRequestWrite.Diff}");
                    isApproved = true;
                    break;
                case PermissionRequestRead permissionRequestRead:
                    Console.WriteLine($"Permission requested for read access to file: {permissionRequestRead.Path}");
                    isApproved = true;
                    break;
            }

#pragma warning disable GHCP001
            return isApproved ? PermissionDecision.ApproveOnce() : PermissionDecision.Reject();
#pragma warning restore GHCP001
        }
    });

session.On<SessionEvent>(e =>
{
    Console.Write(e.GetType());
    switch (e)
    {
        case ToolExecutionStartEvent toolExecutionStartEvent:
            Console.Write($": {toolExecutionStartEvent.Data.ToolName}, {toolExecutionStartEvent.Data.Arguments}");
            break;
        case AssistantMessageEvent assistantMessageEvent:
            Console.Write($": {assistantMessageEvent.Data.Content}");
            break;
    }

    Console.WriteLine();
});

await session.SendAndWaitAsync(".NET SDK を使って「こんにちは GitHub Copilot SDK の世界へ!」 を表示するコンソールアプリを作って実行してから、書いたコードと実行結果を教えてください。");

// 参考: PermissionRequest の派生型
//[JsonDerivedType(typeof(PermissionRequestShell), "shell")]
//[JsonDerivedType(typeof(PermissionRequestWrite), "write")]
//[JsonDerivedType(typeof(PermissionRequestRead), "read")]
//[JsonDerivedType(typeof(PermissionRequestMcp), "mcp")]
//[JsonDerivedType(typeof(PermissionRequestUrl), "url")]
//[JsonDerivedType(typeof(PermissionRequestMemory), "memory")]
//[JsonDerivedType(typeof(PermissionRequestCustomTool), "custom-tool")]
//[JsonDerivedType(typeof(PermissionRequestHook), "hook")]
//[JsonDerivedType(typeof(PermissionRequestExtensionManagement), "extension-management")]
//[JsonDerivedType(typeof(PermissionRequestExtensionPermissionAccess), "extension-permission-access")]

// 実行結果
/*
Deleting existing directory: d:\temp\copilot-sdk-lab
GitHub.Copilot.SessionStartEvent
GitHub.Copilot.PendingMessagesModifiedEvent
GitHub.Copilot.PendingMessagesModifiedEvent
GitHub.Copilot.HookStartEvent
GitHub.Copilot.HookEndEvent
GitHub.Copilot.SessionSkillsLoadedEvent
GitHub.Copilot.SystemMessageEvent
GitHub.Copilot.SessionToolsUpdatedEvent
GitHub.Copilot.UserMessageEvent
GitHub.Copilot.SessionTitleChangedEvent
GitHub.Copilot.HookStartEvent
GitHub.Copilot.HookEndEvent
GitHub.Copilot.AssistantTurnStartEvent
GitHub.Copilot.SessionUsageInfoEvent
GitHub.Copilot.AssistantUsageEvent
GitHub.Copilot.AssistantMessageEvent:
GitHub.Copilot.AssistantReasoningEvent
GitHub.Copilot.AssistantIntentEvent
GitHub.Copilot.ToolExecutionStartEvent: report_intent, {"intent":"Creating .NET console app"}
GitHub.Copilot.ToolExecutionStartEvent: powershell, {"command":"cd d:\\temp\\copilot-sdk-lab && dotnet new console -n HelloCopilot --output HelloCopilot","description":"Create new .NET console app","initial_wait":30}
GitHub.Copilot.HookStartEvent
GitHub.Copilot.HookEndEventPermission requested for shell command: cd d:\temp\copilot-sdk-lab && dotnet new console -n HelloCopilot --output HelloCopilot, readOnly: False

GitHub.Copilot.PermissionRequestedEvent
GitHub.Copilot.HookStartEvent
GitHub.Copilot.PermissionCompletedEvent
GitHub.Copilot.HookEndEvent
GitHub.Copilot.ToolExecutionCompleteEvent
GitHub.Copilot.SessionBackgroundTasksChangedEvent
GitHub.Copilot.SessionBackgroundTasksChangedEvent
GitHub.Copilot.SessionBackgroundTasksChangedEvent
GitHub.Copilot.ToolExecutionPartialResultEvent
GitHub.Copilot.ToolExecutionPartialResultEvent
GitHub.Copilot.ToolExecutionPartialResultEvent
GitHub.Copilot.ToolExecutionPartialResultEvent
GitHub.Copilot.SessionBackgroundTasksChangedEvent
GitHub.Copilot.SessionBackgroundTasksChangedEvent
GitHub.Copilot.SessionBackgroundTasksChangedEvent
GitHub.Copilot.HookStartEvent
GitHub.Copilot.HookEndEvent
GitHub.Copilot.ToolExecutionCompleteEvent
GitHub.Copilot.AssistantTurnEndEvent
GitHub.Copilot.AssistantTurnStartEvent
GitHub.Copilot.SessionUsageInfoEvent
GitHub.Copilot.AssistantUsageEvent
GitHub.Copilot.AssistantMessageEvent:
GitHub.Copilot.ToolExecutionStartEvent: view, {"path":"d:\\temp\\copilot-sdk-lab\\HelloCopilot\\Program.cs"}
GitHub.Copilot.HookStartEvent
GitHub.Copilot.HookEndEvent
Permission requested for read access to file: d:\temp\copilot-sdk-lab\HelloCopilot\Program.cs
GitHub.Copilot.PermissionRequestedEvent
GitHub.Copilot.PermissionCompletedEvent
GitHub.Copilot.HookStartEvent
GitHub.Copilot.HookEndEvent
GitHub.Copilot.ToolExecutionCompleteEvent
GitHub.Copilot.AssistantTurnEndEvent
GitHub.Copilot.AssistantTurnStartEvent
GitHub.Copilot.SessionUsageInfoEvent
GitHub.Copilot.AssistantUsageEvent
GitHub.Copilot.AssistantMessageEvent:
GitHub.Copilot.ToolExecutionStartEvent: edit, {"new_str":"Console.WriteLine(\"こんにちは GitHub Copilot SDK の世界へ!\");","old_str":"Console.WriteLine(\"Hello, World!\");","path":"d:\\temp\\copilot-sdk-lab\\HelloCopilot\\Program.cs"}
GitHub.Copilot.HookStartEvent
GitHub.Copilot.HookEndEvent
Permission requested for write access to file: d:\temp\copilot-sdk-lab\HelloCopilot\Program.cs, Diff:
diff --git a/d:/temp/copilot-sdk-lab/HelloCopilot/Program.cs b/d:/temp/copilot-sdk-lab/HelloCopilot/Program.cs
index 0000000..0000000 100644
--- a/d:/temp/copilot-sdk-lab/HelloCopilot/Program.cs
+++ b/d:/temp/copilot-sdk-lab/HelloCopilot/Program.cs
@@ -1,2 +1,2 @@
-?Console.WriteLine("Hello, World!");
+?Console.WriteLine("こんにちは GitHub Copilot SDK の世界へ!");


GitHub.Copilot.PermissionRequestedEvent
GitHub.Copilot.PermissionCompletedEvent
GitHub.Copilot.HookStartEvent
GitHub.Copilot.HookEndEvent
GitHub.Copilot.ToolExecutionCompleteEvent
GitHub.Copilot.AssistantTurnEndEvent
GitHub.Copilot.AssistantTurnStartEvent
GitHub.Copilot.SessionUsageInfoEvent
GitHub.Copilot.AssistantUsageEvent
GitHub.Copilot.AssistantMessageEvent:
GitHub.Copilot.ToolExecutionStartEvent: powershell, {"command":"cd d:\\temp\\copilot-sdk-lab\\HelloCopilot && dotnet run","description":"Run the console app","initial_wait":30}
GitHub.Copilot.HookStartEvent
Permission requested for shell command: cd d:\temp\copilot-sdk-lab\HelloCopilot && dotnet run, readOnly: False
GitHub.Copilot.HookEndEvent
GitHub.Copilot.PermissionRequestedEvent
GitHub.Copilot.PermissionCompletedEvent
GitHub.Copilot.SessionBackgroundTasksChangedEvent
GitHub.Copilot.SessionBackgroundTasksChangedEvent
GitHub.Copilot.SessionBackgroundTasksChangedEvent
GitHub.Copilot.ToolExecutionPartialResultEvent
GitHub.Copilot.ToolExecutionPartialResultEvent
GitHub.Copilot.ToolExecutionPartialResultEvent
GitHub.Copilot.ToolExecutionPartialResultEvent
GitHub.Copilot.ToolExecutionPartialResultEvent
GitHub.Copilot.ToolExecutionPartialResultEvent
GitHub.Copilot.ToolExecutionPartialResultEvent
GitHub.Copilot.ToolExecutionPartialResultEvent
GitHub.Copilot.ToolExecutionPartialResultEvent
GitHub.Copilot.ToolExecutionPartialResultEvent
GitHub.Copilot.ToolExecutionPartialResultEvent
GitHub.Copilot.ToolExecutionPartialResultEvent
GitHub.Copilot.SessionBackgroundTasksChangedEvent
GitHub.Copilot.SessionBackgroundTasksChangedEvent
GitHub.Copilot.SessionBackgroundTasksChangedEvent
GitHub.Copilot.HookStartEvent
GitHub.Copilot.HookEndEvent
GitHub.Copilot.ToolExecutionCompleteEvent
GitHub.Copilot.AssistantTurnEndEvent
GitHub.Copilot.AssistantTurnStartEvent
GitHub.Copilot.SessionUsageInfoEvent
GitHub.Copilot.AssistantUsageEvent
GitHub.Copilot.AssistantMessageEvent: 完了しました！

## コード (`Program.cs`)

```csharp
Console.WriteLine("こんにちは GitHub Copilot SDK の世界へ!");
```

## 実行結果

```
こんにちは GitHub Copilot SDK の世界へ!
```

.NET のトップレベルステートメント構文を使った1行のシンプルなコンソールアプリです。`dotnet run` で正常に実行・表示できました。
GitHub.Copilot.AssistantTurnEndEvent
GitHub.Copilot.HookStartEvent
GitHub.Copilot.HookEndEvent
GitHub.Copilot.SessionIdleEvent
*/