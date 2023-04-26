using Sales.Web;
using Sales.Web.Repositories;
using Sales.Web.Repositories.Interfaces;
using Microsoft.AspNetCore.Components.Web;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7131/") });
builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddSweetAlert2();

await builder.Build().RunAsync();
