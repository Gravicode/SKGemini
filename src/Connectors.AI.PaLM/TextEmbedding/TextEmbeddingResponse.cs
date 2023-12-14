// Copyright (c) Microsoft. All rights reserved.

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Connectors.AI.Gemini.TextEmbedding;

/// <summary>
/// embedding values in float 
/// </summary>
public class Embedding
{
    [JsonPropertyName("values")]
    public List<float>? values { get; set; }
}

/// <summary>
/// response from embedding function
/// </summary>
public sealed class TextEmbeddingResponse
{
    [JsonPropertyName("embedding")]
    public Embedding? embedding { get; set; }
}
