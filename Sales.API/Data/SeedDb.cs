using Sales.API.Data.Entities;

namespace Sales.API.Data
{
    public class SeedDb
    {
        private readonly SalesDataContex _context;

        public SeedDb(SalesDataContex context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckAsync();
            //await CheckCategoriesAsync();
        }

        private async Task CheckAsync()
        {
            if (!_context.Countries.Any())
            {
                _context.Countries.Add(new Country 
                {
                    Name = "El Salvador",
                    States = new List<State>
                    {
                        new State
                        {
                            Name = "Usulutan",
                            Cities = new List<City>
                            {
                                new City { Name = "Jiquilisco"},
                                new City { Name = "Usulutan"},
                                new City { Name = "Berlin"} 
                            }
                        },
                         new State
                        {
                            Name = "San Salvador",
                            Cities = new List<City>
                            {
                                new City { Name = "San Salvador"},
                                new City { Name = "Soyapango"},
                            }
                        },
                         new State
                        {
                            Name = "San Miguel",
                            Cities = new List<City>
                            {
                                new City { Name = "San Miguel"},
                                new City { Name = "Chapeltique"},
                            }
                        },
                    }
                });
                _context.Countries.Add(new Country 
                {
                    Name = "Costa Rica",
                    States = new List<State>
                    {
                        new State
                        {
                            Name = "Alajuela",
                            Cities = new List<City>
                            {
                                new City { Name = "Alajuela"},
                                new City { Name = "San Ramón"}
                            }
                        },
                         new State
                        {
                            Name = "Cartago",
                            Cities = new List<City>
                            {
                                new City { Name = "Cartago"},
                                new City { Name = "Paraíso"},
                            }
                        },
                         new State
                        {
                            Name = "Puntarenas",
                            Cities = new List<City>
                            {
                                new City { Name = "Puntarenas"},
                                new City { Name = "Quepos"},
                            }
                        },
                    }
                });
                _context.Countries.Add(new Country
                {
                    Name = "Guatemala",
                    States = new List<State>
                    {
                        new State
                        {
                            Name = "Alta Verapaz",
                            Cities = new List<City>
                            {
                                new City { Name = "Cobán"},
                                new City { Name = "San Pedro Carchá"}
                            }
                        },
                         new State
                        {
                            Name = "Chimaltenango",
                            Cities = new List<City>
                            {
                                new City { Name = "Chimaltenango"},
                                new City { Name = "San Juan Comalapa"},
                            }
                        },
                         new State
                        {
                            Name = "El Progreso",
                            Cities = new List<City>
                            {
                                new City { Name = "Guastatoya"},
                                new City { Name = "Sanarate"},
                            }
                        },
                    }
                });
                _context.Countries.Add(new Country 
                { 
                    Name = "Honduras",
                    States = new List<State>
                    {
                        new State
                        {
                            Name = "Cortés",
                            Cities = new List<City>
                            {
                                new City { Name = "San Pedro Sula"},
                                new City { Name = "Puerto Cortés"}
                            }
                        },
                         new State
                        {
                            Name = "Francisco Morazán",
                            Cities = new List<City>
                            {
                                new City { Name = "Tegucigalpa"},
                                new City { Name = "Comayagüela"},
                            }
                        },
                         new State
                        {
                            Name = "Gracias a Dios",
                            Cities = new List<City>
                            {
                                new City { Name = "Puerto Lempira"},
                                new City { Name = "Brus Laguna"},
                            }
                        },
                    }
                });
                _context.Countries.Add(new Country
                {
                    Name = "Nicaragua",
                    States = new List<State>
                    {
                        new State
                        {
                            Name = "Chinandega",
                            Cities = new List<City>
                            {
                                new City { Name = "Chinandega"},
                                new City { Name = "Corinto"}
                            }
                        },
                         new State
                        {
                            Name = "León",
                            Cities = new List<City>
                            {
                                new City { Name = "León"},
                                new City { Name = "La Paz Centro"},
                            }
                        },
                         new State
                        {
                            Name = "Rivas",
                            Cities = new List<City>
                            {
                                new City { Name = "Rivas"},
                                new City { Name = "Potosí"},
                            }
                        },
                    }
                });
                _context.Countries.Add(new Country 
                {
                    Name = "Panamá",
                    States = new List<State>
                    {
                        new State
                        {
                            Name = "Colón",
                            Cities = new List<City>
                            {
                                new City { Name = "Colón"},
                                new City { Name = "Portobelo"}
                            }
                        },
                         new State
                        {
                            Name = "Herrera",
                            Cities = new List<City>
                            {
                                new City { Name = "Chitré"},
                                new City { Name = "Santo Domingo"},
                            }
                        },
                         new State
                        {
                            Name = "Panamá Oeste",
                            Cities = new List<City>
                            {
                                new City { Name = "La Chorrera"},
                                new City { Name = "Capira"},
                            }
                        },
                    }
                });
                _context.Countries.Add(new Country 
                { 
                    Name = "Argentina",
                    States = new List<State>
                    {
                        new State
                        {
                            Name = "Buenos Aires",
                            Cities = new List<City>
                            {
                                new City { Name = "Buenos Aires"},
                                new City { Name = "La Plata"}
                            }
                        },
                         new State
                        {
                            Name = "Córdoba",
                            Cities = new List<City>
                            {
                                new City { Name = "Córdoba"},
                                new City { Name = "Villa Carlos Paz"},
                            }
                        },
                         new State
                        {
                            Name = "Mendoza",
                            Cities = new List<City>
                            {
                                new City { Name = "Mendoza"},
                                new City { Name = "San Rafael"},
                            }
                        },
                    }
                });
                _context.Countries.Add(new Country 
                { 
                    Name = "Brasil",
                    States = new List<State>
                    {
                        new State
                        {
                            Name = "Rio de Janeiro",
                            Cities = new List<City>
                            {
                                new City { Name = "Rio de Janeiro"},
                                new City { Name = "Niterói"}
                            }
                        },
                         new State
                        {
                            Name = "São Paulo",
                            Cities = new List<City>
                            {
                                new City { Name = "São Paulo"},
                                new City { Name = "Guarulhos"},
                            }
                        },
                         new State
                        {
                            Name = "Mendoza",
                            Cities = new List<City>
                            {
                                new City { Name = "Salvador"},
                                new City { Name = "Camaçari"},
                            }
                        },
                    }
                });
                _context.Countries.Add(new Country 
                { 
                    Name = "España",
                    States = new List<State>
                    {
                         new State
                        {
                            Name = "Barcelona",
                            Cities = new List<City>
                            {
                                new City { Name = "Barcelona"},
                                new City { Name = "Hospitalet de Llobregat"},
                            }
                        },
                        new State
                        {
                            Name = "Madrid",
                            Cities = new List<City>
                            {
                                new City { Name = "Madrid"},
                                new City { Name = "Alcalá de Henares"}
                            }
                        },
                         new State
                        {
                            Name = "Valencia",
                            Cities = new List<City>
                            {
                                new City { Name = "Valencia"},
                                new City { Name = "Castelló de la Plana"},
                            }
                        },
                    }
                });
                _context.Countries.Add(new Country 
                { 
                    Name = "Inglaterra",
                    States = new List<State>
                    {
                         new State
                        {
                            Name = "Londres",
                            Cities = new List<City>
                            {
                                new City { Name = "Londres"},
                                new City { Name = "Birmingham"},
                            }
                        },
                        new State
                        {
                            Name = "Liverpool",
                            Cities = new List<City>
                            {
                                new City { Name = "Liverpool"},
                                new City { Name = "Manchester"}
                            }
                        },
                         new State
                        {
                            Name = "Newcastle",
                            Cities = new List<City>
                            {
                                new City { Name = "Newcastle"},
                                new City { Name = "Sunderland"},
                            }
                        },
                    }
                });
                _context.Countries.Add(new Country 
                { 
                    Name = "Colombia",
                    States = new List<State>
                    {
                         new State
                        {
                            Name = "Bogotá",
                            Cities = new List<City>
                            {
                                new City { Name = "Bogotá"},
                                new City { Name = "Soacha"},
                            }
                        },
                        new State
                        {
                            Name = "Antioquia",
                            Cities = new List<City>
                            {
                                new City { Name = "Medellín"},
                                new City { Name = "Envigado"}
                            }
                        },
                         new State
                        {
                            Name = "Valle del Cauca",
                            Cities = new List<City>
                            {
                                new City { Name = "Cali"},
                                new City { Name = "Yumbo"},
                            }
                        },
                    }
                });
                _context.Countries.Add(new Country 
                { 
                    Name = "Estados Unidos",
                    States = new List<State>
                    {
                         new State
                        {
                            Name = "California",
                            Cities = new List<City>
                            {
                                new City { Name = "Los Angeles"},
                                new City { Name = "San Francisco"},
                            }
                        },
                        new State
                        {
                            Name = "Texas",
                            Cities = new List<City>
                            {
                                new City { Name = "Houston"},
                                new City { Name = "Austin"}
                            }
                        },
                         new State
                        {
                            Name = "Florida",
                            Cities = new List<City>
                            {
                                new City { Name = "Miami"},
                                new City { Name = "Orlando"},
                            }
                        },
                    }
                });

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
