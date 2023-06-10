using Sales.API.Helpers;
using Sales.Shared.Enums;
using Sales.Shared.Responses;
using Sales.API.Data.Entities;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Sales.API.Services.Interfaces;

namespace Sales.API.Data
{
    public class SeedDb
    {
        private readonly IApiService _apiService;
        private readonly IUserHelper _userHelper;
        private readonly SalesDataContex _context;
        private readonly IFileStorage _fileStorage;
        private readonly SeedUserDefault _userDefault;

        public SeedDb(SalesDataContex context, IApiService apiService, IUserHelper userHelper, IOptions<SeedUserDefault> userDefault, IFileStorage fileStorage)
        {
            _context = context;
            _apiService = apiService;
            _userHelper = userHelper;
            _userDefault = userDefault.Value;
            _fileStorage = fileStorage;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            //await CheckAsync();
            //await CheckRolesAsync();
            //await CheckProductsAsync();
            await CheckUserAsync("201", "Amy", "Lee", "loveamylee@yopmail.com", "43785695", "Usa", UserType.User);
            await CheckUserAsync("202", "Floki", "God", "flokigod@yopmail.com", "85471235", "Norwa", UserType.User);
            await CheckUserAsync("203", "Ronni", "Radke", "ronnicrack@yopmail.com", "45781263", "Usa", UserType.User);
            await CheckUserAsync("204", "Ragnar", "Lodbrok", "ragnarxdnew@yopmail.com", "45781263", "Norwa", UserType.User);
            await CheckUserAsync("205", "Laguertha", "Lodbrok", "lagerthapatrona@yopmail.com", "87956412", "Norwa", UserType.User);
            await CheckUserAsync("206", "Luis", "Caceres", _userDefault.Email, _userDefault.PhoneNumber, "Jiqui", UserType.Admin);
        }

        private async Task CheckProductsAsync()
        {
            if (!_context.Products.Any())
            {
                await AddProductAsync("Adidas Barracuda", 270000M, 12F, new List<string>() { "Calzado", "Deportes" }, new List<string>() { "adidas_barracuda.png" });
                await AddProductAsync("Adidas Superstar", 250000M, 12F, new List<string>() { "Calzado", "Deportes" }, new List<string>() { "Adidas_superstar.png" });
                await AddProductAsync("AirPods", 1300000M, 12F, new List<string>() { "Tecnología", "Apple" }, new List<string>() { "airpos.png", "airpos2.png" });
                await AddProductAsync("Audifonos Bose", 870000M, 12F, new List<string>() { "Tecnología" }, new List<string>() { "audifonos_bose.png" });
                await AddProductAsync("Bicicleta Ribble", 12000000M, 6F, new List<string>() { "Deportes" }, new List<string>() { "bicicleta_ribble.png" });
                await AddProductAsync("Camisa Cuadros", 56000M, 24F, new List<string>() { "Ropa" }, new List<string>() { "camisa_cuadros.png" });
                await AddProductAsync("Casco Bicicleta", 820000M, 12F, new List<string>() { "Deportes" }, new List<string>() { "casco_bicicleta.png", "casco.png" });
                await AddProductAsync("iPad", 2300000M, 6F, new List<string>() { "Tecnología", "Apple" }, new List<string>() { "ipad.png" });
                await AddProductAsync("iPhone 13", 5200000M, 6F, new List<string>() { "Tecnología", "Apple" }, new List<string>() { "iphone13.png", "iphone13b.png", "iphone13c.png", "iphone13d.png" });
                await AddProductAsync("Mac Book Pro", 12100000M, 6F, new List<string>() { "Tecnología", "Apple" }, new List<string>() { "mac_book_pro.png" });
                await AddProductAsync("Mancuernas", 370000M, 12F, new List<string>() { "Deportes" }, new List<string>() { "mancuernas.png" });
                await AddProductAsync("Mascarilla Cara", 26000M, 100F, new List<string>() { "Belleza" }, new List<string>() { "mascarilla_cara.png" });
                await AddProductAsync("New Balance 530", 180000M, 12F, new List<string>() { "Calzado", "Deportes" }, new List<string>() { "newbalance530.png" });
                await AddProductAsync("New Balance 565", 179000M, 12F, new List<string>() { "Calzado", "Deportes" }, new List<string>() { "newbalance565.png" });
                await AddProductAsync("Nike Air", 233000M, 12F, new List<string>() { "Calzado", "Deportes" }, new List<string>() { "nike_air.png" });
                await AddProductAsync("Nike Zoom", 249900M, 12F, new List<string>() { "Calzado", "Deportes" }, new List<string>() { "nike_zoom.png" });
                await AddProductAsync("Buso Adidas Mujer", 134000M, 12F, new List<string>() { "Ropa", "Deportes" }, new List<string>() { "buso_adidas.png" });
                await AddProductAsync("Suplemento Boots Original", 15600M, 12F, new List<string>() { "Nutrición" }, new List<string>() { "Boost_Original.png" });
                await AddProductAsync("Whey Protein", 252000M, 12F, new List<string>() { "Nutrición" }, new List<string>() { "whey_protein.png" });
                await AddProductAsync("Arnes Mascota", 25000M, 12F, new List<string>() { "Mascotas" }, new List<string>() { "arnes_mascota.png" });
                await AddProductAsync("Cama Mascota", 99000M, 12F, new List<string>() { "Mascotas" }, new List<string>() { "cama_mascota.png" });
                await AddProductAsync("Teclado Gamer", 67000M, 12F, new List<string>() { "Gamer", "Tecnología" }, new List<string>() { "teclado_gamer.png" });
                await AddProductAsync("Silla Gamer", 980000M, 12F, new List<string>() { "Gamer", "Tecnología" }, new List<string>() { "silla_gamer.png" });
                await AddProductAsync("Mouse Gamer", 132000M, 12F, new List<string>() { "Gamer", "Tecnología" }, new List<string>() { "mouse_gamer.png" });
                await _context.SaveChangesAsync();
            }
        }

