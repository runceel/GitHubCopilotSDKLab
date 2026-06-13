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
    "apiCallId": "msg_bdrk_0123456789abcdefghijklmnopqrst",
    "content": "やっほー！何かお手伝いできることはありますか？ \uD83D\uDE0A",
    "interactionId": "00000000-1111-2222-3333-444444444444",
    "messageId": "55555555-6666-7777-8888-999999999999",
    "model": "claude-sonnet-4.6",
    "outputTokens": 26,
    "requestId": "ABCD:12345:67890:ABCDE:01234567",
    "serviceRequestId": "aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee",
    "toolRequests": [],
    "turnId": "0"
  },
  "id": "ffffffff-0000-1111-2222-333333333333",
  "parentId": "99999999-8888-7777-6666-555555555555",
  "timestamp": "2026-01-01T00:00:00.000+00:00"
}
*/