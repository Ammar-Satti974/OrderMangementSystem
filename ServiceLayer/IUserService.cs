using WebApiProject.Models;

namespace WebApiProject.ServiceLayer
{
    public interface IUserService
    {
        Task<string> RegisterUserAsync(User user, string password);
        Task<string> LoginUserAsync(string userName, string password);
    }
}
