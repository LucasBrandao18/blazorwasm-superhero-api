using System.Text.Json.Serialization;

namespace BlazorWasmSuperHeroApi.Models;

public sealed class SuperHeroResponse
{
    public string? Response { get; set; }
    [JsonPropertyName("results-for")]
    public string? ResultsFor { get; set; }
    public List<SuperHero>? Results { get; set; }
    public string? Error { get; set; }
}
