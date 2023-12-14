// Copyright (c) Microsoft. All rights reserved.

using Connectors.AI.Gemini.TextCompletion;
using Microsoft.SemanticKernel.Orchestration;

#pragma warning disable IDE0130

namespace Microsoft.SemanticKernel;

public static class GeminiModelResultExtension
{
    /// <summary>
    /// Retrieves a typed <see cref="TextCompletionResponse"/> Gemini result from PromptResult/>.
    /// </summary>
    /// <param name="resultBase">Current context</param>
    /// <returns>Gemini result <see cref="TextCompletionResponse"/></returns>
    public static TextCompletionResponse GetGeminiResult(this ModelResult resultBase)
    {
        return resultBase.GetResult<TextCompletionResponse>();
    }
}
