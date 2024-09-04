using WebApiProject.Models;

namespace WebApiProject.DAL
{
    public interface IUser : IRepo<User>
    {
            Task<User> GetUserByUsernameAsync(string userName);
            Task AddUserAsync(User user);
    }
}
