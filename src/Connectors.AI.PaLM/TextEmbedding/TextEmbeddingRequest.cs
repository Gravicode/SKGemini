// Copyright (c) Microsoft. All rights reserved.

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Microsoft.SemanticKernel.Connectors.AI.Gemini.TextEmbedding;

/*
/// <summary>
/// HTTP schema to perform embedding request.
/// </summary>
[Serializable]
public sealed class TextEmbeddingRequest
{
    /// <summary>
    /// Data to embed.
    /// </summary>
    [JsonPropertyName("text")]
    public string Text { get; set; } = string.Empty;
}
*/
/// <summary>
/// HTTP schema to perform embedding request.
/// </summary>
[Serializable]
public class TextEmbeddingRequest
{
    [JsonPropertyName("model")]
    public string Model { get; set; } = "models/embedding-001";
    [JsonPropertyName("content")]
    public Content Content { get; set; } = new();
}

public class Content
{
    [JsonPropertyName("parts")]
    public TextEmbeddingPart[] Parts { get; set; } = new TextEmbeddingPart[] { new() };
}

public class TextEmbeddingPart
{
    /// <summary>
    /// Data to embed.
    /// </summary>
    [JsonPropertyName("text")]
    public string Text { get; set; } = string.Empty;
}
