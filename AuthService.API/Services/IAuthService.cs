using AuthMicroService.Models;

namespace AuthMicroService.Interfaces
{
    public interface IAuthService
    {
        User isUserExist(User user);
        bool MatchUserCredentials(User user);
        bool RegisterUser(User user);
    }
}