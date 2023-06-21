namespace Sales.Web.Auth
{
    public interface ILoginService
    {
        Task LogoutAsync();
        Task<bool> IstokenActive();
        Task LoginAsync(string token);
    }
}
