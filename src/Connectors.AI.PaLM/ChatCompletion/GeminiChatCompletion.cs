// Copyright (c) Microsoft. All rights reserved.

using Connectors.AI.Gemini;
using Connectors.AI.Gemini.Helper;
using Microsoft.SemanticKernel.AI;
using Microsoft.SemanticKernel.AI.ChatCompletion;
using Microsoft.SemanticKernel.Services;

namespace Microsoft.SemanticKernel.Connectors.AI.Gemini.ChatCompletion;

/// <summary>
/// Gemini chat completion client.
/// TODO: forward ETW logging to ILogger, see https://learn.microsoft.com/en-us/dotnet/azure/sdk/logging
/// </summary>
public sealed class GeminiChatCompletion :IAIService
{
    GeminiClient client { get; set; }

    public IReadOnlyDictionary<string, string> Attributes => _attributes;

    private readonly Dictionary<string, string> _attributes = new();
    /// <summary>
    /// Create an instance of the Gemini chat completion connector
    /// </summary>
    /// <param name="modelId">Model name</param>
    /// <param name="apiKey">Gemini API Key</param>

    public GeminiChatCompletion(
        string modelId,
        string apiKey
       )
    {
        VerifyHelper.NotNullOrWhiteSpace(modelId);
        VerifyHelper.NotNullOrWhiteSpace(apiKey);
        this._attributes.Add(IAIServiceExtensions.ModelIdKey, modelId);
       
        this.client = new GeminiClient(apiKey,modelId);
    }

    /// <inheritdoc/>
    public ChatHistory CreateNewChat(string? instructions = null)
    {
        return new GeminiChatHistory(instructions);
    }

    public async Task<string> GenerateMessageAsync(GeminiChatHistory chat, AIRequestSettings requestSettings = null, CancellationToken cancellationToken = default)
    {
        return await this.client.GetMessageAsync(chat, requestSettings, cancellationToken);
    }

}
