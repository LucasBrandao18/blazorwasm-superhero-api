using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorWasmSuperHeroApi;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(_ => new HttpClient { Timeout = TimeSpan.FromSeconds(30) });
builder.Services.AddScoped<BlazorWasmSuperHeroApi.Services.SuperHeroService>();

await builder.Build().RunAsync();
