// Copyright (c) Microsoft. All rights reserved.

using System.Net.Http;
using Connectors.AI.Gemini.TextEmbedding;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.AI.Embeddings;
using Microsoft.SemanticKernel.AI.TextCompletion;
using Microsoft.SemanticKernel.Connectors.AI.Gemini.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.AI.Gemini.TextCompletion;

#pragma warning disable IDE0130
// ReSharper disable once CheckNamespace - Using NS of KernelConfig
namespace Microsoft.SemanticKernel;
#pragma warning restore IDE0130

/// <summary>
/// Provides extension methods for the <see cref="KernelBuilder"/> class to configure Gemini connectors.
/// </summary>
public static class GeminiKernelBuilderExtensions
{
    /// <summary>
    /// Registers an Gemini text completion service with the specified configuration.
    /// </summary>
    /// <param name="builder">The <see cref="KernelBuilder"/> instance.</param>
    /// <param name="model">The name of the Gemini model.</param>
    /// <param name="apiKey">The API key required for accessing the Gemini service.</param>
    /// <param name="endpoint">The endpoint URL for the text completion service.</param>
    /// <param name="serviceId">A local identifier for the given AI service.</param>
    /// <param name="httpClient">The optional <see cref="HttpClient"/> to be used for making HTTP requests.
    /// If not provided, a default <see cref="HttpClient"/> instance will be used.</param>
    /// <returns>The modified <see cref="KernelBuilder"/> instance.</returns>
    public static KernelBuilder WithGeminiTextCompletionService(this KernelBuilder builder,
        string model,
        string? apiKey = null,
        string? endpoint = null,
        string? serviceId = null,
        HttpClient? httpClient = null)
    {
        builder.WithAIService<ITextCompletion>(serviceId, (parameters) =>
            new GeminiTextCompletion(
                model,
                apiKey,
                new HttpClient(),
                endpoint));

        return builder;
    }


    public static KernelBuilder WithGeminiChatCompletionService(this KernelBuilder builder,
        string model,
        string apiKey = null, string? serviceId=null)
    {
        builder.WithAIService<GeminiChatCompletion>(serviceId, (parameters) =>
            new GeminiChatCompletion(
                model,
                apiKey));

        return builder;
    }
   

    /// <summary>
    /// Registers an Gemini text embedding generation service with the specified configuration.
    /// </summary>
    /// <param name="builder">The <see cref="KernelBuilder"/> instance.</param>
    /// <param name="model">The name of the Gemini model.</param>
    /// <param name="apiKey">API Key for Gemini.</param>
    /// <param name="serviceId">A local identifier for the given AI service.</param>
    /// <returns>The <see cref="KernelBuilder"/> instance.</returns>
    public static KernelBuilder WithGeminiTextEmbeddingGenerationService(this KernelBuilder builder,
        string model,
        string apiKey,
        string? serviceId = null)
    {
        builder.WithAIService<ITextEmbeddingGeneration>(serviceId, (parameters) =>
            new GeminiTextEmbeddingGeneration(
                model,
                apiKey: apiKey));

        return builder;
    }

    /// <summary>
    /// Registers an Gemini text embedding generation service with the specified configuration.
    /// </summary>
    /// <param name="builder">The <see cref="KernelBuilder"/> instance.</param>
    /// <param name="model">The name of the Gemini model.</param>
    /// <param name="httpClient">The optional <see cref="HttpClient"/> instance used for making HTTP requests.</param>
    /// <param name="endpoint">The endpoint for the text embedding generation service.</param>
    /// <param name="serviceId">A local identifier for the given AI serviceю</param>
    /// <returns>The <see cref="KernelBuilder"/> instance.</returns>
    public static KernelBuilder WithGeminiTextEmbeddingGenerationService(this KernelBuilder builder,
        string model,
        HttpClient? httpClient = null,
        string? endpoint = null,
        string? serviceId = null)
    {
        builder.WithAIService<ITextEmbeddingGeneration>(serviceId, (parameters) =>
            new GeminiTextEmbeddingGeneration(
                model,
                new HttpClient(),
                endpoint));

        return builder;
    }
}
