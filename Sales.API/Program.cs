using System.Text;
using Sales.API.Data;
using Sales.API.Helpers;
using Sales.API.Services;
using Sales.API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Sales.API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json.Serialization;
using Sales.API.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Sales.API.Infrastructure.Repositories.Interfaces;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddDbContext<SalesDataContex>(x => x.UseSqlServer("name=ConnectionStrings:LocalConnection"));

//PosgresConnection
builder.Services.AddDbContext<SalesDataContex>(options => options.UseNpgsql("name=ConnectionStrings:PostgresConnection"));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddTransient<SeedDb>();
builder.Services.AddScoped<IApiService, Apiservice>();
builder.Services.AddScoped<IMailHelper, MailHelper>();
builder.Services.AddScoped<IUserHelper, UserHelper>();
builder.Services.AddScoped<IFileStorage, FileStorage>();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<IStateRepository, StateRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICountryRepository, CountryRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductImageRepository, ProductImageRepository>();
builder.Services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();

//Get data from secrets
builder.Services.Configure<AzureBlobKey>(builder.Configuration.GetSection("AzureBlobKey"));
builder.Services.Configure<SendMailConfiguration>(builder.Configuration.GetSection("Mail"));
builder.Services.Configure<SeedUserDefault>(builder.Configuration.GetSection("DefaultUser"));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(x => x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtConfig:jwtKey"])),
        ClockSkew = TimeSpan.Zero
    });

builder.Services.AddIdentity<User, IdentityRole>(config =>
{
    config.Password.RequireDigit = false;
    config.User.RequireUniqueEmail = true;
    config.Password.RequiredUniqueChars = 0;
    config.Password.RequireLowercase = false;
    config.Password.RequireUppercase = false;
    config.Lockout.AllowedForNewUsers = true;
    config.Lockout.MaxFailedAccessAttempts = 3;
    config.SignIn.RequireConfirmedEmail = true;
    config.Password.RequireNonAlphanumeric = false;
    config.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    config.Tokens.AuthenticatorTokenProvider = TokenOptions.DefaultProvider;
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
