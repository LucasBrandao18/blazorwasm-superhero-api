using System.Text.Json.Serialization;

namespace BlazorWasmSuperHeroApi.Models;

public sealed class Work
{
    [JsonPropertyName("occupation")]
    public string? Occupation { get; set; }

    [JsonPropertyName("base")]
    public string? Base { get; set; }
}
