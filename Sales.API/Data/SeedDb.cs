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
        private readonly SalesDataContex _context;
        private readonly IUserHelper _userHelper;
        private readonly SeedUserDefault _userDefault;

        public SeedDb(SalesDataContex context, IApiService apiService, IUserHelper userHelper, IOptions<SeedUserDefault> userDefault)
        {
            _context = context;
            _apiService = apiService;
            _userHelper = userHelper;
            _userDefault = userDefault.Value;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckAsync();
            await CheckRolesAsync();
            await CheckUserAsync("200", "Luis", "Caceres", _userDefault.Email, _userDefault.PhoneNumber, "Jiqui", UserType.Admin);
        }

        private async Task<User> CheckUserAsync(string document, string firstName, string lastName, string email, string phone, string address, UserType userType)
        {
            var user = await _userHelper.GetUserAsync(email);
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
                    City = _context.Cities.FirstOrDefault(c => c.Name == "Jiquilisco"),
                    UserType = userType
                };

                await _userHelper.AddUserAsync(user, _userDefault.Password);
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());
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
                    if(countries.Count > 50) 
                        countries = countries.Skip(50).Take(100).ToList();

                    foreach (CountryResponse countryResponse in countries)
                    {
                        Country country = await _context.Countries!.FirstOrDefaultAsync(c => c.Name == countryResponse.Name!)!;
                        if (country == null)
                        {
                            country = new()
                            { Name = countryResponse.Name!, States = new List<State>() };
                            Response responseStates = await _apiService.GetListAsync<StateResponse>("/v1", $"/countries/{countryResponse.Iso2}/states");
                            if (responseStates.IsSuccess)
                            {
                                List<StateResponse> states = (List<StateResponse>)responseStates.Result!;
                                if(states.Count > 50)
                                     states = states.Take(50).ToList();

                                foreach (StateResponse stateResponse in states!)
                                {
                                    State state = country.States!.FirstOrDefault(s => s.Name == stateResponse.Name!)!;
                                    if (state == null)
                                    {
                                        state = new() { Name = stateResponse.Name!, Cities = new List<City>() };
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
                _context.Categories.Add(new Category { Name = "Calzado" });
                _context.Categories.Add(new Category { Name = "Electrodomestico" });
                _context.Categories.Add(new Category { Name = "Electronica" });
                _context.Categories.Add(new Category { Name = "Informatica" });
                _context.Categories.Add(new Category { Name = "Telefonia" });
                _context.Categories.Add(new Category { Name = "Muebles" });
                _context.Categories.Add(new Category { Name = "Audio y video" });
                _context.Categories.Add(new Category { Name = "Automovil" });
            }

            await _context.SaveChangesAsync();
        }
    }
}