        private async Task AddProductAsync(string name, decimal price, float stock, List<string> categories, List<string> images)
        {
            Product product = new()
            {
                Name = name,
                Price = price,
                Stock = stock,
                CrateAt = DateTime.UtcNow,
                ProductImages = new List<ProductImage>(),
                ProductCategories = new List<ProductCategory>()
            };

            foreach (string categoryName in categories)
            {
                Category category = await _context.Categories.FirstOrDefaultAsync(c => c.Name == categoryName);
                if (category != null)
                    product.ProductCategories.Add(new ProductCategory { Category = category });
            }

            foreach (string image in images)
            {
                string filePaht = $"{Environment.CurrentDirectory}\\Helpers\\products\\{image}";
                byte[] fileBytes = File.ReadAllBytes(filePaht);
                string imagePaht = await _fileStorage.SaveFileAsync(fileBytes, ".jpg", "products");
                product.ProductImages.Add(new ProductImage { Image = imagePaht });
                _context.ProductImages.Add(new ProductImage { Image = imagePaht, Product = product });
            }
            _context.Products.Add(product);
        }

        private async Task<User> CheckUserAsync(string document, string firstName, string lastName, string email, string phone, string address, UserType userType)
        {
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                user = new User
                {
                    Document = document,
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone,
                    Address = address,
                    CrateAt = DateTime.UtcNow,
                    City = _context.Cities.FirstOrDefault(c => c.Name == "Jiquilisco"),
                    UserType = userType
                };

                await _userHelper.AddUserAsync(user, _userDefault.Password);
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());
                string tokenEmail = await _userHelper.GenerateEmailTokenConfirmAsync(user);
                await _userHelper.ConfirmEmailAsync(user, tokenEmail);
            }
            return user;
        }

        private async Task CheckRolesAsync()
        {
            await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
            await _userHelper.CheckRoleAsync(UserType.User.ToString());
        }

        private async Task CheckAsync()
        {
            if (!_context.Countries.Any())
            {
                Response responseCountries = await _apiService.GetListAsync<CountryResponse>("/v1", "/countries");
                if (responseCountries.IsSuccess)
                {
                    List<CountryResponse> countries = (List<CountryResponse>)responseCountries.Result!;
                    foreach (CountryResponse countryResponse in countries)
                    {
                        Country country = await _context.Countries!.FirstOrDefaultAsync(c => c.Name == countryResponse.Name!)!;
                        if (country == null)
                        {
                            country = new()
                            {
                                Name = countryResponse.Name!,
                                CrateAt = DateTime.UtcNow,
                                States = new List<State>()
                            };
                            Response responseStates = await _apiService.GetListAsync<StateResponse>("/v1", $"/countries/{countryResponse.Iso2}/states");
                            if (responseStates.IsSuccess)
                            {
                                List<StateResponse> states = (List<StateResponse>)responseStates.Result!;
                                if (states.Count > 50)
                                    states = states.Take(50).ToList();

                                foreach (StateResponse stateResponse in states!)
                                {
                                    State state = country.States!.FirstOrDefault(s => s.Name == stateResponse.Name!)!;
                                    if (state == null)
                                    {
                                        state = new()
                                        {
                                            Name = stateResponse.Name!,
                                            CrateAt = DateTime.UtcNow,
                                            Cities = new List<City>()
                                        };
                                        Response responseCities = await _apiService.GetListAsync<CityResponse>("/v1", $"/countries/{countryResponse.Iso2}/states/{stateResponse.Iso2}/cities");
                                        if (responseCities.IsSuccess)
                                        {
                                            List<CityResponse> cities = (List<CityResponse>)responseCities.Result!;
                                            if (cities.Count > 50)
                                                cities = cities.Take(50).ToList();

                                            foreach (CityResponse cityResponse in cities)
                                            {
                                                if (cityResponse.Name == "Mosfellsbær" || cityResponse.Name == "Șăulița")
                                                {
                                                    continue;
                                                }
                                                City city = state.Cities!.FirstOrDefault(c => c.Name == cityResponse.Name!)!;
                                                if (city == null)
                                                {
                                                    state.Cities.Add(new City() { Name = cityResponse.Name! });
                                                }
                                            }
                                        }
                                        if (state.CitiesNumber > 0)
                                        {
                                            country.States.Add(state);
                                        }
                                    }
                                }
                            }
                            if (country.StatesNumber > 0)
                            {
                                _context.Countries.Add(country);
                            }

                            await _context.SaveChangesAsync();
                        }
                    }
                }
            }



            if (!_context.Categories.Any())
            {
                _context.Categories.Add(new Category { Name = "Apple", CrateAt = DateTime.UtcNow });
                _context.Categories.Add(new Category { Name = "Autos", CrateAt = DateTime.UtcNow });
                _context.Categories.Add(new Category { Name = "Belleza", CrateAt = DateTime.UtcNow });
                _context.Categories.Add(new Category { Name = "Calzado", CrateAt = DateTime.UtcNow });
                _context.Categories.Add(new Category { Name = "Comida", CrateAt = DateTime.UtcNow });
                _context.Categories.Add(new Category { Name = "Cosmeticos", CrateAt = DateTime.UtcNow });
                _context.Categories.Add(new Category { Name = "Deportes", CrateAt = DateTime.UtcNow });
                _context.Categories.Add(new Category { Name = "Erótica", CrateAt = DateTime.UtcNow });
                _context.Categories.Add(new Category { Name = "Ferreteria", CrateAt = DateTime.UtcNow });
                _context.Categories.Add(new Category { Name = "Gamer", CrateAt = DateTime.UtcNow });
                _context.Categories.Add(new Category { Name = "Hogar", CrateAt = DateTime.UtcNow });
                _context.Categories.Add(new Category { Name = "Jardín", CrateAt = DateTime.UtcNow });
                _context.Categories.Add(new Category { Name = "Jugetes", CrateAt = DateTime.UtcNow });
                _context.Categories.Add(new Category { Name = "Lenceria", CrateAt = DateTime.UtcNow });
                _context.Categories.Add(new Category { Name = "Mascotas", CrateAt = DateTime.UtcNow });
                _context.Categories.Add(new Category { Name = "Nutrición", CrateAt = DateTime.UtcNow });
                _context.Categories.Add(new Category { Name = "Ropa", CrateAt = DateTime.UtcNow });
                _context.Categories.Add(new Category { Name = "Tecnología", CrateAt = DateTime.UtcNow });
            }

            await _context.SaveChangesAsync();
        }
    }
}
