using Sales.Blazor;
using Sales.Blazor.Repositories;
using Microsoft.AspNetCore.Components.Web;
using Sales.Blazor.Repositories.Interfaces;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7131/") });

await builder.Build().RunAsync();
