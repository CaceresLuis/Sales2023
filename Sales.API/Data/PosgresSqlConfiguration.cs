namespace Sales.API.Data
{
    public class PosgresSqlConfiguration
    {
        public string ConnectionString { get; set; }
        public PosgresSqlConfiguration(string connectionString) => ConnectionString = connectionString;
    }
}
