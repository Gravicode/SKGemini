// Copyright (c) Microsoft. All rights reserved.

using System.Text.Json.Serialization;

namespace Connectors.AI.Gemini.Skills;
/*
public class TokenResponse
{
    [JsonPropertyName("tokenCount")]
    public int? TokenCount { get; set; }
}
*/

public class TokenResponse
{
    [JsonPropertyName("totalTokens")]
    public int TotalTokens { get; set; }
}
