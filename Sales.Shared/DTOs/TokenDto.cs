namespace Sales.Shared.DTOs
{
    public class TokenDto
    {
        public string Token { get; set; } = null!;
        public DateTime Expiration { get; set; }
    }
}
