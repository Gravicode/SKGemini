// Copyright (c) Microsoft. All rights reserved.

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Connectors.AI.Gemini.Skills;

/*
/// <summary>
/// text 
/// </summary>
public class MessageToken
{
    [JsonPropertyName("content")]
    public string? Content { get; set; }
}
/// <summary>
/// contain collection of text for token counting
/// </summary>
public class PromptToken
{
    [JsonPropertyName("messages")]
    public List<MessageToken> Messages { get; set; } = new();
}
/// <summary>
/// class for request token count
/// </summary>
public class TokenRequest
{
    [JsonPropertyName("prompt")]
    public PromptToken Prompt { get; set; } = new();
}
*/
/// <summary>
/// class for request token count
/// </summary>
public class TokenRequest
{
    [JsonPropertyName("contents")]
    public Content[] Contents { get; set; } = new Content[] { new() };
}
/// <summary>
/// contain collection of text for token counting
/// </summary>
public class Content
{
    [JsonPropertyName("parts")]
    public Part[] Parts { get; set; } = new Part[] { new() };
}
/// <summary>
/// text 
/// </summary>
public class Part
{
    [JsonPropertyName("text")]
    public string Text { get; set; } = string.Empty;
}
