using CrossOver.Data;

namespace CrossOver.Core.Service
{
    public interface IAppService
    {
        IDataContextFactory DataContextFactory { get; set; }
        IAuthService AuthService { get; set; }
        ICookieService CookieService { get; set; }
    }
}