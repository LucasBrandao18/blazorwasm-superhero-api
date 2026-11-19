using System.Text.Json.Serialization;

namespace BlazorWasmSuperHeroApi.Models;

public sealed class Connections
{
    [JsonPropertyName("group-affiliation")]
    public string? GroupAffiliation { get; set; }

    [JsonPropertyName("relatives")]
    public string? Relatives { get; set; }
}
