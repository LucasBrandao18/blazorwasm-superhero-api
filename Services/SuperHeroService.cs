using System.Net.Http.Json;
using System.Text.Json;
using BlazorWasmSuperHeroApi.Models;

namespace BlazorWasmSuperHeroApi.Services;

public sealed class SuperHeroService(HttpClient http, IConfiguration configuration)
{
    public async Task<string?> FindFallbackImageAsync(SuperHero hero)
    {
        if (!int.TryParse(hero.Id, out var id) || id <= 0)
        {
            return null;
        }

        try
        {
            var portrait = await http.GetFromJsonAsync<PortraitResponse>(
                $"https://akabab.github.io/superhero-api/api/id/{id}.json");
            if (portrait?.Id == id && string.Equals(portrait.Name, hero.Name, StringComparison.OrdinalIgnoreCase)
                && portrait.Images?.TryGetValue("md", out var image) == true
                && Uri.TryCreate(image, UriKind.Absolute, out var uri) && uri.Scheme == "https")
            {
                return image;
            }
        }
        catch (Exception ex) when (ex is HttpRequestException or OperationCanceledException or JsonException)
        {
            // A ausência da imagem alternativa mantém a ficha disponível.
        }

        return null;
    }

    private sealed class PortraitResponse
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public Dictionary<string, string>? Images { get; set; }
    }

    public async Task<List<SuperHero>> SearchAsync(string name)
    {
        var token = configuration["SuperHeroApi:AccessToken"]?.Trim();
        if (string.IsNullOrWhiteSpace(token))
        {
            throw new InvalidOperationException("Configure o AccessToken em wwwroot/appsettings.json.");
        }

        var url = $"https://www.superheroapi.com/api.php/{Uri.EscapeDataString(token)}/search/{Uri.EscapeDataString(name.Trim())}";
        var result = await http.GetFromJsonAsync<SuperHeroResponse>(url)
            ?? throw new JsonException("A API retornou uma resposta vazia.");

        if (result.Response == "success")
        {
            return result.Results ?? [];
        }

        if (result.Error?.Contains("not found", StringComparison.OrdinalIgnoreCase) == true)
        {
            return [];
        }

        throw new InvalidOperationException("A API recusou a consulta. Confira o AccessToken e tente novamente.");
    }
}
