using UrlShortenerEngine.Models;

namespace UrlShortenerEngine.Services
{
    public interface IAuthService
    {
        Task<AuthModel> RegisterAsync(RegisterModel model);
        Task<AuthModel> LoginAsync(LoginRequestModel model);
    }
}
