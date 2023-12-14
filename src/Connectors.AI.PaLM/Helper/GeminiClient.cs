using Microsoft.SemanticKernel.AI.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.AI.Gemini.ChatCompletion;
using System.Text.Json;
using Microsoft.SemanticKernel.AI;
using Microsoft.SemanticKernel.Connectors.AI.OpenAI;
using System.Text.Json.Serialization;
using Microsoft.SemanticKernel.Connectors.AI.Gemini.TextCompletion;

namespace Connectors.AI.Gemini.Helper
{
    public class GeminiClient
    {
        string Model { set; get; } = "gemini-pro";//@"chat-bison-001";
        string ApiKey { set; get; } = "";
        //const string ServiceUrl = @"https://generativelanguage.googleapis.com/v1beta2/models/[$MODEL]:generateMessage?key=[$API_KEY]";
        const string ServiceUrl = @"https://generativelanguage.googleapis.com/v1beta/models/[$MODEL]:generateContent?key=[$API_KEY]";
        HttpClient Client { get; set; }
        public GeminiClient(string ApiKey, string Model = "")
        {
            this.Client = new();
            if (!string.IsNullOrEmpty(Model))
            {
                this.Model = Model;
            }
            this.ApiKey = ApiKey;
        }

        public virtual async Task<string> GetMessageAsync(GeminiChatHistory history, AIRequestSettings settings, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                OpenAIRequestSettings oaisetting = (OpenAIRequestSettings)settings;
                var context = history.Where(x => x.Role == AuthorRole.System).FirstOrDefault();
                var hist = new List<MessageRequest>();
                history.ForEach(x => { if (x.Role != AuthorRole.System) { hist.Add(new MessageRequest() { Role = x.Role == AuthorRole.Assistant ? "model" : "user", Parts = new GeminiChatPart[] { new() { Text = x.Content } } }); } });
                //var json = new RequestGeminiChat() { candidate_count = 1, prompt = new GeminiPrompt() { context = context?.Content, examples = new(), messages = hist }, temperature = oaisetting.Temperature, top_p = oaisetting.TopP, top_k = 40 };
                
                var json = new RequestGeminiChat();
                json.Contents = hist.ToArray();
                json.GenerationConfig.Temperature = (float)oaisetting.Temperature;
                json.GenerationConfig.StopSequences = oaisetting.StopSequences?.ToArray() ?? new string[0];
                json.GenerationConfig.TopP = (float)oaisetting.TopP;
                json.GenerationConfig.MaxOutputTokens = oaisetting.MaxTokens ?? 2048;
                
                var jsonstr = JsonSerializer.Serialize(json);
                var url = ServiceUrl.Replace("[$MODEL]", this.Model).Replace("[$API_KEY]", this.ApiKey);
                var res = await Client.PostAsync(url, new StringContent(jsonstr, System.Text.Encoding.UTF8, "application/json"), cancellationToken);
                if (res.IsSuccessStatusCode)
                {
                    var content = await res.Content.ReadAsStringAsync();
                    var obj = JsonSerializer.Deserialize<ResponseGeminiChat>(content);

                    var desc = obj.Candidates.FirstOrDefault()?.Content.Parts.FirstOrDefault()?.Text;
                    return string.IsNullOrEmpty(desc) ? "Gemini refuse to answer" : desc;
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
            return string.Empty;
        }


    }
    #region model response
    /*
    public class CandidateChat
    {
        public string author { get; set; }
        public string content { get; set; }
    }

    public class MessageResponse
    {
        public string author { get; set; }
        public string content { get; set; }
    }

    public class ResponseGeminiChat
    {
        public List<CandidateChat> candidates { get; set; }
        public List<MessageResponse> messages { get; set; }
    }
    */

    public class ResponseGeminiChat
    {
        [JsonPropertyName("candidates")]
        public GeminiChatCandidate[] Candidates { get; set; }
        [JsonPropertyName("promptFeedback")]
        public GeminiChatPromptfeedback PromptFeedback { get; set; }
    }

    public class GeminiChatPromptfeedback
    {
        [JsonPropertyName("safetyRatings")]
        public GeminiChatSafetyrating[] SafetyRatings { get; set; }
    }

    public class GeminiChatSafetyrating
    {
        [JsonPropertyName("category")]
        public string Category { get; set; }
        [JsonPropertyName("probability")]
        public string Probability { get; set; }
    }

    public class GeminiChatCandidate
    {
        [JsonPropertyName("content")]
        public GeminiChatContent Content { get; set; }
        [JsonPropertyName("finishReason")]
        public string FinishReason { get; set; }
        [JsonPropertyName("index")]
        public int Index { get; set; }
        [JsonPropertyName("safetyRatings")]
        public GeminiChatSafetyrating1[] SafetyRatings { get; set; }
    }

    public class GeminiChatContent
    {
        [JsonPropertyName("parts")]
        public GeminiChatPart[] Parts { get; set; }
        [JsonPropertyName("role")]
        public string Role { get; set; }
    }

    public class GeminiChatSafetyrating1
    {
        [JsonPropertyName("category")]
        public string Category { get; set; }
        [JsonPropertyName("probability")]
        public string Probability { get; set; }
    }

    #endregion
    #region models request
    /*
    public class ExampleChat
    {
        public InputText input { get; set; }
        public OutputText output { get; set; }
    }

    public class InputText
    {
        public string content { get; set; }
    }

    public class MessageRequest
    {
        public string content { get; set; }
    }

    public class OutputText
    {
        public string content { get; set; }
    }

    public class GeminiPrompt
    {
        public string context { get; set; }
        public List<ExampleChat> examples { get; set; }
        public List<MessageRequest> messages { get; set; }
    }

    public class RequestGeminiChat
    {
        public GeminiPrompt prompt { get; set; }
        public double temperature { get; set; }
        public int top_k { get; set; }
        public double top_p { get; set; }
        public int candidate_count { get; set; }
    }
    */

    public class RequestGeminiChat
    {
        [JsonPropertyName("contents")]
        public MessageRequest[] Contents { get; set; } = new MessageRequest[] { new() };
        [JsonPropertyName("safetySettings")]
        public Safetysetting[] SafetySettings { get; set; } = SafetySettingHelper.GetDefault();
        [JsonPropertyName("generationConfig")]
        public Generationconfig GenerationConfig { get; set; } = new();
    }

    public class MessageRequest
    {
        [JsonPropertyName("role")]
        public string Role { get; set; }
        [JsonPropertyName("parts")]
        public GeminiChatPart[] Parts { get; set; } = new GeminiChatPart[] { new() };
    }

    public class GeminiChatPart
    {
        [JsonPropertyName("text")]
        public string Text { get; set; } = string.Empty;
    }

    #endregion
}

