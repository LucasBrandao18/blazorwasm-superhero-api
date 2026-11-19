using System.Text.Json.Serialization;

namespace BlazorWasmSuperHeroApi.Models;

public sealed class ImageInfo
{
    [JsonPropertyName("url")]
    public string? Url { get; set; }
}
