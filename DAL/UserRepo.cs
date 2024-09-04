using Microsoft.EntityFrameworkCore;
using WebApiProject.Data;
using WebApiProject.Models;

namespace WebApiProject.DAL
{
    public class UserRepo: Repo<User>, IUser
    {
        private readonly ApplicationDBContext _dbContext;
        public UserRepo(ApplicationDBContext dBContext): base(dBContext) 
        {
            _dbContext = dBContext;
        }
        public async Task AddUserAsync(User user)
        {
            await _dbContext.Users.AddAsync(user);
        }
        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
        }
    }
}
