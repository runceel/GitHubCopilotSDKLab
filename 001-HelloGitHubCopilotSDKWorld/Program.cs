using GitHub.Copilot;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

// Copilot クライアントを初期化
// GitHub Copilot CLI のデフォルトの認証情報を使う
CopilotClient client = new();

// Claude Sonnet 4.6 モデルを使用してセッションを作成
var session = await client.CreateSessionAsync(
    new()
    {
        Model = "claude-sonnet-4.6",
    });

// Copilot に日本語メッセージを送信し、レスポンスを待機
var response = await session.SendAndWaitAsync("やっほー！");

// JSON 形式でレスポンスをコンソールに出力（日本語対応）
Console.WriteLine(JsonSerializer.Serialize(response, new JsonSerializerOptions
{
    WriteIndented = true,
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
}));

/*
実行結果
{
  "data": {
    "apiCallId": "msg_bdrk_01Rd92avxG7pyFVH9W7RHjjt",
    "content": "やっほー！何かお手伝いできることはありますか？ \uD83D\uDE0A",
    "interactionId": "ef608320-ccfa-45c8-bf7f-db915eb95574",
    "messageId": "c706246d-f4ba-49e6-b4fb-6c0f347dbd4c",
    "model": "claude-sonnet-4.6",
    "outputTokens": 26,
    "requestId": "FB07:50BD6:9B623D:B857B2:6A2CF634",
    "serviceRequestId": "19654136-1ae5-4d60-b8ed-a19119b78c54",
    "toolRequests": [],
    "turnId": "0"
  },
  "id": "db0c5cfb-40fa-46b5-bb55-c35eead5bb06",
  "parentId": "d3ecd9cd-1eb6-4f15-a9d2-7356f9ec68fe",
  "timestamp": "2026-06-13T06:18:33.537+00:00"
}
*/