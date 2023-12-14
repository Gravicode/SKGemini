// Copyright (c) Microsoft. All rights reserved.

using System;
using System.Text.Json.Serialization;

namespace Microsoft.SemanticKernel.Connectors.AI.Gemini.TextCompletion;

/*
/// <summary>
/// HTTP schema to perform completion request.
/// </summary>
[Serializable]
public sealed class TextCompletionRequest
{
    ///// <summary>
    ///// Prompt to complete.
    ///// </summary>
    [JsonPropertyName("prompt")]
    public Prompt Prompt { get; set; } = new();
    [JsonPropertyName("temperature")]
    public float Temperature { get; set; } = 0.1f;
    [JsonPropertyName("top_p")]
    public float TopP { get; set; } = 0.95f;
    [JsonPropertyName("candidate_count")]
    public int CandidateCount { get; set; } = 3;
    [JsonPropertyName("top_k")]
    public int TopK { get; set; } = 40;
    [JsonPropertyName("max_output_tokens")]
    public int MaxOutputTokens { get; set; } = 2048;
    [JsonPropertyName("stop_sequences")]
    public string[] StopSequences { get; set; } = Array.Empty<string>();
}


/// <summary>
/// Text prompt
/// </summary>
[Serializable]
public class Prompt
{
    [JsonPropertyName("text")]
    public string Text { get; set; } = string.Empty;
}
*/

/// <summary>
/// HTTP schema to perform completion request.
/// </summary>
[Serializable]
public class TextCompletionRequest
{
    [JsonPropertyName("contents")]
    public Content[] Contents { get; set; } = new Content[] { new() };
    [JsonPropertyName("generationConfig")]
    public Generationconfig GenerationConfig { get; set; } = new();
    [JsonPropertyName("safetySettings")]
    public Safetysetting[] SafetySettings { get; set; } = SafetySettingHelper.GetDefault();
}
[Serializable]
public class Generationconfig
{
    [JsonPropertyName("temperature")]
    public float Temperature { get; set; } = 0.9f;
    [JsonPropertyName("topK")]
    public float TopK { get; set; } = 1f;
    [JsonPropertyName("topP")]
    public float TopP { get; set; } = 1f;
    [JsonPropertyName("maxOutputTokens")]
    public int MaxOutputTokens { get; set; } = 2048;
    [JsonPropertyName("stopSequences")]
    public object[] StopSequences { get; set; } = new object[0];
}
[Serializable]
public class Content
{
    [JsonPropertyName("parts")]
    public Part[] Parts { get; set; } = new Part[] { new() };
}
[Serializable]
public class Part
{
    [JsonPropertyName("text")]
    public string Text { get; set; } = string.Empty;
}
[Serializable]
public class Safetysetting
{
    [JsonPropertyName("category")]
    public string Category { get; set; }
    [JsonPropertyName("threshold")]
    public string Threshold { get; set; } 
}

public enum SafetySettingThresholds { BLOCK_ONLY_HIGH, BLOCK_MEDIUM_AND_ABOVE, BLOCK_LOW_AND_ABOVE }
public class SafetySettingHelper
{
    public static string[] SafetyCategories = new string[] { "HARM_CATEGORY_HARASSMENT", "HARM_CATEGORY_HATE_SPEECH", "HARM_CATEGORY_SEXUALLY_EXPLICIT", "HARM_CATEGORY_DANGEROUS_CONTENT" };

    public static Safetysetting[] GetDefault()
    {
        var defaultSetting = new List<Safetysetting>();
        foreach(var category in SafetyCategories)
        {
            defaultSetting.Add(new() { Category = category, Threshold = SafetySettingThresholds.BLOCK_ONLY_HIGH.ToString() });
        }
        return defaultSetting.ToArray();
    }

}
