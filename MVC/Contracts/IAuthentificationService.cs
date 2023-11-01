using MVC.Models;

namespace MVC.Contracts
{
    public interface IAuthentificationService
    {
        Task<bool> Authenticate(string email, string password);

        Task<bool> Register(RegisterVM registerVM);

        Task Logout();
    }
}