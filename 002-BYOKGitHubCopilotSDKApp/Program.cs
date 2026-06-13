
//## Supported providers

//| Provider | Type Value | Notes |
//| ----------| ------------| -------|
//| OpenAI | `"openai"` | OpenAI API and OpenAI - compatible endpoints |
//| Azure OpenAI / Microsoft Foundry | `"azure"` | Azure-hosted models |
//| Anthropic | `"anthropic"` | Claude models |
//| Ollama | `"openai"` | Local models via OpenAI-compatible API |
//| Microsoft Foundry Local | `"openai"` | Run AI models locally on your device via OpenAI-compatible API |
//| Other OpenAI-compatible | `"openai"` | vLLM, LiteLLM, etc. |

using Azure.Identity;
using GitHub.Copilot;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

CopilotClient client = new();
AzureCliCredential credential = new();

// Microsoft Foundry の v1 エンドポイントで使うアクセストークンを Azure CLI のログイン情報から取得します。
var token = await credential.GetTokenAsync(
    requestContext: new(["https://ai.azure.com/.default"]));

// GitHub Copilot SDK に、OpenAI 互換 API として Microsoft Foundry のエンドポイントを渡します。
var session = await client.CreateSessionAsync(config: new()
{
    Provider = new()
    {
        Type = "openai",
        // BaseUrl は Microsoft Foundry のポータルで取れる URL の v1 までを指定
        BaseUrl = "https://<<リソース名>>.services.ai.azure.com/openai/v1/",
        // SDK 側で Authorization ヘッダーを組み立てるため、ここではトークン文字列だけを渡します。
        BearerToken = token.Token,
        // Api Key の場合は BearerToken ではなく ApiKey プロパティにキーを渡します。
        // ApiKey = "xxxxx",
        ModelId = "gpt-5.4",
        WireApi = "responses",
    }
});

// セッションにメッセージを送信し、完了まで待って結果を受け取ります。
var response = await session.SendAndWaitAsync("やっほー！");

// 日本語をエスケープせずに、見やすい JSON 形式で出力します。
Console.WriteLine(JsonSerializer.Serialize(response, new JsonSerializerOptions
{
    WriteIndented = true,
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
}));

// 実行結果
/*
{
  "data": {
    "apiCallId": "resp_0123456789abcdef0123456789abcdef0123456789abcdef01",
    "content": "やっほー！どうした？",
    "encryptedContent": "gAAAAABmExample_...ABCDE==",
    "interactionId": "00000000-1111-2222-3333-444444444444",
    "messageId": "55555555-6666-7777-8888-999999999999",
    "model": "gpt-5.4",
    "outputTokens": 38,
    "phase": "final_answer",
    "reasoningOpaque": "rs_0123456789abcdef0123456789abcdef0123456789abcdef",
    "toolRequests": [],
    "turnId": "0"
  },
  "id": "aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee",
  "parentId": "ffffffff-0000-1111-2222-333333333333",
  "timestamp": "2026-01-01T00:00:00.000+00:00"
}
*/