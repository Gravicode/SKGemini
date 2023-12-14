// Copyright (c) Microsoft. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Connectors.AI.Gemini.Helper;
using Microsoft.SemanticKernel.AI;
using Microsoft.SemanticKernel.AI.Embeddings;
using Microsoft.SemanticKernel.Connectors.AI.Gemini.TextEmbedding;
using Microsoft.SemanticKernel.Diagnostics;
using Microsoft.SemanticKernel.Services;

namespace Connectors.AI.Gemini.TextEmbedding;

/// <summary>
/// Gemini embedding generation service.
/// </summary>
public sealed class GeminiTextEmbeddingGeneration : ITextEmbeddingGeneration, IDisposable
{
    private const string HttpUserAgent = "Microsoft-Semantic-Kernel";

    private readonly string _model = "embedding-001";//"embedding-gecko-001";
    private readonly string? _endpoint = "https://generativelanguage.googleapis.com/v1beta/models";//"https://generativelanguage.googleapis.com/v1beta2/models";
    private readonly HttpClient _httpClient;
    private readonly string? _apiKey;
    private readonly Dictionary<string, string> _attributes = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="GeminiTextEmbeddingGeneration"/> class.
    /// Using default <see cref="HttpClientHandler"/> implementation.
    /// </summary>
    /// <param name="endpoint">Endpoint for service API call.</param>
    /// <param name="model">Model to use for service API call.</param>
    /// <param name="apiKey">Gemini API KEY to use for service API call.</param>
    /// <param name="httpClient">instance of http client if already exist</param>
    public GeminiTextEmbeddingGeneration(Uri endpoint, string model, string apiKey, HttpClient? httpClient = null)
    {
        VerifyHelper.NotNull(endpoint);
        VerifyHelper.NotNullOrWhiteSpace(model);
        VerifyHelper.NotNullOrWhiteSpace(apiKey);

        this._endpoint = endpoint.AbsoluteUri;
        this._model = model;
        this._apiKey = apiKey;
        this._httpClient = httpClient ?? new HttpClient();
        this._attributes.Add(IAIServiceExtensions.ModelIdKey, this._model);
        this._attributes.Add(IAIServiceExtensions.EndpointKey, this._endpoint);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GeminiTextEmbeddingGeneration"/> class.
    /// </summary>
    /// <param name="model">Model to use for service API call.</param>
    /// <param name="apiKey">API Key for Gemini.</param>
    public GeminiTextEmbeddingGeneration(string model, string apiKey)
    {
        VerifyHelper.NotNullOrWhiteSpace(model);
        VerifyHelper.NotNullOrWhiteSpace(apiKey);

        this._model = model;
        this._apiKey = apiKey;
        this._httpClient = new HttpClient();
        this._attributes.Add(IAIServiceExtensions.ModelIdKey, this._model);
        this._attributes.Add(IAIServiceExtensions.EndpointKey, this._endpoint);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GeminiTextEmbeddingGeneration"/> class.
    /// </summary>
    /// <param name="model">Model to use for service API call.</param>
    /// <param name="httpClient">The HttpClient used for making HTTP requests.</param>
    /// <param name="apiKey">API Key for Gemini.</param>
    /// <param name="endpoint">Endpoint for service API call. If not specified, the base address of the HTTP client is used.</param>
    public GeminiTextEmbeddingGeneration(string model, HttpClient httpClient, string? apiKey, string? endpoint = null)
    {
        VerifyHelper.NotNullOrWhiteSpace(model);
        VerifyHelper.NotNullOrWhiteSpace(apiKey);
        VerifyHelper.NotNull(httpClient);

        this._model = model;
        this._endpoint = endpoint;
        this._httpClient = httpClient;
        this._apiKey = apiKey;
        if (httpClient.BaseAddress == null && string.IsNullOrEmpty(endpoint))
        {
            throw new SKException(
                
                "The HttpClient BaseAddress and endpoint are both null or empty. Please ensure at least one is provided.");
        }
        this._attributes.Add(IAIServiceExtensions.ModelIdKey, this._model);
        this._attributes.Add(IAIServiceExtensions.EndpointKey, this._endpoint);
    }
    /// <inheritdoc/>
    public IReadOnlyDictionary<string, string> Attributes => this._attributes;
    /// <inheritdoc/>
    public async Task<IList<ReadOnlyMemory<float>>> GenerateEmbeddingsAsync(IList<string> data, CancellationToken cancellationToken = default)
    {
        return await this.ExecuteEmbeddingRequestAsync(data, cancellationToken).ConfigureAwait(false);
    }

    #region private ================================================================================

    /// <summary>
    /// Performs HTTP request to given endpoint for embedding generation.
    /// </summary>
    /// <param name="data">Data to embed.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> to monitor for cancellation requests. The default is <see cref="CancellationToken.None"/>.</param>
    /// <returns>List of generated embeddings.</returns>
    /// <exception cref="AIException">Exception when backend didn't respond with generated embeddings.</exception>
    private async Task<IList<ReadOnlyMemory<float>>> ExecuteEmbeddingRequestAsync(IList<string> data, CancellationToken cancellationToken)
    {
        try
        {
            var embeddingRequest = new TextEmbeddingRequest();
            embeddingRequest.Content.Parts = data.Select(x => new  TextEmbeddingPart () { Text = x }).ToArray();

            using var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = this.GetRequestUri(),
                Content = new StringContent(JsonSerializer.Serialize(embeddingRequest)),
            };

            httpRequestMessage.Headers.Add("User-Agent", HttpUserAgent);

            var response = await this._httpClient.SendAsync(httpRequestMessage, cancellationToken).ConfigureAwait(false);
            var body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            var embeddingResponse = JsonSerializer.Deserialize<TextEmbeddingResponse>(body);
            if(embeddingResponse is null || embeddingResponse.embedding is null)
            {
                var errorCls = JsonSerializer.Deserialize<ErrorGemini>(body);
                if (errorCls != null)
                {
                    throw new SKException(
                        $"{errorCls.error.code}-{errorCls.error.status}: {errorCls.error.message}");
                }
            }
            return new List<ReadOnlyMemory<float>>() { new ReadOnlyMemory<float>(embeddingResponse?.embedding?.values?.ToArray()) };
        }
        catch (Exception e) 
        {
            throw new SKException(
                $"Something went wrong: {e.Message}", e);
        }
    }

    /// <summary>
    /// Retrieves the request URI based on the provided endpoint and model information.
    /// </summary>
    /// <returns>
    /// A <see cref="Uri"/> object representing the request URI.
    /// </returns>
    private Uri GetRequestUri()
    {
        string? baseUrl = null;

        if (!string.IsNullOrEmpty(this._endpoint))
        {
            baseUrl = this._endpoint;
        }
        else if (this._httpClient.BaseAddress?.AbsoluteUri != null)
        {
            baseUrl = this._httpClient.BaseAddress!.AbsoluteUri;
        }
        else
        {
            throw new SKException( "No endpoint or HTTP client base address has been provided");
        }

        //var url = $"{baseUrl!.TrimEnd('/')}/{this._model}:embedText?key={this._apiKey}";
        var url = $"{baseUrl!.TrimEnd('/')}/{this._model}:embedContent?key={this._apiKey}";

        return new Uri(url);
    }

    public void Dispose()
    {
        _httpClient.Dispose();
    }

    #endregion
}
