using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using StealCatsUI;
using StealCatsUI.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddScoped<CatService>();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:44350") });

await builder.Build().RunAsync();
