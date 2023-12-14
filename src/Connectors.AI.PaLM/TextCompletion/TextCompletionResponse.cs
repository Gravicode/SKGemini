// Copyright (c) Microsoft. All rights reserved.

using System.Text.Json.Serialization;

namespace Connectors.AI.Gemini.TextCompletion;
/*
public class TextCompletionResponse
{
    [JsonPropertyName("candidates")]
    public Candidate[]? Candidates { get; set; }
}

public class Candidate
{
    [JsonPropertyName("output")]
    public string? Output { get; set; }
    [JsonPropertyName("safetyRatings")]
    public Safetyrating[]? SafetyRatings { get; set; }
}

public class Safetyrating
{
    [JsonPropertyName("category")]
    public string? Category { get; set; }
    [JsonPropertyName("probability")]
    public string? Probability { get; set; }
}
*/

public class TextCompletionResponse
{
    [JsonPropertyName("candidates")]
    public Candidate[] Candidates { get; set; }
    [JsonPropertyName("promptFeedback")]
    public Promptfeedback PromptFeedback { get; set; }
}

public class Promptfeedback
{
    [JsonPropertyName("safetyRatings")]
    public Safetyrating[] SafetyRatings { get; set; }
}

public class Safetyrating
{
    [JsonPropertyName("category")]
    public string Category { get; set; }
    [JsonPropertyName("probability")]
    public string Probability { get; set; }
}

public class Candidate
{
    [JsonPropertyName("content")]
    public Content content { get; set; }
    [JsonPropertyName("finishReason")]
    public string finishReason { get; set; }
    [JsonPropertyName("index")]
    public int Index { get; set; }
    [JsonPropertyName("safetyRatings")]
    public Safetyrating1[] SafetyRatings { get; set; }
}

public class Content
{
    [JsonPropertyName("parts")]
    public Part[] Parts { get; set; }
    [JsonPropertyName("role")]
    public string Role { get; set; }
}

public class Part
{
    [JsonPropertyName("text")]
    public string Text { get; set; }
}

public class Safetyrating1
{
    [JsonPropertyName("category")]
    public string Category { get; set; }
    [JsonPropertyName("probability")]
    public string Probability { get; set; }
}
