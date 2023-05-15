using Sales.API.Data;
using Sales.API.Services;
using Microsoft.EntityFrameworkCore;
using Sales.API.Services.Interfaces;
using System.Text.Json.Serialization;
using Sales.API.Infrastructure.Repositories;
using Sales.API.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Sales.API.Data.Entities;
using Sales.API.Helpers;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<SalesDataContex>(x => x.UseSqlServer("name=LocalConnection"));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddTransient<SeedDb>();
builder.Services.AddScoped<IApiService, Apiservice>();
builder.Services.AddScoped<IUserHelper, UserHelper>();

builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<IStateRepository, StateRepository>();
builder.Services.AddScoped<ICountryRepository, CountryRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

builder.Services.AddIdentity<User, IdentityRole>(x =>
{
    x.User.RequireUniqueEmail = true;
    x.Password.RequireDigit = false;
    x.Password.RequiredUniqueChars = 0;
    x.Password.RequireLowercase = false;
    x.Password.RequireNonAlphanumeric = false;
    x.Password.RequireUppercase = false;
})
    .AddEntityFrameworkStores<SalesDataContex>()
    .AddDefaultTokenProviders();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder => { builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); });
});

WebApplication app = builder.Build();

//inyeccion de datos harcode
SeedData(app);
static void SeedData(WebApplication app)
{
    IServiceScopeFactory scopeFactory = app.Services.GetService<IServiceScopeFactory>();

    using IServiceScope scope = scopeFactory.CreateScope();
    SeedDb service = scope.ServiceProvider.GetService<SeedDb>();
    service.SeedAsync().Wait();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseCors();
app.MapControllers();

app.Run();
